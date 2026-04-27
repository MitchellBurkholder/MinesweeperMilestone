using MinesweeperMilestone.Models;

namespace MinesweeperMilestone.Services
{
    public interface IUserManger
    {
        // Methods not implemented yet

        /// <summary>
        /// Gets a list of all users
        /// </summary>
        /// <returns></returns>
        public List<UserModel> GetAllUsers();

        /// <summary>
        /// Return a UserModel via their ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public UserModel GetUserById(int id);

        /// <summary>
        /// Adds a new user
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns></returns>
        public int AddUser(UserModel userModel);

        /// <summary>
        /// Deletes a User
        /// </summary>
        /// <param name="userModel"></param>
        public void DeleteUser(UserModel userModel);

        /// <summary>
        /// Updates the attributes of a user
        /// </summary>
        /// <param name="userModel"></param>
        public void UpdateUser(UserModel userModel);

        /// <summary>
        /// Verifies the credentials of a user
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public int CheckCredentials(string userName, string password);
    }
}
