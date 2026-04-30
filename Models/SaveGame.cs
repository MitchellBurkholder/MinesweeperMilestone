namespace MinesweeperMilestone.Models
{
    public class SaveGame : GameStat
    {
        int Id { get; set; }
        Board GameBoard {  get; set; }
        public SaveGame(int id, string playerName, Board gameBoard, DateTime date, TimeSpan timeElapsed) {
            Id = id;
            this.PlayerName = playerName;
            GameBoard = gameBoard;
            this.Date = date;
            this.TimeElapsed = timeElapsed;
        }

        public SaveGame()
        {
           
        }
    }
}
