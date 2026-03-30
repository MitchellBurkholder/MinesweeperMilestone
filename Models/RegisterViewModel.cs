namespace MinesweeperMilestone.Models
{
    public class RegisterViewModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }
        public string Sex { get; set; }
        public int Age { get; set; }
        public string State { get; set; }
        public string EmailAddress { get; set; }

        public RegisterViewModel()
        {
            Username = "";
            Password = "";
            UserFirstName = "";
            UserLastName = "";
            Sex = string.Empty;
            State = string.Empty;
            EmailAddress = string.Empty;
            
        }
    }
}
