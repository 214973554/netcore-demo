using Common;
using log4net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace CoreDemo
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiControllerBase: ControllerBase
    {
        protected Log4netHelper log4NetHelper;
        public ApiControllerBase(ILog ilog)
        {
            this.log4NetHelper = new Log4netHelper(ilog, this.GetType());
        }
    }
}
