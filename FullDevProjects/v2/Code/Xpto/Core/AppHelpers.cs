using Xpto.Core.Customers;
using Xpto.Core.Customers.Products;
// ReSharper disable All

internal static class AppHelpers
{
    public static int ActionKey = -1;
    public static IList<Customer> Customers = new List<Customer>();
    public static IList<Product> Products = new List<Product>();
    public static Dictionary<int, string> Actions = new()
    {

            { 1, "Listar Cliente" },
            { 2, "Selecionar Cliente" },
            { 3, "Criar Cliente " },
            { 4, "Editar Cliente" },
            { 5, "Excluir Cliente" },
            { 6, "Cadastrar produto" },
            { 7, "Listar produto" },
            { 8, "Selecionar produto" },
            { 9, "Editar produto" },
            { 10, "Excluir produto" },
            { 0, "Voltar" }
        };


    public static void Clear()
    {
        Console.Clear();
        PrintHeader();
    }

    public static int GetAction()
    {
        int windowWidth = Console.WindowWidth;
        int leftMargin = (windowWidth - 10) / 30;
        Console.WriteLine(" ".PadLeft(leftMargin) + "Informe a ação que deseja executar");
        Console.WriteLine();


        foreach (var item in Actions)
            Console.WriteLine("".PadLeft(leftMargin) + $"{item.Key} - {item.Value}");

        Console.WriteLine();

        var success = int.TryParse(Console.ReadLine(), out var action);

        while (!success)
        {
            Console.WriteLine("Ação inválida");
            success = int.TryParse(Console.ReadLine(), out action);
        }

        return action;
    }

    public static void Init()
    {
        var customerRepository = new CustomerRepository();
        customerRepository.Load();
        var productRepository = new ProductRepository();
        productRepository.Load();

        while (true)
        {
            Clear();

            ActionKey = GetAction();
            if (ActionKey == 0)
                return;

            Clear();
            Console.WriteLine($"{Actions[ActionKey]}");

            var customerService = new CustomerService();
            var productService = new ProductService();

            if (ActionKey == 1)
                CustomerService.List();
            else if (ActionKey == 2)
                CustomerService.Select();
            else if (ActionKey == 3)
                CustomerService.Create();
            else if (ActionKey == 4)
                customerService.Edit();
            else if (ActionKey == 5)
                CustomerService.Delete();
            else if (ActionKey == 6)
                ProductService.ProductCreate();
            else if (ActionKey == 7)
                ProductService.ProductList();
            else if (ActionKey == 8)
                ProductService.ProductSelect();
            else if (ActionKey == 9)
                ProductService.ProductEdit();
            else if (ActionKey == 10)
                ProductService.ProductDelete();

        }
    }

    public static void PrintHeader()
    {
        int windowWidth = Console.WindowWidth;
        _ = (windowWidth - 10) / 30;
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine(("").PadRight(100, '='));
        Console.WriteLine(" ".PadLeft(50) + " CRUD -V2");
        Console.WriteLine(("").PadRight(100, '='));
        Console.WriteLine();
        Console.ResetColor();
    }
}