namespace MinesweeperMilestone.Models
{
    public class ButtonModel
    {
        public int Id { get; set; }
        public int ButtonState { get; set; }
        public string ButtonImage { get; set; }

        public ButtonModel(int id, int buttonState, string buttonImage)
        {
            Id = id;
            ButtonState = buttonState;
            ButtonImage = buttonImage;
        }

        public ButtonModel()
        {

        }
    }
}
