using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;
using RoutingServer.Models.OpenRoute;

namespace RoutingServer.Services
{
    [ServiceContract]
    public interface IRoutingService
    {
        [OperationContract]
        Task<List<Itinerary>> GetItinerary(string origin, string destination);
    }
}
