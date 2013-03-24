function comboxExpand(id){$("[id^='combox']").each(function(i){$(this).hide();});$("#"+id).show();}
function comboxChooseItem(srcObject,id){var ev=getEvent();if(id){$("#"+id).val($(srcObject).text());$("#"+id+'data').val($(srcObject).attr('data'));$("#combox"+id).hide();}
else{var eObj=ev.srcElement||ev.target;if(eObj.tagName=="INPUT")return false;var chk=$(srcObject).find("input:first");chk.attr("checked",chk.attr("checked")==false);}
return false;}
function comboxDeSelect(id){var oo=document.getElementById("combox"+id).getElementsByTagName('input');for(var k=0,len=oo.length;k<len;k++){oo[k].checked=!oo[k].checked;}}
function comboxSelectAll(id){var oo=document.getElementById("combox"+id).getElementsByTagName('input');for(var k=0,len=oo.length;k<len;k++){oo[k].checked=true;}
document.getElementById(id).value='全部';document.getElementById(id+"data").value='-1';document.getElementById("combox"+id).style.display='none';}
function comboxGetSelected(id){var oo=document.getElementById("combox"+id).getElementsByTagName('input');var strValue='';var strText='';var isAll=true;for(var k=0,len=oo.length;k<len;k++){var t=oo[k].type;if(t=='checkbox'&&oo[k].checked==true){strValue+=oo[k].value+',';strText+=oo[k].nextSibling.nodeValue+',';}
if(t=='checkbox'&&oo[k].checked!=true){isAll=false;}}
if(isAll==false){if(strText.length>1){strText=strText.substring(0,strText.length-1);}
document.getElementById(id).value=strText.length>10?strText.substring(0,10)+"...":strText;document.getElementById(id+'data').value=strValue;}else{document.getElementById(id).value='全部';document.getElementById(id+'data').value='-1'}
document.getElementById("combox"+id).style.display='none';}
function getELXY(e){return{x:e.offsetLeft,y:e.offsetTop};}
function getELWH(e){return{w:e.offsetWidth,h:e.offsetHeight};}
function getClientXY(e){e=e||event;return{cx:e.clientX,cy:e.clientY};}
function stopBubble(e){if(e&&e.stopPropagation)
e.stopPropagation();else
window.event.cancelBubble=true;}
document.onclick=function(e){var ev=e||window.event;var srcObj=ev.target||ev.srcElement;if($(srcObj).attr("tag")!="combox"){$(".cbList").each(function(i){var obj=this;var lt=getELXY(obj)['x'];var rt=getELXY(obj)['x']+getELWH(obj)['w'];var topY=getELXY(obj)['y'];var bottomY=getELXY(obj)['y']+getELWH(obj)['h'];var mouseXX=getClientXY(e)['cx'];var mouseYY=getClientXY(e)['cy'];if(obj.style.display!="none"&&(mouseXX<lt||mouseXX>rt||mouseYY<topY||mouseYY>bottomY)){obj.style.display="none";try{comboxGetSelected(obj);}
catch(e){}}});}};