using alwatnia.Models;
using alwatnia.Models.JobCV;
using System;
using System.Web.Http;

namespace alwatnia.Controllers.Api
{
    public class JobApplicationController : ApiController
    {
        private readonly DataModel _context = new DataModel();

        [HttpPost]
        public int AddPersonalInformation([FromBody] JobApplication data)
        {
           
                data.Cdate = DateTime.UtcNow;
                _context.JobApplication.Add(data);
                _context.SaveChanges();
                return 1;
           
        }
    }

}
