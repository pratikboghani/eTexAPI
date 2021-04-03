using eTexAPI.Data.Services;
using System.Web.Http;

namespace eTexAPI.Controllers
{
    public class ValuesController : ApiController
    {
        private SettingServices _SettingServices = new SettingServices();

        public string Get()
        {
            return _SettingServices.GetSQLVersion();
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
