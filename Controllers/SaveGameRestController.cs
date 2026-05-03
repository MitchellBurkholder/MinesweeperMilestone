using Microsoft.AspNetCore.Mvc;
using MinesweeperMilestone.Models;
using MinesweeperMilestone.Services;

namespace MinesweeperMilestone.Controllers
{
    [ApiController]
    [Route("api/showSavedGames")]
    public class SaveGameRestController : ControllerBase
    {
        // this is for saving the game to the database, so we can pull the connection string from appsettings.json
        private readonly IConfiguration _config;
        private readonly GameDAO _gameDAO;

        public SaveGameRestController(IConfiguration config, GameDAO gameDAO)
        {
            _config = config;
            _gameDAO = gameDAO;
        }

        /// <summary>
        /// Gets all the save games from the database then is used to display them in json format on a webpage
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [HttpGet]
        public Task<ActionResult<IEnumerable<SaveGame>>> ShowAllSaveGames()
        {
            IEnumerable<SaveGame> saveGames = _gameDAO.GetAllSavedGames();
            //return Ok(saveGames);
            throw new NotImplementedException();
        }

        [HttpGet("{id}")]
        public Task<ActionResult<SaveGame>> GetSaveGameById(int id)
        {
            /*var saveGame = _gameDAO.GetGameById(id);
            /if (saveGame == null)
            {
                return NotFound();
            }
           return Ok(saveGame);*/
            throw new NotImplementedException();
        }

        [Route("api/deleteOneGame")]
        [HttpDelete("{id}")]
        public Task<ActionResult<SaveGame>> DeleteSaveGameById(int id) { 
            // wait for implementation to see what to do
            throw new NotImplementedException(); 
        }
    }
}
