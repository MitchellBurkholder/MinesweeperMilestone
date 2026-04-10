using Microsoft.AspNetCore.Mvc;
using MinesweeperMilestone.Models;
using System.Security.Cryptography;

namespace MinesweeperMilestone.Controllers
{
    public class ButtonController : Controller
    {
        static List<ButtonModel> buttons = new List<ButtonModel>();

        string[] buttonImages = { "button_blue.png", "button_purple.png", "button_red.png", "button_yellow.png" };

        public ButtonController()
        {
            // create a set of buttons with random images
            if (buttons.Count == 0)
            {
                for (int i = 0; i < 25; i++)
                {
                    int number = RandomNumberGenerator.GetInt32(0, 4);
                    buttons.Add(new ButtonModel(i, number, buttonImages[number]));
                }
            }
        }

        public IActionResult Index()
        {
            return View(buttons);
        }

        public IActionResult ButtonClick(int id)
        {
            ButtonModel button = buttons.FirstOrDefault(b => b.Id == id);
            if (button != null)
            {
                button.ButtonState = (button.ButtonState + 1) % 4;
                button.ButtonImage = buttonImages[button.ButtonState];
            }
            return RedirectToAction("Index");
        }
    }
}
