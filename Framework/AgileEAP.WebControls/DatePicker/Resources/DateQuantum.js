
function checkupvalue(start,end,checkstart)
{
    var isOk=true;
    if(start.value!='' && end.value !='')
    {
      if(checkstart=='start'){ 
         if(!isValidDate(start.value))
         {
           alert('格式出错');
           start.value = '' ;
           isOk=false;
         }
       }else{
         if(!isValidDate(end.value))
         {
           alert('格式出错');
           end.value = '' ;
           isOk=false;
         }
       } 
    if(isOk && start.value > end.value){         
         if(checkstart=='start'){ 
           alert('开始日期不能大于结束日期');
           start.value = end.value;
         }else{
           alert('结束日期不能小于开始日期');
           end.value = start.value;
          }
      }    
   }
}

function isValidDate(strDate)
{
	var ls_regex = "^((((((0[48])|([13579][26])|([2468][048]))00)|([0-9][0-9]((0[48])|([13579][26])|([2468][048]))))-02-29)|(((000[1-9])|(00[1-9][0-9])|(0[1-9][0-9][0-9])|([1-9][0-9][0-9][0-9]))-((((0[13578])|(1[02]))-31)|(((0[1,3-9])|(1[0-2]))-(29|30))|(((0[1-9])|(1[0-2]))-((0[1-9])|(1[0-9])|(2[0-8]))))))$";
	var exp = new RegExp(ls_regex, "i");
	return exp.test(strDate);
}