using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinesweeperMilestone.Models
{
    public class MoveResult
    {
        // Properties
        public bool success { get; set; }
        public string message { get; set; } = "";
        public bool collectedPower { get; set; }
        public bool hitBomb { get; set; }
        public int neighborCount { get; set; }
    }
}
