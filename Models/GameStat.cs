using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinesweeperMilestone.Models
{
    public class GameStat
    {
        // Properties
        public string PlayerName { get; set; }
        public int Score { get; set; }
        public int Size { get; set; }
        public int Difficulty { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan TimeElapsed { get; set; }


        // ToString override
        public override string ToString()
        {
            return $"{PlayerName} - Score: {Score}, Time: {TimeElapsed:g}, Difficulty: {Difficulty}, Size: {Size}x{Size}, Date: {Date.ToShortDateString()}";
        }




        // ----- COMPARERS -----


        /* Defining comparer classes to compare two GameStat objects. 
         * Descending bool is used for hightest to lowest, false could be used for low to high.
         * Reverse x's and y's to reverse orders. 
         */

        // Sort by Score
        public class ScoreComparer : IComparer<GameStat>
        {
            private bool descending;
            public ScoreComparer(bool descending = true) => this.descending = descending;

            public int Compare(GameStat x, GameStat y)
            {
                return descending
                    ? y.Score.CompareTo(x.Score)
                    : x.Score.CompareTo(y.Score);
            }
        }

        // Sort by Name
        public class NameComparer : IComparer<GameStat>
        {
            public int Compare(GameStat x, GameStat y)
            {
                return string.Compare(x.PlayerName, y.PlayerName, StringComparison.OrdinalIgnoreCase);
            }
        }

        // Sort by Date
        public class DateComparer : IComparer<GameStat>
        {
            private bool descending;
            public DateComparer(bool descending = true) => this.descending = descending;

            public int Compare(GameStat x, GameStat y)
            {
                return descending
                    ? y.Date.CompareTo(x.Date)
                    : x.Date.CompareTo(y.Date);
            }
        }
    }
}

