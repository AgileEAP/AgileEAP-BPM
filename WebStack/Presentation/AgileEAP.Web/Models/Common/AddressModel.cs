using System.Collections.Generic;
using System.Web.Mvc;
using FluentValidation.Attributes;
using AgileEAP.MVC;
using AgileEAP.Web.Validators.Common;

namespace AgileEAP.Web.Models.Common
{
    [Validator(typeof(AddressValidator))]
    public class AddressModel : AgileEAPEntityModel
    {
        public AddressModel()
        {
            AvailableCountries = new List<SelectListItem>();
            AvailableStates = new List<SelectListItem>();
        }

        [AgileEAPResourceDisplayName("Address.Fields.FirstName")]
        [AllowHtml]
        public string FirstName { get; set; }
        [AgileEAPResourceDisplayName("Address.Fields.LastName")]
        [AllowHtml]
        public string LastName { get; set; }
        [AgileEAPResourceDisplayName("Address.Fields.Email")]
        [AllowHtml]
        public string Email { get; set; }
        [AgileEAPResourceDisplayName("Address.Fields.Company")]
        [AllowHtml]
        public string Company { get; set; }
        [AgileEAPResourceDisplayName("Address.Fields.Country")]
        public int? CountryId { get; set; }
        [AgileEAPResourceDisplayName("Address.Fields.Country")]
        [AllowHtml]
        public string CountryName { get; set; }
        [AgileEAPResourceDisplayName("Address.Fields.StateProvince")]
        public int? StateProvinceId { get; set; }
        [AgileEAPResourceDisplayName("Address.Fields.StateProvince")]
        [AllowHtml]
        public string StateProvinceName { get; set; }
        [AgileEAPResourceDisplayName("Address.Fields.City")]
        [AllowHtml]
        public string City { get; set; }
        [AgileEAPResourceDisplayName("Address.Fields.Address1")]
        [AllowHtml]
        public string Address1 { get; set; }
        [AgileEAPResourceDisplayName("Address.Fields.Address2")]
        [AllowHtml]
        public string Address2 { get; set; }
        [AgileEAPResourceDisplayName("Address.Fields.ZipPostalCode")]
        [AllowHtml]
        public string ZipPostalCode { get; set; }
        [AgileEAPResourceDisplayName("Address.Fields.PhoneNumber")]
        [AllowHtml]
        public string PhoneNumber { get; set; }
        [AgileEAPResourceDisplayName("Address.Fields.FaxNumber")]
        [AllowHtml]
        public string FaxNumber { get; set; }

        public IList<SelectListItem> AvailableCountries { get; set; }
        public IList<SelectListItem> AvailableStates { get; set; }

        public bool FirstNameDisabled { get; set; }
        public bool LastNameDisabled { get; set; }
        public bool EmailDisabled { get; set; }
        public bool CompanyDisabled { get; set; }
        public bool CountryDisabled { get; set; }
        public bool StateProvinceDisabled { get; set; }
        public bool CityDisabled { get; set; }
        public bool Address1Disabled { get; set; }
        public bool Address2Disabled { get; set; }
        public bool ZipPostalCodeDisabled { get; set; }
        public bool PhoneNumberDisabled { get; set; }
        public bool FaxNumberDisabled { get; set; }
    }
}