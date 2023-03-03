using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xpto.Core.Shared.Entities;

namespace Xpto.Core.Customers
{
    public class CustomerService
    {
        public static void List()
        {
            AppHelpers.Clear();
            Console.WriteLine("Lista de Clientes");

            if (AppHelpers.Customers.Count == 1)
                Console.WriteLine("1 registro encontrado");
            else if (AppHelpers.Customers.Count > 1)
                Console.WriteLine("{0} registros encontrados", AppHelpers.Customers.Count);
            else
                Console.WriteLine("nenhum registro encontrado");

            Console.WriteLine();
            Console.WriteLine("Lista de Clientes");
            Console.ForegroundColor= ConsoleColor.Blue;
            Console.WriteLine(("").PadRight(100, '-'));
            Console.ResetColor();
            Console.WriteLine("CÓDIGO".PadRight(10, ' ') + "| NOME");

            foreach (var customer in AppHelpers.Customers)
            {
                Console.ForegroundColor= ConsoleColor.Blue;
                Console.WriteLine(("").PadRight(100, '-'));
                Console.ResetColor();   
                Console.WriteLine($"{customer.Code,-10}| {customer.Name}");
            }
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(("").PadRight(100, '-'));
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

        public static void Select()
        {
            AppHelpers.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Consulta de Cliente");
            Console.ResetColor();
            Console.WriteLine();
            Console.Write("Informe o código do cliente ou 0 para sair: ");

            while (true)
            {
                int.TryParse(Console.ReadLine(), out var code);

                if (code == 0)
                    return;

                var customer = AppHelpers.Customers.FirstOrDefault(x => x.Code == code);

                if (customer == null)
                {
                    AppHelpers.Clear();
                    Console.WriteLine("Consulta de Cliente");
                    Console.WriteLine();
                    Console.ResetColor();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Cliente não encontrato ou código inválido");
                    Console.ResetColor();
                }
                else
                {
                    AppHelpers.Clear();
                    Console.WriteLine("Consulta de Cliente");
                    Console.WriteLine();

                    Console.ForegroundColor= ConsoleColor.Blue;
                    Console.WriteLine(("").PadRight(100, '-'));
                    Console.ResetColor();
                    Console.WriteLine("Cliente Selecionado");
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine(("").PadRight(100, '-'));
                    Console.ResetColor();

                    Console.WriteLine("Código: {0}", customer.Code);
                    Console.WriteLine("Nome: {0}", customer.Name);
                    Console.WriteLine("Tipo de Pessoa: {0}", customer.PersonType);

                    if (customer.PersonType?.ToUpper() == "PJ")
                    {
                        Console.WriteLine("Nome Fantasia:: {0}", customer.Nickname);
                    }

                    Console.WriteLine("CPF/CNPJ: {0}", customer.Identity);

                    if (customer.PersonType?.ToUpper() == "PF" && customer.BirthDate != null)
                    {
                        Console.WriteLine("Data de Nascimento: {0}", ((DateTime)customer.BirthDate).ToString("dd/MM/yyyy"));
                    }


                    foreach (var address in customer.Addresses)
                    {
                        Console.WriteLine("Endereço: {0}", address);
                        // Verifica se o CEP do endereço foi preenchido


                        if (!string.IsNullOrWhiteSpace(address.ZipCode))
                        {
                            // Chama a API ViaCEP para obter informações do endereço a partir do CEP


                            string url = $"https://viacep.com.br/ws/{address.ZipCode}/json/";
                            using var client = new WebClient();
                            string json = client.DownloadString(url);
                            dynamic addressInfo = JsonConvert.DeserializeObject(json);
                            if (addressInfo != null)
                            {
                                address.Street = addressInfo.logradouro;
                                address.District = addressInfo.bairro;
                                address.City = addressInfo.localidade;
                                address.State = addressInfo.uf;
                            }
                        }
                    }
                
            

                    foreach (var item in customer.Phones)
                    {
                        Console.WriteLine("Telefone: {0}", item);
                    }

                    foreach (var item in customer.Emails)
                    {
                        Console.WriteLine("E-mail: {0}", item);
                    }



                    Console.WriteLine("Observação: {0}", customer.Note);
                    Console.ForegroundColor= ConsoleColor.Blue;
                    Console.WriteLine(("").PadRight(100, '-'));
                    Console.ResetColor();
                }

                Console.WriteLine();
                Console.Write("Informe o código do cliente ou 0 para sair: ");
            }
        }

        public static void Create()
        {
            AppHelpers.Clear();

            Console.WriteLine("Novo Cliente");
            Console.WriteLine();

            var customer = new Customer();


            Console.Write("Código (Número inteiro):");
            customer.Code = Convert.ToInt32(Console.ReadLine());

            Console.Write("Tipo de Pessoa (PF ou PJ):");
            customer.PersonType = Console.ReadLine();

            Console.Write("Nome:");
            customer.Name = Console.ReadLine();

            if (customer.PersonType?.ToUpper() == "PJ")
            {
                Console.Write("Nome Fantasia:");
                customer.Nickname = Console.ReadLine();
            }

            Console.Write("CPF/CNPJ:");
            string cpfUsuario = Console.ReadLine();

            if (!IsCpfValid(cpfUsuario))
            {
                Console.WriteLine("CPF inválido.");
                return;
            }

            customer.Identity = Convert.ToUInt64(cpfUsuario.Trim().Replace(".", "").Replace("-", "")).ToString(@"000\.000\.000\-00");



             static bool IsCpfValid(string cpf)
        {
            int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            cpf = cpf.Trim().Replace(".", "").Replace("-", "");
            if (cpf.Length != 11)
                return false;

            for (int j = 0; j < 10; j++)
                if (j.ToString().PadLeft(11, char.Parse(j.ToString())) == cpf)
                    return false;

            int soma = 0;
            for (int i = 0; i < 9; i++)
                soma += int.Parse(cpf[i].ToString()) * multiplicador1[i];

            int resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            if (resto != int.Parse(cpf[9].ToString()))
                return false;

            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += int.Parse(cpf[i].ToString()) * multiplicador2[i];

            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            if (resto != int.Parse(cpf[10].ToString()))
                return false;

            return true;
        }
            customer.Identity = cpfUsuario;


            if (customer.PersonType?.ToUpper() == "PF")
            {
                Console.Write("Data de Nascimento (dd/mm/aaaa):");

                while (true)
                {
                    if (DateTime.TryParseExact(
                            Console.ReadLine(),
                            "d/M/yyyy",
                            CultureInfo.InvariantCulture,
                            DateTimeStyles.None,
                            out var dt))
                    {
                        customer.BirthDate = dt;
                        break;
                    }
                    else
                    {
                        Console.Write("Data de Nascimento inválida:");
                    }
                }
            }

            Console.Write("Endereço:");

            var address = new Address();


            Console.Write("Buscar endereço pelo CEP (S/N)?");
            string confirmar = Console.ReadLine();
            string cep;

            if (confirmar.Equals("S", StringComparison.Ordinal))
            {
                while (true)
                {
                    Console.Write("Informe o CEP ou 0 para adicionar o endereço ");
                    Console.WriteLine();
                    cep = Console.ReadLine();

                    if (string.IsNullOrEmpty(cep))
                    {
                        continue;
                    }
                    else if (cep.Equals("0"))
                    {
                        break;
                    }

                    string url = $"https://viacep.com.br/ws/{cep}/json";
                    using (var client = new WebClient())
                    {
                        string json = client.DownloadString(url);

                        Console.WriteLine("Logradouro:" + ((dynamic)JsonConvert.DeserializeObject(json)).logradouro);
                        Console.WriteLine("Bairro:" + ((dynamic)JsonConvert.DeserializeObject(json)).bairro);
                        Console.WriteLine("Cidade:" + ((dynamic)JsonConvert.DeserializeObject(json)).localidade);
                        Console.WriteLine("Estado:" + ((dynamic)JsonConvert.DeserializeObject(json)).uf);

                        Console.Write("Endereço está correto (S/N)?");
                        string enderecoCorreto = Console.ReadLine();

                        if (enderecoCorreto.ToUpper()== "S")
                        {
                            address.ZipCode = cep;
                            address.Street = ((dynamic)JsonConvert.DeserializeObject(json)).logradouro;
                            address.District = ((dynamic)JsonConvert.DeserializeObject(json)).bairro;
                            address.City = ((dynamic)JsonConvert.DeserializeObject(json)).localidade;
                            address.State = ((dynamic)JsonConvert.DeserializeObject(json)).uf;

                            Console.Write("Número do Enedereço:");
                            address.Number = Console.ReadLine();

                            Console.Write("Complemento:");
                            address.Complement = Console.ReadLine();

                            break;
                        }
                    }
                }
            }
            else
            {
                Console.Write("Logradouro:");
                address.Street = Console.ReadLine();
                
                Console.Write("Número:");
                address.Number = Console.ReadLine();
                
                Console.Write("Complemento:");
                address.Complement = Console.ReadLine();
                
                Console.Write("Bairro:");
                address.District = Console.ReadLine();
                
                Console.Write("Cidade:");
                address.City = Console.ReadLine();
               
                Console.Write("Estado:");
                address.State = Console.ReadLine();
                
                Console.Write("CEP:");
                address.ZipCode = Console.ReadLine();
            }

            customer.Addresses.Add(address);


            Console.Write("Telefone com DDD:");

            var phone = new Phone();
            phone.Number = Convert.ToInt64(Console.ReadLine());
            customer.Phones.Add(phone);


            Console.Write("E-mail:");

            var email = new Email();
            email.Address = Console.ReadLine();
            customer.Emails.Add(email);



            Console.Write("Observação:");
            customer.Note = Console.ReadLine();

            customer.CreationDate = new DateTime();
            AppHelpers.Customers.Add(customer);


            var customerRepository = new CustomerRepository();
            customerRepository.Save();

            Console.WriteLine();
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Cliente cadastrado com sucesso");
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

        public void Edit()
        {
            AppHelpers.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Atualização de Cliente");
            Console.ResetColor();
            Console.WriteLine();
            Console.Write("Informe o código do cliente ou 0 para sair: ");

            while (true)
            {
                int.TryParse(Console.ReadLine(), out var code);

                if (code == 0)
                    return;

                var customer = AppHelpers.Customers.FirstOrDefault(x => x.Code == code);

                if (customer == null)
                {
                    AppHelpers.Clear();
                    Console.WriteLine("Atualização de Cliente");
                    Console.WriteLine();
                    Console.ResetColor();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Cliente não encontrato ou código inválido");
                    Console.ResetColor();
                }
                else
                {
                    AppHelpers.Clear();
                    Console.WriteLine("Atualização de Cliente");
                    Console.WriteLine();


                    Console.WriteLine("Cliente Selecionado");
                    Console.WriteLine(("").PadRight(100, '-'));

                    Console.WriteLine("Código: {0}", customer.Code);
                    var text = Console.ReadLine();
                    if (text != "")
                        customer.Code = Convert.ToInt32(text);

                    Console.WriteLine("Nome: {0}", customer.Name);
                    text = Console.ReadLine();
                    if (text != "")
                        customer.Name = text;

                    Console.WriteLine("Tipo de Pessoa: {0}", customer.PersonType);
                    text = Console.ReadLine();
                    if (text != "")
                        customer.PersonType = text;

                    if (customer.PersonType?.ToUpper() == "PJ")
                    {
                        Console.WriteLine("Nome Fantasia:: {0}", customer.Nickname);
                        text = Console.ReadLine();
                        if (text != "")
                            customer.Nickname = text;
                    }

                    Console.WriteLine("CPF/CNPJ: {0}", customer.Identity);
                    text = Console.ReadLine();
                    if (text != "")
                        customer.Identity = text;

                    if (customer.PersonType?.ToUpper() == "PF" && customer.BirthDate != null)
                    {
                        Console.WriteLine("Data de Nascimento: {0}", ((DateTime)customer.BirthDate).ToString("dd/MM/yyyy"));
                        text = Console.ReadLine();
                        if (text != "")
                        {
                            while (true)
                            {
                                if (DateTime.TryParseExact(
                                        text,
                                        "d/M/yyyy",
                                        CultureInfo.InvariantCulture,
                                        DateTimeStyles.None,
                                        out var dt))
                                {
                                    customer.BirthDate = dt;
                                    break;
                                }
                                else
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.Write("Data de Nascimento inválida:");
                                    Console.ResetColor();
                                }
                            }
                        }
                    }

                    Console.WriteLine("Observação: {0}", customer.Note);
                    text = Console.ReadLine();
                    if (text != "")
                        customer.Note = text;


                    var customerRepository = new CustomerRepository();
                    customerRepository.Save();

                    Console.WriteLine();
                    Console.ResetColor();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Cliente atualizado com sucesso");
                    Console.ResetColor();
                }

                Console.WriteLine();
                Console.Write("Informe o código do cliente ou 0 para sair: ");
            }
        }

        public static void Delete()
        {
            AppHelpers.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Excluir de Cliente");
            Console.ResetColor();
            Console.WriteLine();
            Console.Write("Informe o código do cliente ou 0 para sair: ");

            while (true)
            {
                int.TryParse(Console.ReadLine(), out var code);

                if (code == 0)
                    return;

                var customer = AppHelpers.Customers.FirstOrDefault(x => x.Code == code);

                if (customer == null)
                {
                    AppHelpers.Clear();
                    Console.WriteLine("Excluir de Cliente");
                    Console.WriteLine();
                    Console.ResetColor();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Cliente não encontrato ou código inválido");
                    Console.ResetColor();
                }
                else
                {
                    AppHelpers.Clear();
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Excluir de Cliente");
                    Console.ResetColor();
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine(("").PadRight(100, '-'));
                    Console.ResetColor();
                    Console.WriteLine("Código: {0}", customer.Code);
                    Console.WriteLine("Nome: {0}", customer.Name);
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine(("").PadRight(100, '-'));
                    Console.ResetColor();
                    Console.WriteLine();
                    Console.Write("Deseja excluir o cliente? (S - Sim, N - Não):");
                    var result = Console.ReadLine();
                    if (result?.ToUpper() == "S")
                    {
                        AppHelpers.Customers.Remove(customer);

                        var customerRepository = new CustomerRepository();
                        customerRepository.Save();

                        AppHelpers.Clear();
                        Console.WriteLine("Excluir de Cliente");
                        Console.WriteLine();
                        Console.ForegroundColor= ConsoleColor.Green;
                        Console.WriteLine("Cliente exluído com sucesso");
                        Console.ResetColor();
                    }
                }

                Console.WriteLine();
                Console.Write("Informe o código do cliente ou 0 para sair: ");
            }
        }
    }
}
