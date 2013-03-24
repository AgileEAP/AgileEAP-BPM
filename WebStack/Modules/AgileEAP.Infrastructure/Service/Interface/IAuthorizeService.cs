using System;
using System.Collections.Generic;
using AgileEAP.Infrastructure.Domain;
using AgileEAP.Core.Authentication;

namespace AgileEAP.Infrastructure.Service
{
    public interface IAuthorizeService
    {
        IList<Operate> GetAuthorizedOperates(IUser user, string resourceName);
        void DeleteResource(AgileEAP.Infrastructure.Domain.Resource entity);
        void DelRoleOperatePrivilege(string roleID);
        System.Collections.Generic.List<string> GetAllPrivilegeIDs(string userID);
        System.Collections.Generic.List<string> GetDataPriveleges(string userID);
        System.Collections.Generic.IList<AgileEAP.Infrastructure.Domain.Employee> GetEmployees(string orgID);
        System.Data.DataTable GetOperatorByOrg(System.Collections.Generic.IDictionary<string, object> parameters, AgileEAP.Core.PageInfo pageInfo, string filter = "");
        string GetOrgCodePath(string orgCode);
        System.Collections.Generic.IList<AgileEAP.Infrastructure.Domain.Organization> GetOrgNameByUserID(string userID);
        string GetOrgPath(string orgID);
        System.Collections.Generic.List<string> GetPrivilegeIDs(string userID);
        System.Data.DataTable GetRolePrivilegeIDs(string roleIDs);
        void SaveResource(AgileEAP.Infrastructure.Domain.Resource entity);
        void SaveRoleOperatePrivilege(System.Collections.Generic.IList<AgileEAP.Infrastructure.Domain.MetaData> metaData, System.Collections.Generic.IList<AgileEAP.Infrastructure.Domain.Privilege> privilege, System.Collections.Generic.IList<AgileEAP.Infrastructure.Domain.RolePrivilege> rolePrivileges);
        void SaveRolePrivilege(System.Collections.Generic.IList<AgileEAP.Infrastructure.Domain.RolePrivilege> privileges);
        void SaveSpecialPrivilege(System.Collections.Generic.IList<AgileEAP.Infrastructure.Domain.SpecialPrivilege> specialPrivilege);
    }
}
