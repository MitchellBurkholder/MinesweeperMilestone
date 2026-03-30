using Microsoft.AspNetCore.Mvc;
using MinesweeperMilestone.Filters;
using MinesweeperMilestone.Models;
using MinesweeperMilestone.Models.UserDAO;

namespace MinesweeperMilestone.Controllers
{
    public class UserController : Controller
    {
        static UserDAO users = new UserDAO();
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ProcessLogin(string Username, string password)
        {         

            UserModel userData = new UserModel { Id = 1, Username = Username, Password = password };

            if (users.CheckCredentials(Username, password) > 0)
            {
                string userJson = ServiceStack.Text.JsonSerializer.SerializeToString(userData);
                HttpContext.Session.SetString("User", userJson);
                return View("LoginSuccess", userData);
            }
            else
            {
                return View("LoginFailure", userData);
            }
        }

        [SessionCheckFilter]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("User");
            return View("Index");
        }

        public IActionResult Register()
        {
            return View(new RegisterViewModel());
        }

        public IActionResult ProcessRegister(RegisterViewModel registerViewModel)
        {
            UserModel user = new UserModel();
            user.Username = registerViewModel.Username;
            user.SetPassword(registerViewModel.Password);
            user.Groups = "user";
            user.Groups = user.Groups.TrimEnd(',');
            users.AddUser(user);

            return View("Index");
        }
    }
}
