using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Models
{

    public class PointsManager
    {
        public string playerType { get; set; }
        public int scoreGoal { get; set; }
        public int makeAssist { get; set; }
        public int scoreOwnGoal { get; set; }
        public int getYellowCard { get; set; }
        public int getRedCard { get;set;}
        public int saveGoal { get; set; }
        public int recieveGoal { get; set; }
        public int savePenalty { get; set; }
        public int missPenalty { get; set; }
 

    }
}
