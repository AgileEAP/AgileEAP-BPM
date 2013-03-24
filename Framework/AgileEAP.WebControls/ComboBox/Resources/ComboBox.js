/// <reference path="jquery-1.3.2-vsdoc.js" />

//====================================================================================================
//----------------------------------------------------------------------------------------------------
// [描    述] ComboBox.js脚本
//            
//
//----------------------------------------------------------------------------------------------------
// [作者] 	    chenxiaoxi
// [日期]       2009-10-13
//++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// [历史记录]
// [作者]  
// [描    述]     
// [更新日期]
// [版 本 号] ver1.0
//++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
//====================================================================================================
var isFirst=true;

function ontextchange(text,drop)
{
    var len=drop.options.length;
    var isIn=false;
    for(var i=0;i<len;i++)
    {
        if(text.value==drop.options[i].value)
        {
            isIn=true;
            break;
         }
    }
    if(!isIn)
    {
        if(isFirst)
        {
            var opt=document.createElement("option");
            opt.value=text.value;
            opt.text=text.value;
            drop.options.add (opt);
            drop.selectedIndex=len;
            isFirst=false;
        }
        else
        {
            drop.options[len-1].value=text.value;
            drop.options[len-1].text=text.value;
            drop.selectedIndex=len-1;
        }
    }
}
function ontextblur(text,drop)
{
    drop.selectedIndex=drop.options.length-1;
}
function ondropchange(text,drop)
{
    text.value=drop.options[drop.selectedIndex].text;
}

 	
function showTip(obj)
{  
   clearTimeout(obj.timeout);
           obj.timeout = setTimeout(function(){
                obj.style.display='block';
            },200);
}

function hideTip(obj)
{ 
  clearTimeout(obj.timeout);
            obj.style.display='none';
}

function ondivchange(drop,obj)
{ 
  obj.innerHTML =drop.options[drop.selectedIndex].text;
}