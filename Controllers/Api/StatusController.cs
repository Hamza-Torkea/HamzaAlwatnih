using alwatnia.Models;
using System;
using System.Linq;
using System.Web.Http;

namespace alwatnia.Controllers.Api
{
    public class StatusController : ApiController
    {
        private const string Key = "HEALTH_CHECK_STATUS";

        [HttpGet]
        [Route("api/status/health-check")]
        public IHttpActionResult Check(int status = 1)
        {
            try
            {
                var dataModel = new DataModel();
                var config = dataModel.Configrations.FirstOrDefault(e => e.Config_name.Equals(Key));
                if (config == null)
                {
                    config = new Configration
                    {
                        Config_name = Key,
                        Config_details = status + ""
                    };
                    dataModel.Configrations.Add(config);
                }
                else
                {
                    config.Config_details = status + "";
                }

                dataModel.SaveChanges();

                return Ok(status);
            }
            catch (Exception)
            {
                return Ok(0);
            }
        }
    }
}
