using System.Net;

namespace ManagerStore.Core
{
    internal interface IRestResponse
    {
        HttpStatusCode StatusCode { get; }
        Stream Content { get; set; }
    }
}