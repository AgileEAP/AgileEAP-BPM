﻿#region Description
/*================================================================================
 *  Copyright (c) SunTek.  All rights reserved.
 * ===============================================================================
 * Solution: Workflow
 * Module:  ExtendAttr
 * Descrption:
 * CreateDate: 2010/11/18 14:21:44
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



namespace AgileEAP.Workflow.Domain
{
    [Serializable]
    public partial class ExtendAttr :DomainObject<string>
    {
        #region Fields
		
		private string _entity = string.Empty;
		private string _entityID = string.Empty;
		private string _name = string.Empty;
		private string _value = string.Empty;
		
		
        #endregion

        #region Constructors
		public ExtendAttr(){}
		
		
		public ExtendAttr(string id,string entity,string entityID,string name,string value)
		{
		  this.ID=id;
		this._entity=entity;
		this._entityID=entityID;
		this._name=name;
		this._value=value;
		}
        #endregion

        #region Methods

        public override int GetHashCode()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            
            sb.Append(this.GetType().FullName);
			sb.Append(_entity);
			sb.Append(_entityID);
			sb.Append(_name);
			sb.Append(_value);

            return sb.ToString().GetHashCode();
        }
		
		public virtual bool Validate()
        {
			return true;
        }

        #endregion

        #region Properties
		
		/// <summary>
        /// 扩展实体
        /// </summary>
		public virtual string Entity
        {
            get { return  _entity; }
			set	{	_entity =  value;}
        }
		
		/// <summary>
        /// 实例ID
        /// </summary>
		public virtual string EntityID
        {
            get { return  _entityID; }
			set	{	_entityID =  value;}
        }
		
		/// <summary>
        /// 属性名
        /// </summary>
		public virtual string Name
        {
            get { return  _name; }
			set	{	_name =  value;}
        }
		
		/// <summary>
        /// 属性值
        /// </summary>
		public virtual string Value
        {
            get { return  _value; }
			set	{	_value =  value;}
        }
		
        #endregion
    }
}
