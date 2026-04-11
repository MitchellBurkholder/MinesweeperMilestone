namespace MinesweeperMilestone.Services
{
    public class SecurityDAO
    {
        private readonly string connectionString;

        // The DAO asks for the string when it gets created
        public SecurityDAO(string connString)
        {
            connectionString = connString;
        }
    }
}