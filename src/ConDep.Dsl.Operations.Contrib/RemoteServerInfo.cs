using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Management.Automation;
using ConDep.Dsl.Logging;

namespace ConDep.Dsl.Operations.Contrib
{
    public static class RemoteServerInfo
    {
        public static bool RestartRequired(IOfferRemoteOperations remote)
        {
            const string restartRequired = @"
$val = [Environment]::GetEnvironmentVariable(""CONDEP_RESTART_NEEDED"",""Machine"")
if($val -eq 'true'){
    return $true
}
else {
    return $false
}
";
            try
            {
                var executionResult = ((Collection<PSObject>)remote.Execute.PowerShell(restartRequired).Result.Data.PsResult).First().ToString().ToLowerInvariant();

                Logger.Info("Restart required result: " + executionResult);
                return Convert.ToBoolean(executionResult);
            }
            catch (Exception e)
            {
                Logger.Error($"Error during check if restart is required: {e.Message}.");
                throw;
            }
        }

        public static bool UacEnabled(IOfferRemoteOperations remote)
        {
            const string uacEnabled = @"
$val = Get-ItemProperty -Path hklm:software\microsoft\windows\currentversion\policies\system -Name ""EnableLUA""
if ($val.EnableLUA -eq 1){
    return $true
}
else {
    return $false
}
";
            try
            {
                var executionResult = ((Collection<PSObject>)remote.Execute.PowerShell(uacEnabled).Result.Data.PsResult).First().ToString().ToLowerInvariant();

                Logger.Info("UAC enabled result: " + executionResult);
                return Convert.ToBoolean(executionResult);
            }
            catch (Exception e)
            {
                Logger.Error($"Error during check if uac is enabled: {e.Message}.");
                throw;
            }
        }

        public static bool PathExists(IOfferRemoteOperations remote, string path)
        {
            var pathExists = $@"if ((Test-Path {path})) {{ return $true }} else {{ return $false }}";

            try
            {
                var executionResult = ((Collection<PSObject>)remote.Execute.PowerShell(pathExists).Result.Data.PsResult).First().ToString().ToLowerInvariant();

                Logger.Info("Path exists result: " + executionResult);
                return Convert.ToBoolean(executionResult);
            }
            catch (Exception e)
            {
                Logger.Error($"Error during check if path [{path}] exists: {e.Message}.");
                throw;
            }
        }

        public static bool ProgramIsInstalled(IOfferRemoteOperations remote, string programName)
        {
            string isProgramInstalled = $@"
$progs = Get-WmiObject -Class Win32_Product | Select-Object -Property Name

	foreach($prog in $progs){{
		if($prog.Name -eq ""{programName}""){{
            return $true
		}}
	}}
	return $false
";
            try
            {
                var executionResult = ((Collection<PSObject>)remote.Execute.PowerShell(isProgramInstalled).Result.Data.PsResult).First().ToString().ToLowerInvariant();
                Logger.Info(programName + " install result: " + executionResult);

                return Convert.ToBoolean(executionResult);
            }
            catch (Exception e)
            {
                Logger.Error($"Error during check if [{programName}] is installed: {e.Message}.");
                throw;
            }
        }

        public static string HostName(IOfferRemoteOperations remote)
        {
            const string hostname = @"
return $env:computername
";
            try
            {
                var executionResult = ((Collection<PSObject>)remote.Execute.PowerShell(hostname).Result.Data.PsResult).First().ToString().ToLowerInvariant();

                Logger.Info("Hostname: " + executionResult);
                return executionResult;
            }
            catch (Exception e)
            {
                Logger.Error($"Error getting hostname: {e.Message}.");
                throw;
            }
        }

        public static string EnvironmentVariable(IOfferRemoteOperations remote, string variableName)
        {
            if (string.IsNullOrWhiteSpace(variableName))
                throw new ArgumentNullException(nameof(variableName), "Variable name cannot be null or empty");

            var script = $@"
return [environment]::GetEnvironmentVariable(""{variableName}"",""Machine"")
";

            try
            {
                var executionResult = ((Collection<PSObject>)remote.Execute.PowerShell(script).Result.Data.PsResult).First().ToString();
                Logger.Info($"Successfully read environment variable [{variableName}]");

                return executionResult;
            }
            catch (Exception e)
            {
                Logger.Error($"Error when reading environment [{variableName}]: {e.Message}.");
                throw;
            }
        }

        public static string OSCaption(IOfferRemoteOperations remote)
        {

            try
            {
                string caption = ((Collection<PSObject>)remote.Execute.PowerShell("return (Get-WmiObject win32_operatingsystem).caption").Result.Data.PsResult).First().ToString();
                Logger.Info($"Running OS {caption}");

                return caption;
            }
            catch (Exception e)
            {
                Logger.Error($"Error when getting OS caption: {e.Message}.");
                throw;
            }
        }
    }
}
