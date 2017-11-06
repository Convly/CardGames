using Server;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;

namespace OwinSelfhostSample
{
    public class CardGamesController : ApiController
    {
        // GET api/cardGames 
        public IEnumerable<string> Get()
        {
            return new string[] { };
        }

        // GET api/cardGames/request
        public Object Get(string request)
        {
            switch (request)
            {
                case "Coucou":
                    return "Salut herbaux";
            }
            return "Bad entry: Request unknow";
        }

        // POST api/cardGames/moduleName
        [HttpPost]
        public void Post([FromUri]string request, [FromBody]Object data)
        {
            switch (request)
            {
                case "":
                    break;
            }
        }

        // PUT api/cardGames/5 
        [HttpPut]
        public void Put(string module, [FromBody]dynamic data)
        {
            string toto = data.value;
        }

        // DELETE api/cardGames/5 
        public void Delete(int id)
        {
        }
    }
}
