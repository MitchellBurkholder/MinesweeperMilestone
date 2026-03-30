
using Microsoft.Data.SqlClient;
using MinesweeperMilestone.Models;
namespace MinesweeperMilestone.Models.UserDAO { 
    public class UserDAO : IUserManger
    {
        string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog = Players; Integrated Security = True; Connect Timeout = 30; Encrypt=True;Trust Server Certificate=False;Application Intent = ReadWrite; Multi Subnet Failover=False;Command Timeout = 30";
    
        public int AddUser(UserModel userModel)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = @"INSERT INTO users (Username, UserFirstName, UserLastName,
                Sex, Age, State, EmailAddress, PasswordHash, Salt, GROUPS)
                VALUES (@Username, @UserFirstName, @UserLastName,
                @Sex, @Age, @State, @EmailAddress, @PasswordHash, @Salt, @GROUPS)
                SELECT SCOPE_IDENTITY();";

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

        public int CheckCredentials(string userName, string password)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM users WHERE Username = @Username";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Username", userName);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
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
                        Groups = reader.GetString(reader.GetOrdinal("Groups"))
                    };

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

        public List<UserModel> GetAllUsers()
        {
            List<UserModel> users = new List<UserModel>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM users";
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
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
                        Groups = reader.GetString(reader.GetOrdinal("Groups"))
                    };
                    users.Add(userModel);
                }
            }
            return users;
        }

        public UserModel GetUserById(int id)
        {

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM users WHERE Id = @Id";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", id);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
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
                        Groups = reader.GetString(reader.GetOrdinal("Groups"))
                    };
                    return userModel;
                }
            }

            return null;
        }

        public void UpdateUser(UserModel userModel)
        {
            int id = userModel.Id;
            UserModel found = GetUserById(id);
            if (found != null)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = @"UPDATE users SET @Username, @UserFirstName, @UserLastName,
                    @Sex, @Age, @State, @EmailAddress, @PasswordHash, @Salt, @GROUPS WHERE Id = @Id";
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
