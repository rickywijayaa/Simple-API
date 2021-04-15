using CategoryModel;
using CreateProductModel;
using Database.MyDbContext;
using Microsoft.AspNetCore.Mvc;
using ProductModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UpdateProductModel;

namespace FirstAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    
    public class ProductController : ControllerBase
    {
        private MyDbContext _context;
        public ProductController(MyDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return StatusCode(200, _context.Products.OrderBy(x => x.Name));
        }

        [HttpGet("{id}")]
        public IActionResult Show(Guid id)
        {
            Product product = _context.Products.Where(x => x.Id == id).FirstOrDefault();
            if(product == null)
            {
                return StatusCode(400, string.Format("Theres no product with this {0} id",id));
            }
            return StatusCode(200, product);
        }

        [HttpPost]
        public IActionResult Create([FromBody]CreateProduct Request)
        {
            Product product = _context.Products.Where(x => x.Name == Request.Name).FirstOrDefault();
            Category category = _context.Categories.Where(x => x.Id == Request.CategoryId).FirstOrDefault();
            if(product != null)
            {
                return StatusCode(400, string.Format("Product with this {0} name already taken!",Request.Name));
            }
            if(category == null)
            {
                return StatusCode(400, string.Format("Theres no category with this {0} id", Request.CategoryId));
            }

            _context.Products.Add(new Product()
            {
                Name = Request.Name,
                Code = Request.Code,
                Description = Request.Description,
                ExpireDate = Request.ExpireDate,
                CategoryId = Request.CategoryId
            });

            _context.SaveChanges();
            return StatusCode(201, "Product Create Successfully");
        }

        [HttpPut]
        public IActionResult Update([FromBody] UpdateProduct Request)
        {
            Product product = _context.Products.Where(x => x.Id == Request.Id).FirstOrDefault();
            Category category = _context.Categories.Where(x => x.Id == Request.CategoryId).FirstOrDefault();
            if (product == null)
            {
                return StatusCode(400, string.Format("Theres no product with this {0} id", Request.Id));
            }
            if (category == null)
            {
                return StatusCode(400, string.Format("Theres no category with this {0} id", Request.CategoryId));
            }

            product.Name = Request.Name;
            product.Code = Request.Code;
            product.Description = Request.Description;
            product.ExpireDate = Request.ExpireDate;
            product.CategoryId = Request.CategoryId;

            _context.SaveChanges();

            return StatusCode(200, "Updated Product Succesfully");
        }
    }
}
