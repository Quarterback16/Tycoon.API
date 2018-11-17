using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ProgramAssuranceTool.Interfaces;
using ProgramAssuranceTool.Models;
using ProgramAssuranceTool.Repositories;

namespace ProgramAssuranceToolWebApi.Controllers
{
    public class BulletinsController : ApiController
    {
		  IRepository<Bulletin> repository;

		  public BulletinsController( IRepository<Bulletin> repository )
		  {
			  this.repository = repository;
		  }

        // GET api/bulletins
        public IEnumerable<Bulletin> Get()
        {
			  var repository = new BulletinRepository();
            return repository.GetAll();
        }

        // GET api/bulletins/5
        public Bulletin Get(int id)
        {
			  var repository = new BulletinRepository();
			  var bulletin = repository.GetById( id );
			  if ( bulletin == null )
				  throw new HttpResponseException( HttpStatusCode.NotFound );  // embrace the HTTP
			  return bulletin;
        }

        // POST api/bulletins
        public void Post([FromBody]string value)
        {
        }

        // PUT api/bulletins/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/bulletins/5
        public void Delete(int id)
        {
        }
    }
}
