using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BFO;
using Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoreDemo.Controllers
{
    /// <summary>
    /// 验证注入对象的三种生命周期
    /// 
    /// 暂时:
    ///暂时生存期服务是每次请求时创建的。 这种生存期适合轻量级、 无状态的服务。
    ///作用域（Scoped）:
    ///作用域生存期服务以每个请求一次的方式创建。
    ///有作用域的对象在一个请求内是相同的，但在请求之间是不同的。
    ///单例:
    ///单一实例生存期服务是在第一次请求时
    ///（或者在运行 ConfigureServices并且使用服务注册指定实例时）创建的。
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class LifeCycleController : ControllerBase
    {
        private OperationService operationService;
        private IOperationTransient operationTransient;
        private IOperationScoped operationScoped;
        private IOperationSingleton operationSingleton;
        public LifeCycleController(OperationService operationService, IOperationTransient operationTransient, IOperationScoped operationScoped, IOperationSingleton operationSingleton)
        {
            this.operationService = operationService;
            this.operationTransient = operationTransient;
            this.operationScoped = operationScoped;
            this.operationSingleton = operationSingleton;
        }

        /// <summary>
        ///  
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("LifeCycleTest")]
        public string LifeCycleTest()
        {
            return operationService.Do() + Environment.NewLine + Do();
        }

        private string Do()
        {
            StringBuilder result = new StringBuilder();
            result.AppendLine(string.Format("LifeCycleController Transient:{0}", operationTransient.OperationId));
            result.AppendLine(string.Format("LifeCycleController Scoped:{0}", operationScoped.OperationId));
            result.AppendLine(string.Format("LifeCycleController Singleton:{0}", operationSingleton.OperationId));

            return result.ToString();
        }
    }




}