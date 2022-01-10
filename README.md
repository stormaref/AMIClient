# AMIClient

Simple .net standard client for connecting to asterisk voip server AMI

How to add package:

```
Install-Package AsteriskAMIClient -Version 1.0.0
```

How to use:

```c#
var server = "192.168.1.1";
var port = 5038;
var user = "admin";
var password = "password";
var client = new AmiClient();
client.DataReceived += Voip_DataReceived;
client.Run(server, port, user, password);
```

just pass your ami ip and port to connect with your username and password
