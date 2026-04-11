using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static ServiceStack.Script.Lisp;

namespace MinesweeperMilestone.Models
{
    public class Board
    {

        // ----- PROPERTIES -----

        public int size { get; set; }
        public int difficulty { get; set; }
        public int numBombs { get; set; }
        public int score { get; set; }
        public Cell[,] cells { get; set; }
        public int rewardsRemaining { get; set; }
        public DateTime startTime { get; set; }
        public DateTime endTime { get; set; }
        public enum GameState { InProgress, Won, Lost }
        private static Random rand = new Random();
        public int powersAvailable { get; private set; }




        // ----- CONSTRUCTOR ----- 

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
            InitializeBoard();
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





        // ----- SETUP METHODS -----

        /// <summary> Call the methods to setup the board. 
        /// </summary>
        /// <remarks>
        /// Intializes all cells in the grid.
        /// SetupBombs places bombs on the board based on difficulty.
        /// SetupPowers places power-ups on the board based on difficulty.
        /// CalculateNumBombNeighbors calculates the number of bomb neighbors for each cell.
        /// Start Time is set to current time.
        /// </remarks>
        private void InitializeBoard()
        {
            // Initialize all cells
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    cells[i, j] = new Cell();
                }
            }
            SetupBombs();
            SetupPowers();
            CalculateNumBombNeighbors();
            startTime = DateTime.Now;
        }

        /// <summary> Calculate number of bomb neighbors for *every* cell.
        /// </summary>
        /// <remarks>
        /// Looping through each cell in the grid and calling GetNumBombNeighbors.
        /// Used for setting up the board before exposing it to the player. 
        /// Slowly revealed as the player visits cells.
        /// </remarks>
        private void CalculateNumBombNeighbors()
        {
            // Loop through each cell in the grid
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    cells[i, j].numBombNeighbors = GetNumBombNeighbors(i, j);
                }
            }
        }

        /// <summary> Determine bomb placement based on difficulty level.
        /// </summary>
        /// <remarks>
        /// Cases corrspond to difficulty levels - easy, medium, hard.
        /// each difficulty level has a different percentage of bombs based on the size of the board.
        /// The bigger the board, the more bombs (as a %). 
        /// When correct # of bombs are placed, the method returns.
        /// </remarks>
        /// <exception cref="ArgumentException"></exception>
        private void SetupBombs()
        {
            numBombs = 0;
            switch (difficulty)
            {
                // Difficulty levels:
                case 1: // Easy
                    numBombs = (int)(size * size * 0.05); // 10% of cells are bombs
                    break;
                case 2: // Medium
                    numBombs = (int)(size * size * 0.15); // 15% of cells are bombs
                    break;
                case 3: // Hard
                    numBombs = (int)(size * size * 0.25); // 20% of cells are bombs
                    break;
                default:
                    throw new ArgumentException("Invalid difficulty level.");
            }

            // Randomly place bombs on the board
            int bombsPlaced = 0;
            while (bombsPlaced < numBombs)
            {
                int row = rand.Next(size);
                int col = rand.Next(size);

                // Place bomb (if the cell is not already a bomb)
                if (!cells[row, col].isBomb)
                {
                    cells[row, col].isBomb = true;
                    bombsPlaced++;
                }
            }
        }

        /// <summary> Setup Power cells based on difficulty level.
        /// </summary>
        /// <remarks>
        /// Higher difficulty levels have more power-ups.
        /// Players can't use powers until they are claimed from a visited cell
        /// </remarks>
        /// <exception cref="ArgumentException"></exception>
        private void SetupPowers()
        {
            int placed = 0;
            int count = 0; // Number of powers to place
            switch (difficulty)
            {
                case 1: // Easy
                    count = 1;
                    break;
                case 2: // Medium
                    count = 3;
                    break;
                case 3: // Hard
                    count = 5;
                    break;
                default:
                    throw new ArgumentException("Invalid Powers (check difficulty).");
            }

            while (placed < count)
            {
                int row = rand.Next(size);
                int col = rand.Next(size);

                if (!cells[row, col].isBomb && !cells[row, col].hasReward)
                {
                    cells[row, col].hasReward = true;
                    placed++;
                }
            }
        }






        // ----- USER METHODS -----

        /// <summary> Use a Powerup to peek at a cell.
        /// </summary>
        /// <remarks>
        /// This is called when the player uses a powerup to see if a cell is a bomb.
        /// It returns the answer, which the player can read. It does not visit the cell.
        /// If it's safe, they can choose to visit it, if it's a bomb, they can flag it. 
        /// Good idea for those cells that you are not sure about.
        /// </remarks>
        /// <param name="cell">the target cell to peek at.</param> 
        /// <returns>returns if the cell is a bomb or not</returns>
        public string UsePower(Cell cell)
        {
            // If no powers
            if (powersAvailable <= 0)
            {
                return "No powerups remaining.";
            }
            // Decrement
            powersAvailable--;

            // If its a bomb
            if (cell.isBomb)
            {
                return "Peek result: Bomb";
            }
            // If its a powerup
            else if (cell.hasReward)
            {
                return "Peek result: Powerup";
            }
            // If its safe
            else
            {
                return $"Peek result: Safe (Neighbors = {cell.numBombNeighbors})";
            }
        }


        /// <summary> Collect a power-up from a visited cell.
        /// </summary>
        /// <remarks>
        /// Helper method to increase the number of powers available.
        /// Runs when a player visits a cell with a power-up.
        /// </remarks>
        public void CollectPower()
        {
            powersAvailable++;
            Score += 50;
        }

        /// <summary>
        /// Toggles the flagged state of the specified cell. 
        /// </summary>
        /// <remarks>
        /// Helper method for toggling the flag on a cell in ProcessMove
        /// </remarks>
        /// <param name="cell">The cell whose flagged state is to be toggled. Must not be null.</param>
        private void ToggleFlag(Cell cell)
        {
            if (cell.isVisited)
            {
                return;
            }
            cell.isFlagged = !cell.isFlagged;
        }

        /// <summary> Process a player's move on the board.
        /// </summary>
        /// <remarks>
        /// Runs in the game loop continuously.
        /// </remarks>
        /// <param name="cell">target cell to process the move on.</param> 
        /// <param name="moveType">Type of move: "Visit", "Flag", or "Power".</param> 
        /// <param name="row">Row index of the cell to process the move on.</param> 
        /// <param name="col">Column index of the cell to process the move on.</param> 
        public MoveResult ProcessMove(Cell cell, string moveType, int row, int col)
        {
            // If invalid
            if (!IsCellOnBoard(row, col))
            {
                return new MoveResult
                {
                    success = false,
                    message = "Move is out of bounds."
                };
            }

            // Determine the type of move
            switch (moveType)
            {
                case "Visit":
                    if (cell.isVisited)
                    {
                        return new MoveResult
                        {
                            success = false,
                            message = $"Cell ({row}, {col}) is already visited."
                        };
                    }

                    bool rewardCollected = FloodFill(row, col);

                    if (!cell.isBomb && !cell.isFlagged)
                    {
                        Score += 1;
                    }
                    return new MoveResult
                    {
                        success = true,
                        message = $"Visited cell at ({row}, {col}).",
                        collectedPower = rewardCollected,
                        hitBomb = cell.isBomb
                    };


                case "Flag":
                    ToggleFlag(cell);
                    return new MoveResult
                    {
                        success = true,
                        message = cell.isFlagged
                            ? $"Flag placed at ({row}, {col})."
                            : $"Flag removed at ({row}, {col})."
                    };

                case "Power":
                    return new MoveResult
                    {
                        success = true,
                        message = UsePower(cell)
                    };
                default:
                    return new MoveResult
                    {
                        success = false,
                        message = "Invalid move type."
                    };
            }
        }

        /// <summary> Visit a cell on the board.
        /// </summary>
        /// <remarks>
        /// Helper method to process a visit to a cell.
        /// Handles if its flagged, has a power, is a bomb, or is safe. 
        /// </remarks>
        /// <param name="cell">The cell to visit.</param> 
        /// <param name="row">Row index of the cell to visit.</param> 
        /// <param name="col">Column index of the cell to visit.</param> 
        private MoveResult VisitCell(Cell cell, int row, int col)
        {
            if (cell.isVisited || cell.isFlagged)
            {
                return new MoveResult
                {
                    success = false,
                    message = "Cell already visited or flagged."
                };
            }

            if (cell.hasReward)
            {
                CollectPower();
                cell.hasReward = false; // Remove the power after collecting
                cell.isVisited = true;
                return new MoveResult
                {
                    success = true,
                    collectedPower = true,
                    message = "Power collected!"
                };
            }

            if (cell.isBomb)
            {
                cell.isVisited = true;
                return new MoveResult
                {
                    success = true,
                    message = "Hit a bomb! Game over.",
                    hitBomb = true
                };
            }

            if (cell.numBombNeighbors > 0)
            {
                cell.isVisited = true;
                return new MoveResult
                {
                    success = true,
                    neighborCount = cell.numBombNeighbors,
                    message = $"Visited cell at ({row}, {col}) with {cell.numBombNeighbors} bomb neighbors."
                };
            }
            else
            {
                // If the cell has no bomb neighbors, flood fill
                FloodFill(row, col);
                return new MoveResult
                {
                    success = true,
                    message = "Flood fill executed."
                };
            }
        }









        // ----- GAME LOGIC METHODS -----

        /// <summary> Determines the final score based on game state and time.
        /// </summary>
        /// <returns></returns>
        public int DetermineFinalScore()
        {
            int finalScore = score;

            switch (size)
            {
                case 8:
                    {
                        finalScore += 30;
                        break;
                    }
                case 12:
                    {
                        finalScore += 50;
                        break;
                    }
                case 16:
                    {
                        finalScore += 100;
                        break;
                    }
            }

            switch (difficulty)
            {
                case 1:
                    {
                        finalScore += 30;
                        break;
                    }
                case 2:
                    {
                        finalScore += 50;
                        break;
                    }
                case 3:
                    {
                        finalScore += 100;
                        break;
                    }
            }
            return finalScore;
        }

        /// <summary> Fill in nearby cells with 0 bomb neighbors. 
        /// </summary>
        /// <remarks>
        /// Pulls a target cell, then recursively iterates until no more open neighbors. 
        /// Simulates how minesweeper usually works (does not touch corners, instant with no delay). 
        /// Theoretically, the game would work without it, but it makes it more fun.
        /// </remarks>
        /// <param name="row">The row of the cell to start the flood fill.</param> 
        /// <param name="col">the col of the cell to start the flood fill.</param> 
        private bool FloodFill(int row, int col)
        {
            if (!IsCellOnBoard(row, col)) return false;

            if (cells[row, col].isVisited) return false;

            bool collectedReward = RevealCell(row, col);

            Cell current = cells[row, col];

            // Only expand if no bomb neighbors
            if (current.numBombNeighbors == 0 && !current.isBomb)
            {
                for (int dr = -1; dr <= 1; dr++) // delta row
                {
                    for (int dc = -1; dc <= 1; dc++) // delta col
                    {
                        if (dr == 0 && dc == 0) continue;

                        int newRow = row + dr;
                        int newCol = col + dc;

                        if (IsCellOnBoard(newRow, newCol))
                        {
                            // if a neighbor collected a reward, bubble it
                            if (FloodFill(newRow, newCol))
                            {
                                collectedReward = true;
                            }
                        }
                    }
                }
            }

            return collectedReward;
        }



        /// <summary>
        /// Reveals a single cell, collecting rewards if present.
        /// Returns true if a reward was collected.
        /// </summary>
        public bool RevealCell(int row, int col)
        {
            if (!IsCellOnBoard(row, col)) return false;

            Cell cell = cells[row, col];

            if (cell.isVisited || cell.isBomb) return false;

            bool collectedReward = false;

            if (cell.hasReward)
            {
                CollectPower();
                cell.hasReward = false;
                collectedReward = true;
            }

            cell.isVisited = true;
            Score += 1;

            return collectedReward;
        }



        /// <summary> Check if a cell is a bomb.
        /// 
        /// </summary>
        /// <param name="cell">target cell</param>
        /// <returns>returns if the cell is a bomb or not</returns>
        public bool IsBomb(Cell cell)
        {
            // Check if the cell is a bomb
            if (cell.isBomb)
            {
                bool loss = true;
                return loss;
            }
            return false;
        }

        /// <summary> Check if the game is over, won, or in progress.
        /// </summary>
        /// <returns>returns a gamestate</returns>
        public GameState CheckGameState()
        {
            // Game Over Check
            foreach (Cell cell in cells)
            {
                if (cell.isVisited && cell.isBomb)
                { return GameState.Lost; }
            }

            // Win Check
            int revealedCount = 0;
            foreach (Cell cell in cells)
            {
                if (cell.isVisited) { revealedCount++; }
            }
            int safeCount = size * size - numBombs;
            if (revealedCount == safeCount)
            { return GameState.Won; } // Player revealed all safe cells 

            // Game In Progress
            return GameState.InProgress;
        }

        /// <summary> Check if a cell is within the bounds of the board.
        /// </summary>
        /// <remarks>
        /// This is used to ensure that we don't access cells outside the grid.
        /// That would be relevant when checking neighbors of a cell for bombs.
        /// Edge cells would have neighbors that are outside the grid. 
        /// This method prevents that. 
        /// </remarks>
        /// <param name="row">row index of the cell to check.</param> 
        /// <param name="col">column index of the cell to check.</param> 
        /// <returns>returns true or false if a cell is on the board</returns>
        private bool IsCellOnBoard(int row, int col)
        {
            // Row and column must be within the bounds of the grid
            return row >= 0 && row < size && col >= 0 && col < size;
            // Returns true if cell is within grid bounds
        }

        /// <summary> Get # of bomb neighbors for a *specific* cell.
        /// </summary>
        /// <remarks>
        /// Loop through the 3x3 grid around the cell and count bombs.
        /// Skip the center cell (itself).
        /// Returns the count of bomb neighbors.
        /// Used in a loop inside CalculateNumBombNeighbors.
        /// </remarks>
        /// <param name="row">row index of the cell to check.</param> 
        /// <param name="col">column index of the cell to check.</param> 
        /// <returns>returns int count of bomb neighbors</returns>
        private int GetNumBombNeighbors(int row, int col)
        {
            int count = 0;
            // Loop through the 3x3 (8) grid around the cell
            for (int i = row - 1; i <= row + 1; i++)
            {
                for (int j = col - 1; j <= col + 1; j++)
                {
                    // Skip center cell
                    if (i == row && j == col) continue;
                    // Check if neighbor exists and is bomb
                    if (IsCellOnBoard(i, j) && cells[i, j].isBomb)
                    {
                        count++;
                    }
                }
            }
            return count;
        }

    }
}
