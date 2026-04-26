using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using MinesweeperMilestone.Filters;
using MinesweeperMilestone.Models;

namespace MinesweeperMilestone.Controllers
{
    public class MinesweeperController : Controller
    {
        // Accepts the form data. Defaults to 5x5, Easy if no data is passed yet.
        [SessionCheckFilter]
        public IActionResult Index()
        {
            // get the board from the session
            Board gameBoard = HttpContext.Session.GetObjectFromJson<Board>("CurrentGame");

            // If it's null (first time visiting), create a default one
            if (gameBoard == null)
            {
                gameBoard = new Board(5, 1);
                HttpContext.Session.SetObjectAsJson("CurrentGame", gameBoard);
            }

            return View(gameBoard);
        }

        // method specifically for the form submission
        [HttpPost]
        public IActionResult ProcessCreate(int size, int difficulty)
        {
            // create a fresh board when the form is submitted
            Board gameBoard = new Board(size, difficulty);
            HttpContext.Session.SetObjectAsJson("CurrentGame", gameBoard);

            return RedirectToAction("Index");
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

                    // Check for bomb hit 
                    if (targetCell.isBomb)
                    {
                        // gameBoard.
                    }

                    // Save the updated board state back to the session
                    HttpContext.Session.SetObjectAsJson("CurrentGame", gameBoard);
                }
            }

            return RedirectToAction("Index");
        }

        // code that should be used for the Partail page update. 
       [HttpPost]
        public IActionResult PartialPageCellUpdate(int row, int col)
        {
            Board gameBoard = HttpContext.Session.GetObjectFromJson<Board>("CurrentGame");
            
            if (gameBoard == null)
            {
                return BadRequest();
            }
            else
            {
                // Only allow moves if the game isn't over
                if (gameBoard.CheckGameState() == Board.GameState.InProgress)
                {
                    // Grab the specific cell
                    var targetCell = gameBoard.cells[row, col];

                    // Run the existing game logic
                    gameBoard.ProcessMove(targetCell, "Visit", row, col);

                    // Check for bomb hit 
                    /*if (targetCell.isBomb)
                    {
                        // gameBoard.
                    }*/

                    // Save the updated board state back to the session
                    HttpContext.Session.SetObjectAsJson("CurrentGame", gameBoard);
                }
            }
            return PartialView("_Cells", gameBoard.cells[row, col]);
        }
    }
}