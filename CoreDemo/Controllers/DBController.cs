using BFO;
using Common;
using Entity;
using log4net;
using Microsoft.AspNetCore.Mvc;

namespace CoreDemo.Controllers
{
    /// <summary>
    /// 数据库访问示例
    /// </summary>
    public class DBController : ApiControllerBase
    {
        private VBBFO vbBFO;
        private VB_SRC_TPBFO vb_src_tpBFO;

        public DBController(ILog ilog, BaseBFO<VB> vbBFO,BaseBFO<VB_SRC_TP> vb_src_tpBFO) : base(ilog)
        {
            this.vbBFO = vbBFO.ConvertTo<VBBFO>();
            this.vb_src_tpBFO = vb_src_tpBFO.ConvertTo<VB_SRC_TPBFO>();
        }

        #region VB
        /// <summary>
        /// 查询所有数据
        /// </summary>
        /// <returns>返回变量所有数据</returns>
        [HttpGet, Route("GetAll")]
        public ActionResult GetAll()
        {
            var vbs = vbBFO.GetAll();
            return new JsonResult(vbs);
        }

        [HttpGet, Route("GetVb/{id}")]
        public ActionResult GetVb(int id)
        {
            var vb = vbBFO.GetBy(id);
            return new JsonResult(vb);
        }

        [HttpPost,Route("AddVb")]
        public ActionResult AddVb(VB vb)
        {
           bool success =  vbBFO.Insert(vb);

            return new JsonResult(success);
        }
        #endregion

        #region VB_SRC_TP
        /// <summary>
        /// 获取所有VB_SRC_TP数据
        /// </summary>
        /// <returns></returns>
        [HttpGet,Route("GetAllVbSrcTp")]
        public ActionResult GetAllVbSrcTp()
        {
            var list = vb_src_tpBFO.GetAll();

            return new JsonResult(list);
        }

        /// <summary>
        /// 根据主键获取VB_SRC_TP数据
        /// </summary>
        /// <param name="vb_src_tp_id">主键</param>
        /// <returns></returns>
        [HttpGet,Route("GetVbSrcTpBy")]
        public ActionResult GetVbSrcTpBy(int vb_src_tp_id)
        {
           var entity = vb_src_tpBFO.GetBy(vb_src_tp_id);

            return new JsonResult(entity);
        }
        #endregion
    }
}