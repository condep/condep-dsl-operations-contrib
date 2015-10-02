---
layout: doc
title: Dns
permalink: /3-0/remote/dns/
version_added: 3.2.0
---

Configures DNS on servers.

## Syntax

{% highlight csharp %}
Dns(IEnumerable<string> dnsServersIpList)
{% endhighlight %}

<table>
	<thead>
		<tr>
			<th>Parameter</th>
			<th>Description</th>
		</tr>
	</thead>
	<tbody>
    <tr>
      <td>dnsServersIpList</td>
      <td>List of ip's for DNS servers</td>
    </tr>
	</tbody>
</table>

## Code Example

{% highlight csharp %}
public class DnsExample : Artifact.Remote
{
    public override void Configure(IOfferRemoteOperations server, ConDepSettings settings)
    {
        server.Configure.Dns(new []{"10.65.89.20", "10.65.89.21"});
    }
}
{% endhighlight %}
