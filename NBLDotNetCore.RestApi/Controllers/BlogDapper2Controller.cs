using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NBLDotNetCore.RestApi.Models;
using NBLDotNetCore.Shared;
using System.Data;
using System.Data.SqlClient;
using System.Reflection.Metadata;

namespace NBLDotNetCore.RestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogDapper2Controller : ControllerBase
    {
        private readonly DapperService _dapperService = new DapperService(ConnectionStrings.SqlConnectionStringBuilder.ConnectionString);

        [HttpGet]
        public IActionResult GetBlogs()
        {
            string query = "select * from Tbl_Blog";
            var lst = _dapperService.Query<BlogModel>(query);
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
            string message = _dapperService.Execute(query, "Inserting", blog);
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

            string message = _dapperService.Execute(query, "Updating", blog);
            return Ok(message);
        }

        [HttpPatch("{id}")]
        public IActionResult PatchBlog(int id, BlogModel blog)
        {
            var item = FindById(id);
            if (item is null)
            {
                return NotFound("No Data Found");
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
            if(conditions.Length == 0)
            {
                return NotFound("No data for update");
            }

            conditions = conditions.Substring(0, conditions.Length - 2);
            blog.BlogId = id;
            string query = $@"UPDATE [dbo].[Tbl_Blog]
                           SET {conditions}
                           WHERE [BlogId] = @BlogId";
            string message = _dapperService.Execute(query, "Updating Patch", blog);
            return Ok(message);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteBlog(int id)
        {
            string query = @"DELETE FROM [dbo].[Tbl_Blog]
                              WHERE [BlogId] = @BlogId";
            string message = _dapperService.Execute(query, "Deleting", new BlogModel { BlogId = id });
            return Ok(message);
        }

        private BlogModel? FindById(int id)
        {
            string query = "Select * from tbl_blog where BlogId = @BlogId";
            var item = _dapperService.QueryFirstOrDefault<BlogModel>(query, new BlogModel { BlogId = id });
            return item;
        }
    }
}
 