function openChooseDialog(title, url, width, height) {
    var appendSign = url.indexOf("?") > 0 ? "&" : "?";
    url = url + appendSign + "random=" + Math.random();

    var cssDialog = "dialogHeight:" + height + "px; dialogWidth:" + width + "px; edge: Raised; center: Yes; resizable: Yes; status: No;scrollbars=no,";
    return window.showModalDialog(url, title, cssDialog);
}
function openChooseBoxDialog(clientId,id,title, url, width, height)
{
   var dlgDetail = openChooseDialog(title, url, width, height) ;
   if(dlgDetail != null)
   {
      $("#"+clientId).val(dlgDetail.text);
      $("#" +id + "data").val(dlgDetail.value);
      $("#" +id + "tag").val(dlgDetail.tag);
   }
} 