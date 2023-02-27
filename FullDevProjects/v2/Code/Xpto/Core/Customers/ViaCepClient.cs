using ManagerStore.Core;
using RestSharp;
using System.Text.Json;
//using Xpto.Core.Shared.Entities;

//namespace Xpto.Core.Customers
//{
//    public class ViaCepClient
//    {
//        public static Address GetAddressByZipCode(string zipCode)
//        {
//            var client = new RestClient($"https://viacep.com.br/ws/{zipCode}/json/");
//            RestRequest request = new();

//            IRestResponse response = (IRestResponse)client.Execute(request);

//            if (response.StatusCode != System.Net.HttpStatusCode.OK)
//            {
//                Console.WriteLine("Erro ao buscar endereço.");
//                return null;
//            }

//            var address = JsonSerializer.Deserialize<Address>(response.Content);

//            return address;
//        }

//        public object Search(string? zipCode)
//        {
//            throw new NotImplementedException();
//        }
//    }
//}