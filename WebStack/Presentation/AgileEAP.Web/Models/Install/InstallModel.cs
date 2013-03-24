using System.Web.Mvc;
using FluentValidation.Attributes;
using AgileEAP.MVC;
using AgileEAP.Web.Validators.Install;

namespace AgileEAP.Web.Models.Install
{
    [Validator(typeof(InstallValidator))]
    public class InstallModel : AgileEAPModel
    {
        [AllowHtml]
        public string AdminEmail { get; set; }
        [AllowHtml]
        public string AdminPassword { get; set; }
        [AllowHtml]
        public string ConfirmPassword { get; set; }


        [AllowHtml]
        public string DatabaseConnectionString { get; set; }
        public string DataProvider { get; set; }
        //SQL Server properties
        public string SqlConnectionInfo { get; set; }
        [AllowHtml]
        public string SqlServerName { get; set; }
        [AllowHtml]
        public string SqlDatabaseName { get; set; }
        [AllowHtml]
        public string SqlServerUsername { get; set; }
        [AllowHtml]
        public string SqlServerPassword { get; set; }
        public string SqlAuthenticationType { get; set; }
        public bool SqlServerCreateDatabase { get; set; }



        public bool InstallSampleData { get; set; }
    }
}