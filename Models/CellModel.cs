namespace MinesweeperMilestone.Models
{
    public class CellModel
    {
        public int Id { get; set; }
        public int CellState { get; set; }
        public string CellImage { get; set; }

        public CellModel(int id, int cellState, string cellImage)
        {
            Id = id;
            CellState = cellState;
            CellImage = cellImage;
        }

        public CellModel()
        {

        }
    }
}
