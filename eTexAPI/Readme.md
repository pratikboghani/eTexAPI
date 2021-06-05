1) Global Company Select : /Glb/CompSel?int mfg
2) Sale Order
	2.1)view : /saleorder/view?int mfg & int ycode & string status & string fdate & string tdate
	2.2)Combos : /SaleOrder/CmbFill
	2.3)Combos in L entry : /SaleOrder/CmbFillItem?string icat
	2.4)New OrdNo : /saleorder/newordno?int mfg=1 & int ycode=1
	2.5)HSave : SaleOrder/HSave	
			{	
				"MfgUnit":2,
				"YCode"	:4,
				"OrdNo"	:1,
				"ICat"	:"GRAY",
				"TrnDate":"2021-05-18T18:25:43.511Z" or "2021-05-18",
				"AC_Code":51,
				"BAC_Code":21,
				"ChkName" :"RAMKISHAN",
				"ChkMobNo":"5858585858",
				"PayTerms":0,
				"Remark":"zz",
				"PartyOrderNo":"",
				"OrderStatus":"-",
				"IUser":"ADMIN"
			}
	2.5)LSave: SaleOrder/LSave
			{
			"MfgUnit":2,
			"YCode"  :4,
			"OrdNo"  :1,
			"DetNo"  :1,
			"ICode"  :1,
			"Lot"	 :2,
			"Pcs"	 :24,
			"Meter"	 :2760,
			"Rate"	 :12,
			"Unit"	 :"METER",
			"Amt"	 :33120,
			"IGSTPer":0,
			"SGSTPer":2.5,
			"CGSTPer":2.5,
			"IGSTAmt":0,
			"SGSTAmt":828,
			"CGSTAmt":828			
			}
	2.7)EmailProfiledata : http://localhost:49203/api/glb/emailprofile
	2.8)getNewMsgLogID : http://localhost:49203/api/glb/NewMsgLogId
	2.9)MsgLogSave : http://localhost:49203/api/glb/MsgLogSave
			{
			"LogID":4, 
			"LogWp":1, 
			"LogMail":1,
			"LogBrok":1,
			"LogParty":1,
			"FormType":"a",
			"FormKey":"b",
			"IUser":"ADMIN"
			}