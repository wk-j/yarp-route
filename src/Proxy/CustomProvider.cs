using Yarp.ReverseProxy.Configuration;
using Yarp.ReverseProxy.LoadBalancing;

namespace Proxy;

public class CustomProvider : IProxyConfigProvider {
    private CustomMemoryConfig _config;

    public CustomProvider() {

        var routeConfigs = new[] {
            new RouteConfig {
                RouteId = "route1",
                ClusterId = "cluster1",
                Match = new RouteMatch {
                    Path = "/web/{**catch-all}"
                },
                Transforms = new List<IReadOnlyDictionary<string,string>> {
                    new Dictionary<string,string> {
                        ["PathRemovePrefix"] = "/web"
                    }
                }
            },
            new RouteConfig {
                RouteId = "route2",
                ClusterId = "cluster2",
                Match = new RouteMatch {
                    Path = "/admin/{**catch-all}"
                },
                Transforms = new List<IReadOnlyDictionary<string,string>> {
                    new Dictionary<string,string> {
                        ["PathRemovePrefix"] = "/admin"
                    }
                }
            }
        };

        var customConfigs = new[] {
            new ClusterConfig {
                ClusterId = "cluster1",
                LoadBalancingPolicy = LoadBalancingPolicies.RoundRobin,
                Destinations = new Dictionary<string, DestinationConfig> {
                    {
                        "destination1", new DestinationConfig {
                            Address = "http://localhost:8010/"
                        }
                    }
                }
            },
            new ClusterConfig {
                ClusterId = "cluster2",
                LoadBalancingPolicy = LoadBalancingPolicies.RoundRobin,
                Destinations = new Dictionary<string, DestinationConfig> {
                    {
                        "destination1", new DestinationConfig {
                            Address = "http://localhost:8020/"
                        }
                    }
                }
            }
        };

        _config = new CustomMemoryConfig(routeConfigs, customConfigs);

    }

    public IProxyConfig GetConfig() {
        return _config;
    }

}
