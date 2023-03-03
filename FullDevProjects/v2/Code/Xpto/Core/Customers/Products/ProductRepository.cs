using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Xpto.Core.Customers.Products
{
    public class ProductRepository
    {
        public void Load()
        {
            AppHelpers.Products = new List<Product>();

            var dir = Directory.GetCurrentDirectory() + "\\db";
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            var path = dir + "\\product.json";
            AppHelpers.Products = JsonSerializer.Deserialize<IList<Product>>(File.ReadAllText(path))!;
        }

        public void Save()
        {
            var dir = Directory.GetCurrentDirectory() + "\\db";
            var path = dir + "\\product.json";

            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(AppHelpers.Products, options);
            File.WriteAllText(path, json);
        }
    }
}
