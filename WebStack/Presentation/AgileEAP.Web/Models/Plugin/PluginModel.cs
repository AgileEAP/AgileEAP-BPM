using System.Web.Mvc;
using FluentValidation.Attributes;
using AgileEAP.MVC;

namespace AgileEAP.Web.Models.Plugin
{
    public partial class PluginModel : AgileEAPModel
    {
        [AllowHtml]
        public string Group { get; set; }

        [AllowHtml]
        public string FriendlyName { get; set; }

        [AllowHtml]
        public string SystemName { get; set; }

        [AllowHtml]
        public string Version { get; set; }

        [AllowHtml]
        public string Author { get; set; }

        public int DisplayOrder { get; set; }

        public string ConfigurationUrl { get; set; }

        public bool Installed { get; set; }

        public bool CanChangeEnabled { get; set; }
        public bool IsEnabled { get; set; }
    }
}