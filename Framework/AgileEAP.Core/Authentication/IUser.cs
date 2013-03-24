using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgileEAP.Core.Authentication
{
    /// <summary>
    /// 用户接口
    /// </summary>
    public interface IUser
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        string ID
        {
            get;
        }

        /// <summary>
        /// 登陆名
        /// </summary>
        string LoginName
        {
            get;
        }

        string Name
        {
            get;
        }

        short UserType
        {
            get;
        }

        string OrgID
        {
            get;
        }
           

        /// <summary>
        /// 主题皮肤
        /// </summary>
        string Theme
        {
            get;
        }
    }
}
