﻿using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NBLDotNetCore.RestApi.Models;
using System.Data;
using System.Data.SqlClient;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace NBLDotNetCore.RestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogAdoDotNetController : ControllerBase
    {

        [HttpGet]
        public IActionResult GetBlogs()
        {
            string query = "select * from Tbl_Blog";
            SqlConnection connection = new SqlConnection(ConnectionStrings.SqlConnectionStringBuilder.ConnectionString);
            connection.Open();
            SqlCommand cmd = new SqlCommand(query, connection);
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sqlDataAdapter.Fill(dt);
            connection.Close();
            List<BlogModel> lst = dt.AsEnumerable().Select(dr=> new BlogModel
            {
                BlogId = Convert.ToInt32(dr["BlogId"]),
                BlogTitle = Convert.ToString(dr["BlogTitle"]),
                BlogAuthor = Convert.ToString(dr["BlogAuthor"]),
                BlogContent = Convert.ToString(dr["BlogContent"])
            }).ToList();
            return Ok(lst);
        }

        [HttpGet("{id}")]
        public IActionResult GetBlog(int id)
        {
            string query = @"select * from Tbl_Blog
                             WHERE [BlogId] = @BlogId";
            SqlConnection connection = new SqlConnection(ConnectionStrings.SqlConnectionStringBuilder.ConnectionString);
            connection.Open();
            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@BlogId", id);
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sqlDataAdapter.Fill(dt);
            connection.Close();
            if(dt.Rows.Count == 0)
            {
                return NotFound("No Data Found");
            }
            DataRow dr = dt.Rows[0];
            var item = new BlogModel
            {
                BlogId = Convert.ToInt32(dr["BlogId"]),
                BlogTitle = Convert.ToString(dr["BlogTitle"]),
                BlogAuthor = Convert.ToString(dr["BlogAuthor"]),
                BlogContent = Convert.ToString(dr["BlogContent"])
            };
            return Ok(item);
        }

        [HttpPost]
        public IActionResult CreateBlog(BlogModel blog) 
        {
            string query = @"INSERT INTO [dbo].[Tbl_Blog]
                           ([BlogTitle]
                           ,[BlogAuthor]
                           ,[BlogContent])
                     VALUES
                           (@BlogTitle,
		                   @BLogAuthor,
		                   @BlogContent)";
            SqlConnection connection = new SqlConnection(ConnectionStrings.SqlConnectionStringBuilder.ConnectionString);
            connection.Open();
            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@BlogTitle", blog.BlogTitle);
            cmd.Parameters.AddWithValue("@BLogAuthor", blog.BlogAuthor);
            cmd.Parameters.AddWithValue("@BlogContent", blog.BlogContent);
            int result = cmd.ExecuteNonQuery();
            connection.Close();
            string message = result > 0 ? "Inserting Success" : "Inserting Failed";
            return Ok(message);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateBlog(int id, BlogModel blog)
        {
            string getQuery = "SELECT COUNT(*) FROM Tbl_Blog WHERE BlogId = @BlogId";
            SqlConnection connection = new SqlConnection(ConnectionStrings.SqlConnectionStringBuilder.ConnectionString);
            connection.Open();
            SqlCommand checkCmd = new SqlCommand(getQuery, connection);
            checkCmd.Parameters.AddWithValue("@BlogId", id);
            var count = (int)checkCmd.ExecuteScalar();
            if (count == 0)
            {
                return NotFound("Data Not Found");
            }
            string updateQuery = @"UPDATE [dbo].[Tbl_Blog]
                           SET [BlogTitle] = @BlogTitle,
                               [BlogAuthor] = @BlogAuthor,
                               [BlogContent] = @BlogContent
                           WHERE BlogId = @BlogId";

            SqlCommand cmd = new SqlCommand(updateQuery, connection);
            cmd.Parameters.AddWithValue("@BlogId", id);
            cmd.Parameters.AddWithValue("@BlogTitle", blog.BlogTitle);
            cmd.Parameters.AddWithValue("@BlogAuthor", blog.BlogAuthor);
            cmd.Parameters.AddWithValue("@BlogContent", blog.BlogContent);

            int result = cmd.ExecuteNonQuery();
            connection.Close();
            string message = result > 0 ? "Updating Successful." : "Updating Failed.";
            return Ok(message);
        }

        [HttpPatch("{id}")]
        public IActionResult PatchBlog(int id, BlogModel blog)
        {
            SqlConnection connection = new SqlConnection(ConnectionStrings.SqlConnectionStringBuilder.ConnectionString);
            connection.Open();
            string query = "select * from Tbl_Blog where BlogId = @BlogId";
            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@BlogId", id);
            var count = (int)cmd.ExecuteScalar();
            if (count == 0) return NotFound();
            SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            dataAdapter.Fill(dt);
            List<BlogModel> lst = new List<BlogModel>();
            if (dt.Rows.Count == 0)
            {
                var response = new { IsSuccess = false, Message = "No data found." };
                return NotFound(response);
            }

            DataRow row = dt.Rows[0];

            BlogModel item = new BlogModel
            {
                BlogId = Convert.ToInt32(row["BlogId"]),
                BlogTitle = Convert.ToString(row["BlogTitle"]),
                BlogAuthor = Convert.ToString(row["BlogAuthor"]),
                BlogContent = Convert.ToString(row["BlogContent"]),
            };
            lst.Add(item);
            string conditions = "";
            List<SqlParameter> parameters = new List<SqlParameter>();

            if (!string.IsNullOrEmpty(blog.BlogTitle))
            {
                conditions += " [BlogTitle] = @BlogTitle, ";
                parameters.Add(new SqlParameter("@BlogTitle", SqlDbType.NVarChar) { Value = blog.BlogTitle });
                item.BlogTitle = blog.BlogTitle;
            }

            if (!string.IsNullOrEmpty(blog.BlogAuthor))
            {
                conditions += " [BlogAuthor] = @BlogAuthor, ";
                parameters.Add(new SqlParameter("@BlogAuthor", SqlDbType.NVarChar) { Value = blog.BlogAuthor });
                item.BlogAuthor = blog.BlogAuthor;
            }

            if (!string.IsNullOrEmpty(blog.BlogContent))
            {
                conditions += " [BlogContent] = @BlogContent, ";
                parameters.Add(new SqlParameter("@BlogContent", SqlDbType.NVarChar) { Value = blog.BlogContent });
                item.BlogContent = blog.BlogContent;
            }

            if (conditions.Length == 0)
            {
                var response = new { IsSuccess = false, Message = "No data found." };
                return NotFound(response);
            }
            conditions = conditions.TrimEnd(',', ' ');
            query = $@"UPDATE [dbo].[Tbl_Blog] SET {conditions} WHERE BlogId = @BlogId";

            using SqlCommand cmd2 = new SqlCommand(query, connection);
            cmd2.Parameters.AddWithValue("@BlogId", id);
            cmd2.Parameters.AddRange(parameters.ToArray());

            int result = cmd2.ExecuteNonQuery();
            connection.Close();
            string message = result > 0 ? "Patch Updating Successful." : "Patch Updating Failed.";
            return Ok(message);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteBlog(int id)
        {
            string query = @"DELETE FROM [dbo].[Tbl_Blog]
                              WHERE [BlogId] = @BlogId";
            SqlConnection connection = new SqlConnection(ConnectionStrings.SqlConnectionStringBuilder.ConnectionString);
            connection.Open();
            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@BlogId", id);
            int result = cmd.ExecuteNonQuery();
            connection.Close();
            string message = result > 0 ? "Deleting Success" : "Deleting Failed";
            return Ok(message);
        }
    }
}
