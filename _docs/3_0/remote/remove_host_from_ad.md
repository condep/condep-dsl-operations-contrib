---
layout: doc
title: Remove Host from AD
permalink: /3-0/remote/remove_host_from_ad/
version_added: 3.2.0
---

Deletes a host from Active Directory.

## Syntax

{% highlight csharp %}
RemoveHostFromActiveDirectory(string hostname, string domainController, string domainUsername, string domainUserPassword)
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
      <td>hostname</td>
      <td>Hostname for the computer you want to remove</td>
    </tr>
    <tr>
      <td>domainController</td>
      <td>Domain controller with FQDN</td>
    </tr>
    <tr>
      <td>domainUsername</td>
      <td>Username for a domain user, in format 'DOMAIN\\Username'</td>
    </tr>
    <tr>
      <td>domainUserPassword</td>
      <td>Password for the domain user.</td>
    </tr>
	</tbody>
</table>

## Code Example

{% highlight csharp %}
public class Example : Artifact.Remote
{
    public override void Configure(IOfferRemoteOperations server, ConDepSettings settings)
    {
        server.RemoveHostFromActiveDirectory("HOSTNAME", "domain.controller.fqdn", "DOMAIN\\Username", "Password");
    }
}
{% endhighlight %}
