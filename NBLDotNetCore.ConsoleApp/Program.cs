using NBLDotNetCore.ConsoleApp;
using System.Data;
using System.Data.SqlClient;

AdoDotNetExample adoDotNetExample = new AdoDotNetExample();
///adoDotNetExample.Read();
//adoDotNetExample.Insert("U Mya Book","U Mya","This is Content");
//adoDotNetExample.Update(1,"U Mya One Book", "U Mya One", "This is Content One");
adoDotNetExample.Delete(5);
Console.ReadKey();
