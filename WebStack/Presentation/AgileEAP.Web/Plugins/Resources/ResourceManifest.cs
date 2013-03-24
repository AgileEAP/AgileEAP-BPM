using AgileEAP.UI.Resources;

namespace AgileEAP.Plugin.Resources
{
    public class ResourceManifest : IResourceManifestProvider
    {
        public void BuildManifests(ResourceManifestBuilder builder)
        {
            var manifest = builder.Add();
            manifest.DefineScript("jQuery").SetUrl("jquery-1.8.1.min.js", "jquery-1.8.1.js").SetVersion("1.8.1").SetCompress(false); ;
            manifest.DefineScript("jQueryUI").SetUrl("jquery-ui-1.9.2.custom.min.js", "jquery-ui-1.9.2.custom.js").SetVersion("1.9.2").SetCompress(false); ;
            manifest.DefineScript("jQueryValidate").SetUrl("jquery.validate.js", "jquery.validate.js").SetVersion("1.9.0");
            manifest.DefineScript("jQueryContextMenu").SetUrl("jquery.contextMenu.js").SetVersion("1.0");
            manifest.DefineScript("jqueryquery").SetUrl("jquery.query.js");
            // Utils
            //manifest.DefineScript("jQueryUtils").SetUrl("jquery.utils.js").SetDependencies("jQuery");
            //manifest.DefineScript("jQueryUtils_TimePicker").SetUrl("ui.timepickr.js").SetVersion("0.7.0a").SetDependencies("jQueryUtils", "jQueryUI_Core", "jQueryUI_Widget");

            // site
            manifest.DefineScript("cookie").SetUrl("Cookie.js");
            manifest.DefineScript("json2").SetUrl("Json2.js");
            manifest.DefineScript("processbar").SetUrl("jquery.processbar/processbar.js");
            manifest.DefineScript("tab").SetUrl("Tab.js");
            manifest.DefineScript("dialog").SetUrl("Dialog.js").SetDependencies("jQueryUI");
            manifest.DefineScript("site").SetUrl("site.js").SetDependencies("jQuery", "jqueryquery", "cookie", "json2");
            manifest.DefineScript("dateFormat").SetUrl("dateFormat.js");

            //formwizard
            manifest.DefineScript("jquerybbq").SetUrl("jquery.formwizard/bbq.js");
            manifest.DefineScript("jqueryForm").SetUrl("jquery.formwizard/jquery.form.js");
            manifest.DefineScript("formwizard").SetUrl("jquery.formwizard/jquery.form.wizard-3.0.5.min.js", "jquery.formwizard/jquery.form.wizard-3.0.5.js").SetVersion("3.0.5").SetDependencies("jqueryForm", "jQueryValidate", "jquerybbq").SetCompress(false);

            //progressbar
            manifest.DefineScript("progressbar").SetUrl("jquery.progress/jquery.progressbar.js").SetVersion("1.0.5");

            manifest.DefineStyle("base").SetUrl("base.css").SetVersion("1.0");
            manifest.DefineStyle("smoothnessjQueryUI_default").SetUrl("Themes/Default/jqueryUI/smoothness/jquery-ui-1.9.2.custom.css").SetVersion("1.9.2");
            
            manifest.DefineStyle("jQueryUI_default").SetUrl("Themes/Default/jqueryUI/jquery-ui.custom.css").SetVersion("1.8.20");
            manifest.DefineStyle("site_default").SetUrl("Themes/Default/site.css").SetDependencies("base").SetVersion("1.0");
            manifest.DefineStyle("layout_default").SetUrl("Themes/Default/layout.css").SetVersion("1.0");
            manifest.DefineStyle("toggle").SetUrl("Themes/Default/toggle.css").SetVersion("1.0");
            manifest.DefineStyle("tab_default").SetUrl("Themes/Default/tab.css").SetDependencies("toggle").SetVersion("1.0");
            manifest.DefineStyle("jQueryContextMenu_default").SetUrl("Themes/Default/jqueryUI/jquery.contextMenu.css").SetVersion("1.0");
            //manifest.DefineStyle("jQueryUI_DatePicker").SetUrl("ui.datepicker.css").SetDependencies("jQueryUI_Orchard").SetVersion("1.7.2");
            //manifest.DefineStyle("jQueryUtils_TimePicker").SetUrl("ui.timepickr.css");


            //highcharts
            //manifest.DefineScript("exporting").SetUrl("exporting/highcharts.js").SetDependencies("jQuery").SetVersion("1.0");
            //manifest.DefineScript("highcharts").SetUrl("highcharts/highcharts.js").SetDependencies("exporting").SetVersion("1.0");
            manifest.DefineScript("exporting").SetUrl("highcharts/exporting.src.js").SetVersion("1.0");
            manifest.DefineScript("highcharts").SetUrl("highcharts/highcharts.src.js").SetVersion("1.0");

            //dragjs
            manifest.DefineScript("dragjs").SetUrl("drag/drag.js").SetVersion("1.0");//.SetDependencies("jQuery");
            manifest.DefineStyle("drag_default").SetUrl("Themes/Default/drag/drag.css").SetVersion("1.0");

            //Wizard
            manifest.DefineScript("Wizardjs").SetUrl("Wizard/jquery.smartWizard-2.0.js").SetVersion("1.0");//.SetDependencies("jQuery");
            manifest.DefineStyle("Wizard_default").SetUrl("Themes/Default/Wizard/smart_wizard.css").SetVersion("1.0");
            //colorpicker
            manifest.DefineScript("colorpickerjs").SetUrl("colorpicker/colorpicker.js").SetVersion("1.0");//.SetDependencies("jQuery");
            manifest.DefineStyle("colorpicker_default").SetUrl("Themes/Default/colorpicker/colorpicker.css").SetVersion("1.0");
            //jsplumb
            manifest.DefineScript("jq_jsPlumb").SetUrl("jsplumb/jsPlumb-1.3.14-all-min.js", "jsplumb/jsPlumb-1.3.14-all.js").SetVersion("1.0").SetCompress(false);

            //jQuery-Validation-Engine-master
            manifest.DefineScript("ValidationEnginezh_CNjs").SetUrl("jQuery-Validation-Engine-master/jquery.validationEngine-zh_CN.js").SetVersion("1.0");//.SetDependencies("jQuery");
            manifest.DefineScript("ValidationEnginejs").SetUrl("jQuery-Validation-Engine-master/jquery.validationEngine.js").SetVersion("1.0");//.SetDependencies("jQuery");
            manifest.DefineStyle("ValidationEngine_default").SetUrl("Themes/Default/jquery.validationEngine/validationEngine.jquery.css").SetVersion("1.0");
           
            //angularjs
          //  manifest.DefineScript("angularjs").SetUrl("angularjs/angular-1.0.1.min.js", "angularjs/angular-1.0.1.js").SetVersion("1.0").SetCompress(false);
            manifest.DefineScript("angularjs").SetUrl("angularjs/angular.min.js","angularjs/angular-1.1.2.js").SetVersion("1.0").SetCompress(false);
            //konckoutjs
            manifest.DefineScript("knockOut").SetUrl("knockOut/KnockOut.js").SetVersion("1.0").SetCompress(false);

            //kindeditor
            manifest.DefineScript("kindeditorjs").SetUrl("kindeditor/kindeditor-min.js", "kindeditor/kindeditor.js").SetVersion("1.0").SetCompress(false);

            //ckeditor.js
            manifest.DefineScript("ckeditorjs").SetUrl("ckeditor/ckeditor.js").SetVersion("1.0").SetCompress(false);
            //old site css and script
            #region old site
            manifest.DefineStyle("datepicker_default").SetUrl("Themes/Default/jqueryUI/DatePicker/WdatePicker.css").SetVersion("1.0").SetCompress(false);
            manifest.DefineStyle("oldsite_default").SetUrl("Themes/Default/oldsite.css").SetDependencies("base", "jQueryUI_default", "datepicker_default").SetVersion("1.0");
            manifest.DefineStyle("oldtab_default").SetUrl("Themes/Default/oldtab.css").SetDependencies("toggle").SetVersion("1.0");

            manifest.DefineScript("datepicker").SetUrl("datePicker/WdatePicker.js").SetVersion("1.0").SetCompress(false);
            manifest.DefineScript("gridview").SetUrl("gridview.js");
            manifest.DefineScript("jqueryFormValidatorRegex").SetUrl("jquery.formValidator/formValidatorRegex.js");
            manifest.DefineScript("jqueryFormValidator").SetUrl("jquery.formValidator/formValidator.js").SetDependencies("jqueryFormValidatorRegex");
            manifest.DefineScript("correctpng").SetUrl("correctpng.js");
            manifest.DefineScript("oldsite").SetUrl("site.js").SetDependencies("cookie", "jQuery", "json2", "jqueryquery", "jQueryUI", "dialog", "correctpng", "datepicker", "processbar");
            #endregion

            manifest.DefineScript("upload_widget").SetUrl("jQuery-File-Upload/js/vendor/jquery.ui.widget.js").SetVersion("1.0").SetCompress(false);
            manifest.DefineScript("upload_transport").SetUrl("jQuery-File-Upload/js/jquery.iframe-transport.js").SetVersion("1.0").SetCompress(false);
            manifest.DefineScript("upload_fileupload").SetUrl("jQuery-File-Upload/js/jquery.fileupload.js").SetVersion("1.0").SetCompress(false);
            manifest.DefineStyle("jqueryfileupload_default").SetUrl("Themes/Default/upload/jquery.fileupload-ui.css").SetVersion("1.0");

            manifest.DefineScript("uploadify").SetUrl("uploadify/jquery.uploadify-3.1.min.js").SetVersion("1.0").SetCompress(false);
            manifest.DefineStyle("uploadify_default").SetUrl("Themes/Default/uploadify/uploadify.css").SetVersion("1.0");
       
        //jstree
            manifest.DefineScript("jstree").SetUrl("jstree/jquery.jstree.js").SetVersion("1.0").SetCompress(false);
            manifest.DefineStyle("jstree_default").SetUrl("Themes/Default/jstree/themes/default-rtl/style.css").SetVersion("1.0");
       //kedoUI
            manifest.DefineScript("jskendo").SetUrl("kendo/2012.3.1114/kendo.web.min.js").SetVersion("1.0").SetCompress(false);
            manifest.DefineStyle("kendoCommon_default").SetUrl("kendo/2012.3.1114/kendo.common.min.css").SetVersion("1.0");
            manifest.DefineStyle("kendoDefault_default").SetUrl("kendo/2012.3.1114/kendo.default.min.css").SetVersion("1.0");
        } 
    }
}