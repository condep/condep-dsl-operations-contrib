using System;
using System.Collections.Generic;
using System.Threading;
using ConDep.Dsl.Config;

namespace ConDep.Dsl.Operations.Contrib.Network.Dns
{
    public class SetDnsOperation : RemoteOperation
    {
        private readonly IEnumerable<string> _dnsServersIpList;

        public SetDnsOperation(IEnumerable<string> dnsServersIpList)
        {
            _dnsServersIpList = dnsServersIpList;
        }

        public override Result Execute(IOfferRemoteOperations remote, ServerConfig server, ConDepSettings settings, CancellationToken token)
        {
            var setDnsScript = $@"
$nics = Get-WmiObject Win32_NetworkAdapterConfiguration -ErrorAction Inquire | Where{{$_.IPEnabled -eq ""TRUE""}}
$newDNS = {FormattedIpList()}

foreach($nic in $nics)
{{
    Write-Host ""`tExisting DNS Servers "" $nic.DNSServerSearchOrder
	if(Compare-Object $nic.DNSServerSearchOrder $newDNS) 
    {{
	    Write-Host ""`tDNS servers differes from provided. Overwriting now...""
	    $result = $nic.SetDNSServerSearchOrder($newDNS)

	    if($result.ReturnValue -eq 0)
	    {{
	        Write-Host ""`tSuccessfully Changed DNS Servers""
	    }}
	    else
	    {{
            throw ""Failed to Change DNS Servers""
	    }}
    }}
	else {{
        Write-Host ""`tDNS servers is equal to provided. Doing nothing.""
	}}
}}
";

            remote.Execute.PowerShell(setDnsScript);
            return Result.SuccessChanged();
        }

        public override string Name => "Setting DNS.";

        private string FormattedIpList()
        {
            var formattedIpList = "";
            foreach (var ip in _dnsServersIpList)
            {
                formattedIpList += @"""" + ip + @""",";
            }
            return formattedIpList.Remove(formattedIpList.Length - 1);
        }
    }
}