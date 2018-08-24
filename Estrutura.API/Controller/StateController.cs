using Estrutura.Application;
using Estrutura.Model;
using System.Collections.Generic;
using System.Web.Http;

namespace Estrutura.API.Controller
{
    [RoutePrefix("api/states")]
    public class StateController : ApiController
    {
        [HttpGet, Route("")]
        public IEnumerable<States> GetAll()
        {
            var sr = new StatesRules();            
            return sr.GetAll();
        }

        [HttpGet, Route("{name}")]
        public States GetAll(string name)
        {
            var sr = new StatesRules();
            return sr.GetByName(name);
        }
        
        [HttpPost, Route("")]
        public void Insert([FromBody] States state)
        {
            var sr = new StatesRules();
            sr.Insert(state);
        }
    }
}
