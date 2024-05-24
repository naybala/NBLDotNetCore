using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBLDotNetCore.PizzaApi
{
    internal class ConnectionStrings
    {
        public static SqlConnectionStringBuilder SqlConnectionStringBuilder = new SqlConnectionStringBuilder()
        {
            DataSource = "DAVION",
            InitialCatalog = "DotNetTrainning",
            UserID = "sa",
            Password = "sa@123",
            TrustServerCertificate = true
        };

    }
}
