using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Xpto.Core.Shared.Entities;

namespace Xpto.Core.Customers.Products
{
    internal class Product
    {
        public string produto { get; set; }
        public string Description { get; set; }
        public int Code { get; set; }
        public Guid Id { get; set; }
        public IList<Product> Products { get; set; }
        public double Price { get; set; }
        public string Note { get; set; }
        

        public Product()
        {
            Id = Guid.NewGuid();
            Products = new List<Product>();

        }
        public override string ToString()
        {
            return produto;
        }
    }
}
