using Entity;
using log4net;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace DAO
{
    public class VBDAO : RCBaseDAO, IDAO<VB>
    {
        public VBDAO(IConfiguration configuration, ILog ilog) : base(configuration, ilog)
        {
        }

        public bool Delete(object id)
        {
            string deleteSql = "DELETE FROM [RC].[VB] WHERE [VB_ID] = @VB_ID";
            var param = new Dictionary<string, object>()
            {
                {"@VB_ID", id }
            };

            return Execute(deleteSql, param, connectKey) > 0;
        }

        public IEnumerable<VB> GetAll()
        {
            string selectSql = @"SELECT [VB_ID]
      ,[VB_NM]
      ,[VB_COL]
      ,[VB_SRC_ID]
      ,[VB_STAT]
      ,[VB_TP]
      ,[VB_VER]
      ,[VB_SUBVER]
      ,[VB_VER_GP]
      ,[VB_DESC]
      ,[VB_CT]
      ,[VB_UT]
      ,[VB_CU]
      ,[VB_DEFAULT]
      ,[VB_SRC_TP]
      ,[VB_SRC_TP_ID]
  FROM[RC].[VB] WITH(NOLOCK)
  ORDER BY VB_ID";

            return Query<VB>(selectSql, null, connectKey);
        }

        public VB GetBy(object id)
        {
            string selectSql = @"SELECT [VB_ID]
      ,[VB_NM]
      ,[VB_COL]
      ,[VB_SRC_ID]
      ,[VB_STAT]
      ,[VB_TP]
      ,[VB_VER]
      ,[VB_SUBVER]
      ,[VB_VER_GP]
      ,[VB_DESC]
      ,[VB_CT]
      ,[VB_UT]
      ,[VB_CU]
      ,[VB_DEFAULT]
      ,[VB_SRC_TP]
      ,[VB_SRC_TP_ID]
  FROM[RC].[VB] WITH(NOLOCK)
  WHERE VB_ID = @VB_ID";

            var param = new Dictionary<string, object>()
            {
                {"@VB_ID", id}
            };

            var vbs = Query<VB>(selectSql, param, connectKey);
            VB vb = default(VB);
            if (vbs != null && vbs.Count > 0)
            {
                vb = vbs[0];
            }

            return vb;
        }

        public bool Insert(VB t)
        {
            string insertSql = @"INSERT INTO [RC].[VB]
           ([VB_NM]
           ,[VB_COL]
           ,[VB_SRC_ID]
           ,[VB_STAT]
           ,[VB_TP]
           ,[VB_VER]
           ,[VB_SUBVER]
           ,[VB_VER_GP]
           ,[VB_DESC]
           ,[VB_CT]
           ,[VB_UT]
           ,[VB_CU]
           ,[VB_DEFAULT]
           ,[VB_SRC_TP]
           ,[VB_SRC_TP_ID])
     VALUES
           (@VB_NM
           ,@VB_COL
           ,@VB_SRC_ID
           ,@VB_STAT
           ,@VB_TP
           ,@VB_VER
           ,@VB_SUBVER
           ,@VB_VER_GP
           ,@VB_DESC
           ,@VB_CT
           ,@VB_UT
           ,@VB_CU
           ,@VB_DEFAULT
           ,@VB_SRC_TP
           ,@VB_SRC_TP_ID)";

            IDictionary<string, object> param = GetParams(t);

            return Execute(insertSql, param, connectKey) > 0;
        }

        private IDictionary<string, object> GetParams(VB t)
        {
            IDictionary<string, object> param = new Dictionary<string, object>()
            {
                {"@VB_NM", t.VB_NM},
                {"@VB_COL", t.VB_COL},
                {"@VB_SRC_ID", t.VB_SRC_ID},
                {"@VB_STAT", t.VB_STAT},
                {"@VB_TP", t.VB_TP},
                {"@VB_VER", t.VB_VER},
                {"@VB_SUBVER", t.VB_SUBVER},
                {"@VB_VER_GP", t.VB_VER_GP},
                {"@VB_DESC", t.VB_DESC},
                {"@VB_CT", t.VB_CT},
                {"@VB_UT", t.VB_UT},
                {"@VB_CU", t.VB_CU},
                {"@VB_DEFAULT", t.VB_DEFAULT},
                {"@VB_SRC_TP", t.VB_SRC_TP},
                {"@VB_SRC_TP_ID", t.VB_SRC_TP_ID}
            };

            return param;
        }

        public bool Update(VB t)
        {
            string updateSql = @"UPDATE [RC].[VB]
   SET [VB_NM] = @VB_NM
      ,[VB_COL] = @VB_COL
      ,[VB_SRC_ID] = @VB_SRC_ID
      ,[VB_STAT] = @VB_STAT
      ,[VB_TP] = @VB_TP
      ,[VB_VER] = @VB_VER
      ,[VB_SUBVER] = @VB_SUBVER
      ,[VB_VER_GP] = @VB_VER_GP
      ,[VB_DESC] = @VB_DESC
      ,[VB_CT] = @VB_CT
      ,[VB_UT] = @VB_UT
      ,[VB_CU] = @VB_CU
      ,[VB_DEFAULT] = @VB_DEFAULT
      ,[VB_SRC_TP] = @VB_SRC_TP
      ,[VB_SRC_TP_ID] = @VB_SRC_TP_ID
 WHERE VB_ID = @VB_ID";

            IDictionary<string, object> param = GetParams(t);
            param.Add("@VB_ID", t.VB_ID);

            return Execute(updateSql, param, connectKey) > 0;

        }
    }
}
