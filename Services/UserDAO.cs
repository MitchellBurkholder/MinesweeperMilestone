using Microsoft.Data.SqlClient;
using MinesweeperMilestone.Models;

namespace MinesweeperMilestone.Services
{
    public class UserDAO : IUserManger
    {
        // private so it can't be accidentally changed outside this class
        private readonly string connectionString;

        // constructor to catch the string passed from UserController
        public UserDAO(string connString)
        {
            connectionString = connString;
        }

        // method to save the current game data to the database
        public bool SaveCurrentGame(int userId, string gameDataJson, string userInfoJson)
        {
            bool success = false;
            string connectionString = this.connectionString;
            string sqlStatement = "INSERT INTO dbo.Games (UserID, DateSaved, GameData, UserInfo) VALUES (@UserId, @DateSaved, @GameData, @UserInfo)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(sqlStatement, connection);

                command.Parameters.AddWithValue("@UserId", userId);
                command.Parameters.AddWithValue("@DateSaved", DateTime.Now);
                command.Parameters.AddWithValue("@GameData", gameDataJson);
                command.Parameters.AddWithValue("@UserInfo", userInfoJson);

                try
                {
                    // Open the connection and execute the command
                    connection.Open();
                    command.ExecuteNonQuery();
                    success = true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return success;
        }

        /// <summary>
        /// Adds a user to the database
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns></returns>
        public int AddUser(UserModel userModel)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // SQL Connection and query prep
                connection.Open();
                string query = @"
                INSERT INTO users 
                (Username, UserFirstName, UserLastName, Sex, Age, State, EmailAddress, PasswordHash, Salt, [GROUPS])
                VALUES 
                (@Username, @UserFirstName, @UserLastName,@Sex, @Age, @State, @EmailAddress, @PasswordHash, @Salt, @GROUPS)
                SELECT SCOPE_IDENTITY();";

                // INSERT statements
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", userModel.Username);
                    command.Parameters.AddWithValue("@UserFirstName", userModel.UserFirstName);
                    command.Parameters.AddWithValue("@UserLastName", userModel.UserLastName);
                    command.Parameters.AddWithValue("@Sex", userModel.Sex);
                    command.Parameters.AddWithValue("@Age", userModel.Age);
                    command.Parameters.AddWithValue("@State", userModel.State);
                    command.Parameters.AddWithValue("@EmailAddress", userModel.EmailAddress);
                    command.Parameters.AddWithValue("@PasswordHash", userModel.PasswordHash);
                    command.Parameters.AddWithValue("@Salt", userModel.Salt);
                    command.Parameters.AddWithValue("@GROUPS", userModel.Groups);

                    object result = command.ExecuteScalar();
                    return Convert.ToInt32(result);
                }

            }

        }

        /// <summary>
        /// Checks to see if the user exist in the database
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public int CheckCredentials(string userName, string password)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // SQL connection and query
                connection.Open();
                string query = "SELECT * FROM users WHERE Username = @Username";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Username", userName);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    // Create the new user
                    UserModel userModel = new UserModel
                    {
                        Id = reader.GetInt32(0),
                        Username = reader.GetString(reader.GetOrdinal("Username")),
                        UserFirstName = reader.GetString(reader.GetOrdinal("UserFirstName")),
                        UserLastName = reader.GetString(reader.GetOrdinal("UserLastName")),
                        Sex = reader.GetString(reader.GetOrdinal("Sex")),
                        Age = reader.GetInt32(5),
                        State = reader.GetString(reader.GetOrdinal("State")),
                        EmailAddress = reader.GetString(reader.GetOrdinal("EmailAddress")),
                        PasswordHash = reader.GetString(reader.GetOrdinal("PasswordHash")),
                        Salt = (byte[])reader["Salt"],
                        Groups = reader.GetString(reader.GetOrdinal("GROUPS"))
                    };

                    // Verify the new user
                    bool valid = userModel.VerifyPassword(password);
                    if (valid)
                    {
                        return userModel.Id;
                    }
                    else
                    {
                        return 0;
                    }
                }
                return 0;
            }
        }

        /// <summary>
        /// Gets rid of a user
        /// </summary>
        /// <param name="userModel"></param>
        public void DeleteUser(UserModel userModel)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "DELETE FROM users WHERE Id = @Id";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", userModel.Id);
                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Gets all the users 
        /// </summary>
        /// <returns></returns>
        public List<UserModel> GetAllUsers()
        {
            List<UserModel> users = new List<UserModel>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // SQL Connection and query
                connection.Open();
                string query = "SELECT * FROM users";
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    // retrieve userModel data
                    // Creates a new list that adds all the found users to it
                    UserModel userModel = new UserModel
                    {
                        Id = reader.GetInt32(0),
                        Username = reader.GetString(reader.GetOrdinal("Username")),
                        UserFirstName = reader.GetString(reader.GetOrdinal("UserFirstName")),
                        UserLastName = reader.GetString(reader.GetOrdinal("UserLastName")),
                        Sex = reader.GetString(reader.GetOrdinal("Sex")),
                        Age = reader.GetInt32(5),
                        State = reader.GetString(reader.GetOrdinal("State")),
                        EmailAddress = reader.GetString(reader.GetOrdinal("EmailAddress")),
                        PasswordHash = reader.GetString(reader.GetOrdinal("PasswordHash")),
                        Salt = (byte[])reader["Salt"],
                        Groups = reader.GetString(reader.GetOrdinal("GROUPS"))
                    };
                    users.Add(userModel);
                }
            }
            return users;
        }

        /// <summary>
        /// Fetches user based on the parameter 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public UserModel GetUserById(int id)
        {

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // SQL Connection and query
                connection.Open();
                string query = "SELECT * FROM users WHERE Id = @Id";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", id);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    // retrieve userModel data 
                    UserModel userModel = new UserModel
                    {
                        Id = reader.GetInt32(0),
                        Username = reader.GetString(reader.GetOrdinal("Username")),
                        UserFirstName = reader.GetString(reader.GetOrdinal("UserFirstName")),
                        UserLastName = reader.GetString(reader.GetOrdinal("UserLastName")),
                        Sex = reader.GetString(reader.GetOrdinal("Sex")),
                        Age = reader.GetInt32(5),
                        State = reader.GetString(reader.GetOrdinal("State")),
                        EmailAddress = reader.GetString(reader.GetOrdinal("EmailAddress")),
                        PasswordHash = reader.GetString(reader.GetOrdinal("PasswordHash")),
                        Salt = (byte[])reader["Salt"],
                        Groups = reader.GetString(reader.GetOrdinal("GROUPS"))
                    };
                    return userModel;
                }
            }
            return null;
        }

        /// <summary>
        /// Updates information about a user 
        /// </summary>
        /// <param name="userModel"></param>
        public void UpdateUser(UserModel userModel)
        {
            // Find the user by ID
            int id = userModel.Id;
            UserModel found = GetUserById(id);
            if (found != null)
            {
                // SQL Connection and UPDATE command
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = @"UPDATE users SET Username = @Username, UserFirstName = @UserFirstName, UserLastName = @UserLastName,
                    Sex = @Sex, Age = @Age, State = @State, EmailAddress = @EmailAddress, PasswordHash = @PasswordHash, Salt = @Salt, GROUPS = @GROUPS WHERE Id = @Id";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Username", userModel.Username);
                    command.Parameters.AddWithValue("@UserFirstName", userModel.UserFirstName);
                    command.Parameters.AddWithValue("@UserLastName", userModel.UserLastName);
                    command.Parameters.AddWithValue("@Sex", userModel.Sex);
                    command.Parameters.AddWithValue("@Age", userModel.Age);
                    command.Parameters.AddWithValue("@State", userModel.State);
                    command.Parameters.AddWithValue("@EmailAddress", userModel.EmailAddress);
                    command.Parameters.AddWithValue("@PasswordHash", userModel.PasswordHash);
                    command.Parameters.AddWithValue("@Salt", userModel.Salt);
                    command.Parameters.AddWithValue("@GROUPS", userModel.Groups);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}