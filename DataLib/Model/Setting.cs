using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eTexAPI.Data.Model
{
    public class Setting
    {
        public string SKEY { get; set; }
        public string SVALUE { get; set; }

    }

    public class CmbFill
    {
        public string Type{ get; set; }
        public bool ChkOrd { get; set; } = false;
        public string Typ { get; set; }
        public string U_Name { get; set; }
        public bool AlsoWithYouAdded { get; set; } = true;

        public int Dept_Code { get; set; } = 0;
        public int Y_Code { get; set; } = 0;
        public int MfgUnit { get; set; } = 1;
    }
}
