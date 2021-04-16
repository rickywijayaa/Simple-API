using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CategoryModel
{
    public class Category
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public static implicit operator List<object>(Category v)
        {
            throw new NotImplementedException();
        }
    }
}
