# AMIClient

Simple .net standard client for connecting to asterisk voip server AMI

How to add package:

```
Install-Package AsteriskAMIClient -Version 1.0.0
```

How to use:

```
AMIClient voip = new AMIClient();
voip.DataReceived += Voip_DataReceived; ;
voip.Run(server, port, user, password);
```

