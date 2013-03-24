using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgileEAP.Core
{
    public enum LogType
    {
        /// <summary>
        /// 操作
        /// </summary>
        [Remark("操作")]
        Operate = 0,

        /// <summary>
        /// 登陆
        /// </summary>
        [Remark("登陆")]
        Login = 1,

        /// <summary>
        /// 接口
        /// </summary>
        [Remark("接口")]
        API = 2
    }

    /// <summary>
    /// Action类型
    /// </summary>
    public enum ActionType
    {
        /// <summary>
        /// 新增
        /// </summary>
        [Remark("新增")]
        Add = 0,

        /// <summary>
        /// 删除
        /// </summary>
        [Remark("删除")]
        Delete = 1,

        /// <summary>
        /// 修改
        /// </summary>
        [Remark("修改")]
        Update = 2,

        /// <summary>
        /// 查看
        /// </summary>
        [Remark("查看")]
        View = 3,

        /// <summary>
        /// 查询
        /// </summary>
        [Remark("查询")]
        Search = 4,

        /// <summary>
        /// 返回
        /// </summary>
        [Remark("返回")]
        Return = 5,

        /// <summary>
        /// 刷新
        /// </summary>
        [Remark("刷新")]
        Refresh = 6,

        [Remark("关闭")]
        Close = 7,

        /// <summary>
        /// 登陆
        /// </summary>
        [Remark("登陆")]
        Login = 8,

        /// <summary>
        /// 退出
        /// </summary>
        [Remark("退出")]
        Exit = 9,

        /// <summary>
        /// 其他
        /// </summary>
        [Remark("操作")]
        Operate = 255
    }

    /// <summary>
    /// 页面上下文存储位置
    /// </summary>
    public enum PageContextStore
    {
        Cookie = 0,

        Database = 1
    }

    /// <summary>
    /// 操作结果
    /// </summary>
    public enum DoResult
    {
        /// <summary>
        /// 操作失败
        /// </summary>
        [Remark("操作失败")]
        Failed = 0,

        /// <summary>
        /// 操作成功
        /// </summary>
        [Remark("操作成功")]
        Success = 1,

        /// <summary>
        /// 登陆成功
        /// </summary>
        [Remark("登陆成功")]
        LoginSuccess = 2,

        /// <summary>
        /// 其他
        /// </summary>
        [Remark("其他")]
        Other = 255
    }

    /// <summary>
    /// 数据库类型
    /// </summary>
    public enum DatabaseType
    {
        Oracle = 1,

        MsSQL2000 = 11,

        MsSQL2005 = 12,

        MsSQL2008 = 13,

        MySQL = 20
    }

    /// <summary>
    /// 用户类型
    /// </summary>
    public enum UserType
    {
        /// <summary>
        /// 超级管理员
        /// </summary>
        [Remark("超级管理员")]
        Administrator = 0,

        /// <summary>
        /// 公司管理员
        /// </summary>
        [Remark("管理员")]
        CorpAdmin = 1,

        /// <summary>
        /// 普通用户
        /// </summary>
        [Remark("普通用户")]
        User = 2
    }

    /// <summary>
    /// 资源类型
    /// </summary>
    public enum ResourceType
    {
        /// <summary>
        /// 菜单
        /// </summary>
        [Remark("菜单")]
        Menu = 1,

        /// <summary>
        /// 页面
        /// </summary>
        [Remark("页面")]
        Page = 2,

        /// <summary>
        /// 按钮
        /// </summary>
        [Remark("按钮")]
        Button = 3,

        /// <summary>
        /// 业务数据
        /// </summary>
        [Remark("业务数据")]
        BizData = 4,

        /// <summary>
        /// 组织数据
        /// </summary>
        [Remark("业务子系统")]
        SubBizSystem = 5
    }

    /// <summary>
    /// 页面打开方式
    /// </summary>
    public enum MenuTarget
    {
        /// <summary>
        /// 嵌入导航区
        /// </summary>
        [Remark("嵌入导航区")]
        NavigateFrame = 1,

        /// <summary>
        /// 嵌入内容区
        /// </summary>
        [Remark("嵌入内容区")]
        ContentFrame = 2,

        /// <summary>
        /// 嵌入主框架
        /// </summary>
        [Remark("嵌入主框架")]
        MainFrame = 3,

        /// <summary>
        /// 嵌入新框架
        /// </summary>
        [Remark("嵌入新框架")]
        NewFrame = 4,

        /// <summary>
        /// 弹出新框架
        /// </summary>
        [Remark("弹出新框架")]
        PopuNewFrame = 5,
    }

    /// <summary>
    /// 按钮触发方式
    /// </summary>
    public enum Runat
    {
        /// <summary>
        /// Ajax
        /// </summary>
        [Remark("Ajax")]
        Ajax = 1,

        /// <summary>
        /// Post
        /// </summary>
        [Remark("Post")]
        Post = 2,

        /// <summary>
        /// 弹出
        /// </summary>
        [Remark("弹出")]
        Popup = 3
    }

    /// <summary>
    /// 选项
    /// </summary>
    public enum Options
    {
        /// <summary>
        /// 否
        /// </summary>
        [Remark("否")]
        No = 0,

        /// <summary>
        /// 是
        /// </summary>
        [Remark("是")]
        Yes = 1
    }

    public enum UserStatus
    {
        [Remark("正常")]
        Normal = 1,
        //[Remark("挂起")]
        //Suspend = 2,
        //[Remark("注销")]
        //Invalid = 3,
        [Remark("冻结")]
        Freezon = 2
    }

    public enum AuthMode
    {
        [Remark("本地密码认证")]
        LocalPassword = 1,
        [Remark("LDAP认证")]
        LDAP = 2
    }
}
