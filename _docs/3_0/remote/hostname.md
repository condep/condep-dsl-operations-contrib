---
layout: doc
title: Hostname
permalink: /3-0/remote/hostname/
version_added: 3.2.0
---

Set hostname.

## Syntax

{% highlight csharp %}
Hostname(string hostname)
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
      <td>Wanted hostname</td>
    </tr>
	</tbody>
</table>

## Code Example

{% highlight csharp %}
public class Example : Artifact.Remote
{
    public override void Configure(IOfferRemoteOperations server, ConDepSettings settings)
    {
        server.Configure.Hostname("HOSTNAME");
    }
}
{% endhighlight %}