using ServiceStack;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MinesweeperMilestone.Services
{
    public class SecurityDAO
    {
        string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog = Players; Integrated Security = True; Connect Timeout = 30; Encrypt=True;Trust Server Certificate=False;Application Intent = ReadWrite; Multi Subnet Failover=False;Command Timeout = 30";
    }
}
