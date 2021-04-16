using CategoryModel;
using CreateProductModel;
using Database.MyDbContext;
using DeleteProductModel;
using FirstAPI.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UpdateProductModel;

namespace FirstAPI.Controllers
{
    [ApiVersion("1.0")]
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
            ResponseModel<List<Product>> response = new ResponseModel<List<Product>>();
            try
            {
                //return StatusCode(200, _context.Products.Include(x => x.Category).OrderBy(x => x.Name));
                response.Data = _context.Products.OrderBy(x => x.Name).ToList();
                return StatusCode(200, response);
            }
            catch(Exception err)
            {
                response.Errors.Add(err.Message);
                return StatusCode(500, response);
            }
        }

        [HttpGet("{id}")]
        public IActionResult Show(Guid id)
        {
            ResponseModel<Product> response = new ResponseModel<Product>();
            try
            {
                Product product = _context.Products.Where(x => x.Id == id).FirstOrDefault();
                if (product == null) response.Errors.Add(string.Format("Theres no product with this {0} id", id));
                if(response.Errors.Count > 0) return StatusCode(400, response);
                response.Data = product;
                return StatusCode(200, response);
            }
            catch (Exception err)
            {
                response.Errors.Add(err.Message);
                return StatusCode(500, response);
            }
        }

        [HttpPost]
        public IActionResult Create([FromBody]CreateProduct Request)
        {
            ResponseModel<Product> response = new ResponseModel<Product>();
            try
            {
                Product product = _context.Products.Where(x => x.Name == Request.Name).FirstOrDefault();
                Category category = _context.Categories.Where(x => x.Id == Request.CategoryId).FirstOrDefault();
                if (product != null) response.Errors.Add(string.Format("Product with this {0} name already taken!", Request.Name));
                if (category == null) response.Errors.Add(string.Format("Theres no category with this {0} id!", Request.CategoryId));
                if (response.Errors.Count > 0) return StatusCode(500, response);

                product = new Product()
                {
                    Name = Request.Name,
                    Code = Request.Code,
                    Description = Request.Description,
                    ExpireDate = Request.ExpireDate,
                    CategoryId = Request.CategoryId
                };

                _context.Products.Add(product);
                _context.SaveChanges();
                _context.Entry(product).Reload();
                response.Data = product;
                return StatusCode(201, response);
            }
            catch (Exception err)
            {
                response.Errors.Add(err.Message);
                return StatusCode(500, response);
            }
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

        [HttpDelete]
        public IActionResult Delete([FromBody]DeleteProduct Request)
        {
            Product product = _context.Products.Where(x => x.Id == Request.Id).FirstOrDefault();
            if(product == null)
            {
                return StatusCode(400, string.Format("Theres no product with this {0} id", Request.Id));
            }

            _context.Remove(product);
            _context.SaveChanges();
            return StatusCode(200, "Delete Product Succesfully");
        }
    }
}
