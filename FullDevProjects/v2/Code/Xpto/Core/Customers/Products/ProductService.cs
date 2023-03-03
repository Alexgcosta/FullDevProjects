using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xpto.Core.Shared.Entities;

namespace Xpto.Core.Customers.Products
{
    public class ProductService
    {
        //Listando um produto
        public static void ProductList()
        {
            AppHelpers.Clear();
            Console.WriteLine("Lista de Produtos");

            if (AppHelpers.Products.Count == 1)
                Console.WriteLine("1 registro encontrado");
            else if (AppHelpers.Products.Count > 1)
                Console.WriteLine("{0} registros encontrados", AppHelpers.Products.Count);
            else
                Console.WriteLine("nenhum registro encontrado");

            Console.WriteLine();
            Console.WriteLine("Lista de produtos");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("".PadRight(100, '-'));
            Console.ResetColor();
            Console.WriteLine("CÓDIGO".PadRight(10, ' ') + "| Produto");

            foreach (var produto in AppHelpers.Products)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("".PadRight(100, '-'));
                Console.ResetColor();
                Console.WriteLine($"{produto.Code,-10}| {produto.produto}");
            }
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("".PadRight(100, '-'));
            Console.ResetColor();

            Console.WriteLine();
            Console.WriteLine("0 - Voltar");
            Console.WriteLine();

            _ = int.TryParse(Console.ReadLine(), out var action);

            while (action != 0)
            {
                Console.WriteLine("Comando inválido");
                int.TryParse(Console.ReadLine(), out action);
            }
        }
        //Criando o Produto
        public static void ProductCreate()
        {
            AppHelpers.Clear();

            Console.WriteLine("Novo Produto");
            Console.WriteLine();

            var product = new Product();


            Console.Write("Código (Número inteiro):");
            product.Code = Convert.ToInt32(Console.ReadLine());

            Console.Write("Produto:");
            product.produto = Console.ReadLine();
            _ = new Product();
            Console.Write("Descrição:");
            product.Description = Console.ReadLine();

            Console.Write("Preço:");
            product.Price = Convert.ToDouble(Console.ReadLine());
            //Adiciona os produtos
            AppHelpers.Products.Add(product);
            var customerRepository = new CustomerRepository();
            customerRepository.Save();


            var productRepository = new ProductRepository();
            productRepository.Save();

            Console.WriteLine();
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Produto cadastrado");
            Console.ResetColor();

            Console.WriteLine();
            Console.WriteLine("0 - Voltar");
            Console.WriteLine();

            int.TryParse(Console.ReadLine(), out var action);

            while (action != 0)
            {
                Console.WriteLine("Comando inválido");
                int.TryParse(Console.ReadLine(), out action);

              
            }
        }
        //Selecionando o Produto
        public static void ProductSelect()
        {
            AppHelpers.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Consulta de Produtos");
            Console.ResetColor();
            Console.WriteLine();
            Console.Write("Informe o código do produto ou 0 para sair: ");

            while (true)
            {
                int.TryParse(Console.ReadLine(), out var code);

                if (code == 0)
                    return;

                var product = AppHelpers.Products.FirstOrDefault(x => x.Code == code);

                if (product == null)
                {
                    AppHelpers.Clear();
                    Console.WriteLine("Consulta de Produto");
                    Console.WriteLine();
                    Console.ResetColor();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Produto não encontrato ou código inválido");
                    Console.ResetColor();
                }
                else
                {
                    AppHelpers.Clear();
                    Console.WriteLine("Consulta de Produtos");
                    Console.WriteLine();

                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine(("").PadRight(100, '-'));
                    Console.ResetColor();
                    Console.WriteLine("Produto Selecionado");
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine(("").PadRight(100, '-'));
                    Console.ResetColor();

                    Console.WriteLine("Código: {0}", ($"{product.Code}| {product.produto}"));
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine(("").PadRight(100, '-'));
                    Console.ResetColor();
                    Console.WriteLine("Produto: {0}", product.produto);
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine(("").PadRight(100, '-'));
                    Console.ResetColor();
                    Console.WriteLine("Descrição: {0}", product.Description);
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine(("").PadRight(100, '-'));
                    Console.ResetColor();
                    Console.WriteLine("Preço: {0}",product.Price);
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine(("").PadRight(100, '-'));
                    Console.ResetColor();


                    //foreach (var item in product.Code)
                    //{
                    //    Console.WriteLine("Descrição: {0}", item);
                    //}

                    //foreach (var item in product.produto)
                    //{
                    //    Console.WriteLine("Produto: {0}", item);
                    //}
                    //foreach (var item in product.Description)
                    //{
                    //    Console.WriteLine("Descrição: {0}", item);
                    //}
                    //foreach (var item in product.Price)
                    //{
                    //    Console.WriteLine("Preço: {0}", item);
                    //}
                    var productRepository = new ProductRepository();
                    productRepository.Save();


                    Console.WriteLine("Observação: {0}", product.Note);
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine(("").PadRight(100, '-'));
                    Console.ResetColor();
                }

                Console.WriteLine();
                Console.Write("Informe o código do cliente ou 0 para sair: ");
            }
        }
        //Editando Produto ainda não funciona
        public static void ProductEdit()
        {
            AppHelpers.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Atualização de produto");
            Console.ResetColor();
            Console.WriteLine();
            Console.Write("Informe o código do produto ou 0 para sair: ");

            while (true)
            {
                int.TryParse(Console.ReadLine(), out var code);

                if (code == 0)
                    return;

                var product = AppHelpers.Products.FirstOrDefault(x => x.Code == code);

                if (product == null)
                {
                    AppHelpers.Clear();
                    Console.WriteLine("Atualização de Produto");
                    Console.WriteLine();
                    Console.ResetColor();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Produto não encontrato ou código inválido");
                    Console.ResetColor();
                }
                else
                {
                    AppHelpers.Clear();
                    Console.WriteLine("Atualização de Produto");
                    Console.WriteLine();

                    Console.WriteLine("Produto Selecionado");
                    Console.WriteLine(("").PadRight(100, '-'));

                    Console.WriteLine("Código: {0}", product.Code);
                    var text = Console.ReadLine();
                    if (text != "")
                        product.Code = Convert.ToInt32(text);

                    Console.WriteLine("Produto: {0}", product.produto);
                    text = Console.ReadLine();
                    if (text != "")
                        product.produto = text;

                    Console.WriteLine("Observação: {0}", product.Note);
                    text = Console.ReadLine();
                    if (text != "")
                        product.Note = text;


                    var productRepository = new ProductRepository();
                    productRepository.Save();

                    Console.WriteLine();
                    Console.ResetColor();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Produto atualizado com sucesso");
                    Console.ResetColor();
                }

                Console.WriteLine();
                Console.Write("Informe o código do produto ou 0 para sair: ");
            }
        }
        //Excluir Produto ainda não funciona
        public static void ProductDelete()
        {
            AppHelpers.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Excluir de Produto");
            Console.ResetColor();
            Console.WriteLine();
            Console.Write("Informe o código do produto ou 0 para sair: ");

            while (true)
            {
                int.TryParse(Console.ReadLine(), out var code);

                if (code == 0)
                    return;

                var product = AppHelpers.Products.FirstOrDefault(x => x.Code == code);

                if (product == null)
                {
                    AppHelpers.Clear();
                    Console.WriteLine("Excluir Produto");
                    Console.WriteLine();
                    Console.ResetColor();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Produto não encontrado ou código inválido");
                    Console.ResetColor();
                }
                else
                {
                    AppHelpers.Clear();
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Excluir Produto");
                    Console.ResetColor();
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine(("").PadRight(100, '-'));
                    Console.ResetColor();
                    Console.WriteLine("Código: {0}", product.Code);
                    Console.WriteLine("Nome: {0}", product.produto);
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine(("").PadRight(100, '-'));
                    Console.ResetColor();
                    Console.WriteLine();
                    Console.Write("Deseja excluir o Produto? (S - Sim, N - Não):");
                    var result = Console.ReadLine();
                    if (result?.ToUpper() == "S")
                    {
                        AppHelpers.Products.Remove(product);

                        var productRepository = new ProductRepository();
                        productRepository.Save();

                        AppHelpers.Clear();
                        Console.WriteLine("Excluir Produto");
                        Console.WriteLine();
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Produto exluído com sucesso");
                        Console.ResetColor();
                    }
                }

                Console.WriteLine();
                Console.Write("Informe o código do produto ou 0 para sair: ");
            }
        }


        private class CustomerPruduct
        {


            public int produto { get; set; }
            public string? Description { get; set; }
            public int Code { get; set; }
            public Guid Id { get; set; }
            public int price { get; set; }

        }

    }


}
