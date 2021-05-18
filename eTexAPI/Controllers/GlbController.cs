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
    public class GlbController: ApiController
    {
        private FillComboServices _FillCmpSel = new FillComboServices();
        #region Company Select
        //---------------------------------Company Select-------------------------------------//
        [System.Web.Http.Route("Glb/CompSel")]
        [System.Web.Http.HttpGet]
        public IHttpActionResult CompSel(int mfg)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _FillCmpSel.TableName= "CmpMast";
            _FillCmpSel.MfgUnit = mfg;
            _FillCmpSel.Fill();

            _FillCmpSel.TableName = "YearMast";
            _FillCmpSel.Fill();

            if (_FillCmpSel.DS == null)
            {
                return NotFound();
            }
            return Ok(_FillCmpSel.DS);
        }
        #endregion
    }
}
