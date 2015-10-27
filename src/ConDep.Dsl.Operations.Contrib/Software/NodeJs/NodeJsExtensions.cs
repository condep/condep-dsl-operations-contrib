using ConDep.Dsl.Operations.Contrib.Software.NodeJs;

namespace ConDep.Dsl
{
    public static class NodeJsExtensions
    {
        /// <summary>
        /// Installs NodeJs on the server. Versions supported by this operation is 0.10.36, and 4.2.1.
        /// The operation can upgrade existing installed version, but not downgrade.
        /// </summary>
        /// <param name="installation"></param>
        /// <param name="version">The version you want to install, default is version 4.2.1</param>
        /// <returns></returns>
        public static IOfferRemoteInstallation NodeJs(this IOfferRemoteInstallation installation, string version = "4.2.1")
        {
            var operation = new InstallNodeJsOperation(version);
            Configure.Operation(installation, operation);
            return installation;
        }
    }
}
