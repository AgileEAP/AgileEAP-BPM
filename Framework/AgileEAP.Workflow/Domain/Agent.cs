﻿#region Description
/*================================================================================
 *  Copyright (c) SunTek.  All rights reserved.
 * ===============================================================================
 * Solution: Workflow
 * Module:  Agent
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
    public partial class Agent :DomainObject<string>
    {
        #region Fields
		
		private string _agentFrom = string.Empty;
		private string _agentTo = string.Empty;
		private short _agentToType = default(Int16);
		private short _agentType = default(Int16);
		private System.DateTime _startTime = DateTime.Now;
		private System.DateTime _endTime = DateTime.Now;
		private string _agentReason = string.Empty;
		private string _creator = string.Empty;
		private System.DateTime _createTime = DateTime.Now;
		
		
        #endregion

        #region Constructors
		public Agent(){}
		
		
		public Agent(string id,string agentFrom,string agentTo,short agentToType,short agentType,System.DateTime startTime,System.DateTime endTime,string agentReason,string creator,System.DateTime createTime)
		{
		  this.ID=id;
		this._agentFrom=agentFrom;
		this._agentTo=agentTo;
		this._agentToType=agentToType;
		this._agentType=agentType;
		this._startTime=startTime;
		this._endTime=endTime;
		this._agentReason=agentReason;
		this._creator=creator;
		this._createTime=createTime;
		}
        #endregion

        #region Methods

        public override int GetHashCode()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            
            sb.Append(this.GetType().FullName);
			sb.Append(_agentFrom);
			sb.Append(_agentTo);
			sb.Append(_agentToType);
			sb.Append(_agentType);
			sb.Append(_startTime);
			sb.Append(_endTime);
			sb.Append(_agentReason);
			sb.Append(_creator);
			sb.Append(_createTime);

            return sb.ToString().GetHashCode();
        }
		
		public virtual bool Validate()
        {
			return true;
        }

        #endregion

        #region Properties
		
		/// <summary>
        /// 委托人
        /// </summary>
		public virtual string AgentFrom
        {
            get { return  _agentFrom; }
			set	{	_agentFrom =  value;}
        }
		
		/// <summary>
        /// 代理人
        /// </summary>
		public virtual string AgentTo
        {
            get { return  _agentTo; }
			set	{	_agentTo =  value;}
        }
		
		/// <summary>
        /// 代理人类型
        /// </summary>
		public virtual short AgentToType
        {
            get { return  _agentToType; }
			set	{	_agentToType =  value;}
        }
		
		/// <summary>
        /// 代理方式
        /// </summary>
		public virtual short AgentType
        {
            get { return  _agentType; }
			set	{	_agentType =  value;}
        }
		
		/// <summary>
        /// 生效时间
        /// </summary>
		public virtual System.DateTime StartTime
        {
            get { return  _startTime.ToSafeDateTime(); }
			set	{	_startTime =  value.ToSafeDateTime();}
        }
		
		/// <summary>
        /// 结束时间
        /// </summary>
		public virtual System.DateTime EndTime
        {
            get { return  _endTime.ToSafeDateTime(); }
			set	{	_endTime =  value.ToSafeDateTime();}
        }
		
		/// <summary>
        /// 代理原因
        /// </summary>
		public virtual string AgentReason
        {
            get { return  _agentReason; }
			set	{	_agentReason =  value;}
        }
		
		/// <summary>
        /// 创建者
        /// </summary>
		public virtual string Creator
        {
            get { return  _creator; }
			set	{	_creator =  value;}
        }
		
		/// <summary>
        /// 创建时间
        /// </summary>
		public virtual System.DateTime CreateTime
        {
            get { return  _createTime.ToSafeDateTime(); }
			set	{	_createTime =  value.ToSafeDateTime();}
        }
		
        #endregion
    }
}
