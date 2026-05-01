using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using MinesweeperMilestone.Extensions;
using MinesweeperMilestone.Filters;
using MinesweeperMilestone.Models;
using MinesweeperMilestone.Services;
using Newtonsoft.Json;
using System.Reflection;
using System.Runtime.InteropServices.JavaScript;

namespace MinesweeperMilestone.Controllers
{
    public class MinesweeperController : Controller
    {
        // this is for saving the game to the database, so we can pull the connection string from appsettings.json
        private readonly IConfiguration _config;

        // ditto
        public MinesweeperController(IConfiguration config)
        {
            _config = config;
        }

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
            Board gameBoard = new Board(size, difficulty);

            GameService gameService = new GameService(gameBoard);

            gameService.InitializeBoard();

            HttpContext.Session.SetObjectAsJson("CurrentGame", gameBoard);

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        // Handles the right click from the grid
        public IActionResult Flag(int row, int col)
        {
            Board gameBoard = HttpContext.Session.GetObjectFromJson<Board>("CurrentGame");

            if (gameBoard != null)
            {
                GameService gameService = new GameService(gameBoard);

                if (gameService.CheckGameState() == Board.GameState.InProgress)
                {
                    var targetCell = gameBoard.cells[row, col];

                    gameService.ProcessMove(targetCell, "Flag", row, col);

                    HttpContext.Session.SetObjectAsJson("CurrentGame", gameBoard);
                }
            }

            return PartialView("_board", gameBoard);
        }

        // Handles the left click from the grid (updating the individual cell, not the whole page)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult PartialPageCellUpdate(int row, int col)
        {
            Board gameBoard = HttpContext.Session.GetObjectFromJson<Board>("CurrentGame");

            if (gameBoard == null)
            {
                return BadRequest();
            }
            else
            {
                GameService gameService = new GameService(gameBoard);

                if (gameService.CheckGameState() == Board.GameState.InProgress)
                {
                    var targetCell = gameBoard.cells[row, col];

                    gameService.ProcessMove(targetCell, "Visit", row, col);

                    HttpContext.Session.SetObjectAsJson("CurrentGame", gameBoard);
                }
            }
                  
            return PartialView("_board", gameBoard);
                
        }

        // Gets the list of saved games for the current user and sends it to the View to be displayed in a table
        [HttpGet]
        public IActionResult LoadGame()
        {
            // Figure out who is logged in
            UserModel currentUser = HttpContext.Session.GetObjectFromJson<UserModel>("User");
            if (currentUser == null) return RedirectToAction("Login", "User");

            // Get ONLY their games from the DB
            string connString = _config.GetConnectionString("DefaultConnection");
            UserDAO dao = new UserDAO(connString);
            List<SaveGame> savedGames = dao.GetSavedGames(currentUser.Id);

            // Send the list to the View
            return View(savedGames);
        }

        // Gets the game data from the DB, converts it back into a Board object, puts it in the session, and redirects to the game page
        [HttpPost]
        public IActionResult ProcessLoadGame(int gameId)
        {
            string connString = _config.GetConnectionString("DefaultConnection");
            UserDAO dao = new UserDAO(connString);

            // Get JSON string from DB
            string gameJson = dao.GetGameData(gameId);

            if (!string.IsNullOrEmpty(gameJson))
            {
                // Convert it back into a Board object
                Board loadedBoard = JsonConvert.DeserializeObject<Board>(gameJson);

                // put it in the  session and go to the game screen
                HttpContext.Session.SetObjectAsJson("CurrentGame", loadedBoard);
                return RedirectToAction("Index");
            }

            return RedirectToAction("LoadGame");
        }

        // Deletes the game from the database, then refreshes the page to update the list
        [HttpPost]
        public IActionResult ProcessDeleteGame(int gameId)
        {
            string connString = _config.GetConnectionString("DefaultConnection");
            UserDAO dao = new UserDAO(connString);

            dao.DeleteGame(gameId);

            // Refresh the page so the deleted game disappears from the table
            return RedirectToAction("LoadGame");
        }

        [HttpPost]
        public IActionResult SaveGame()
        {
            // Get the game board and the user
            Board gameBoard = HttpContext.Session.GetObjectFromJson<Board>("CurrentGame");
            UserModel currentUser = HttpContext.Session.GetObjectFromJson<UserModel>("User");

            if (gameBoard != null && currentUser != null)
            {
                // Serialize both objects into JSON
                string gameDataJson = JsonConvert.SerializeObject(gameBoard);
                string userInfoJson = JsonConvert.SerializeObject(currentUser);

                // Send to database
                string connectionString = _config.GetConnectionString("DefaultConnection"); // important for secrets.json
                UserDAO dao = new UserDAO(connectionString);
                dao.SaveCurrentGame(currentUser.Id, gameDataJson, userInfoJson);

                // success message
                TempData["SaveMessage"] = "Game saved successfully!";
            }

            // Redirect back to the game page (should continue the game)
            return RedirectToAction("Index");
        }
    }
}