       
<%
RepCount = session("oRpt").Database.Tables.COUNT
	For ILoop = 1 to repcount 
		
		set crtable = session("oRpt").Database.Tables.Item(Iloop)
		crtable.SetLogonInfo "10.228.210.15","eProcure","sa", "manager01"			
	Next	
	SecCount = Session("oRpt").Sections.count
		
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
					crtable.SetLogonInfo "10.228.210.15","eProcure","sa", "manager01"			
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

