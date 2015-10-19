using ConDep.Dsl.Operations.Contrib.Network.Hostname;

namespace ConDep.Dsl
{
    public static class HostNameExtensions
    {
        /// <summary>
        /// This operation sets the hostname on the server the hard way. 
        /// Operation overwrites the existing hostname. 
        /// If hosname changes, the operation will restart the server.
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="hostname">Wanted hostname for the server.</param>
        /// <returns></returns>
        public static IOfferRemoteConfiguration Hostname(this IOfferRemoteConfiguration configuration, string hostname)
        {
            var operation = new SetHostnameOperation(hostname);
            Configure.Operation(configuration, operation);
            return configuration;
        }
    }
}
