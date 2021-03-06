﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microshaoft;
using Microshaoft.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace App1.WebApplication.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        [BearerTokenBasedAuthorizeWebApiFilter]
        public ActionResult<JToken> Get()
        {
            var result = new JObject();
            result.Add
                    (
                        "User"
                        , HttpContext
                            .User
                            .Identity
                            .Name
                    );
            if
                (
                    HttpContext
                        .User
                        .TryGetClaimTypeJTokenValue
                            (
                                "Extension"
                                , out var claimValue
                            )
                )
            {
                result.Add
                    (
                        "ExtensionClaims"
                        , claimValue
                    );
            }
            return
                result;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
            // For more information on protecting this API from Cross Site Request Forgery (CSRF) attacks, see https://go.microsoft.com/fwlink/?LinkID=717803
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
            // For more information on protecting this API from Cross Site Request Forgery (CSRF) attacks, see https://go.microsoft.com/fwlink/?LinkID=717803
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            // For more information on protecting this API from Cross Site Request Forgery (CSRF) attacks, see https://go.microsoft.com/fwlink/?LinkID=717803
        }
    }
}
