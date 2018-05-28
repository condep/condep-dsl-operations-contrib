using System;
using System.Threading;
using ConDep.Dsl.Config;

namespace ConDep.Dsl.Operations.Contrib.Network.Hostname
{
    public class SetHostnameOperation : RemoteOperation
    {
        private readonly string _hostname;

        public SetHostnameOperation(string hostname)
        {
            _hostname = hostname;
        }

        public override Result Execute(IOfferRemoteOperations remote, ServerConfig server, ConDepSettings settings, CancellationToken token)
        {
            //Assume restart is not necessary.
            remote.Configure.EnvironmentVariable("CONDEP_RESTART_NEEDED", "false", EnvironmentVariableTarget.Machine);

            //Set hostname if not already set.
            remote.Execute.PowerShell(SetHostNameScript());

            //Restart server and set env variable for restart NOT necessary, since the machine rebooted.
            if (RemoteServerInfo.RestartRequired(remote))
            {
                remote.Restart();
                remote.Configure.EnvironmentVariable("CONDEP_RESTART_NEEDED", "false", EnvironmentVariableTarget.Machine);
            }

            return new Result(true, false);
        }

        public override string Name => "Setting Host Name";

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
    }
}