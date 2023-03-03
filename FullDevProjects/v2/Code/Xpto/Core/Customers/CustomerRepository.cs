using System.Text.Json;

namespace Xpto.Core.Customers
{
    public class CustomerRepository
    {
        public void Load()
        {
            AppHelpers.Customers = new List<Customer>();

            var dir = Directory.GetCurrentDirectory() + "\\db";
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            var path = dir + "\\customer.json";
            AppHelpers.Customers = JsonSerializer.Deserialize<IList<Customer>>(File.ReadAllText(path))!;
        }

        public void Save()
        {
            var dir = Directory.GetCurrentDirectory() + "\\db";
            var path = dir + "\\customer.json";

            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(AppHelpers.Customers, options);
            File.WriteAllText(path, json);
        }
    }
}
