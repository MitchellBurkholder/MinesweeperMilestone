using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinesweeperMilestone.Models
{
    public class Cell
    {
        // ----- PROPERTIES -----
        public int row { get; set; }
        public int col { get; set; }
        public int numBombNeighbors { get; set; }
        public bool isVisited { get; set; } = false;
        public bool isBomb { get; set; } = false;
        public bool isFlagged { get; set; } = false;
        public bool hasReward { get; set; } = false;
    }
}
