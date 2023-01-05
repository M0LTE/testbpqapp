# testbpqapp

A repo containing the simplest possible BPQ app in C#, plus the simplest way I've found to integrate it with BPQ.

This recipe will create an interactive text based application which a packet user can access by connecting to your node over the air and typing a command.

This can be the basis for much greater things.

## Integration

In bpq32.cfg, define a Telnet port if you don't have one already. Note its `PORTNUM`.

Within your Telnet port definition, define a `CONFIG` section.

Within the `CONFIG` section, have a `CMDPORT` line:

```
PORT
  PORTNUM=2
  ID=Telnet
  DRIVER=Telnet
...
  CONFIG
    ...
    CMDPORT 63000
ENDPORT
```

Where `63000` is the TCP port of some program you have written, like the one in this Github repo, listening on localhost.

Later in your bpq32.cfg, define a new application:

```
APPLICATION 3,TEST,C 2 HOST 0 S
```

Where:
 - `3` is the application number, just incremenent by one after your previous application, or if you have none, use 1
 - `TEST` is the node command which has to be typed by a visitor to your node to invoke the command (this shows up when you type `HELP`)
 - `2` in `C 2 HOST 0 S` is the `PORTNUM` of your Telnet port definition, whatever it happens to be
 
Don't know what the rest of the magic is in that line but it's probably defined here: https://www.cantab.net/users/john.wiseman/Documents/LinBPQ%20Applications%20Interface.html

Run an application which listens to TCP port 63000 (or whatever port you specified above).

When a user types `TEST` (or whatever you defined in the `APPLICATION` line) BPQ will make a TCP connection to localhost:63000 and send a line of test. If your application sends a line of text back down the socket, BPQ will send it out over the air. If something comes in over the air, i.e. the connected user sends us a line of text, it will be sent to your application via the open TCP connection.

For your application to drop your user back to the BPQ node prompt, just close the TCP connection.
