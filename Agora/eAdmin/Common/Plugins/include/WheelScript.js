		//****** Common Argument ********************************//
		// id : client id (check box at datagrid item) generated by code behind (asp.net engine) eg MyDataGrid__ctl3_chkSelection				
		// pChkAllID : client id (check box at datagrid header) generated by code behind
		// pChkSelName : name of check box at datagrid item that assign by developer eg chkSelection
		//****** Argument ********************************//
		
		//Call by function CheckRow to return a TR
		function getParentObject(CurrObj,parentTag)
		{
			var tempObj = new Object;
			if (parentTag=="") 
				return null;
			else
			{
				tempObj=CurrObj;
				while (tempObj!=null && tempObj.tagName!=parentTag)
					tempObj = tempObj.parentElement;
				return tempObj;
			}
		}		

		//Call only if you want to display color if checked
		function checkRow(objo)
		{
			var tempObj = getParentObject(document.getElementById(objo),"TR");
			//debugger;
				if (document.getElementById(objo).checked==true)
				{	tempObj.style.backgroundColor = "#ccccdd";
					tempObj.style.color="green";
				}
				else
				{	tempObj.style.backgroundColor = "lightyellow";
					tempObj.style.color="black";
				}	
		}
		
		//called by checkBox in datagrid item
		function checkChildG(id,pChkAllID,pChkSelName) {
			var oform = document.forms[0];
			//checkRow(id);
			var chkAllBox = document.getElementById(pChkAllID); 
			CheckAll(chkAllBox,pChkSelName);
		}
				
		//called by "checkAll" checkbox (at datagrid header)
		function SelectAllG(pChkAllID,pChkSelName) {
			var oform = document.forms[0];
			var num = 0;
			var chkAllBox = document.getElementById(pChkAllID);
			CheckAllDataGridCheckBoxes(pChkSelName,chkAllBox.checked);
		}
	 
	 //unselect all include "checkall", to be called by reset button 
	  function DeselectAllG(pChkAllID,pChkSelName) {
			var oform = document.forms[0];
			var num = 0;
			var chkAllBox = document.getElementById(pChkAllID);
			chkAllBox.checked=false;
			CheckAllDataGridCheckBoxes(pChkSelName,chkAllBox.checked);
		}
	
	 //check/uncheck the "CheckAll" checkbox if all checkbiox in the datagrid is check/uncheck
	  function CheckAll(pChkAllID,pChkSelName)
	  {
			var oform = document.forms[0];
			re = new RegExp(':' + pChkSelName + '$')  //generated control name starts with a colon	
			for (var i=0;i<oform.elements.length;i++)
			{
				var e = oform.elements[i];
				if (e.type=="checkbox" && e.name != pChkAllID.name && re.test(e.name))
				{ if (e.checked==false) 
					{pChkAllID.checked=false;return;}}
			}
			pChkAllID.checked=true;
	  }
		
		//Check/uncheck all CheckBox in DataGrid only
	  function CheckAllDataGridCheckBoxes(pChkSelName, checkVal) {
			re = new RegExp(':' + pChkSelName + '$')  //generated control name starts with a colon
			for(i = 0; i < document.forms[0].elements.length; i++) {
				elm = document.forms[0].elements[i]
					if (elm.type == 'checkbox') { //alert (elm.name)
						if (re.test(elm.name)){  
							elm.checked = checkVal; //checkRow(elm.id);
					}
				}
			}
		}				
	
	 //check at least one checkbox is select (for datagrid item)	
	 //called by delete or modify button
	 function CheckAtLeastOne(pChkSelName,pButFunc){
		var oform = document.forms[0];
			re = new RegExp(':' + pChkSelName + '$')  //generated control name starts with a colon	
			for (var i=0;i<oform.elements.length;i++)
			{
				var e = oform.elements[i];
				if (e.type=="checkbox"){
					if (re.test(e.name) && e.checked==true)
				{
					    if (pButFunc=="unlock")
							return confirm('Are you sure that you want to unlock this item(s)?');
					   if (pButFunc=="activate")
							return confirm('Are you sure that you want to activate this item(s)?');
					   else if (pButFunc=="deactive")
							return confirm('Are you sure that you want to deactivate  this item(s)?');
					   else if (pButFunc=="delete")
							return confirm('Are you sure that you want to permanently delete this item(s)?');
					   else
							return true;
				}
				}
			}
			alert('Please make at least one selection!');
			return false;
	 }
	 
	  function CheckOnlyOne(pChkSelName){
		var oform = document.forms[0];
			re = new RegExp(':' + pChkSelName + '$')  //generated control name starts with a colon	
			var i;
			iCnt=0;
			for (var i=0;i<oform.elements.length;i++)
			{
				var e = oform.elements[i];
				if (e.type=="checkbox"){
					if (re.test(e.name) && e.checked==true)
				{
					iCnt+=1;//alert(iCnt);
					if (iCnt>1) {alert("Please choose only one selection!");return false;}
				}
			  }
			}
			if (iCnt==0){alert('Please make one selection!');return false;}
			else return true;
	 }
	 
	 function ValidatorReset() {	
		var oform = document.forms[0];
		oform.reset();	
		//alert(Page_Validators.length);	
		if (typeof(Page_Validators) != "undefined") {
			for (i = 0; i < Page_Validators.length; i++) {					
				Page_Validators[i].isvalid = true;	
				ValidatorUpdateDisplay(Page_Validators[i]);
				//Page_Validators[i].Text = '';
				//Page_Validators[i].errormessage = '';
			}
		}
		
		if (typeof(Page_ValidationSummaries) != "undefined") {
			summary = Page_ValidationSummaries[0];
			summary.style.display = "none";
		}
        
        //ValidationSummaryOnSubmit()	
	}
	
	function checkDocFile(fileType, fle)	{
		var strFile, ch, i, strlength;		
		var File = document.getElementById(fle);				
		
		strFile = File.value;
		//alert(getFileSize(strFile));		
		
		if (strFile != '')	{							
			strlength = strFile.length;														
			i = strFile.lastIndexOf('.');
			ch = strFile.substring(i + 1, strFile.length);
			ch = ch.toLowerCase();
			
			switch (fileType) {
				case 'doc':
					if ((ch=="bat" ) || (ch=="dll" ) || (ch=="exe" ) || (ch=="asp" )){
						alert("Cannot upload .bat, .dll, .exe and .asp file.");	
						File.focus;			
						return false;
					}		
					else
						return true;		
					break;
					
				case 'img':
					if ((ch!="gif" ) && (ch!="jpg" ) && (ch!="jpeg" )){
						alert("Only jpeg, jpg  and gif files.");
						File.focus;			
						return false;
					}		
					else
						return true;		
					break;
			}
		}
		else
			return true;		
	}
	
	function getFileSize (fileName) {
		if (document.layers) {
			if (navigator.javaEnabled()) {
				var file = new java.io.File(fileName);
				if (location.protocol.toLowerCase() != 'file:')
					netscape.security.PrivilegeManager.enablePrivilege('UniversalFileRead');
				return file.length();
			}
			else return -1;
		}
		else if (document.all) {
			window.oldOnError = window.onerror;
			window.onerror = function (err) {
				if (err.indexOf('utomation') != -1) {
					alert('file access not possible');
					return true;
				}
				else 
					return false;
			};
			var fso = new ActiveXObject('Scripting.FileSystemObject');
			var file = fso.GetFile(fileName);
			window.onerror = window.oldOnError;
			return file.Size;
		}
	}
	
	function limitText (textObj, maxCharacters){
		if (textObj.innerText.length >= maxCharacters){
			if ((event.keyCode == 13)||(event.keyCode == 32)||(event.keyCode >= 41 && event.keyCode <= 44) || (event.keyCode >= 47 && event.keyCode <= 126) || (event.keyCode >= 128 && event.keyCode <= 254))
				event.returnValue = false;
		}
		else {
			if (event.keyCode == 13){
				if (textObj.innerText.length + 2 >= maxCharacters){
					event.returnValue = false;
				}
			}
		}
	}
	
	function resetSummary(remarkCnt, blnLine)
	{				
		// blnLine : 1 - item line exists; 0 - else
		// remarkCnt : number of remark control for header
		var formValidate;	
		var oform = document.forms[0];								
		var blnRemark;
		var strRemark, strMsg, i, j, k, ctlName, strCode, ctlQ, strQ;		
		var s1, s2, s3, s4;		
		
		s1 = "<ul>";
        s2 = "<li>";
        s3 = "</li>";
        s4 = "</ul>";
        
        // rebind array    	
		var arrSummary1 = new Array();
		var arrSummary2 = new Array();
		arr1 = oform.hidSummary.value.split(',');			
		for (i= 0 ; i < arr1.length; i++) {
			arr2 = arr1[i].split('-');
			arrSummary1[i] = arr2[0];
			arrSummary2[i] = arr2[1];
		}		
		
		var arrCode1 = new Array();
		var arrCode2 = new Array();
		arrCode = oform.hidControl.value.split(',');
		for (i= 0 ; i < arrCode.length; i++) {
			arr3 = arrCode[i].split('-');
			arrCode1[i] = arr3[0];
			arrCode2[i] = arr3[1];
		}
		
		blnRemark = true;
		strMsg = '';
		
		for (i=0; i<arrSummary1.length;i++){			
			j = arrSummary2[i].indexOf('ctl');
			
			if (document.getElementById(arrSummary2[i]) != null) {
				strRemark = document.getElementById(arrSummary2[i]).value;	
				
				// j>0 if control in datagrid			
				if (j>1){
					ctlQ = document.getElementById(arrCode2[i - parseInt(remarkCnt)]); 
					
					if (strRemark.length > 400){
						blnRemark = false;
						if (parseInt(blnLine)==1){	
							strCode = document.getElementById(arrCode1[i - parseInt(remarkCnt)]).value;				
							strMsg += s2 + strCode + ". " + arrSummary1[i] + " is over limit." + s3;
						}
						else
							strMsg += s2 + arrSummary1[i] + " is over limit." + s3;
						ctlQ.value = "?";
					}	
					else
						ctlQ.value = "";
				}
				else{								
					if (strRemark.length > 1000){
						blnRemark = false;
						strMsg += s2 + arrSummary1[i] + " is over limit." + s3;
					}
				}				
			} 
		}		
		
		if (typeof(Page_Validators) == "undefined")
			formValidate = true;
		else	
			formValidate = Page_ClientValidate();	
				
		if (formValidate == false){			
			if (blnRemark == false){
				summary = Page_ValidationSummaries[0];
				summary.innerHTML = summary.innerHTML + s1 + strMsg + s4;											
			}
			Page_IsValid = false;
		}
		else{	
			if (blnRemark == true){						
				Page_IsValid = true;
			}
			else {						
				summary = Page_ValidationSummaries[0];
				summary.style.display = "";
				summary.innerHTML = s1 + strMsg + s4;
				Page_IsValid = false;
			}
		}		
		return Page_IsValid;
	}

	function PreviewAttach(fle)       
	{	
		var temp;
		if (fle != ""){	
			temp= fle;
			msg=window.open("","","Width=500,Height=400,resizable=yes,scrollbars=yes");
		 	msg.document.clear();
			msg.document.write('<HTML><HEAD><TITLE>Image Preview</TITLE></HEAD>'
			+ '<BODY><img src="' + temp + '"></img></BODY></HTML>');
		}
	}	

	function resetValue(id, val)
	{
		// id = control's client id
		// val = default value assigned to id
	
		var ctl, m;
		ctl = document.getElementById(id);
		m = ctl.value.match(/^\s*(\S+(\s+\S+)*)\s*$/);
		
		if (m == null)
			ctl.value = val;
	}