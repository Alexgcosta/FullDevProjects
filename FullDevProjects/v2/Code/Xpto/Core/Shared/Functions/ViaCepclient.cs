
using RestSharp;
using System;
//using System.Net.Http;
//using System.Text.Json;
//using System.Threading.Tasks;
//using Xpto.Core.Shared.Entities;

//namespace ManagerStore.Core
//{
//    public static class ViaCepService
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
//    }
//}
