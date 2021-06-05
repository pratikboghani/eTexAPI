using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eTexAPI.Data.Model
{
    public class ClsMsg
    {
        public string BrokerMobile { get; set; }
        public string PartyMobile { get; set; }
        public string BrokerEmail { get; set; }
        public string PartyEmail { get; set; }
        public string FormType { get; set; }
        public string FormKey { get; set; }
        public string WPMobile { get; set; }
        public string WPPass { get; set; }
        public string IUser { get; set; }
        public int LogID { get; set; }
        public int LogWp { get; set; }
        public int LogMail { get; set; }
        public int LogBrok { get; set; }
        public int LogParty { get; set; }
        public int MFGUNIT { get; set; }
        public int ORDNO { get; set; }
    }
}
