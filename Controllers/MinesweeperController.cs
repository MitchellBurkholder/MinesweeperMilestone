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
        public IActionResult Reveal(int row, int col)
        {
            // Grab the board out of memory
            Board gameBoard = HttpContext.Session.GetObjectFromJson<Board>("CurrentGame");

            if (gameBoard != null)
            {
                // TODO: You will call your board's logic here! 
                // e.g., gameBoard.RevealCell(row, col);
                // gameBoard.CheckForWinOrLoss();

                // For right now, let's just forcefully alter the cell's state to test the UI:
                // gameBoard.cells[row, col].IsRevealed = true; // (Assuming your cell has this property)

                // Save the updated board back to the session
                HttpContext.Session.SetObjectAsJson("CurrentGame", gameBoard);
            }

            // Refresh the page
            return RedirectToAction("Index");
        }
    }
}