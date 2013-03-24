﻿using AgileEAP.Core;
using AgileEAP.Core.Infrastructure;
using AgileEAP.MVC;

namespace AgileEAP.MVC
{
    public class AgileEAPResourceDisplayName : System.ComponentModel.DisplayNameAttribute, IModelAttribute
    {
        private string _resourceValue = string.Empty;
        //private bool _resourceValueRetrived;

        public AgileEAPResourceDisplayName(string resourceKey)
            : base(resourceKey)
        {
            ResourceKey = resourceKey;
        }

        public string ResourceKey { get; set; }

        public override string DisplayName
        {
            get
            {
                ////do not cache resources because it causes issues when you have multiple languages
                ////if (!_resourceValueRetrived)
                ////{
                //var langId = EngineContext.Current.Resolve<IWorkContext>().WorkingLanguage.Id;
                //    _resourceValue = EngineContext.Current
                //        .Resolve<ILocalizationService>()
                //        .GetResource(ResourceKey, langId, true, ResourceKey);
                ////    _resourceValueRetrived = true;
                ////}
                //return _resourceValue;
                return "敏捷企业开发平台";
            }
        }

        public string Name
        {
            get { return "AgileEAPDisplayName"; }
        }
    }
}
