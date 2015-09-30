using System;
using System.Collections.Generic;
using ConDep.Dsl.Validation;

namespace ConDep.Dsl.Operations.Contrib.Network.Dns
{
    public class SetDnsOperation : RemoteCompositeOperation
    {
        private readonly IEnumerable<string> _dnsServersIpList;

        public SetDnsOperation(IEnumerable<string> dnsServersIpList)
        {
            _dnsServersIpList = dnsServersIpList;
        }

        public override bool IsValid(Notification notification)
        {
            return true;
        }

        public override string Name
        {
            get { return "Setting DNS."; }
        }

        public override void Configure(IOfferRemoteComposition server)
        {
            var setDnsScript = String.Format(@"
$nics = Get-WmiObject Win32_NetworkAdapterConfiguration -ErrorAction Inquire | Where{{$_.IPEnabled -eq ""TRUE""}}
$newDNS = {0}

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
", FormattedIpList());

            server.Execute.PowerShell(setDnsScript);
        }

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