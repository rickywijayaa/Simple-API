using CategoryModel;
using Database.MyDbContext;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FirstAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {

        private MyDbContext _context;

        public CategoryController(MyDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return StatusCode(200, _context.Categories);
        }

        [HttpGet("{id}")]
        public IActionResult Show(Guid id)
        {
            Category category = _context.Categories.Where(x => x.Id == id).FirstOrDefault();
            if(category == null)
            {
                return StatusCode(400, string.Format("Theres no category with this {0} name", category.Name));
            }
            return StatusCode(200, category);
        }
    }
}
