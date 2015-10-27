using System;
using ConDep.Dsl.Validation;

namespace ConDep.Dsl.Operations.Contrib.Software.NodeJs
{
    public class InstallNodeJsOperation : RemoteCompositeOperation
    {
        private readonly string _version;
        private readonly string _downloadUrl;

        public InstallNodeJsOperation(string version)
        {
            _version = version;
            _downloadUrl = GetDownloadUrl();
        }

        public override bool IsValid(Notification notification)
        {
            return true;
        }

        public override string Name
        {
            get { return "Install NodeJs " + _version; }
        }

        public override void Configure(IOfferRemoteComposition server)
        {
            server.Install.Msi("Node.js", new Uri(_downloadUrl), options => options.Version(_version));
        }

        private string GetDownloadUrl()
        {
            switch (_version)
            {
                case "0.10.36":
                    return "https://nodejs.org/dist/v0.10.36/x64/node-v0.10.36-x64.msi";
                case "4.2.1":
                    return "https://nodejs.org/dist/v4.2.1/node-v4.2.1-x64.msi";
                default:
                    return "";
            }
        }
    }
}