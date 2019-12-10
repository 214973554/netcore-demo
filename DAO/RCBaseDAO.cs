using Common;
using log4net;
using Microsoft.Extensions.Configuration;
using System;

namespace DAO
{
    public class RCBaseDAO : BaseDao
    {
        protected readonly string connectKey = "RCDB";

        public RCBaseDAO(IConfiguration configuration, ILog ilog) : base(configuration, ilog)
        {
        }
    }
}
