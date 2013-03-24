using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AgileEAP.Core;
using AgileEAP.MVC;
using AgileEAP.Core.Data;
using AgileEAP.Core.Infrastructure;
using AgileEAP.Core.Plugins;
using AgileEAP.Infrastructure.Domain;
using AgileEAP.Infrastructure.Service;

namespace AgileEAP.Plugin.FormDesigner
{
    public class MenuProvider
    {
        IRepository<string> repository = new Repository<string>();

        public void BuildMenu(string parentMenu, PluginDescriptor pluginDescriptor)
        {
            if (pluginDescriptor.Installed) return;

            var parentResource = repository.Query<Resource>().FirstOrDefault(r => r.Name == parentMenu);

            Resource resource = new Resource()
            {
                ID = "__" + pluginDescriptor.SystemName,
                Name = pluginDescriptor.SystemName,
                Text = pluginDescriptor.FriendlyName,
                URL = pluginDescriptor.ConfigurationUrl,
                ShowToolBar = 0,
                Type = (short)ResourceType.Menu,
                ExpandIcon = "",
                ParentID = parentResource.ID,
                OpenMode = 1,
                ShowNavigation = 0,
                SortOrder = 1,
                CreateTime = DateTime.Now,
                Creator = "AgileEAP",
                Operates = new List<Operate> 
                {
                    new Operate { ID="_newForm_", OperateName="新增", CommandName="newForm", Runat=(short)Runat.Ajax,SortOrder=1 },
                    new Operate { ID="_deleteForm_", OperateName="删除", CommandName="deleteForm", Runat=(short)Runat.Ajax,SortOrder=2 },  
                    new Operate { ID="_designForm_", OperateName="修改", CommandName="designForm", Runat=(short)Runat.Ajax,SortOrder=3 }
                }
            };
            IAuthorizeService authService = new AuthorizeService();
            authService.SaveResource(resource);
        }

        public void DeleteMenu(PluginDescriptor pluginDescriptor)
        {
            if (!pluginDescriptor.Installed) return;

            Resource resource = repository.GetDomain<Resource>("__" + pluginDescriptor.SystemName);
            if (resource != null)
            {
                IAuthorizeService authService = new AuthorizeService();
                authService.DeleteResource(resource);
            }
        }
    }
}
