using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBLDotNetCore.ConsoleApp
{
    internal class AdoDotNetExample
    {

        private readonly SqlConnectionStringBuilder _sqlConnectionStringBuilder = new SqlConnectionStringBuilder()
        {
            DataSource = "DAVION",
            InitialCatalog = "DotNetTrainning",
            UserID = "sa",
            Password = "sa@123"
        };

        private string Result(int result,string processType)
        {
           return result > 0 ? processType+" Success" : processType+" Failed";
        }

        public void Read()
        {            
            SqlConnection connection = new SqlConnection(_sqlConnectionStringBuilder.ConnectionString);
            connection.Open();
            Console.WriteLine("Open is Here");
            string query = "select * from Tbl_Blog";
            SqlCommand cmd = new SqlCommand(query, connection);
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sqlDataAdapter.Fill(dt);
            connection.Close();
            Console.WriteLine("Close is Here");
            Console.WriteLine("--------------------------------------------");

            foreach (DataRow dr in dt.Rows)
            {
                Console.WriteLine(dr["BlogId"]);
                Console.WriteLine(dr["BlogTitle"]);
                Console.WriteLine(dr["BlogAuthor"]);
                Console.WriteLine(dr["BlogContent"]);
                Console.WriteLine("--------------------------------------------");

            }
        }

        public void Insert(string title,string author , string content)
        {
            SqlConnection connection = new SqlConnection(_sqlConnectionStringBuilder.ConnectionString);
            connection.Open();
            string query = @"INSERT INTO [dbo].[Tbl_Blog]
                           ([BlogTitle]
                           ,[BlogAuthor]
                           ,[BlogContent])
                     VALUES
                           (@BlogTitle,
		                   @BLogAuthor,
		                   @BlogContent)";
            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@BlogTitle", title);
            cmd.Parameters.AddWithValue("@BLogAuthor", author);
            cmd.Parameters.AddWithValue("@BlogContent", content);
            int result = cmd.ExecuteNonQuery();
            connection.Close();
            string message = Result(result,"Inserting");
            Console.WriteLine(message);
        }

        public void Update(int id,string title, string author , string content)
        {
            SqlConnection connection = new SqlConnection(_sqlConnectionStringBuilder.ConnectionString);
            connection.Open();
            string query = @"UPDATE [dbo].[Tbl_Blog]
                           SET [BlogTitle] = @BlogTitle
                              ,[BlogAuthor] = @BlogAuthor
                              ,[BlogContent] = @BlogContent
                           WHERE [BlogId] = @BlogId";
            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@BlogId", id);
            cmd.Parameters.AddWithValue("@BlogTitle", title);
            cmd.Parameters.AddWithValue("@BLogAuthor", author);
            cmd.Parameters.AddWithValue("@BlogContent", content);
            int result = cmd.ExecuteNonQuery();
            connection.Close();
            string message = Result(result, "Updating");
            Console.WriteLine(message);
        }

        public void Delete(int id)
        {
            SqlConnection connection = new SqlConnection(_sqlConnectionStringBuilder.ConnectionString);
            connection.Open();
            string query = @"DELETE FROM [dbo].[Tbl_Blog]
                              WHERE [BlogId] = @BlogId";
            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@BlogId", id);
            int result = cmd.ExecuteNonQuery();
            connection.Close();
            string message = Result(result, "Deleting");
            Console.WriteLine(message);
        }

    }
}
