using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eTexAPI.Data.Model
{
    public class AttTrn
    {
        public int MFGUNIT { get; set; }
        public int DEPT_CODE { get; set; }
        public DateTime? Att_Date { get; set; }
        public string SHIFT { get; set; }
        public string FlorNo { get; set; }
        public string TYPE { get; set; }
        public int SrNo { get; set; }
        public int F_MacNo { get; set; }
        public int T_MacNo { get; set; }
        public int Tot_Mac { get; set; }
        public int Emp_Code { get; set; }
        public int BEmp_Code { get; set; }
        public string IUser { get; set; }
        public string IComp { get; set; }
        public double PDAY { get; set; }
    }
}
