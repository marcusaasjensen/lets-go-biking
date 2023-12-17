﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RoutingServer.JCDecauxService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="JCDecauxService.IJCDecauxService")]
    public interface IJCDecauxService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IJCDecauxService/GetNearestAvailableStationFromCoordinate", ReplyAction="http://tempuri.org/IJCDecauxService/GetNearestAvailableStationFromCoordinateRespo" +
            "nse")]
        ProxyCacheServer.Proxy.Station GetNearestAvailableStationFromCoordinate(System.Device.Location.GeoCoordinate coordinate, string city, bool isOrigin);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IJCDecauxService/GetNearestAvailableStationFromCoordinate", ReplyAction="http://tempuri.org/IJCDecauxService/GetNearestAvailableStationFromCoordinateRespo" +
            "nse")]
        System.Threading.Tasks.Task<ProxyCacheServer.Proxy.Station> GetNearestAvailableStationFromCoordinateAsync(System.Device.Location.GeoCoordinate coordinate, string city, bool isOrigin);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IJCDecauxServiceChannel : RoutingServer.JCDecauxService.IJCDecauxService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class JCDecauxServiceClient : System.ServiceModel.ClientBase<RoutingServer.JCDecauxService.IJCDecauxService>, RoutingServer.JCDecauxService.IJCDecauxService {
        
        public JCDecauxServiceClient() {
        }
        
        public JCDecauxServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public JCDecauxServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public JCDecauxServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public JCDecauxServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public ProxyCacheServer.Proxy.Station GetNearestAvailableStationFromCoordinate(System.Device.Location.GeoCoordinate coordinate, string city, bool isOrigin) {
            return base.Channel.GetNearestAvailableStationFromCoordinate(coordinate, city, isOrigin);
        }
        
        public System.Threading.Tasks.Task<ProxyCacheServer.Proxy.Station> GetNearestAvailableStationFromCoordinateAsync(System.Device.Location.GeoCoordinate coordinate, string city, bool isOrigin) {
            return base.Channel.GetNearestAvailableStationFromCoordinateAsync(coordinate, city, isOrigin);
        }
    }
}
