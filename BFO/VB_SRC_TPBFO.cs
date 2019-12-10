using DAO;
using Entity;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace BFO
{
    public class VB_SRC_TPBFO : BaseBFO<VB_SRC_TP>
    {
        private IDAO<VB_SRC_TP> dao;
        public VB_SRC_TPBFO(IDAO<VB_SRC_TP> dao)
        {
            this.dao = dao;
        }


        public IEnumerable<VB_SRC_TP> GetAll()
        {
            return dao.GetAll();
        }

        public VB_SRC_TP GetBy(int vb_src_tp_id)
        {
            return dao.GetBy(vb_src_tp_id);
        }
    }
}
