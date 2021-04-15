using CategoryModel;
using CreateCategoryModel;
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
                return StatusCode(400, string.Format("Theres no category with this {0} id", id));
            }
            return StatusCode(200, category);
        }

        [HttpPost]
        public IActionResult Create([FromBody]CreateCategory Request)
        {
            Category category = _context.Categories.Where(x => x.Name == Request.Name).FirstOrDefault();
            if(category != null)
            {
                return StatusCode(400, string.Format("Category with this {0} name already taken!",Request.Name));
            }

            _context.Categories.Add(new Category()
            {
                Name = Request.Name
            });

            _context.SaveChanges();
            return StatusCode(201, "Category Created Successfully");
        }

    }
}
