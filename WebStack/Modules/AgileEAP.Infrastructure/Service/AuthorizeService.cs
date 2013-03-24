using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using AgileEAP.Core;
using AgileEAP.Core.Authentication;
using AgileEAP.Core.Data;
using AgileEAP.Core.Caching;
using AgileEAP.Core.Utility;
using AgileEAP.Core.Service;
using AgileEAP.Core.Extensions;
using AgileEAP.Infrastructure.Domain;

namespace AgileEAP.Infrastructure.Service
{
    public class AuthorizeService : IAuthorizeService
    {
        private readonly ILogger logger = LogManager.GetLogger(typeof(AuthorizeService));
        private readonly IRepository<string> repository;
        public AuthorizeService(IRepository<string> repository)
        {
            this.repository = repository;
        }

        public AuthorizeService()
        {
            this.repository = new Repository<string>();
        }

        #region Operator


        public DataTable GetOperatorByOrg(IDictionary<string, object> parameters, PageInfo pageInfo, string filter = "")
        {
            string cmdText = string.Format("select t1.id,username,loginname,orgid,creator,createTime,status from AC_Operator t1 join OM_EmployeeOrg t2 on t1.ID=t2.employeeid ");
            if (!string.IsNullOrEmpty(filter))
            {
                cmdText += " where " + filter + " and 1=1 ";
            }
            //cmdText += " order by createtime desc ";
            return repository.ExecuteDataTable<Operator>(cmdText, parameters, " createtime desc ", pageInfo);
        }

        /// <summary>
        /// 根据用户取得相应的页面权限（已经处理特殊权限）
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <returns></returns>
        public List<string> GetPrivilegeIDs(string userID)
        {
            List<string> privilegeIDs = GetAllPrivilegeIDs(userID);
            //特殊权限添加
            List<SpecialPrivilege> specialPrivileges = repository.All<SpecialPrivilege>().Where(o => o.OperatorID.Equals(userID, StringComparison.OrdinalIgnoreCase)).ToList();

            foreach (SpecialPrivilege item in specialPrivileges.ToList())
            {
                if (item.AuthFlag == 1)
                {
                    if (!privilegeIDs.Contains(item.PrivilegeID))
                        privilegeIDs.Add(item.PrivilegeID);
                }
                else if (item.AuthFlag == 2)
                {
                    privilegeIDs.Remove(item.PrivilegeID);
                }
            }

            return privilegeIDs;
        }

        /// <summary>
        /// 根据用户取得相应的页面权限（未处理特殊权限）
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <returns></returns>
        public List<string> GetAllPrivilegeIDs(string userID)
        {
            DataTable dt = repository.ExecuteDataTable<Operator>(string.Format(@"select c.PrivilegeID from AC_RolePrivilege c where c.RoleID in
                                                                        (select d.RoleID from OM_ObjectRole d where d.ObjectID='{0}')", userID)) ?? new DataTable();
            return dt.AsEnumerable().Select(dr => dr["PrivilegeID"].ToSafeString()).ToList();

        }


        /// <summary>
        /// 获取用户数据权限
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <returns></returns>
        public List<string> GetDataPriveleges(string userID)
        {
//            DataTable dt = repository.ExecuteDataTable<Operator>(string.Format(@"select  Value from AC_MetaData a where a.ID in
//                                                                        (select b.MetaDataID from AC_Privilege b where b.ID in 
//                                                                        (select c.PrivilegeID from AC_RolePrivilege c where c.RoleID in
//                                                                        (select d.RoleID from OM_ObjectRole d where d.ObjectID='{0}')))", userID)) ?? new DataTable();

            DataTable dt = repository.ExecuteDataTable<Operator>(string.Format(@"select distinct Value from AC_MetaData a inner join AC_Privilege b on a.ID=b.MetaDataID
inner join AC_RolePrivilege c on b.ID=c.PrivilegeID
inner join OM_ObjectRole d on c.RoleID=d.RoleID where d.ObjectID='{0}'", userID)) ?? new DataTable();

            return dt.AsEnumerable().Select(dr => dr["Value"].ToSafeString()).ToList();
        }
        #endregion

        #region Org
        /// <summary>
        /// 获取顶级组织到当前组织的层级路径,以/分隔
        /// </summary>
        /// <param name="orgID"></param>
        public string GetOrgPath(string orgID)
        {
            var func = ExpressionUtil.MakeRecursion<string, string>(f => r =>
            {
                Organization org = repository.All<Organization>().FirstOrDefault(o => o.ID == r);
                if (org == null)
                    return string.Empty;

                if (string.IsNullOrEmpty(org.ParentID) || org.ParentID == "-1")
                    return org.ID;

                return string.Format("{0}/{1}", f(org.ParentID), org.ID);
            });

            return func(orgID);
        }

        /// <summary>
        /// 获取顶级组织到当前组织的编码层级路径,以/分隔
        /// </summary>
        /// <param name="orgCode"></param>
        public string GetOrgCodePath(string orgCode)
        {
            var func = ExpressionUtil.MakeRecursion<string, string>(f => r =>
            {
                Organization org = repository.All<Organization>().FirstOrDefault(o => o.Code == r);
                if (org == null)
                    return string.Empty;

                if (string.IsNullOrEmpty(org.ParentID) || org.ParentID == "-1")
                    return org.Code;

                return string.Format("{0}/{1}", f(org.ParentID), org.Code);
            });

            return func(orgCode);
        }

        /// <summary>
        /// 获取组织下的所有员工
        /// </summary>
        /// <param name="orgID">组织ID</param>
        /// <returns></returns>
        public IList<Employee> GetEmployees(string orgID)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.SafeAdd("ID", new Condition(string.Format("ID in (select b.EmployeeID from OM_EmployeeOrg b where b.OrgID='{0}')", orgID)));

            return repository.FindAll<Employee>(parameters);
        }

        /// <summary>
        /// 获取员工所在部门名称
        /// </summary>
        /// <param name="orgID">组织ID</param>
        /// <returns></returns>
        public IList<Organization> GetOrgNameByUserID(string userID)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.SafeAdd("ID", new Condition(string.Format("ID in (select b.OrgID from OM_EmployeeOrg b where b.EmployeeID='{0}')", userID)));

            return repository.FindAll<Organization>(parameters);
        }
        #endregion

        #region IRolePrivilegeService Imp
        public void SaveRolePrivilege(IList<RolePrivilege> privileges)
        {

            using (ITransaction trans = UnitOfWork.BeginTransaction(typeof(RolePrivilege)))
            {
                //删除已经存在的
                repository.ExecuteSql<RolePrivilege>(string.Format("delete from AC_RolePrivilege where RoleID='{0}' and PrivilegeID not in (select ID from AC_Privilege where Type={1})", privileges[0].RoleID, (int)ResourceType.BizData));

                foreach (var p in privileges)
                {
                    repository.SaveOrUpdate(p);
                }
                trans.Commit();
            }
            repository.ClearCache<RolePrivilege>();
        }

        public void SaveRoleOperatePrivilege(IList<MetaData> metaData, IList<Privilege> privilege, IList<RolePrivilege> rolePrivileges)
        {
            using (ITransaction trans = UnitOfWork.BeginTransaction(typeof(RolePrivilege)))
            {
                //删除已经存在的
                string roleID = rolePrivileges[0].RoleID;
                repository.ExecuteSql<MetaData>(string.Format(@"delete a from AC_MetaData a
                                                              join AC_Privilege b on a.ID=b.MetaDataID
                                                              join AC_RolePrivilege c on b.ID=c.PrivilegeID
                                                              where b.Type=4 and c.RoleID='{0}'", roleID));
                repository.ExecuteSql<Privilege>(string.Format(@"delete a from AC_Privilege a 
                                                                  join AC_RolePrivilege b on a.ID=b.PrivilegeID
                                                                  where a.Type=4 and b.RoleID='{0}'", roleID));

                repository.ExecuteSql<RolePrivilege>(string.Format("delete from AC_RolePrivilege where PrivilegeID in (select id from AC_Privilege where type=4) and RoleID='{0}'", roleID));

                foreach (var p in metaData)
                {
                    repository.SaveOrUpdate(p);
                }
                foreach (var p in privilege)
                {
                    repository.SaveOrUpdate(p);
                }
                foreach (var p in rolePrivileges)
                {
                    repository.SaveOrUpdate(p);
                }


                trans.Commit();
            }

            repository.ClearCache<MetaData>();
            repository.ClearCache<Privilege>();
            repository.ClearCache<RolePrivilege>();

        }

        public DataTable GetRolePrivilegeIDs(string roleIDs)
        {

            string cmdText = string.Format("select PrivilegeID from AC_RolePrivilege where roleid in ({0})", roleIDs);
            return repository.ExecuteDataTable<RolePrivilege>(cmdText, null);
        }


        /// <summary>
        /// 删除已经存在的
        /// </summary>
        /// <param name="roleID"></param>
        public void DelRoleOperatePrivilege(string roleID)
        {
            using (ITransaction trans = UnitOfWork.BeginTransaction(typeof(RolePrivilege)))
            {
                repository.ExecuteSql<MetaData>(string.Format(@"delete a from AC_MetaData a
                                                              join AC_Privilege b on a.ID=b.MetaDataID
                                                              join AC_RolePrivilege c on b.ID=c.PrivilegeID
                                                              where b.Type=4 and c.RoleID='{0}'", roleID));
                repository.ExecuteSql<Privilege>(string.Format(@"delete a from AC_Privilege a 
                                                                  join AC_RolePrivilege b on a.ID=b.PrivilegeID
                                                                  where a.Type=4 and b.RoleID='{0}'", roleID));

                repository.ExecuteSql<RolePrivilege>(string.Format("delete from AC_RolePrivilege where PrivilegeID in (select id from AC_Privilege where type=4) and RoleID='{0}'", roleID));

                trans.Commit();
            }

            repository.ClearCache<MetaData>();
            repository.ClearCache<Privilege>();
            repository.ClearCache<RolePrivilege>();
        }

        public void SaveSpecialPrivilege(IList<SpecialPrivilege> specialPrivilege)
        {

            using (ITransaction trans = UnitOfWork.BeginTransaction(typeof(SpecialPrivilege)))
            {
                //删除已经存在的
                repository.ExecuteSql<SpecialPrivilege>(string.Format("delete from AC_SpecialPrivilege where OperatorID='{0}' and AuthFlag={1}", specialPrivilege[0].OperatorID, specialPrivilege[0].AuthFlag));

                foreach (var p in specialPrivilege)
                {
                    repository.SaveOrUpdate(p);
                }
                trans.Commit();
            }

            repository.ClearCache<SpecialPrivilege>();
        }
        #endregion


        #region IResourceService Imp
        /// <summary>
        /// 删除资源
        /// </summary>
        /// <param name="entity">资源实体</param>
        public void DeleteResource(Resource entity)
        {
            IList<Privilege> previleges = repository.Query<Privilege>().Where(p => p.ResourceID == entity.ID).ToList();

            using (ITransaction trans = UnitOfWork.BeginTransaction(typeof(Resource)))
            {
                foreach (var p in previleges)
                {
                    if (!string.IsNullOrWhiteSpace(p.OperateID))
                        repository.Delete<Operate>(p.OperateID);

                    repository.Delete<Privilege>(p);
                }

                repository.Delete<Resource>(entity);

                trans.Commit();
            }

            repository.ClearCache<Resource>();
            repository.ClearCache<Operate>();
            repository.ClearCache<Privilege>();
        }

        /// <summary>
        /// 保存资源
        /// </summary>
        /// <param name="entity">资源实体</param>
        public void SaveResource(Resource entity)
        {
            using (ITransaction trans = UnitOfWork.BeginTransaction(typeof(Resource)))
            {
                repository.Clear<Resource>();
                repository.SaveOrUpdate(entity);

                IDictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add("ResourceID", entity.ID);
                parameters.Add("OperateID", string.Empty);
                Privilege privilege = repository.FindOne<Privilege>(parameters);
                if (privilege == null)
                {
                    repository.SaveOrUpdate(new Privilege()
                    {
                        ID = IdGenerator.NewComb().ToString(),
                        MetaDataID = string.Empty,
                        Name = entity.Text,
                        OperateID = string.Empty,
                        //AppID = entity.AppID,
                        //OwnerOrg = entity.OwnerOrg,
                        //ModuleID = entity.ModuleID,
                        //Description = entity.Description,
                        ResourceID = entity.ID,
                        SortOrder = entity.SortOrder,

                        Type = entity.Type,
                        CreateTime = DateTime.Now,
                        Creator = entity.Creator,
                    });
                }

                if (entity.Operates != null)
                {
                    string idList = "";
                    foreach (var operate in entity.Operates)
                    {
                        idList += string.Format(" '{0}',", operate.ID);
                    }
                    string sWhere = string.IsNullOrEmpty(idList.TrimEnd(',')) ? "" : string.Format(" and OperateID not in ({0}) ", idList.TrimEnd(','));
                    repository.ExecuteSql<Resource>(string.Format("delete from AC_Operate where id in (select OperateID from AC_Privilege where ResourceID='{0}' and (OperateID is not null and OperateID<>'') {1})", entity.ID, sWhere));
                    repository.ExecuteSql<Resource>(string.Format("delete from AC_Privilege where ResourceID='{0}' and (OperateID is not null and OperateID<>'') {1}", entity.ID, sWhere));

                    foreach (var operate in entity.Operates)
                    {
                        repository.SaveOrUpdate(operate);
                        parameters.Clear();
                        parameters.SafeAdd("ResourceID", entity.ID);
                        parameters.SafeAdd("OperateID", operate.ID);
                        IList<Privilege> privilegeList = repository.FindAll<Privilege>(parameters);
                        if (privilegeList.Count == 0)
                        {
                            repository.SaveOrUpdate(new Privilege()
                            {
                                ID = IdGenerator.NewComb().ToString(),
                                //AppID = entity.AppID,
                                MetaDataID = string.Empty,
                                Name = operate.OperateName,
                                OperateID = operate.ID,
                                //ModuleID = entity.ModuleID,
                                //OwnerOrg = entity.OwnerOrg,
                                //Description = entity.Description,
                                ResourceID = entity.ID,
                                SortOrder = operate.SortOrder,
                                Type = 3,
                                CreateTime = DateTime.Now,
                                Creator = entity.Creator,
                            });
                        }
                    }
                }

                trans.Commit();
            }

            repository.ClearCache<Resource>();
            repository.ClearCache<Privilege>();
            repository.ClearCache<Operate>();
        }
        #endregion


        public IList<Operate> GetAuthorizedOperates(IUser user, string resourceName)
        {
            Resource resource = repository.All<Resource>().FirstOrDefault(o => o.Name == resourceName);

            if (resource == null) return null;

            List<string> privilegeIDs = GetPrivilegeIDs(user.ID);
            //根据权限id取到操作项id集合
            List<string> operateIds = repository.All<Privilege>().Where(p => p.ResourceID == resource.ID && !string.IsNullOrWhiteSpace(p.OperateID) && (user.UserType == (short)UserType.Administrator || privilegeIDs.Contains(p.ID))).Select(p => p.OperateID).ToList() ?? new List<string>();

            IList<Operate> operates = repository.All<Operate>().Where(o => operateIds.Contains(o.ID)).OrderBy(o => o.SortOrder).ToList();//UserBiz.GetAuthorizeActions(User, requestUrl, entry);

            return operates;
        }

        public void SaveObjectRoles(IList<ObjectRole> objectRoles)
        {

            using (ITransaction trans = UnitOfWork.BeginTransaction(typeof(ObjectRole)))
            {
                //删除已经存在的
                repository.ExecuteSql<ObjectRole>(string.Format("delete from OM_ObjectRole where ObjectID='{0}'", objectRoles[0].ObjectID));


                foreach (var p in objectRoles)
                {
                    repository.SaveOrUpdate(p);
                }
                trans.Commit();
            }
        }

        public string GetPidByResId(string resourceID)
        {

            DataTable dt = repository.ExecuteDataTable<Privilege>(string.Format("select id from AC_Privilege where (type={0} or type={1}) and ResourceID='{2}'", (int)ResourceType.Menu, (int)ResourceType.Page, resourceID));

            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0][0].ToString();
            }
            return "";

        }
    }
}
