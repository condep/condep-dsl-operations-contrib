using System;
using ConDep.Dsl.Validation;

namespace ConDep.Dsl.Operations.Contrib.Network.Hostname
{
    public class SetHostnameOperation : RemoteCompositeOperation
    {
        private readonly string _hostname;

        public SetHostnameOperation(string hostname)
        {
            _hostname = hostname;
        }

        public override bool IsValid(Notification notification)
        {
            return true;
        }

        public override string Name
        {
            get { return "Setting Host Name"; }
        }

        public override void Configure(IOfferRemoteComposition server)
        {
            //Assume restart is not necessary.
            server.Configure.EnvironmentVariable("CONDEP_RESTART_NEEDED", "false", EnvironmentVariableTarget.Machine);

            //Set hostname if not already set.
            server.Execute.PowerShell(SetHostNameScript());

            //Restart server and set env variable for restart NOT necessary, since the machine rebooted.
            server
                .OnlyIf(RestartNeeded())
                    .Restart()
                    .Configure.EnvironmentVariable("CONDEP_RESTART_NEEDED", "false", EnvironmentVariableTarget.Machine);

        }

        private string SetHostNameScript()
        {
            return String.Format(@"
$currentHostName = [System.Net.Dns]::GetHostName()
$wantedHostName = ""{0}""

if($currentHostName -ine $wantedHostName) {{
    Write-Host ""Hostname is not equal to the one given, changing host to $wantedHostName""
    Rename-Computer $wantedHostName
    [Environment]::SetEnvironmentVariable(""CONDEP_RESTART_NEEDED"", ""true"", ""Machine"")
}}
else {{
    Write-Host ""Hostname is equal to the one given, doing nothing""
}}
", _hostname);
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