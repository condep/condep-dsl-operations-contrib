---
layout: doc
title: NodeJs
permalink: /3-0/remote/nodejs/
version_added: 3.2.0
---

Installs NodeJs on server.

## Syntax

{% highlight csharp %}
NodeJs(string version)
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
      <td>Version</td>
      <td>The version you want to install. Default is version 4.2.1</td>
    </tr>
	</tbody>
</table>

## Code Example

{% highlight csharp %}
public class Example : Artifact.Remote
{
    public override void Configure(IOfferRemoteOperations server, ConDepSettings settings)
    {
        server.Install.NodeJs("4.2.1");
    }
}
{% endhighlight %}