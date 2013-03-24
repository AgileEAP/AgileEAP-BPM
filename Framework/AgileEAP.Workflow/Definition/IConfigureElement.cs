using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace AgileEAP.Workflow.Definition
{
    /// <summary>
    /// Xml元素接口
    /// </summary>
    public interface IConfigureElement
    {
        /// <summary>
        /// 元素名称
        /// </summary>
        string ElementName
        {
            get;
            set;
        }

        /// <summary>
        /// 元素值
        /// </summary>
        string ElementValue
        {
            get;
            set;
        }

        ///// <summary>
        ///// 当前结点元素
        ///// </summary>
        //XElement Current
        //{
        //    get;
        //    set;
        //}

        ///// <summary>
        ///// 当前结点父元素
        ///// </summary>
        //ConfigureElement parent
        //{
        //    get;
        //    set;
        //}

        /// <summary>
        /// 初始化对象
        /// </summary>
        /// <param name="xElem"></param>
        void Initilize(XElement xElem);

        /// <summary>
        /// 把对象转换为XElement元素
        /// </summary>
        /// <returns></returns>
        XElement ToXElement();

        ///// <summary>
        ///// 保存为文件
        ///// </summary>
        ///// <param name="xmlFile"></param>
        //void Save(string xmlFile);

        /// <summary>
        /// 转换为xml格式的字符串
        /// </summary>
        string ToXml();
    }
}
