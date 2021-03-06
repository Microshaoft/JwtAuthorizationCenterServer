﻿#if !NETFRAMEWORK4_X && !NETSTANDARD2_0
namespace Microshaoft.WebApi.Controllers
{
    using Microshaoft;
    using Microshaoft.Web;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Cors;
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json.Linq;
    using System;

    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAllOrigins")]
    [Authorize]
    public class StoreProcedureExecutorController
                    : AbstractStoreProceduresExecutorControllerBase
    {
        public StoreProcedureExecutorController(IStoreProceduresWebApiService service)
                : base(service)
        {
        }

        [BearerTokenBasedAuthorizeWebApiFilter]
        public override ActionResult<JToken> ProcessActionRequest
             (
                //[FromRoute]
                    string routeName
                , //[ModelBinder(typeof(JTokenModelBinder))]
                    JToken parameters = null
                , //[FromRoute]
                    string resultPathSegment1 = null
                , //[FromRoute]
                    string resultPathSegment2 = null
                , //[FromRoute]
                    string resultPathSegment3 = null
                , //[FromRoute]
                    string resultPathSegment4 = null
                , //[FromRoute]
                    string resultPathSegment5 = null
                , //[FromRoute]
                    string resultPathSegment6 = null
            )
        {
            return
                ProcessActionRequest
                    (
                        routeName
                        , parameters
                    );
        }
        private ActionResult<JToken> ProcessActionRequest
                        (
                            //[FromRoute]
                            string routeName
                            ,
                                //[ModelBinder(typeof(JTokenModelBinder))]
                                JToken parameters = null
                        )
        {
            var jsonObject = ((JObject)parameters);
            jsonObject
                    .Add
                        (
                            "UserName"
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
                jsonObject
                        .Add
                            (
                                "ExtensionClaims"
                                , claimValue
                            );
            }
            JToken result = null;
            (int StatusCode, JToken Result) r =
                    _service
                        .Process
                            (
                                routeName
                                , jsonObject
                                , (reader, fieldType, fieldName, rowIndex, columnIndex) =>
                                {
                                    JProperty field = null;
                                    if (fieldType == typeof(string))
                                    {
                                        if (fieldName.Contains("Json", StringComparison.OrdinalIgnoreCase))
                                        {
                                            //fieldName = fieldName.Replace("json", "", System.StringComparison.OrdinalIgnoreCase);
                                            var json = reader.GetString(columnIndex);
                                            field = new JProperty
                                                            (
                                                                fieldName
                                                                , JObject.Parse(json)
                                                            );
                                        }
                                    }
                                    return field;
                                }
                                , Request.Method
                            );
            if (r.StatusCode == 200)
            {
                result =
                    r.Result
                        .GetDescendantByPath
                            (
                                "Outputs"
                                , "ResultSets"
                                , "0"
                                , "Rows"
                            );
            }
            else
            {
                Response
                    .StatusCode = r.StatusCode;
            }
            return
                result;
        }
    }
}
#endif
