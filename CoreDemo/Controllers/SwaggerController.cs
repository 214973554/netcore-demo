using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoreDemo.Controllers
{
    public class SwaggerController : ApiControllerBase
    {
        public SwaggerController(ILog ilog) : base(ilog)
        {
        }

        /// <summary>
        /// Swagger中不需要显示的方法可以，添加注解ApiExplorerSettings(IgnoreApi = true)
        /// </summary>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet,Route("IgnoreAction")]
        public void IgnoreAction()
        {

        }

        /// <summary>
        /// 演示示例入参
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /ExampleParam
        ///     {
        ///        "id": 1,
        ///        "name": "Item1",
        ///        "isComplete": true
        ///     }
        ///
        /// </remarks>
        /// <param name="param">原始请求json</param>
        /// <returns></returns>
        [HttpPost,Route("ExampleParam")]
        public JsonResult ExampleParam([FromBody] IDictionary<string,object> param)
        {
            return new JsonResult(param);
        }
    }
}