namespace MinesweeperMilestone.Models
{
    public class UserCollection : IUserManger
    {
        private List<UserModel> _users;

        public UserCollection() 
        { 
            _users = new List<UserModel>();
            GenerateUserData();
        }

        /// <summary>
        /// Makes an admin account 
        /// </summary>
        public void GenerateUserData()
        {
            UserModel admin = new UserModel();
            admin.Username = "admin";
            admin.SetPassword("admin");
            admin.Groups = "Admin";
            AddUser(admin);
        }

        public int AddUser(UserModel userModel)
        {
            userModel.Id = _users.Count + 1;
            _users.Add(userModel);
            return userModel.Id;
        }

        public int CheckCredentials(string userName, string password)
        {
            foreach (UserModel user in _users)
            {
                if (user.Username == userName && user.VerifyPassword(password)) 
                {
                    return user.Id;
                }
            }

            return 0;
        }

        public void DeleteUser(UserModel userModel)
        {
            _users.Remove(userModel);
        }

        public List<UserModel> GetAllUsers()
        {
            return _users;
        }

        public UserModel GetUserById(int id)
        {
            return _users.Find(u => u.Id == id);
        }

        public void UpdateUser(UserModel userModel)
        {
            UserModel findUser = GetUserById(userModel.Id);

            if (findUser != null)
            {
                int index = _users.IndexOf(findUser);
                _users[index] = userModel;
            }
        }
    }
}
