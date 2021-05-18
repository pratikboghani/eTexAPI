using eTexAPI.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ope = eTexAPI.Data.OperationSql;

using System.Web.Http;
using eTexAPI.Data.Services;
using System.Web.Mvc;
using System.Web.Http.Description;
using System.Threading.Tasks;

namespace eTexAPI.Controllers
{
    public class SaleOrderController: ApiController
    {
        private SaleOrderServices mSaleOrd = new SaleOrderServices();
        private FillComboServices mFillCombo = new FillComboServices();
        private FillComboServices mFillComboItemMast = new FillComboServices();

        [System.Web.Http.Route("api/SaleOrder/View")]
        [System.Web.Http.HttpGet]
        public IHttpActionResult View(int mfg = 0,int ycode = 0,string status="",string fdate="",string tdate="")
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            clsSaleOrder cls = new clsSaleOrder();
            cls.MfgUnit = mfg;
            cls.YCode = ycode;
            cls.ViewStrStsType = status;
            cls.F_DATE = Ope.DTDBDate(fdate);
            cls.T_DATE = Ope.DTDBDate(tdate);
            
            mSaleOrd.SaleOrderView(cls);
            
            if (mSaleOrd.DS_VIEW == null)
            {
                return NotFound();
            }
            return Ok(mSaleOrd.DS_VIEW);
        }

        [System.Web.Http.Route("api/SaleOrder/CmbFill")]
        [System.Web.Http.HttpGet]
        public IHttpActionResult CmbFill()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            mFillCombo.TableName = "View_ItemCategory";
            mFillCombo.Fill();

            mFillCombo.TableName = "PARTYMAST";
            mFillCombo.Typ = "SPAR";
            mFillCombo.Fill();

            mFillCombo.TableName = "OrderStatus";
            mFillCombo.Fill();


            if (mFillCombo.DS == null)
            {
                return NotFound();
            }
            return Ok(mFillCombo.DS);
        }

        [System.Web.Http.Route("api/SaleOrder/CmbFillItem")]
        [System.Web.Http.HttpGet]
        public IHttpActionResult ItemMastFill(string icat)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            mFillComboItemMast.TableName = "ITEMMAST";
            mFillComboItemMast.IsOrderBy = 1;
            mFillComboItemMast.Typ = "1";
            mFillComboItemMast.ICAT = icat;
            mFillComboItemMast.Fill();


            if (mFillComboItemMast.DS == null)
            {
                return NotFound();
            }
            return Ok(mFillComboItemMast.DS);
        }

        [System.Web.Http.Route("api/SaleOrder/NewOrdNo")]
        [System.Web.Http.HttpGet]
        public IHttpActionResult GetNewTrnNo(int mfg,int ycode)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(mSaleOrd.GetNewOrdNo(mfg, ycode));
        }

        [System.Web.Http.Route("api/SaleOrder/HSave")]
        [ResponseType(typeof(AttTrn))]
        [System.Web.Http.HttpPost]
        public async Task<IHttpActionResult> HSave(clsSaleOrder cls)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var x = await mSaleOrd.HSave(cls);
                return Ok(x.ToString());

            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [System.Web.Http.Route("api/SaleOrder/LSave")]
        [ResponseType(typeof(AttTrn))]
        [System.Web.Http.HttpPost]
        public async Task<IHttpActionResult> LSave(clsSaleOrder cls)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var x = await mSaleOrd.LSave(cls);
                return Ok(x.ToString());

            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
    }
}
