using System;
using System.Threading;
using ConDep.Dsl.Config;

namespace ConDep.Dsl.Operations.Contrib.Software.SevenZip
{
    public class InstallSevenZipOperation : RemoteOperation
    {
        public override Result Execute(IOfferRemoteOperations remote, ServerConfig server, ConDepSettings settings, CancellationToken token)
        {
            remote.Install.Custom("7-Zip 9.20", new Uri("http://www.7-zip.org/a/7z920.exe"), @"/S /D=C:\7-Zip");
            return new Result(true, false);
        }

        public override string Name => "Install 7-zip";
    }
}