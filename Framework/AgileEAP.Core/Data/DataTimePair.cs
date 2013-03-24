#region Description
/*==============================================================================
 *  Copyright (c) trenhui.  All rights reserved.
 * ===============================================================================
 * This code and information is provided "as is" without warranty of any kind,
 * either expressed or implied, including but not limited to the implied warranties
 * of merchantability and fitness for a particular purpose.
 * ===============================================================================
 * This code is only for study
 * ==============================================================================*/
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Criterion;

namespace AgileEAP.Core.Data
{
    public class DateTimePair
    {
        private DateTime? _startTime = DateTime.MinValue;
        private DateTime? _endTime = DateTime.MaxValue;

        public DateTime? StartTime
        {
            get { return _startTime; }
            set { _startTime = value; }
        }

        public DateTime? EndTime
        {
            get { return _endTime; }
            set { _endTime = value; }
        }

        public DateTimePair()
        { }

        public DateTimePair(DateTime? startTime, DateTime? endTime)
        {
            _startTime = startTime;
            _endTime = endTime;
        }

        public string ToSql(string propertyName)
        {
            if (_startTime != null && _endTime != null)
                return string.Format(" {0} Between {1} And {2}) ", propertyName, _startTime, _endTime);
            else if (_startTime != null)
                return string.Format(" {0}>={1} ", propertyName, _startTime);
            else if (_endTime != null)
                return string.Format(" {0}<={1} ", propertyName, _endTime);

            return string.Empty;
        }

        public ICriterion ToExpression(string propertyName)
        {
            if (_startTime != null && _endTime != null)
                return Expression.Between(propertyName, _startTime, _endTime);
            else if (_startTime != null)
                return Expression.Ge(propertyName, _startTime);
            else if (_endTime != null)
                return Expression.Le(propertyName, _endTime);

            return Expression.Sql(" 1=1 ");
        }
    }

    /// <summary>
    /// 自定义条件
    /// </summary>
    public class Condition
    {
        public Condition()
        { }

        /// <summary>
        /// 查询条件表达式如 (a<>1) 或者(a is null) 或者 (a>0 and a<100)
        /// </summary>
        /// <param name="expression"></param>
        public Condition(string expression)
        {
            Expression = expression;
        }

        /// <summary>
        /// 条件表达式
        /// </summary>
        public string Expression
        {
            get;
            set;
        }
    }
}
