using Common;
using log4net;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;

namespace CoreDemo.Controllers
{
    /// <summary>
    /// json、xml、ini配置文件读取
    /// </summary>
    public class ConfigController : ApiControllerBase
    {
        public ConfigController(ILog ilog) : base(ilog)
        {

        }

        /// <summary>
        /// 通过键读取配置文件的值
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /GetConfigValue
        ///     {
        ///        "fileType": "json",
        ///        "key": "section0:key0"
        ///     }
        ///     
        ///     POST /GetConfigValue
        ///     {
        ///        "fileType": "ini",
        ///        "key": "section1:subsection:key"
        ///     }
        ///  
        ///     POST /GetConfigValue
        ///     {
        ///        "fileType": "xml",
        ///        "key": "section1:key1"
        ///     }
        ///
        /// </remarks>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost, Route("GetConfigValue")]
        public string GetConfigValue([FromBody] IDictionary<string, object> param)
        {
            string fileType = param["fileType"].ToString();
            string key = param["key"].ToString();

            switch (fileType)
            {
                case "json":
                    break;
                case "xml":
                    break;
                case "ini":
                    break;
                default:
                    throw new ApplicationException("不支持的fileType，仅支持json、xml、ini类型。");
            }

            string path = Path.Combine("ConfigFile", string.Format("config.{0}", fileType));
            var config = new AppSettingsHelper(path);

            return config.GetValueBy(key);
        }
    }
}