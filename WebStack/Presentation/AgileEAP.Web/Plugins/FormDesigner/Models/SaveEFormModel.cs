using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgileEAP.Plugin.FormDesigner.Models
{
    public class SaveEFormModel
    {
        private IList<string> _columnsName;
        private IList<string> _columnsValue;
        #region Properties

        /// <summary>
        /// 名称
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// 列名
        /// </summary>
        public IList<string> ColumnsName
        {
            get
            {
                if (_columnsName == null)
                {
                    _columnsName = new List<string>();
                }
                return _columnsName;

            }

            set { _columnsName = value; }
        }

        public IList<string> ColumnsValue
        {
            get
            {
                if (_columnsValue == null)
                {
                    _columnsValue = new List<string>();
                }
                return _columnsValue;

            }

            set { _columnsValue = value; }
        }
        #endregion
    }

    public class EFormExtendModel
    {
        private IList<string> _columnsName;
        private IList<string> _columnsValue;
        public IList<string> ColumnsName
        {
            get
            {
                if (_columnsName == null)
                {
                    _columnsName = new List<string>();
                }
                return _columnsName;

            }

            set { _columnsName = value; }
        }

        public IList<string> ColumnsValue
        {
            get
            {
                if (_columnsValue == null)
                {
                    _columnsValue = new List<string>();
                }
                return _columnsValue;

            }

            set { _columnsValue = value; }
        }
    }
}
