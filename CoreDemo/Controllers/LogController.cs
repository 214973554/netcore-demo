using Common;
using log4net;
using Microsoft.AspNetCore.Mvc;
using System;

namespace CoreDemo.Controllers
{
    /// <summary>
    /// 日志示例
    /// </summary>
    public class LogController : ApiControllerBase
    {
        public LogController(ILog ilog) : base(ilog)
        {
        }

        [HttpPost, Route("WriteLog")]
        public void WriteLog([FromBody]string log)
        {
            this.log4NetHelper.Log(log, LogType.Debug);
            this.log4NetHelper.Log(log, LogType.Info);
            this.log4NetHelper.Log(log, LogType.Warn);
            this.log4NetHelper.Log(log, LogType.Error);
            this.log4NetHelper.Log(log, LogType.Fatal);
        }
    }
}