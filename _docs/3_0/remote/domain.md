---
layout: doc
title: Domain
permalink: /3-0/remote/domain/
version_added: 3.2.0
---

Joining server to domain.

## Syntax

{% highlight csharp %}
Domain(string domain, string domainUsername, string domainUserPassword, string domainController, string adOuPath)
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
      <td>domain</td>
      <td>Domain name</td>
    </tr>
    <tr>
      <td>domainUsername</td>
      <td>Username for a domain user, in format 'DOMAIN\\Username'</td>
    </tr>
    <tr>
      <td>domainUserPassword</td>
      <td>Password for the domain user.</td>
    </tr>
    <tr>
      <td>domainController</td>
      <td>Domain controller with FQDN</td>
    </tr>
    <tr>
      <td>adOuPath</td>
      <td>The OUPath for Active Directory</td>
    </tr>
	</tbody>
</table>

## Code Example

{% highlight csharp %}
public class Example : Artifact.Remote
{
    public override void Configure(IOfferRemoteOperations server, ConDepSettings settings)
    {
        server.Configure.Domain("DOMAIN", "DOMAIN\\Username", "Password", "domain.controller.fqdn", "adOuPath");
    }
}
{% endhighlight %}