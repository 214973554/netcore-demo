using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using log4net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoreDemo.Controllers.apiVersion.v1
{
    /// <summary>
    /// API版本控制，v1
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ApiVersionController : ApiControllerBase
    {
        public ApiVersionController(ILog ilog) : base(ilog)
        {
        }

        /// <summary>
        /// GetApiResult v1.0版本实现
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetApiResult")]
        public string GetApiResult()
        {
            return "CoreDemo.Controllers.apiVersion.v1-GetApiResult:v:" + HttpContext.GetRequestedApiVersion().ToString();
        }
    }
}