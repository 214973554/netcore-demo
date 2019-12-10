using DAO;
using Entity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace BFO
{
    public class VBBFO : BaseBFO<VB>
    {
        private IDAO<VB> dao;
        public VBBFO(IDAO<VB> dao)
        {
            this.dao = dao;
        }
        

        public IEnumerable<VB> GetAll()
        {
            return dao.GetAll();
        }

        public VB GetBy(int vb_id)
        {
            return dao.GetBy(vb_id);
        }

        public bool Insert(VB vb)
        {
            return dao.Insert(vb);
        }
    }
}
