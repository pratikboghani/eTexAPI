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
        private FillComboServices _EmailProfile = new FillComboServices();
        private GlbServices _MSGLOG = new GlbServices();
        #region Company Select
        //---------------------------------Company Select-------------------------------------//
        [System.Web.Http.Route("api/Glb/CompSel")]
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

        #region EmailProfile
        //---------------------------------WhatsApp Password & Email-------------------------------------//
        [System.Web.Http.Route("api/Glb/EmailProfile")]
        [System.Web.Http.HttpGet]
        public IHttpActionResult EmailProfile()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _EmailProfile.TableName = "EMAILPROFILE";
            _EmailProfile.Fill();

            if (_EmailProfile.DS == null)
            {
                return NotFound();
            }
            return Ok(_EmailProfile.DS);
        }
        #endregion

        #region EmailProfile
        //---------------------------------WhatsApp & Email msg Log-------------------------------------//
        [System.Web.Http.Route("api/Glb/NewMsgLogId")]
        [System.Web.Http.HttpGet]
        public IHttpActionResult GetNewNo()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(_MSGLOG.FindNewMsgLogId());
        }

        [System.Web.Http.Route("api/Glb/MsgLogSave")]
        //[ResponseType(typeof(ClsMsg))]
        [System.Web.Http.HttpPost]
        public async Task<IHttpActionResult> MsgLogSave(ClsMsg cls)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var x = await _MSGLOG.SaveMsgLog(cls);
                return Ok(x.ToString());

            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
        #endregion
    }
}
