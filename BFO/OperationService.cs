using Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BFO
{
    public class OperationService
    {
        private IOperationTransient operationTransient;
        private IOperationScoped operationScoped;
        private IOperationSingleton operationSingleton;
        public OperationService(IOperationTransient operationTransient, IOperationScoped operationScoped, IOperationSingleton operationSingleton)
        {
            this.operationTransient = operationTransient;
            this.operationScoped = operationScoped;
            this.operationSingleton = operationSingleton;
        }

        public string Do()
        {
            StringBuilder result = new StringBuilder();
            result.AppendLine(string.Format("Transient:{0}", operationTransient.OperationId));
            result.AppendLine(string.Format("Scoped:{0}", operationScoped.OperationId));
            result.AppendLine(string.Format("Singleton:{0}", operationSingleton.OperationId));

            return result.ToString();
        }
    }
}
