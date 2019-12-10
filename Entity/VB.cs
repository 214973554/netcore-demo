using System;

namespace Entity
{
    /// <summary>
    /// 变量
    /// </summary>
    public class VB
    {
        /// <summary>
        /// 变量ID
        /// </summary>
        public int VB_ID
        {
            get; set;
        }

        /// <summary>
        /// 变量中文名称
        /// </summary>		
        public string VB_NM
        {
            get; set;
        }

        /// <summary>
        /// 变量英文名称
        /// </summary>		
        public string VB_COL
        {
            get; set;
        }

        /// <summary>
        /// 变量类型ID
        /// </summary>		
        public int VB_SRC_ID
        {
            get; set;
        }

        /// <summary>
        /// VB_STAT
        /// </summary>		
        public int VB_STAT
        {
            get; set;
        }

        /// <summary>
        /// VB_VER
        /// </summary>		
        public string VB_VER
        {
            get; set;
        }

        /// <summary>
        /// VB_SUBVER
        /// </summary>		
        public string VB_SUBVER
        {
            get; set;
        }

        /// <summary>
        /// 变量描述
        /// </summary>		
        public string VB_DESC
        {
            get; set;
        }

        /// <summary>
        /// VB_CT
        /// </summary>		
        public DateTime VB_CT
        {
            get; set;
        }

        /// <summary>
        /// VB_UT
        /// </summary>		
        public DateTime VB_UT
        {
            get; set;
        }

        /// <summary>
        /// VB_CU
        /// </summary>		
        public int VB_CU
        {
            get; set;
        }

        /// <summary>
        /// _vb_src_tp_id
        /// </summary>		
        public int VB_SRC_TP_ID
        {
            get; set;
        }

        /// <summary>
        /// VB_Type
        /// </summary>		
        public string VB_TYPE
        {
            get; set;
        }

        public int VB_TP { get; set; }

        public int VB_VER_GP { get; set; }

        public string VB_DEFAULT { get; set; }

        public int VB_SRC_TP { get; set; }
    }
}


