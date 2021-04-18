using CategoryModel;
using CreateCategoryModel;
using Database.MyDbContext;
using DeleteCategoryModel;
using FirstAPI.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UpdateCategoryModel;

namespace FirstAPI.Controllers
{
    [ApiVersion("1.0")]
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
            ResponseModel<List<Category>> response = new ResponseModel<List<Category>>();
            try
            {
                response.Data = _context.Categories.OrderBy(x => x.Name).ToList();
                return StatusCode(200, response);
            } catch (Exception err)
            {
                response.Errors.Add(err.Message);
                return StatusCode(500, response);
            }

        }

        [HttpGet("{id}")]
        public IActionResult Show(Guid id)
        {
            ResponseModel<Category> response = new ResponseModel<Category>();
            try
            {
                Category category = _context.Categories.Where(x => x.Id == id).FirstOrDefault();
                if (category == null) response.Errors.Add(string.Format("Theres no category with this {0} id", id));
                if (response.Errors.Count > 0) return StatusCode(400, response);
                response.Data = category;
                return StatusCode(200, response);

            }
            catch (Exception err)
            {
                response.Errors.Add(err.Message);
                return StatusCode(500, response);
            }

        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateCategory Request)
        {
            ResponseModel<Category> response = new ResponseModel<Category>();
            try
            {
                Category category = _context.Categories.Where(x => x.Name == Request.Name).FirstOrDefault();
                if (category != null) response.Errors.Add(string.Format("This category with this {0} name already taken!", Request.Name));
                if (response.Errors.Count > 0) return StatusCode(400, response);
                category = new Category()
                {
                    Name = Request.Name
                };
                _context.Categories.Add(category);
                _context.SaveChanges();
                _context.Entry(category).Reload();
                response.Data = category;
                return StatusCode(201, response);
            }
            catch (Exception err)
            {
                response.Errors.Add(err.Message);
                return StatusCode(500, response);
            }
        }

        [HttpPut]
        public IActionResult Update([FromBody] UpdateCategory Request)
        {
            ResponseModel<Category> response = new ResponseModel<Category>();
            try
            {
                Category category = _context.Categories.Where(x => x.Id == Request.Id).FirstOrDefault();
                if (category == null) response.Errors.Add(string.Format("Theres no category with this {0} id", Request.Id));
                if (response.Errors.Count > 0) return StatusCode(400, response);
                category.Name = Request.Name;
                _context.SaveChanges();
                _context.Entry(category).Reload();
                response.Data = category;
                return StatusCode(200, response);
            }
            catch (Exception err)
            {
                response.Errors.Add(err.Message);
                return StatusCode(500, response);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            ResponseModel<object> response = new ResponseModel<object>();
            try
            {
                Category category = _context.Categories.Where(x => x.Id == id).FirstOrDefault();
                if (category == null) response.Errors.Add(string.Format("Theres no category with this {0} id", id));
                if (response.Errors.Count > 0) return StatusCode(400, response);
                _context.Categories.Remove(category);
                _context.SaveChanges();
                _context.Entry(category).Reload();
                return StatusCode(200, response);
            }
            catch (Exception err)
            {
                response.Errors.Add(err.Message);
                return StatusCode(500, response);
            }
        }

    }
}
