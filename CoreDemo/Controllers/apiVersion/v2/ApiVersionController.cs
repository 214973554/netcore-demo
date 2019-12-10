using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using log4net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoreDemo.Controllers.apiVersion.v2
{
    /// <summary>
    /// API版本控制，v2
    /// </summary>
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ApiVersionController : ApiControllerBase
    {
        public ApiVersionController(ILog ilog) : base(ilog)
        {
        }

        /// <summary>
        /// GetApiResult v2.0版本实现
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetApiResult")]
        public string GetApiResult()
        {
            return "CoreDemo.Controllers.apiVersion.v2-GetApiResult:v:" + HttpContext.GetRequestedApiVersion().ToString();
        }
    }
}