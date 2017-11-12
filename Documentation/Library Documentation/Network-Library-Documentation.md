# Network

<table>
<tbody>
<tr>
<td><a href="#client">Client</a></td>
<td><a href="#infosclient">InfosClient</a></td>
</tr>
<tr>
<td><a href="#locker">Locker</a></td>
<td><a href="#lockmanager">LockManager</a></td>
</tr>
<tr>
<td><a href="#server">Server</a></td>
</tr>
</tbody>
</table>


## Client

Client class for the Network library. This class allow the user to start a transmission in order to receive and send requests to a single <a href="#server">Server</a>.

### CallBackFct

Static callback method for the server request

### ClientRequest(header, connection, data)

Trigered function when the server want to communicate with the client

| Name | Description |
| ---- | ----------- |
| header | *NetworkCommsDotNet.PacketHeader*<br> |
| connection | *NetworkCommsDotNet.Connections.Connection*<br> |
| data | *System.String*<br>Data send by the server |

### Finalize

Destructor of the <a href="#client">Client</a>. When called, it'll shutdown all the transmissions

### Instance

Getter for the singleton instance of a <a href="#client">Client</a>

### MsgCallbackFct

Static callback method for the chat server request

### MsgRequest(header, connection, msg)

Get a msg for the chat from the server

| Name | Description |
| ---- | ----------- |
| header | *NetworkCommsDotNet.PacketHeader*<br> |
| connection | *NetworkCommsDotNet.Connections.Connection*<br> |
| msg | *System.String*<br> |

### SendDataToServer(data)

Send an object to the server (you will minimum need "public string name" in your object

| Name | Description |
| ---- | ----------- |
| data | *System.Object*<br> |

### SendMsgChat(msg)

Send a msg for the chat to the server

| Name | Description |
| ---- | ----------- |
| msg | *System.String*<br> |

### Start(callBackFct, msgCallbackFct, serverIP, serverPort)

Launch the ClientNetwork

| Name | Description |
| ---- | ----------- |
| callBackFct | *System.Func{System.Object,System.Int32}*<br>Function called when the client receive a message from the server |
| msgCallbackFct | *System.Func{System.String,System.Int32}*<br>Function called when the client received a chat message from the server |
| serverIP | *System.String*<br>Ip of the server you want to connect to |
| serverPort | *System.Int32*<br>Port of the server you want to connect to |


## InfosClient

A class which contains the network information for a client.

### _ip

A <a href="#system.string">System.String</a> for the IP of the client

### _port

An <a href="#system.int32">System.Int32</a> for the port of the client


## Locker

Class designed to lock and unlock asynchronous event. Usually managed by a <a href="#lockmanager">LockManager</a>.

### Constructor(key_, username_, duration_)

Second constructor of the <a href="#locker">Locker</a>, allowing the user to customize the timeout duration.

| Name | Description |
| ---- | ----------- |
| key_ | *System.UInt32*<br>The unique key code of the <a href="#locker">Locker</a> |
| username_ | *System.String*<br>The username of the <a href="#locker">Locker</a>'s owner |
| duration_ | *System.Int32*<br>The duration of the lock period timeout. |

### Constructor(key_, username_)

Main constructor of the <a href="#locker">Locker</a>

| Name | Description |
| ---- | ----------- |
| key_ | *System.UInt32*<br>The unique key code of the <a href="#locker">Locker</a> |
| username_ | *System.String*<br>The username of the <a href="#locker">Locker</a>'s owner |

### Duration

Getter and Setter for the timeout duration of the <a href="#locker">Locker</a>

### Key

Getter and Setter for the <a href="#locker">Locker</a> key

### State

Getter and Setter for the current State of the <a href="#locker">Locker</a>

### Username

Getter and Setter for the Username of the owner of the <a href="#locker">Locker</a>


## LockManager

Class used to store, delete, lock and unlock <a href="#locker">Locker</a>

### Add(username)

Add a new <a href="#locker">Locker</a> in the manager

| Name | Description |
| ---- | ----------- |
| username | *System.String*<br>The owner of the <a href="#locker">Locker</a> |

#### Returns

This method return the generated key of the <a href="#locker">Locker</a>

### Delete(key)

Delete the locker associated to the key given in parameter

| Name | Description |
| ---- | ----------- |
| key | *System.UInt32*<br>The key of the <a href="#locker">Locker</a> |

#### Returns

True if the <a href="#locker">Locker</a> has been successfully deleted, false otherwise.

### Lock(key)

Lock the <a href="#locker">Locker</a> linked to the key given in parameter and wait for a call to <a href="#lockmanager.unlock(system.uint32)">LockManager.Unlock(System.UInt32)</a>

| Name | Description |
| ---- | ----------- |
| key | *System.UInt32*<br>The key of the <a href="#locker">Locker</a> |

#### Returns

False if no <a href="#locker">Locker</a> has been found with this key, true otherwise

### Locks

Getter and Setter for the Dictionnary of <a href="#locker">Locker</a>s, which assign an <a href="#system.uint32">System.UInt32</a> key to its <a href="#locker">Locker</a>

### Unlock(key)

Unlock the <a href="#locker">Locker</a> linked to the key given in parameter.

| Name | Description |
| ---- | ----------- |
| key | *System.UInt32*<br>The key of the <a href="#locker">Locker</a> |

#### Returns

False if the manager does not contain a <a href="#locker">Locker</a> with this key, true otherwise


## Server

Server class for the Network library. This class allow the user to start a transmission in order to receive and send requests to multiples <a href="#client">Client</a>.

### _currentId

Current client ID

### CallBackFct

Callback method which will be called when the server receive client request

### Clients

A getter and a setter to a dictionnary which link for each client's name their network informations (<a href="#infosclient">InfosClient</a>)

### DeleteClient(name)

Delete a client to the server

| Name | Description |
| ---- | ----------- |
| name | *System.String*<br> |

#### Returns



### Finalize

Destructor for the <a href="#server">Server</a> class. It will shutdwon the connection.

### GetIpAddr

Get ip of the pc

#### Returns



### Instance

A getter and a setter for the server singleton instance

### Lock_m

A getter and a setter for the stored LockerManager wich will manager all the synchronous network operations

### MsgRequest(header, connection, msg)

Get a Msg from a client for the chat

| Name | Description |
| ---- | ----------- |
| header | *NetworkCommsDotNet.PacketHeader*<br> |
| connection | *NetworkCommsDotNet.Connections.Connection*<br> |
| msg | *System.String*<br> |

### SendDataToClient(name, data)

Send an object to a client

| Name | Description |
| ---- | ----------- |
| name | *System.String*<br>Name of the client you want to send something |
| data | *CardGameResources.Net.Packet*<br> |

#### Returns



### SendMsgChat(msg)

Send a msg to all the clients for the chat

| Name | Description |
| ---- | ----------- |
| msg | *System.String*<br> |

#### Returns



### ServerRequest(header, connection, data)

Function trigered by the server when the client send a request

| Name | Description |
| ---- | ----------- |
| header | *NetworkCommsDotNet.PacketHeader*<br> |
| connection | *NetworkCommsDotNet.Connections.Connection*<br> |
| data | *System.String*<br>Data sent by the client |

### Start(callBackFct)



| Name | Description |
| ---- | ----------- |
| callBackFct | *System.Func{System.Object,System.Int32}*<br>Function called when the client receive a request from the client |

#### Returns



### Stop

Stop the server by closing all packet handlers and stop the request listening. It also delete all the client's infos
