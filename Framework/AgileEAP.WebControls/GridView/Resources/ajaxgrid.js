function getFilterValues(gridID){var filterValues=new Object();var inputs=$("input[id^='filter']");inputs.each(function(){var property=this.id.substr(6);if($(this).attr("tag")=="choosebox"){if(endWith(property,"data")){filterValues[property.substr(0,property.length-4)]=$(this).val();}}
else if($(this).attr("tag")=="combox"){if(endWith(property,"data")){var enumValue=parseInt($(this).val());if(!enumValue){enumValue=$(this).val();}
filterValues[property.substr(0,property.length-4)]=enumValue;}}
else{filterValues[property]=$(this).val();}});filterValues["PageIndex"]=parseInt($("#"+gridID+"_container").find("#txtpageindex").val());filterValues["PageSize"]=parseInt($("#"+gridID+"_container").find("#txtpagesize").val());filterValues["PageCount"]=parseInt($("#"+gridID+"_container").find("#pageCount").text());return filterValues;}
function getCurrentUrl(){return document.URL.split('#')[0];}
function getNewPageIndex(pageIndex,pageCount,action){var newPageIndex=1;if(action=="Previous"){newPageIndex=Math.max(pageIndex-1,1);}
else if(action=="Next"){newPageIndex=Math.min(pageIndex+1,pageCount);}
else if(action=="Last"){newPageIndex=pageCount}
return newPageIndex;}
function setPageInfo(gridID,pageInfo){$("#"+gridID+"_container").find("#pageCount").text(pageInfo.PageCount);$("#"+gridID+"_container").find("#recordCount").text(pageInfo.ItemCount);$("#"+gridID+"_container").find("#txtpageindex").val(pageInfo.PageIndex);$("#"+gridID+"_container").find("#txtpagesize").val(pageInfo.PageSize);}
function ajaxSearchGrid(gridID,action){var filterValues=getFilterValues(gridID);filterValues.Action=action;var pageCount=filterValues.PageCount;if(action!="Query"&&pageCount==filterValues.PageIndex&&((filterValues.PageIndex>1&&action!="First"&&action!="Previous")||filterValues.PageIndex==1)){return;}
filterValues.PageIndex=getNewPageIndex(filterValues.PageIndex,pageCount,action);var orderSpan=$("#"+gridID).find("th").find("span:first");var field=orderSpan.parent().attr("field");if(field&&field!=""){var orderFlag=orderSpan.text();orderFlag=orderFlag==""?"▲":orderFlag;filterValues.SortExpression="Order By "+field+(orderFlag!="▼"?" Asc ":" Desc ");}
var loadbar=$("#loadingBar").loadBar({message:'正在查询...'});$.post(getCurrentUrl(),{AjaxAction:gridID+"AjaxSearch",AjaxArgument:JSON2.stringify(filterValues)},function(value){try{var ajaxResult=JSON2.parse(value);if(ajaxResult&&ajaxResult.ActionResult==1){$("#"+gridID).html(ajaxResult.RetValue);setPageInfo(gridID,ajaxResult.Tag)
if(field&&field!=""){var th=$("#"+gridID).find("th[field='"+field+"']");th.html("<span>"+orderFlag+"</span>"+th.text());}}
else{alert("系统出错，请与管理员联系！");}}catch(e){}
loadbar.complete();});}
function onGridSort(gridID,field){var filterValues=getFilterValues(gridID);var pageCount=filterValues.PageCount;if(pageCount==filterValues.PageIndex&&filterValues.PageIndex==1){return;}
var th=$("#"+gridID).find("th[field='"+field+"']");var orderSpan=th.find("span:first");var orderFlag=orderSpan.text();orderFlag=orderFlag!="▲"?"▲":"▼";var sortExpression="Order By "+field+(orderFlag=="▲"?" Asc ":" Desc ");filterValues.Action="Sort";filterValues.PageIndex=1;filterValues.SortExpression=sortExpression;var loadbar=$("#loadingBar").loadBar({message:'正在对'+th.text()+'排序...'});$.post(getCurrentUrl(),{AjaxAction:gridID+"AjaxSearch",AjaxArgument:JSON2.stringify(filterValues),AjaxSort:"true"},function(value){try{var ajaxResult=JSON2.parse(value);if(ajaxResult&&ajaxResult.ActionResult==1){$("#"+gridID).html(ajaxResult.RetValue);setPageInfo(gridID,ajaxResult.Tag);th=$("#"+gridID).find("th[field='"+field+"']");th.html("<span>"+orderFlag+"</span>"+th.text());}
else{alert("系统出错，请与管理员联系！");}}catch(e){}
loadbar.complete();});}
function setDisplayFields(gridID){}
function setFilterFields(gridID){}
function exportExcel(gridID,action){var filterValues=getFilterValues(gridID);filterValues.action=action;var loadbar=$("#loadingBar").loadBar({message:'正在导出...'});$.post(getCurrentUrl(),{AjaxAction:"ExportExcel",AjaxArgument:JSON2.stringify(filterValues)},function(value){var ajaxResult=JSON2.parse(value);if(ajaxResult&&ajaxResult.ActionResult==1){window.open(ajaxResult.RetValue,"_blank");}
else{alert("系统出错，请与管理员联系！");}
loadbar.complete();});}
function exportCsv(gridID,action){var filterValues=getFilterValues(gridID);filterValues.action=action;var loadbar=$("#loadingBar").loadBar({message:'正在导出...'});$.post(getCurrentUrl(),{AjaxAction:"ExportCsv",AjaxArgument:JSON2.stringify(filterValues)},function(value){var ajaxResult=JSON2.parse(value);if(ajaxResult&&ajaxResult.ActionResult==1){window.open(ajaxResult.RetValue,"_blank");}
else{alert("系统出错，请与管理员联系！");}
loadbar.complete();});}
var currentRow=null;var oldRow=null;function rowClick(me,gridID){if(currentRow){currentRow.className="gridview_row";}
me.className="rowcurrent";currentRow=me;$("input:radio",me).attr("checked",true);}
function rowOver(row){if(oldRow==null){oldRow=row;row.className="rowover";}
else{oldRow=row;if(currentRow!=row)
row.className="rowover";}}
function rowOut(me){if(oldRow!=null){if(currentRow!=me)
me.className="rowout";oldRow=me;}}
function rowDbClick(me,gridID){}