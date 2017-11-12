# Communication Protocol Documentation

The following document showcase the design of the communication protocol used for the CardGames project.

## Packet
Class used to transfer data across the network.

| Type | Member| Description |
| ---- | ---- | ----------- |
| string| Name | The name of the emitter |
| PacketType| type | The real type of Data |
| Object| Data| The Object which contains the transmission data which can be of the following types: EnvCall, SysCall, ErrCall, GameCall.  **Each of these objects contains a Type (*also called Action or Command*) and an associated object. These Types are defined in appropriates enum defined below** |
| bool | Registration | This boolean must be set to true only if the type is <a href="#packettype.sys">PacketType.SYS</a> and the data's command is of value <a href="#syscommand.c_register">SysCommand.C_REGISTER</a> |
| uint | Key | The locker key |

***

#### The following enums are designed to define which command can be used in the communication process.

***

## EnvInfos

Enum for the ENV part of the communication protocol. **(Must be attached to an EnvCall object)**

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
> The object associated with the command must a List of string of size 1 to 4

***

## Err **(Must be attached to an ErrCall object)**

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

***

## GameAction **(Must be attached to a GameCall object)**

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

***

## SysCommand **(Must be attached to a SysCall object)**

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
