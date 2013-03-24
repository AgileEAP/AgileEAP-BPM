using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Reflection;
using System.Collections;

using AgileEAP.Core;
using AgileEAP.Core.Data;
using AgileEAP.Core.Authentication;
using AgileEAP.Core.Web;
using AgileEAP.Core.Utility;
using AgileEAP.WebControls;
using AgileEAP.Core.Extensions;
using AgileEAP.Infrastructure.Domain;
using AgileEAP.Infrastructure.Service;


namespace AgileEAP.Administration
{
    public static class PageExtension
    {
        static IRepository<string> repository = new Repository<string>();

        public static void BindCombox(this Combox combox, Enum value)
        {
            Dictionary<string, string> values = value.GetRemarks();
            combox.Items = values.Select(v => new ComboxItem() { Tag = "combox", Text = v.Value, Value = v.Key }).ToList();
            combox.SelectedValue = ((int)(object)value).ToSafeString();
        }

        public static void BindCombox(this Combox combox, Hashtable itemList)
        {
            foreach (DictionaryEntry item in itemList)
            {
                combox.Items.Add(new ComboxItem() { Tag = "combox", Text = item.Value.ToString(), Value = item.Key.ToString() });
            }
        }

        public static string GetUerName(this string operID)
        {
            return (repository.GetDomain<Operator>(operID) ?? new Operator() { UserName = operID }).UserName;
        }


        public static string GetOperID(this string userName)
        {
            return repository.FindOne<Operator>(ParameterBuilder.BuildParameters().SafeAdd("LoginName", userName)).ID;
        }
        /// <summary>
        /// 绑定下拉列表
        /// </summary>
        /// <param name="cbo">下拉控件</param>
        /// <param name="dictName">字典名</param>
        public static void BindDict(this Combox cbo, string dictName)
        {
            IUtilService utilService = new UtilService(repository);
            IList<DictItem> items = utilService.GetDictItems(dictName);

            if (items != null && items.Count > 0)
            {
                foreach (var item in items)
                {
                    cbo.Items.Add(new ComboxItem() { Value = item.Value, Text = item.Text });
                }

                cbo.SelectedValue = items[0].Value;
            }
        }

        /// <summary>
        /// 绑定下拉列表
        /// </summary>
        /// <param name="cbo">下拉控件</param>
        /// <param name="dictName">字典名</param>
        public static void BindDictID(this Combox cbo, string dictName)
        {
            IUtilService utilService = new UtilService(repository);
            IList<DictItem> items = utilService.GetDictItems(dictName);

            if (items != null && items.Count > 0)
            {
                foreach (var item in items)
                {
                    cbo.Items.Add(new ComboxItem() { Value = item.ID, Text = item.Text });
                }

                cbo.SelectedValue = items[0].ID;
            }
        }


        /// <summary>
        /// 获取字典项值
        /// </summary>
        /// <param name="dictItemID"></param>
        /// <returns></returns>
        public static string GetDictItemText(this string dictValue, string dict)
        {
            Dict odict = repository.All<Dict>().FirstOrDefault(d => d.Name == dict);
            if (odict != null)
            {
                DictItem odictitem = repository.All<DictItem>().FirstOrDefault(d => d.DictID == odict.ID && d.Value == dictValue);
                if (odictitem != null)
                    return odictitem.Text;
            }

            return dictValue;
        }

        /// <summary>
        /// 获取字典项值
        /// </summary>
        /// <param name="dictItemID"></param>
        /// <returns></returns>
        public static string GetDictItemText(this string dictID)
        {
            DictItem odictitem = repository.All<DictItem>().FirstOrDefault(d => d.ID == dictID);
            if (odictitem != null)
                return odictitem.Text;
            return dictID;
        }

        /// <summary>   
        /// 实体转化为XML   
        /// </summary>   
        public static string ParseToXML(this object model, string fatherNodeName)
        {
            XmlDocument xmldoc = new XmlDocument();
            XmlElement ModelNode = xmldoc.CreateElement(fatherNodeName);
            xmldoc.AppendChild(ModelNode);

            if (model != null)
            {
                foreach (PropertyInfo property in model.GetType().GetProperties())
                {
                    XmlElement attribute = xmldoc.CreateElement(property.Name);
                    if (property.GetValue(model, null) != null)
                        attribute.InnerText = property.GetValue(model, null).ToString();
                    //else
                    //    attribute.InnerText = "[Null]";
                    ModelNode.AppendChild(attribute);
                }
            }
            return xmldoc.OuterXml;
        }
    }
}