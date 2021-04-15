using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UpdateProductModel
{
    public class UpdateProduct
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public DateTime ExpireDate { get; set; }
        public Guid CategoryId { get; set; }
    }
}
