using System.Collections.Generic;
using ConDep.Dsl.Builders;
using ConDep.Dsl.Operations.Contrib.Network.Dns;

namespace ConDep.Dsl
{
    public static class DnsExtensions
    {
        /// <summary>
        /// Sets the DNS on the server, on all network adapters. If DNS on server is different from the one provided here, this operation will overwrite the existing DNS information
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="dnsServersIpList">List of strings with the IPs to the DNS server(s)</param>
        /// <returns></returns>
        public static void Dns(this IOfferRemoteConfiguration configuration, IEnumerable<string> dnsServersIpList)
        {
            var operation = new SetDnsOperation(dnsServersIpList);
            OperationExecutor.Execute((RemoteBuilder)configuration, operation);
        }
    }
}
