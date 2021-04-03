using eTexAPI.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using eTexAPI.Data.Services;
using System.Web.Mvc;

namespace eTexAPI.Controllers
{
    public class eTexController: ApiController
    {
        private etexServices _eTexServices = new etexServices();
        [System.Web.Http.HttpGet]
        public IHttpActionResult DeptDisp(int mfg=0,int dept=0)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            AttTrn cls = new AttTrn();
            cls.MFGUNIT = mfg;
            cls.DEPT_CODE = dept;
            //cls.Att_Date = attdate;
            //cls.SHIFT = shift;
            //cls.FLOOR = floor;
            _eTexServices.AttCmbDisp(cls);
            
            if (_eTexServices.DS == null)
            {
                return NotFound();
            }
            return Ok(_eTexServices.DS);
        }
        [System.Web.Http.Route("api/etex/atttrn")]
        [System.Web.Http.HttpGet]
        public IHttpActionResult AttTrnFill(int mfg, int dept, string date, string shift, string floor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            AttTrn cls = new AttTrn();
            cls.MFGUNIT = mfg;
            cls.DEPT_CODE = dept;
            cls.Att_Date = date;
            cls.SHIFT = shift;
            cls.FLOOR = floor;
            _eTexServices.AttTrnFill(cls);

            if (_eTexServices.DS == null)
            {
                return NotFound();
            }
            return Ok(_eTexServices.DS);
        }
    }
}
