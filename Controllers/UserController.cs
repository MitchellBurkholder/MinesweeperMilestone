using Microsoft.AspNetCore.Mvc;
using MinesweeperMilestone.Filters;
using MinesweeperMilestone.Models;
using Microsoft.Extensions.Configuration;
using MinesweeperMilestone.Services; 

namespace MinesweeperMilestone.Controllers
{
    public class UserController : Controller
    {
        
        private readonly UserDAO users;

        // constructor to grab the configuration
        public UserController(IConfiguration config)
        {
            // Pull the string out of appsettings.json (User Secrets for Jackson's Mac)
            string connString = config.GetConnectionString("DefaultConnection");

            // Pass it to the DAO
            users = new UserDAO(connString);
        }

        /// <summary>
        /// Launches the login page
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Deals with the processing of login information
        /// </summary>
        /// <param name="Username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public IActionResult ProcessLogin(string Username, string password)
        {

            UserModel userData = new UserModel { Id = 1, Username = Username, Password = password };

            if (users.CheckCredentials(Username, password) > 0)
            {
                string userJson = ServiceStack.Text.JsonSerializer.SerializeToString(userData);
                HttpContext.Session.SetString("User", userJson);
                return View("LoginSuccess");
            }
            else
            {
                return View("LoginFailure");
            }
        }

        /// <summary>
        /// Removes active users on the site 
        /// </summary>
        /// <returns></returns>
        [SessionCheckFilter]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("User");
            return View("Index");
        }

        /// <summary>
        /// Launches the account register page
        /// </summary>
        /// <returns></returns>
        public IActionResult Register()
        {
            return View(new RegisterViewModel());
        }

        /// <summary>
        /// Processes the information that comes from the registerViewModel
        /// </summary>
        /// <param name="registerViewModel"></param>
        /// <returns></returns>
        public IActionResult ProcessRegister(RegisterViewModel registerViewModel)
        {
            UserModel user = new UserModel();
            user.Username = registerViewModel.Username;
            user.UserFirstName = registerViewModel.UserFirstName;
            user.UserLastName = registerViewModel.UserLastName;
            user.SetPassword(registerViewModel.Password);
            user.Age = registerViewModel.Age;
            user.Sex = registerViewModel.Sex;
            user.State = registerViewModel.State;
            user.EmailAddress = registerViewModel.EmailAddress;
            user.Groups = "User";

            users.AddUser(user);

            return View("Index");
        }

        /// <summary>
        /// Launches the StartGame page
        /// </summary>
        /// <returns></returns>
        [SessionCheckFilter]
        public IActionResult StartGame()
        {
            return View();
        }
    }
}