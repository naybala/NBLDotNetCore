using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using NBLDotNetCore.RestApi.Db;
using NBLDotNetCore.RestApi.Models;

namespace NBLDotNetCore.RestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BlogController()
        {
            _context = new AppDbContext();
        }   

        [HttpGet]
        public IActionResult Read()
        {
            var lst = _context.Blogs.ToList();
            return Ok(lst);
        }

        [HttpGet("{id}")]
        public IActionResult Edit(int id)
        {
            var item = _context.Blogs.FirstOrDefault(x=> x.BlogId == id);
            if(item is null)
            {
                return NotFound("No Data Found");
            }
            return Ok(item);
        }

        [HttpPost]
        public IActionResult Create(BlogModel blog)
        {
            _context.Blogs.Add(blog);
            var result =  _context.SaveChanges();
            string message = result > 0 ? "Inserting Successfull" : "Inserting Failed";
            return Ok(message);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id , BlogModel blog)
        {
            var item = _context.Blogs.FirstOrDefault(x => x.BlogId == id);
            if (item is null)
            {
                return NotFound("No Data Found");
            }
            item.BlogTitle = blog.BlogTitle;
            item.BlogAuthor = blog.BlogAuthor;
            item.BlogContent = blog.BlogContent;
            var result = _context.SaveChanges();
            string message = result > 0 ? "Updating Successfull" : "Updating Failed";
            return Ok(message);
        }

        [HttpPatch]
        public IActionResult Patch(int id , BlogModel blog)
        {
            var item = _context.Blogs.FirstOrDefault(x => x.BlogId == id);
            if (item is null)
            {
                return NotFound("No Data Found");
            }

            if(!string.IsNullOrEmpty(item.BlogTitle))
            {
                item.BlogTitle = item.BlogTitle;
            }
            if (!string.IsNullOrEmpty(item.BlogAuthor))
            {
                item.BlogAuthor = item.BlogAuthor;
            }
            if (!string.IsNullOrEmpty(item.BlogContent))
            {
                item.BlogContent = item.BlogContent;
            }
            var result = _context.SaveChanges();
            string message = result > 0 ? "Updating Successfull" : "Updating Failed";
            return Ok(message);
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var item = _context.Blogs.FirstOrDefault(x => x.BlogId == id);
            if (item is null)
            {
                return NotFound("No Data Found");
            }
            _context.Blogs.Remove(item);
            var result = _context.SaveChanges();
            string message = result > 0 ? "Updating Successfull" : "Updating Failed";
            return Ok(message);
        }
    }
}
