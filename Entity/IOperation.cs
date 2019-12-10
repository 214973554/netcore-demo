using System;
using System.Collections.Generic;
using System.Text;

namespace Entity
{
    public interface IOperation
    {
        Guid OperationId { get; }
    }

    public interface IOperationTransient : IOperation
    {

    }

    public interface IOperationScoped : IOperation
    {

    }

    public interface IOperationSingleton : IOperation
    {

    }

    public class Operation : IOperationTransient, IOperationScoped, IOperationSingleton
    {
        public Guid OperationId { get; private set; }

        public Operation(Guid guid)
        {
            this.OperationId = guid;
        }

        public Operation() : this(Guid.NewGuid())
        {

        }
    }
}
