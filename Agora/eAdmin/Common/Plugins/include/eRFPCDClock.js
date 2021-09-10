<!-- Countdown in Java Script ..
speed=1000;
len=40;
tid = 0;
num=0;
clockA = new Array();
timeA = new Array();
formatA = new Array();
dd = new Date();
var d,x;
function doDate(x)
{
  for (i=0;i<num;i++) {

	var curDateInc = parseInt(clockA[i].hdn_CurDateInc.value);  
	var curTime = clockA[i].hdn_CurTime.value.split("/");
	//get the closing date
	clockA[i].hdn_CurDateInc.value = curDateInc + 1000;  
	// get the server's time
	dt = new Date(Date.UTC(parseInt(curTime[0]),parseInt(curTime[1]),parseInt(curTime[2]),parseInt(curTime[3]),parseInt(curTime[4]),parseInt(curTime[5]))+ curDateInc);

	//dt = new Date();
    if (timeA[i] != 0) {
      v1 = Math.round(( timeA[i] - dt )/1000) ;
      if (v1 < 0)
        clockA[i].date.value = "**BANG!**";
      if (formatA[i] == 1)
        clockA[i].date.value = v1;
      else if (formatA[i] ==2) {
        sec = v1%60;

	v1 = Math.floor( v1/60);
	min = v1 %60 ;
	hour = Math.floor(v1 / 60);
	if (sec < 10 ) sec = "0"+sec;
	if (min < 10 ) min = "0"+min;
        clockA[i].date.value = hour+"h "+min+"m "+sec+"s";
        }
      else if (formatA[i] ==3) {
        sec = v1%60;
	v1 = Math.floor( v1/60);
	min = v1 %60 ;
	v1 = Math.floor(v1 / 60);
	hour = v1 %24 ;
	day = Math.floor(v1 / 24);
	if (sec < 10 ) sec = "0"+sec;
	if (min < 10 ) min = "0"+min;
	if (hour < 10 ) hour = "0"+hour;
        clockA[i].date.value = day+"d "+hour+"h "+min+"m "+sec+"s";
        }
      else if (formatA[i] ==4 ) {
        sec = v1%60;
	v1 = Math.floor( v1/60);
	min = v1 %60 ;
	v1 = Math.floor(v1 / 60);
	hour = v1 %24 ;
	day = Math.floor(v1 / 24);
        clockA[i].date.value = day+(day<=1?":Day ":":Days ")+hour+(hour<=1?":Hour ":":Hours ")+min+(min<=1?":Minute ":":Minutes ")+sec+(sec<=1?":Second ":":Seconds ")
        }
      else
        clockA[i].date.value = "Invalid Format spec";
      }
    else
      clockA[i].date.value = "Countdown till when?";
    }

  tid=window.setTimeout("doDate()",speed);
}

function start(d,x,format) {


  clockA[num] = x
  timeA[num] = new Date(d);
  formatA[num] = format;
//window.alert(timeA[num]+":"+d);
  if (num == 0)  
    tid=window.setTimeout("doDate()",speed);
  num++;

}


function CountdownLong(t,ct,format,len,formtitle)
{
  document.write('<table  bgcolor="lightyellow" width="100%" align="left" ><tr>');
  document.write('<td bgcolor="lightyellow" >&nbsp;<input type="hidden" name="hdn_CurDateInc" value="0" ><input type="hidden" name="hdn_CurTime" value='+ ct +'><input name=date size=')
  document.write(len)
  document.write(' style="border=0;font-weight:bold;font-size:14;BACKGROUND-COLOR:lightyellow" value="eBiz"></td></tr></table>')
  start(t,document.forms['Form1'],format);
}


function Countdown(t,ct,formtitle)
{
  CountdownLong(t,ct,4,50,formtitle);
}
