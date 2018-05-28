using ConDep.Dsl.Builders;
using ConDep.Dsl.Operations.Contrib.Software.SevenZip;

namespace ConDep.Dsl
{
    public static class SevenZipExtensions
    {
        /// <summary>
        /// Installs v9.20 of 7-zip.
        /// </summary>
        /// <param name="installation"></param>
        public static void SevenZip(this IOfferRemoteInstallation installation)
        {
            var operation = new InstallSevenZipOperation();
            OperationExecutor.Execute((RemoteBuilder)installation, operation);
        }
    }
}
