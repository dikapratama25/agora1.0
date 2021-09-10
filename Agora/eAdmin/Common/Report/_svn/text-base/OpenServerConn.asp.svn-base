       
<%
RepCount = session("oRpt").Database.Tables.COUNT
	For ILoop = 1 to repcount 
		
		set crtable = session("oRpt").Database.Tables.Item(Iloop)
		'Response.Write crtable.NAME & "<BR>"
		'Response.Write crtable.DLLNAME& "<BR>"
		'Response.Write crtable.LOgOnServerName& "<BR>"
		
		'crtable.SetLogonInfo "SSB",, "HOSmgr", "HOSmgr"
		'crtable.SetLogonInfo "Wheel_db","Wheel", "sa", "manager01"
		crtable.SetLogonInfo "10.228.210.15","eProcure","sa", "manager01"
		'crtable.SetLogonInfo "SSB",, "wsmsmgr", "wsmsmgr"
		'crtable.SetTableLocation crtable.LOCATION,"",""
		
		'//For ODBC
		' With crtable.ConnectionProperties
		'	.Item("user ID") = "sa"
		'	.Item("Password") = "manager01"
		'	.Item("DSN") = "Pubs Sample Database"
		'	.Item("Database") ="Wheel_Conversion"
		'End With
		
		'//For ODBC
		'Set CPProperties = crtable.ConnectionProperties("Data Source")
		'response.Write CPProperties.Value & "<BR>"
		'CPProperties.Value="SSB"
		
		'Set CPProperties = crtable.ConnectionProperties("User ID")
		'response.Write CPProperties.Value & "<BR>"
		'CPProperties.Value="wsmsmgr" & "<BR>"
		'response.Write CPProperties.Value
		
		'Set CPProperties = crtable.ConnectionProperties("Password")
		'response.Write CPProperties.Value & "<BR>"
		'CPProperties.Value="wsmsmgr"
		
		'Set CPProperties = crtable.ConnectionProperties("Table Name")
		'response.Write CPProperties.Value & "<BR>"
		'crtable.SetTableLocation crtable.NAME,"",""
		'Response.Write crtable.TestConnectivity
		
	Next
	
	sql= session("oRpt").SQLQueryString 
	sql=Replace(sql,"Wheel_Conversion","Wheel")
	session("oRpt").SQLQueryString = CStr(sql)
	
	SecCount = Session("oRpt").Sections.count
	
	'Response.Write session("oRpt").SQLQueryString
	'Response.end
	For iLoop1 = 1 to SecCount
		set crSection =  Session("oRpt").Sections.Item(cint(iLoop1))
		RepObjCount = crSection.ReportObjects.count
		For j=1 to RepObjCount
			set crRepObj = crSection.ReportObjects.item(cint(j))
			IF crRepObj.kind=5 then 'subreport
				set crSubRepObj=crRepObj
				set crSubRep = crSubRepObj.OpenSubReport
				RecCount = crSubRep.Database.Tables.COUNT
				For Z = 1 to RecCount	
					set crtable = crSubRep.Database.Tables.Item(Z)
					'crtable.SetLogonInfo "SSB",, "HOSmgr", "HOSmgr"
					'crtable.SetLogonInfo "10.228.210.15","Wheel_Conversion","sa", "manager01"
					crtable.SetLogonInfo "10.228.210.15","eProcure", "sa", "manager01"
					crtable.SetTableLocation crtable.LOCATION,"",""
				Next
			End if
		Next
	Next

	
'session("oRpt").LogonServer "pdsoledb.dll",,"LVISKA", "LVISKADBA","LVISKAsb2w"
'set crtable = session("oRpt").Database.Tables.Item(1)
'crtable.SetLogonInfo "LVISKA",, "LVISKADBA", "LVISKAsb2w"
'crtable.SetTableLocation crtable.LOCATION,"",""

%>

