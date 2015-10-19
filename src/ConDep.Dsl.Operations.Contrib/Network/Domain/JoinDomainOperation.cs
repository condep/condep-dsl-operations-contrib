using System;
using ConDep.Dsl.Validation;

namespace ConDep.Dsl.Operations.Contrib.Network.Domain
{
    public class JoinDomainOperation : RemoteCompositeOperation
    {
        private readonly string _domain;
        private readonly string _domainUsername;
        private readonly string _domainUserPassword;
        private readonly string _domainController;
        private readonly string _adOuPath;

        public JoinDomainOperation(string domain, string domainUsername, string domainUserPassword, string domainController, string adOuPath)
        {
            _domain = domain;
            _domainUsername = domainUsername;
            _domainUserPassword = domainUserPassword;
            _domainController = domainController;
            _adOuPath = adOuPath;
        }

        public override bool IsValid(Notification notification)
        {
            return true;
        }

        public override string Name
        {
            get { return "Join Domain."; }
        }

        public override void Configure(IOfferRemoteComposition server)
        {
            //Adding feature needed to join domain using PowerShell
            server.Configure.Windows(win => win.InstallFeature("RSAT-AD-PowerShell"));

            //Assume restart is not necessary.
            server.Configure.EnvironmentVariable("CONDEP_RESTART_NEEDED", "false", EnvironmentVariableTarget.Machine);

            server.Execute.PowerShell(JoindDomainScript());

            //Restart server and set env variable for restart NOT necessary, since the machine rebooted.
            server
                .OnlyIf(RestartNeeded())
                    .Restart()
                    .Configure.EnvironmentVariable("CONDEP_RESTART_NEEDED", "false", EnvironmentVariableTarget.Machine);
        }

        private string JoindDomainScript()
        {
            return String.Format(@"

Write-Host ""Joining domain {0}...""
$currentHostName = [System.Net.Dns]::GetHostName()
$reg = [Microsoft.Win32.RegistryKey]::OpenRemoteBaseKey('LocalMachine', $currentHostName)
$regKey = $reg.OpenSubKey(""SYSTEM\\CurrentControlSet\\services\\Tcpip\\Parameters"")
$currentDomain = $regKey.GetValue(""Domain"")
Write-Host ""Current domain: ""$currentDomain

$secpassword = ConvertTo-SecureString {2} -AsPlainText -Force
$credentials = New-Object System.Management.Automation.PSCredential(""{1}"", $secpassword)

if($currentDomain -ine ""{0}"") {{

	$adComputer = Get-ADComputer -Filter {{cn -eq $currentHostName}} -Server {3} -Credential $credentials
	    
    if($adComputer) {{
        Write-Host ""Hostname already registered in Active Directory. Choose another hostname for the machine or use server.RemoveHostFromActiveDirectory() to delete host from Active Directory.""       
    }}
    else{{
        Write-Host ""Adding computer to domain.""
	    Add-Computer -DomainName {0} -Credential $credentials -OUPath ""{4}""
        [Environment]::SetEnvironmentVariable(""CONDEP_RESTART_NEEDED"", ""true"", ""Machine"")
    }}
}}
", _domain, _domainUsername, _domainUserPassword, _domainController, _adOuPath);
        }

        private string RestartNeeded()
        {
            return @"
$val = [Environment]::GetEnvironmentVariable(""CONDEP_RESTART_NEEDED"",""Machine"")

if($val -eq 'true'){
    return $true
}
else {
    return $false
}
";
        }
    }
}