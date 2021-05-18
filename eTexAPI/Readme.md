1) Global Company Select : /Glb/CompSel?int mfg
2) Sale Order
	i)view : /saleorder/view?int mfg & int ycode & string status & string fdate & string tdate
	ii)Combos : /SaleOrder/CmbFill
	ii)Combos in L entry : /SaleOrder/CmbFillItem?string icat
	iii)New OrdNo : /saleorder/newordno?int mfg=1 & int ycode=1
	iv)HSave : SaleOrder/HSave	
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
	v)LSave: SaleOrder/LSave
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