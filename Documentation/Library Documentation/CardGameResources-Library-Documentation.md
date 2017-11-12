# CardGameResources Library

<table>
<tbody>
<tr>
<td><a href="#card">Card</a></td>
<td><a href="#deck">Deck</a></td>
</tr>
<tr>
<td><a href="#trumpinfos">TrumpInfos</a></td>
<td><a href="#envcall">Envcall</a></td>
</tr>
<tr>
<td><a href="#envinfos">EnvInfos</a></td>
<td><a href="#err">Err</a></td>
</tr>
<tr>
<td><a href="#errcall">Errcall</a></td>
<td><a href="#gameaction">GameAction</a></td>
</tr>
<tr>
<td><a href="#gamecall">Gamecall</a></td>
<td><a href="#packet">Packet</a></td>
</tr>
<tr>
<td><a href="#packettype">PacketType</a></td>
<td><a href="#syscall">Syscall</a></td>
</tr>
<tr>
<td><a href="#syscommand">SysCommand</a></td>
</tr>
</tbody>
</table>


## Card

Class used to define a Card object.

### Constructor(v, c)

Main constructor for a <a href="#card">Card</a>

| Name | Description |
| ---- | ----------- |
| v | *System.Char*<br>The value of the card |
| c | *System.String*<br>The color of the card |

### Color

Getter and Setter for the color of the <a href="#card">Card</a>

### Value

Getter and Setter for the value of the <a href="#card">Card</a>.


## Deck

Class used to store and manipulate one or multiples <a href="#card">Card</a>

### Constructor

Default constructor for <a href="#deck">Deck</a>

### Constructor(array_)

Second constructor for <a href="#deck">Deck</a>. It allow to fill directly the <a href="#deck">Deck</a> with some <a href="#card">Card</a>.

| Name | Description |
| ---- | ----------- |
| array_ | *System.Collections.Generic.List{CardGameResources.Game.Card}*<br>The List of <a href="#card">Card</a> which will be copied into the <a href="#deck">Deck</a> |

### Add(card_)

Add a <a href="#card">Card</a> into the <a href="#deck">Deck</a>

| Name | Description |
| ---- | ----------- |
| card_ | *CardGameResources.Game.Card*<br>The <a href="#card">Card</a> which will be append to thee <a href="#deck">Deck</a> |

#### Returns

This method always returns true

### Array

Getter and Setter for the List of <a href="#card">Card</a> contained in the <a href="#deck">Deck</a>

### Clear

Remove all the <a href="#card">Card</a> objects in the <a href="#deck">Deck</a>

### Contains(card)

Check if a <a href="#card">Card</a> is in the <a href="#deck">Deck</a>

| Name | Description |
| ---- | ----------- |
| card | *CardGameResources.Game.Card*<br>The <a href="#card">Card</a> used for the search |

#### Returns

True if the <a href="#card">Card</a> is contained in the <a href="#deck">Deck</a>, false otherwise.

### Remove(card)

Remove a specific <a href="#card">Card</a> from the <a href="#deck">Deck</a>

| Name | Description |
| ---- | ----------- |
| card | *CardGameResources.Game.Card*<br>The <a href="#card">Card</a> which will be removed |

### Remove(index)

Remove a <a href="#card">Card</a> by its index in the <a href="#deck">Deck</a>

| Name | Description |
| ---- | ----------- |
| index | *System.Int32*<br>The index of the <a href="#card">Card</a> |

#### Returns

False if the index is greater than the number of cards in the <a href="#deck">Deck</a>, true otherwise.


## TrumpInfos

TrumpInfos is a class used to store information about a trump in a card game.

### Constructor

Default constructor for TrumpInfos

### Constructor(card_, owner_, realColor_)

Complete constructor for TrumpInfos

| Name | Description |
| ---- | ----------- |
| card_ | *CardGameResources.Game.Card*<br>The card which will be stored as the trump |
| owner_ | *System.String*<br>The name of the player which took the Trump |
| realColor_ | *System.String*<br>The real color of the Trump. It can differ from the Card color |

### Constructor(card_)

Constructor which take a Card in parameter

| Name | Description |
| ---- | ----------- |
| card_ | *CardGameResources.Game.Card*<br>The <a href="#trumpinfos.card">TrumpInfos.Card</a> associated to the trump, the color of the card may differs from the real trump color |

### Card

Getter and Setter for the Card

### Owner

Getter and Setter for the Owner of the trump Card

### RealColor

Getter and Setter for the trump real color


## Envcall

This class have to be used as the <a href="#system.object">System.Object</a> parameter for a <a href="#packet">Packet</a> with a <a href="#packettype">PacketType</a> of value <a href="#packettype.env">PacketType.ENV</a>.

### Constructor(CardGameResources.Net.EnvInfos,System.Object)

Complete constructor for <a href="#envcall">Envcall</a>

### Data

Getter and Setter for the data <a href="#system.object">System.Object</a> of the <a href="#envcall">Envcall</a>

### Type

Getter and Setter for the type of the <a href="#envcall">Envcall</a>


## EnvInfos

Enum for the ENV part of the communication protocol.

### S_SCORES

**Communication direction:** *Server => Client.*  

Give to the client the scores for the two teams.  
> The object associated with the command must a List of integer of size 2  

### S_SET_REMAINING_TIME

**Communication direction:** *Server => Client.*  

Give to the client the remaining time before the end of the round.  
> The object associated with the command must a <a href="#system.int32">System.Int32</a>  

### S_SET_TEAM

**Communication direction:** *Server => Client.*  

Give to the client the information about the teams' composition.  
> The object associated with the command must a Dictionary which link a player name with its team number  

### S_SET_TOUR

**Communication direction:** *Server => Client.*  

Give to the clients the name of the current player.  
> The object associated to the command must be a <a href="#system.string">System.String</a>

### S_USER_LIST

**Communication direction:** *Server => Client.*  

Give to the client the list of connected users  
> The object associated to the command must a List of string of size 1 to 4


## Err

Enum for the ERROR part of the communication protocol.

### BAD_ARGUMENT

**Communication direction:** *Server => Client.*  

Inform the client that the server has received a bad argument on one of the client's request.  
> The object associated with the command must be null

### BROKEN_RULE

**Communication direction:** *Server => Client.*  

Inform the client that he broke a rule by one of its action.  
> The object associated with the command must be null

### FORBIDDEN_ACTION

**Communication direction:** *Server => Client.*  

Inform the client that the action he did was not allowed.  
> The object associated with the command must be null

### FORBIDDEN_CARD

**Communication direction:** *Server => Client.*  

Inform the client that the card he played was not valid.  
> The object associated with the command must be null

### SERVER_FULL

**Communication direction:** *Server => Client.*  

Inform the client that he can't register to the server because it has no remaining slot.  
> The object associated with the command must be null

### UNKNOWN_ERROR

**Communication direction:** *Server => Client.*  

Inform the client that an unexpected error has been thrown on the server.  
> The object associated with the command must be null


## Errcall

This class have to be used as the <a href="#system.object">System.Object</a> parameter for a <a href="#packet">Packet</a> with a <a href="#packettype">PacketType</a> of value <a href="#packettype.err">PacketType.ERR</a>.

### Constructor

Default constructor for <a href="#errcall">Errcall</a>

### Constructor(CardGameResources.Net.Err,System.String)

Complete constructor for <a href="#errcall">Errcall</a>

### Constructor(CardGameResources.Net.Err)

Constructor with type defined only for <a href="#errcall">Errcall</a>

### Message

Getter and Setter for the data <a href="#system.object">System.Object</a> of the <a href="#errcall">Errcall</a>

### Type

Getter and Setter for the type of the <a href="#errcall">Errcall</a>


## GameAction

Enum for the GAME part of the communication protocol.

### C_PLAY_CARD

**Communication direction:** *Client => Server.*  

Inform the server that the client want to play a specific <a href="#cardgameresources.game.card">CardGameResources.Game.Card</a> of its <a href="#cardgameresources.game.deck">CardGameResources.Game.Deck</a>.  
The object associated to the command must be a <a href="#cardgameresources.game.card">CardGameResources.Game.Card</a>

### C_TAKE_TRUMP

**Communication direction:** *Client => Server.*  

This is the answer to the first lap of the trump phase.  
The client uses this command to inform if want (or not) to take the trump.  
> The object associated with the command must be a <a href="#system.boolean">System.Boolean</a>

### C_TAKE_TRUMP_AS

**Communication direction:** *Client => Server.*  

This is the answer to the second lap of the trump phase.  
The client uses this command to inform if want (or not) to take the trump as the color of its choice.  
> The object associated with the command must be a <a href="#system.string">System.String</a> (either empty or with the name of the wanted color)

### S_REQUEST_TRUMP_FROM

**Communication direction:** *Server => Client.*  

The server uses this command to ask if a client wants to take the trump or not.  
Following the lap number (1 or 2), the answer might be either <a href="#gameaction.c_take_trump">GameAction.C_TAKE_TRUMP</a> or <a href="#gameaction.c_take_trump_as">GameAction.C_TAKE_TRUMP_AS</a>.  
> The object associated with the command must be a KeyValuePair(int, string) where the key is the lap number and the string the player targeted.

### S_SET_BOARD_DECK

**Communication direction:** *Server => Client.*  

The server use this command to send the updated board <a href="#cardgameresources.game.deck">CardGameResources.Game.Deck</a>.  
The object associated to the command must be a <a href="#cardgameresources.game.deck">CardGameResources.Game.Deck</a>.

### S_SET_LASTROUND_DECK

**Communication direction:** *Server => Client.*  

The server uses this command to send the updated <a href="#cardgameresources.game.deck">CardGameResources.Game.Deck</a> for the last played round.  
> The object associated with the command must be a Dictionary(string, Card) of size 4 where each key represents the name of the player which play the card in Value.

### S_SET_TRUMP

**Communication direction:** *Server => Client.*  

The server uses this command to send the information about the trump to clients.  
The object associated to the command must be a <a href="#cardgameresources.game.trumpinfos">CardGameResources.Game.TrumpInfos</a>

### S_SET_USER_DECK

**Communication direction:** *Server => Client.*  

The server use this command to send the updated user <a href="#cardgameresources.game.deck">CardGameResources.Game.Deck</a> to the client.  
The object associated to the command must be a <a href="#cardgameresources.game.deck">CardGameResources.Game.Deck</a>.


## Gamecall

This class have to be used as the <a href="#system.object">System.Object</a> parameter for a <a href="#packet">Packet</a> with a <a href="#packettype">PacketType</a> of value <a href="#packettype.game">PacketType.GAME</a>.

### Constructor(action_, data_)

Main constructor for <a href="#gamecall">Gamecall</a>

| Name | Description |
| ---- | ----------- |
| action_ | *CardGameResources.Net.GameAction*<br>The type of the action you want to send. See also 
#### See also

- <a href="#syscommand">SysCommand</a>
 |
| data_ | *System.Object*<br>The object associated to the <a href="#syscommand">SysCommand</a> |

### Action

Getter and Setter for the action of the <a href="#gamecall">Gamecall</a>

### Data

Getter and Setter for the data <a href="#system.object">System.Object</a> of the <a href="#gamecall">Gamecall</a>


## Packet

Class used to transfer data across the network.

### Constructor

Default constructor for a <a href="#packet">Packet</a>

### Constructor(name_, type_, data_, registration_)

Registration constructor for client side. The key is initialized to 0.

| Name | Description |
| ---- | ----------- |
| name_ | *System.String*<br>The name of the emitter. |
| type_ | *CardGameResources.Net.PacketType*<br>The type of the <a href="#system.object">System.Object</a> member |
| data_ | *System.Object*<br>The <a href="#system.object">System.Object</a> which contain the data |
| registration_ | *System.Boolean*<br>This boolean must be set to tru only if the type is <a href="#packettype.sys">PacketType.SYS</a> and the data's command is of value <a href="#syscommand.c_register">SysCommand.C_REGISTER</a> |

### Constructor(name_, type_, data_)

Main constructor for client side. The key is initialized to 0 and the registration state to false.

| Name | Description |
| ---- | ----------- |
| name_ | *System.String*<br>The name of the emitter. |
| type_ | *CardGameResources.Net.PacketType*<br>The type of the <a href="#system.object">System.Object</a> member |
| data_ | *System.Object*<br>The <a href="#system.object">System.Object</a> which contain the data |

### Constructor(name_, key_, type_, data_)

Main constructor for server side, the key must be initialized with a value.

| Name | Description |
| ---- | ----------- |
| name_ | *System.String*<br>The name of the emitter. For server side, it will be "root" |
| key_ | *System.UInt32*<br>The value of the Locker key |
| type_ | *CardGameResources.Net.PacketType*<br>The type of the <a href="#system.object">System.Object</a> member |
| data_ | *System.Object*<br>The <a href="#system.object">System.Object</a> which contain the data |

### Data

Getter and Setter for the Packet's data.

### Key

Getter and Setter for the Packet's reference on a Locker key

### Name

Getter and Setter for the Packet's emitter name.

### Registration

Getter and Setter for the registration state

### Type

Getter and Setter for the Packet's type.


## PacketType

Enum used to defined the type of the <a href="#packet">Packet</a>'s <a href="#system.object">System.Object</a>

### ENV

The associated object must be an <a href="#envcall">Envcall</a>

### ERR

The associated object must be an <a href="#errcall">Errcall</a>

### GAME

The associated object must be a <a href="#gamecall">Gamecall</a>

### SYS

The associated object must be a <a href="#syscall">Syscall</a>


## Syscall

This class have to be used as the <a href="#system.object">System.Object</a> parameter for a <a href="#packet">Packet</a> with a <a href="#packettype">PacketType</a> of value <a href="#packettype.sys">PacketType.SYS</a>.

### Constructor(command_, data_)

Main constructor for <a href="#syscall">Syscall</a>

| Name | Description |
| ---- | ----------- |
| command_ | *CardGameResources.Net.SysCommand*<br>The type of the command you want to send. See also 
#### See also

- <a href="#syscommand">SysCommand</a>
 |
| data_ | *System.Object*<br>The object associated to the <a href="#syscommand">SysCommand</a> |

### Command

Getter and Setter for the command of the <a href="#syscall">Syscall</a>

### Data

Getter and Setter for the data <a href="#system.object">System.Object</a> of the <a href="#syscall">Syscall</a>


## SysCommand

Enum for the SYS part of the communication protocol.

### C_QUIT

**Communication direction:** *Client => Server.*  

Request a registration to the server from a client.  
This command must be called only once by game.  
> The object associated with the command must be null.

### C_REGISTER

**Communication direction:** *Client => Server.*  

Request a registration to the server from a client.  
This command must be called only once by game.  
> The object associated with the command must be null.

### S_CONNECTED

**Communication direction:** *Server => Clien*  

Inform the client that he has been disconnected from the server.  
> The object associated with the command must be null.

### S_DISCONNECTED

**Communication direction:** *Client => Server.*  

Inform the client that a client is about to be ejected from the server.  
> The object associated with the command must be the name of a client.

### S_END_GAME

**Communication direction:** *Server => Client.*  

Inform the client that the game has ended.  
> The object associated with the command must be null.

### S_POKE

**Communication direction:** *Client => Server.*  

Indicate to the server that you have successfully received a message. It also unlocks the next action for the server.  
You must send this request less than one second after you received the server message with the same key.  
The key of the Packet must match the Locker key you want to unlock.  
> The object associated with the command must be null.

### S_START_GAME

**Communication direction:** *Server => Client.*  

Inform the client that the game has begun.  
> The object associated with the command must be null.
