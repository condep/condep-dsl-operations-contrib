DNS operations
==============
This part of the solution contains operations setting up and configuring DNS on servers.

Set DNS Operation
-----------------
Operations for setting DNS servers on all network adapters on a server. If DNS on server is different from the one provided here, this operation will overwrite the existing DNS information. In that way, this operation is idempotent and can run over and over again on the server.

#### Usage
Here is an example of how you use this operation:
```cs
public class DnsExample : Artifact.Remote
{
    public override void Configure(IOfferRemoteOperations server, ConDepSettings settings)
    { 
        server.Configure.Dns(new []{"10.65.89.20", "10.65.89.21"});
    }
}
```