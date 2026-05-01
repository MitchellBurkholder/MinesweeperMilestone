using Microsoft.AspNetCore.Mvc.RazorPages;
using MinesweeperMilestone.Models;
using MinesweeperMilestone.Services;
using System.Text.RegularExpressions;

namespace MinesweeperMilestone.Services
{
    public class GameDAO : IGameDAO
    {
        private readonly string _connectionString;
        
        public GameDAO(string connString)
        {
            _connectionString = connString;
        }
        public int AddSave(SaveGame newSave)
        {

            // general idea of how the sql should look for the insert of data
            /*  INSERT INTO users 
            (UserId, DateSaved, GameData, UserInfo)
                VALUES
                (@UserId, CURRENT_DATE, @GameData, @UserInfo)*/
            throw new NotImplementedException();
        }

        public void DeleteSaveGame(SaveGame saveGame)
        {
            throw new NotImplementedException();
        }

        public List<SaveGame> GetAllSavedGames()
        {
            throw new NotImplementedException();
        }

        public SaveGame GetGameById(int id)
        {
            throw new NotImplementedException();
        }

        
    }
}
