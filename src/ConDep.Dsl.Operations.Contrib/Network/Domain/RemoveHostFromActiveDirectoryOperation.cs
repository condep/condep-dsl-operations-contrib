using System;
using System.Threading;
using ConDep.Dsl.Config;
using ConDep.Dsl.Remote;
using ConDep.Dsl.Validation;

namespace ConDep.Dsl.Operations.Contrib.Network.Domain
{
    public class RemoveHostFromActiveDirectoryOperation : ForEachServerOperation
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

        public override void Execute(ServerConfig server, IReportStatus status, ConDepSettings settings, CancellationToken token)
        {
            var psExecutor = new PowerShellExecutor(server);
            psExecutor.Execute("Add-WindowsFeature RSAT-AD-PowerShell");
            psExecutor.Execute(RemoveHostFromActiveDirectoryScript());
        }

        public override bool IsValid(Notification notification)
        {
            return true;
        }

        public override string Name
        {
            get { return "Removing host from Active Directory"; }
        }

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