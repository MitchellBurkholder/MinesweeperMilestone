using Microsoft.AspNetCore.Mvc;
using MinesweeperMilestone.Models;

namespace MinesweeperMilestone.Controllers
{
    public class MinesweeperController : Controller
    {
        // Accepts the form data. Defaults to 5x5, Easy if no data is passed yet.
        public IActionResult Index(int size = 5, int difficulty = 1)
        {
            // Try to load an existing game from the session
            Board gameBoard = HttpContext.Session.GetObjectFromJson<Board>("CurrentGame");

            // If no game exists, or if a new size/difficulty was submitted, create a new board
            if (gameBoard == null || gameBoard.size != size || gameBoard.difficulty != difficulty)
            {
                gameBoard = new Board(size, difficulty);
                HttpContext.Session.SetObjectAsJson("CurrentGame", gameBoard);
            }

            return View(gameBoard);
        }

        // Handles the left click from the grid
        public IActionResult Visit(int row, int col)
        {
            Board gameBoard = HttpContext.Session.GetObjectFromJson<Board>("CurrentGame");

            if (gameBoard != null)
            {
                // Only allow moves if the game isn't over
                if (gameBoard.CheckGameState() == Board.GameState.InProgress)
                {
                    // Grab the specific cell
                    var targetCell = gameBoard.cells[row, col];

                    // Run the existing game logic
                    gameBoard.ProcessMove(targetCell, "Visit", row, col);

                    // Save the updated board state back to the session
                    HttpContext.Session.SetObjectAsJson("CurrentGame", gameBoard);
                }
            }

            return RedirectToAction("Index");
        }
    }
}