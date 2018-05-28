using System;
using System.Threading;
using ConDep.Dsl.Config;
using ConDep.Dsl.Remote;

namespace ConDep.Dsl.Operations.Contrib.Network.Domain
{
    public class RemoveHostFromActiveDirectoryOperation : RemoteOperation
    {
        private readonly string _domainController;
        private readonly string _domainUsername;
        private readonly string _domainUserPassword;
        private readonly string _hostname;

        public RemoveHostFromActiveDirectoryOperation(string hostname, string domainController, string domainUsername, string domainUserPassword)
        {
            _hostname = hostname;
            _domainController = domainController;
            _domainUsername = domainUsername;
            _domainUserPassword = domainUserPassword;
        }

        public override Result Execute(IOfferRemoteOperations remote, ServerConfig server, ConDepSettings settings, CancellationToken token)
        {
            var psExecutor = new PowerShellExecutor();
            psExecutor.Execute(server, "Add-WindowsFeature RSAT-AD-PowerShell");
            psExecutor.Execute(server, RemoveHostFromActiveDirectoryScript());
            return new Result(true, false);
        }

        public override string Name => "Removing host from Active Directory";

        private string RemoveHostFromActiveDirectoryScript()
        {
            return String.Format(@"
$secpassword = ConvertTo-SecureString {2} -AsPlainText -Force
$credentials = New-Object System.Management.Automation.PSCredential(""{1}"", $secpassword)

$adComputer = Get-ADComputer -Filter {{cn -eq ""{3}""}} -Server {0} -Credential $credentials

if($adComputer) {{
    Write-Host ""Removing host $adComputer from Active Directory.""
    $adComputer | Remove-ADComputer -confirm:$false        
}}
else {{
    Write-Host ""Found no host in Active Directory with given name, doing nothing.""
}}
", _domainController, _domainUsername, _domainUserPassword, _hostname);
        }
    }
}