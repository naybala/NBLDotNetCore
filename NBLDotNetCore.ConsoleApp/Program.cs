using NBLDotNetCore.ConsoleApp;
using System.Data;
using System.Data.SqlClient;

AdoDotNetExample adoDotNetExample = new AdoDotNetExample();
///adoDotNetExample.Read();
adoDotNetExample.Insert("U Mya Book","U Mya","This is Content");

Console.ReadKey();
