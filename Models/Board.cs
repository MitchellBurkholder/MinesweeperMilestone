//using ServiceStack.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static ServiceStack.Script.Lisp;
using Newtonsoft.Json;

namespace MinesweeperMilestone.Models
{
    public class Board
    {

        // ----- PROPERTIES -----
        [DisplayName("Enter a number for the board size. The entered number will be the number you entered sqaured")]
        [Required]
        [Range(5, 15)]
        [JsonProperty("size")]
        public int size { get; set; }

        [DisplayName("Enter a number between 1 & 3. Higher the number Higher the difficulty")]
        [Required]
        [Range(1, 3)]
        [JsonProperty("difficulty")]
        public int difficulty { get; set; }
        public int numBombs { get; set; }
        public int score { get; set; }
        [JsonProperty("cells")]
        public Cell[,] cells { get; set; }
        public int rewardsRemaining { get; set; }
        public DateTime startTime { get; set; }
        public DateTime endTime { get; set; }
        public enum GameState { InProgress, Won, Lost }
        public GameState state { get; set; } = GameState.InProgress;
        
        public int powersAvailable { get; set; }




        // ----- CONSTRUCTOR ----- 

        // Added for the serializer
        public Board()
        {

        }

        /// <summary>
        /// Calls initializeBoard to setup the board.
        /// </summary>
        /// <param name="size">Size of board, e.g. 10 for a 10x10 board.</param> 
        /// <param name="difficulty">1 = easy, 2 = medium, 3 = hard.</param> 
        public Board(int size, int difficulty)
        {
            this.size = size;
            this.difficulty = difficulty;

            cells = new Cell[size, size];
            // InitializeBoard();
        }


        // Event for score updating dynamically
        public event Action<int> ScoreChanged;
        public int Score
        {
            get { return score; }
            set
            {
                score = value;
                ScoreChanged?.Invoke(score); // Notify listeners
            }
        }
        





        

    }
}
