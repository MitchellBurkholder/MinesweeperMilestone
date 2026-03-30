namespace MinesweeperMilestone.Models
{
    public interface IUserManger
    {
        public List<UserModel> GetAllUsers();
        public UserModel GetUserById(int id);
        public int AddUser(UserModel userModel);
        public void DeleteUser(UserModel userModel);
        public void UpdateUser(UserModel userModel);
        public int CheckCredentials(string userName, string password);
    }
}
