using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NBLDotNetCore.RestApi.Models;
using NBLDotNetCore.Shared;
using System.Data;
using System.Data.SqlClient;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace NBLDotNetCore.RestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogAdoDotNet2Controller : ControllerBase
    {
        private readonly AdoDotNetService _adoDotNetService = new AdoDotNetService(ConnectionStrings.SqlConnectionStringBuilder.ConnectionString);
        [HttpGet]
        public IActionResult GetBlogs()
        {
            string query = "select * from Tbl_Blog";
            var lst = _adoDotNetService.Query<BlogModel>(query);
            return Ok(lst);
        }

        [HttpGet("{id}")]
        public IActionResult GetBlog(int id)
        {
            string query = @"select * from Tbl_Blog
                             WHERE [BlogId] = @BlogId";
            var item = _adoDotNetService.QueryFirstOrDefault<BlogModel>(query, new AdoDotNetParameter("@BlogId", id));
            if (item is null)
            {
                return NotFound("No data found.");
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
            string message = _adoDotNetService.Execute(query,"Inserting",
            new AdoDotNetParameter("@BlogTitle", blog.BlogTitle),
            new AdoDotNetParameter("@BlogAuthor", blog.BlogAuthor),
            new AdoDotNetParameter("@BlogContent", blog.BlogContent)
            );

            return Ok(message);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateBlog(int id, BlogModel blog)
        {
            string getQuery = "SELECT COUNT(*) FROM Tbl_Blog WHERE BlogId = @BlogId";
            var item = _adoDotNetService.QueryFirstOrDefault<BlogModel>(getQuery, new AdoDotNetParameter("@BlogId", id));
            if (item is null)
            {
                return NotFound("No data found.");
            }
            string updateQuery = @"UPDATE [dbo].[Tbl_Blog]
                           SET [BlogTitle] = @BlogTitle,
                               [BlogAuthor] = @BlogAuthor,
                               [BlogContent] = @BlogContent
                           WHERE BlogId = @BlogId"
            ;

            string message = _adoDotNetService.Execute(updateQuery, "Updating",
            new AdoDotNetParameter("@BlogId", id),
            new AdoDotNetParameter("@BlogTitle", blog.BlogTitle),
            new AdoDotNetParameter("@BlogAuthor", blog.BlogAuthor),
            new AdoDotNetParameter("@BlogContent", blog.BlogContent)
            );

            return Ok(message);
        }

        [HttpPatch("{id}")]
        public IActionResult PatchBlog(int id, BlogModel blog)
        {
            string getQuery = "SELECT COUNT(*) FROM Tbl_Blog WHERE BlogId = @BlogId";
            var item = _adoDotNetService.QueryFirstOrDefault<BlogModel>(getQuery, new AdoDotNetParameter("@BlogId", id));
            if (item is null)
            {
                return NotFound("No data found.");
            }
            string conditions = string.Empty;
            if (!string.IsNullOrEmpty(blog.BlogTitle))
            {
                conditions += " [BlogTitle] = @BlogTitle, ";
            }
            if (!string.IsNullOrEmpty(blog.BlogAuthor))
            {
                conditions = " [BlogAuthor] = @BlogAuthor, ";
            }
            if (!string.IsNullOrEmpty(blog.BlogContent))
            {
                conditions = " [BlogContent] = @BlogContent, ";
            }
            if (conditions.Length == 0)
            {
                return NotFound("No data for update");
            }

            conditions = conditions.Substring(0, conditions.Length - 2);
            blog.BlogId = id;
            string query = $@"UPDATE [dbo].[Tbl_Blog]
                           SET {conditions}
                           WHERE [BlogId] = @BlogId";
           
            return Ok("Patch");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteBlog(int id)
        {
            string query = @"DELETE FROM [dbo].[Tbl_Blog]
                              WHERE [BlogId] = @BlogId";
            string message = _adoDotNetService.Execute(query,"Deleting", new AdoDotNetParameter("@BlogId", id));
            return Ok(message);
        }
    }
}
