using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NBLDotNetCore.RestApiWithNLayer.Features.Blog;
using Newtonsoft.Json;

namespace NBLDotNetCore.RestApiWithNLayer.Features.Bird
{
    [Route("api/[controller]")]
    [ApiController]
    public class BirdController : ControllerBase
    {
        private async Task<MainDto> GetDataAsync()
        {
            string jsonStr = await System.IO.File.ReadAllTextAsync("data.json"); 
            var model = JsonConvert.DeserializeObject<MainDto>(jsonStr);
            return model;
        }

        [HttpGet]
        public async Task<IActionResult> list()
        {
            var model = await GetDataAsync();
            return Ok(model.Tbl_Bird);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> show(int id)
        {
            var model = await GetDataAsync();
            return Ok(model.Tbl_Bird.FirstOrDefault(x => x.Id == id));
        }

    }
}

public class MainDto
{
    public Tbl_Bird[] Tbl_Bird { get; set; }
}

public class Tbl_Bird
{
    public int Id { get; set; }
    public string BirdMyanmarName { get; set; }
    public string BirdEnglishName { get; set; }
    public string Description { get; set; }
    public string ImagePath { get; set; }
}
