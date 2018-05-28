using ConDep.Dsl.Builders;
using ConDep.Dsl.Operations.Contrib.Network.Domain;

namespace ConDep.Dsl
{
    public static class DomainExtensions
    {
        /// <summary>
        /// Joins server to domain with current hostname.
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="domain">The domain name</param>
        /// <param name="domainUsername">A domain user with rights  to remove hosts from Actice Directory.</param>
        /// <param name="domainUserPassword">The password for the domain user.</param>
        /// <param name="domainController">The domaincontrollers FQDN.</param>
        /// <param name="adOuPath">Actice Directory OU path</param>
        /// <returns></returns>
        public static void Domain(this IOfferRemoteConfiguration configuration, string domain, string domainUsername, string domainUserPassword, string domainController, string adOuPath)
        {
            var operation = new JoinDomainOperation(domain, domainUsername, domainUserPassword, domainController, adOuPath);
            OperationExecutor.Execute((RemoteBuilder)configuration, operation);
        }

        /// <summary>
        /// Checks if the host exists in Active Directory and removes the host if it does.
        /// </summary>
        /// <param name="remote"></param>
        /// <param name="hostname">The hostname for the host you want to remove from Active Directory.</param>
        /// <param name="domainController">The domaincontrollers FQDN.</param>
        /// <param name="domainUsername">A domain user with rights  to remove hosts from Actice Directory.</param>
        /// <param name="domainUserPassword">The password for the domain user.</param>
        /// <returns></returns>
        public static void RemoveHostFromActiveDirectory(this IOfferRemoteOperations remote, string hostname, string domainController, string domainUsername, string domainUserPassword)
        {
            var operation = new RemoveHostFromActiveDirectoryOperation(hostname, domainController, domainUsername, domainUserPassword);
            OperationExecutor.Execute((RemoteBuilder)remote, operation);
        }
    }
}
