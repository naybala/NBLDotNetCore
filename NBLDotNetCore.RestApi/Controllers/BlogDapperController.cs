using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NBLDotNetCore.RestApi.Models;
using System.Data;
using System.Data.SqlClient;

namespace NBLDotNetCore.RestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogDapperController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetBlogs()
        {
            string query = "Select * from tbl_blog";
            using IDbConnection db = new SqlConnection(ConnectionStrings.SqlConnectionStringBuilder.ConnectionString);
            List<BlogModel> lst = db.Query<BlogModel>(query).ToList();
            return Ok(lst);
        }

        [HttpGet("{id}")]
        public IActionResult GetBlog(int id)
        {
            var item = FindById(id);
            if (item is null)
            {
                return NotFound("No Data Found");
            }
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

            using IDbConnection db = new SqlConnection(ConnectionStrings.SqlConnectionStringBuilder.ConnectionString);
            int result = db.Execute(query, blog);
            string message = result > 0 ? "Inserting Success." : "Inserting Failed.";
            return Ok(message);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateBlog(int id, BlogModel blog)
        {
            var item = FindById(id);
            if (item is null)
            {
                return NotFound("No Data Found");
            }
            blog.BlogId = id;
            string query = @"UPDATE [dbo].[Tbl_Blog]
                           SET [BlogTitle] = @BlogTitle
                              ,[BlogAuthor] = @BlogAuthor
                              ,[BlogContent] = @BlogContent
                           WHERE [BlogId] = @BlogId";

            using IDbConnection db = new SqlConnection(ConnectionStrings.SqlConnectionStringBuilder.ConnectionString);
            int result = db.Execute(query, blog);
            string message = result > 0 ? "Updating Success." : "Updating Failed.";
            return Ok(message);
        }

        [HttpPatch("{id}")]
        public IActionResult PatchBlog(int id, BlogModel blog)
        {
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteBlog(int id)
        {
            string query = @"DELETE FROM [dbo].[Tbl_Blog]
                              WHERE [BlogId] = @BlogId";
            using IDbConnection db = new SqlConnection(ConnectionStrings.SqlConnectionStringBuilder.ConnectionString);
            int result = db.Execute(query, new BlogModel { BlogId = id});
            string message = result > 0 ? "Deleting Success." : "Deleting Failed.";
            return Ok(message);
        }

        private BlogModel? FindById(int id)
        {
            string query = "Select * from tbl_blog where BlogId = @BlogId";
            using IDbConnection db = new SqlConnection(ConnectionStrings.SqlConnectionStringBuilder.ConnectionString);
            return db.Query<BlogModel>(query, new BlogModel { BlogId = id }).FirstOrDefault();
        }
    }
}
