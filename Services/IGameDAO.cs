using MinesweeperMilestone.Models;

namespace MinesweeperMilestone.Services
{
    public interface IGameDAO
    {
        public List<SaveGame> GetAllSavedGames();

        /// <summary>
        /// Return a UserModel via their ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SaveGame GetGameById(int id);

        /// <summary>
        /// Adds a new user
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns></returns>
        public int AddSave(SaveGame newSave);

        /// <summary>
        /// Deletes a User
        /// </summary>
        /// <param name="userModel"></param>
        public void DeleteSaveGame(SaveGame saveGame);
    }
}
