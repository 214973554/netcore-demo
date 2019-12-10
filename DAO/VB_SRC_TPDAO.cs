using Entity;
using log4net;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAO
{
    public class VB_SRC_TPDAO : RCBaseDAO, IDAO<VB_SRC_TP>
    {
        public VB_SRC_TPDAO(IConfiguration configuration, ILog ilog) : base(configuration, ilog)
        {
        }

        public bool Delete(object id)
        {
            throw new NotImplementedException(); 
        }

        public IEnumerable<VB_SRC_TP> GetAll()
        {
            string selectSql = @"SELECT [VB_SRC_TP_ID]
      ,[VB_SRC_TP_NM]
      ,[VB_SRC_TP_CT]
      ,[VB_SRC_TP_DB]
      ,[VB_SRC_TP_ADDR]
      ,[VB_SRC_TP_PARAS]
      ,[VB_SRC_TP_DESC]
      ,[VB_SRC_TP_CACHE]
      ,[VB_SRC_TP_VER]
      ,[VB_MAP_ID]
  FROM [RC].[VB_SRC_TP] WITH(NOLOCK)";

            return Query<VB_SRC_TP>(selectSql, null, connectKey);
        }

        public VB_SRC_TP GetBy(object id)
        {
            string selectSql = @"SELECT [VB_SRC_TP_ID]
      ,[VB_SRC_TP_NM]
      ,[VB_SRC_TP_CT]
      ,[VB_SRC_TP_DB]
      ,[VB_SRC_TP_ADDR]
      ,[VB_SRC_TP_PARAS]
      ,[VB_SRC_TP_DESC]
      ,[VB_SRC_TP_CACHE]
      ,[VB_SRC_TP_VER]
      ,[VB_MAP_ID]
  FROM [RC].[VB_SRC_TP] WITH(NOLOCK)
  WHERE VB_SRC_TP_ID = @VB_SRC_TP_ID";

            var param = new Dictionary<string, object>()
            {
                {"@VB_SRC_TP_ID", id}
            };

            var list = Query<VB_SRC_TP>(selectSql, param, connectKey);
            VB_SRC_TP entity = default(VB_SRC_TP);
            if (list != null && list.Count > 0)
            {
                entity = list[0];
            }

            return entity;
        }

        public bool Insert(VB_SRC_TP t)
        {
            throw new NotImplementedException();
        }

        public bool Update(VB_SRC_TP t)
        {
            throw new NotImplementedException();
        }
    }
}
