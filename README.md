# AMIClient

Simple .net standard client for connecting to asterisk voip server AMI

How to add package:

```
Install-Package AsteriskAMIClient -Version 1.0.0
```

How to use:

```c#
var client = new AmiClient();
client.DataReceived += Voip_DataReceived; ;
client.Run(server, port, user, password);
```

