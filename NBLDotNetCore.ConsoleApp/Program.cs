using System.Data.SqlClient;

Console.WriteLine("Hello, World!");
SqlConnectionStringBuilder stringBuilder = new SqlConnectionStringBuilder();
stringBuilder.DataSource = "DAVION"; //server name
stringBuilder.InitialCatalog = "DotNetTrainning";
stringBuilder.UserID = "sa";
stringBuilder.Password = "sa@123";
SqlConnection connection = new SqlConnection(stringBuilder.ConnectionString);
connection.Open();
Console.WriteLine("Open is Here");
connection.Close();
Console.WriteLine("Close is Here");
Console.ReadKey();
