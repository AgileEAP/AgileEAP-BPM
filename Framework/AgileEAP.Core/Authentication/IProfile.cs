using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgileEAP.Core.Authentication
{
    /// <summary>
    /// 个人档案信息
    /// </summary>
    public interface IProfile
    {
        /// <summary>
        /// 用户名
        /// </summary>
        string Name
        {
            get;
        }

        /// <summary>
        /// 组织Id
        /// </summary>
        string OrgID
        {
            get;
        }

        /// <summary>
        /// 组织名称
        /// </summary>
        string OrgName
        {
            get;
        }

        /// <summary>
        /// 组织路径
        /// </summary>
        string OrgPath
        {
            get;
        }

        /// <summary>
        /// 公司ID
        /// </summary>
        string CorpID
        {
            get;
        }

        /// <summary>
        /// 公司名称
        /// </summary>
        string CorpName
        {
            get;
        }

        /// <summary>
        /// 手机号码
        /// </summary>
        string Phone
        {
            get;
        }

        /// <summary>
        /// 住址
        /// </summary>
        string Address
        {
            get;
        }

        /// <summary>
        /// 电子邮件
        /// </summary>
        string Email
        {
            get;
        }
    }
}
