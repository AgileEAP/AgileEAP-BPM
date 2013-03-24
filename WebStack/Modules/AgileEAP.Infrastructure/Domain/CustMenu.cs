﻿#region Description
/*================================================================================
 *  Copyright (c) SunTek.  All rights reserved.
 * ===============================================================================
 * Solution: AuthorizeCenter
 * Module:  CustMenu
 * Descrption:
 * CreateDate: 2010/11/18 13:55:34
 * Author: trenhui
 * Version:1.0
 * ===============================================================================
 * History
 *
 * Update Descrption:
 * Remark:
 * Update Time:
 * Author:generated by codesmithTemplate
 * 
 * ===============================================================================*/
#endregion
using System;
using System.Collections;
using System.Collections.Generic;

using AgileEAP.Core.Domain;
using AgileEAP.Core.Extensions;



namespace AgileEAP.Infrastructure.Domain
{
    public partial class CustMenu :DomainObject<string>
    {
        #region Fields
		
		private string _operatorID = string.Empty;
		private string _resourceID = string.Empty;
		private string _name = string.Empty;
		private string _text = string.Empty;
		private string _rootID = string.Empty;
		private string _parentID = string.Empty;
		private int _sortOrder = default(Int32);
		private string _icon = string.Empty;
		private string _expandIcon = string.Empty;
		
		
        #endregion

        #region Constructors
		public CustMenu(){}
		
		
		public CustMenu(string id,string operatorID,string resourceID,string name,string text,string rootID,string parentID,int sortOrder,string icon,string expandIcon)
		{
		  this.ID=id;
		this._operatorID=operatorID;
		this._resourceID=resourceID;
		this._name=name;
		this._text=text;
		this._rootID=rootID;
		this._parentID=parentID;
		this._sortOrder=sortOrder;
		this._icon=icon;
		this._expandIcon=expandIcon;
		}
        #endregion

        #region Methods

        public override int GetHashCode()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            
            sb.Append(this.GetType().FullName);
			sb.Append(_operatorID);
			sb.Append(_resourceID);
			sb.Append(_name);
			sb.Append(_text);
			sb.Append(_rootID);
			sb.Append(_parentID);
			sb.Append(_sortOrder);
			sb.Append(_icon);
			sb.Append(_expandIcon);

            return sb.ToString().GetHashCode();
        }
		
		public virtual bool Validate()
        {
			return true;
        }

        #endregion

        #region Properties
		
		/// <summary>
        /// 操作员ID
        /// </summary>
		public virtual string OperatorID
        {
            get { return  _operatorID; }
			set	{	_operatorID =  value;}
        }
		
		/// <summary>
        /// 菜单编号
        /// </summary>
		public virtual string ResourceID
        {
            get { return  _resourceID; }
			set	{	_resourceID =  value;}
        }
		
		/// <summary>
        /// 菜单名称
        /// </summary>
		public virtual string Name
        {
            get { return  _name; }
			set	{	_name =  value;}
        }
		
		/// <summary>
        /// 菜单显示（中文）
        /// </summary>
		public virtual string Text
        {
            get { return  _text; }
			set	{	_text =  value;}
        }
		
		/// <summary>
        /// 根菜单
        /// </summary>
		public virtual string RootID
        {
            get { return  _rootID; }
			set	{	_rootID =  value;}
        }
		
		/// <summary>
        /// 父菜单
        /// </summary>
		public virtual string ParentID
        {
            get { return  _parentID; }
			set	{	_parentID =  value;}
        }
		
		/// <summary>
        /// 序号
        /// </summary>
		public virtual int SortOrder
        {
            get { return  _sortOrder; }
			set	{	_sortOrder =  value;}
        }
		
		/// <summary>
        /// 菜单图标
        /// </summary>
		public virtual string Icon
        {
            get { return  _icon; }
			set	{	_icon =  value;}
        }
		
		/// <summary>
        /// 菜单展开图标
        /// </summary>
		public virtual string ExpandIcon
        {
            get { return  _expandIcon; }
			set	{	_expandIcon =  value;}
        }
		
        #endregion
    }
}
