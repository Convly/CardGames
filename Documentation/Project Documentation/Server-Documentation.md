# Servers

<table>
<tbody>
<tr>
<td><a href="#program">Program</a></td>
<td><a href="#core">Core</a></td>
</tr>
<tr>
<td><a href="#game">Game</a></td>
<td><a href="#referee">Referee</a></td>
</tr>
</tbody>
</table>


## Program

Entry point of the server project


## Core

Core class of the server

### Constructor

Default constructor of the <a href="#core">Core</a>

### Instance

Getter and Setter of the singleton instance of the <a href="#core">Core</a>

### Locker

Getter and Setter for the locker state of the <a href="#core">Core</a>

### Start

Start the server's dependencies like the Network and loop infinitly the execution.


## Game

Game Logic for the Belote card Game used by <a href="#referee">Referee</a>

### Constructor

Default constructor for <a href="#game">Game</a>

### AIEntryPoint(name, p)

Main entry point for all the AI actions

| Name | Description |
| ---- | ----------- |
| name | *System.String*<br>Define the name of the player which the AI will play for. |
| p | *CardGameResources.Net.Packet*<br>The packet transmission that the AI will use |

### AIPlayCard(name)

Logic for AI on <a href="#cardgameresources.net.envinfos.s_set_tour">CardGameResources.Net.EnvInfos.S_SET_TOUR</a> event.

| Name | Description |
| ---- | ----------- |
| name | *System.String*<br>The name of the player who must play |

### AITakeTrump(data)

Logic for AI on <a href="#cardgameresources.net.gameaction.s_request_trump_from">CardGameResources.Net.GameAction.S_REQUEST_TRUMP_FROM</a> event.

| Name | Description |
| ---- | ----------- |
| data | *System.Collections.Generic.KeyValuePair{System.Int32,System.String}*<br>A pair of value which contains the lap number and the name of the player who must play |

### AssignTrump(name, color)

Assign a name and a color to the <a href="#game.trumpinfos">Game.TrumpInfos</a>

| Name | Description |
| ---- | ----------- |
| name | *System.String*<br>The name of the owner |
| color | *System.String*<br>The name of the real color of the tump |

### CheckDeckWinner

This method check who's the winner for the current round, update the scores and notify the users about it

#### Returns

The name of the winner

### FillUserDeck

This method will randomly fill the <a href="#cardgameresources.game.deck">CardGameResources.Game.Deck</a> of all the connected users with a limit of 8 <a href="#cardgameresources.game.card">CardGameResources.Game.Card</a> per user.

### GamePlayCheckMove(userDeck, playedCard, board, color, powerCheck)

Utility method mainly used by <a href="#game.playcard_callback(system.string,cardgameresources.game.card)">Game.PlayCard_callback(System.String,CardGameResources.Game.Card)</a> to check if a user can play a specific <a href="#cardgameresources.game.card">CardGameResources.Game.Card</a>

| Name | Description |
| ---- | ----------- |
| userDeck | *CardGameResources.Game.Deck*<br>The deck from which "playedCard" is issued |
| playedCard | *CardGameResources.Game.Card*<br>The <a href="#cardgameresources.game.card">CardGameResources.Game.Card</a> played |
| board | *CardGameResources.Game.Deck*<br>The current <a href="#cardgameresources.game.deck">CardGameResources.Game.Deck</a> on the board |
| color | *System.String*<br>The current board main color |
| powerCheck | *System.Boolean*<br>If set to true, the method will also check if the user have a better card in he's <a href="#cardgameresources.game.deck">CardGameResources.Game.Deck</a> |

#### Returns



### GetCardPoints(card)

Get the amount of point for a specific <a href="#cardgameresources.game.card">CardGameResources.Game.Card</a>. This method take in consideration the actual trump.

| Name | Description |
| ---- | ----------- |
| card | *CardGameResources.Game.Card*<br> |

#### Returns

The amount of point for card

### GetCardValue(card)

Return the real value for the paramaeter card. This method allow the user to do some arithmetics and comparaison with the cards.

| Name | Description |
| ---- | ----------- |
| card | *CardGameResources.Game.Card*<br> |

#### Returns

The value of the card as an integer

### GetUserListFrom(begin)

Generate a new list of user which indicate the order of the plays

| Name | Description |
| ---- | ----------- |
| begin | *System.String*<br>The username which will play first |

#### Returns

The ordered list of players

### GiveCards

This method distribute randomly 5 <a href="#cardgameresources.game.card">CardGameResources.Game.Card</a> per user

### Init

This method will initialize each component of the game logic

### InitMasterDeck

Initialize the master deck, which is a constant <a href="#cardgameresources.game.deck">CardGameResources.Game.Deck</a> of all the 32 avalaible cards.

### InitUsersDeck

Initialize the user <a href="#cardgameresources.game.deck">CardGameResources.Game.Deck</a>s with 5 <a href="#cardgameresources.game.card">CardGameResources.Game.Card</a>

### IsColorInDeck(deck, color)

This method will check if there is at least one <a href="#cardgameresources.game.card">CardGameResources.Game.Card</a> with the color color in the deck

| Name | Description |
| ---- | ----------- |
| deck | *CardGameResources.Game.Deck*<br>The <a href="#cardgameresources.game.deck">CardGameResources.Game.Deck</a> of card which will be the object of the search. |
| color | *System.String*<br>The color which will be used for the search. |

#### Returns

The number of <a href="#cardgameresources.game.card">CardGameResources.Game.Card</a> which match the color

### PlayCard_callback(name, card)

Entry point of the main rules method. It will be called when the server receive a request of type <a href="#cardgameresources.net.gameaction.c_play_card">CardGameResources.Net.GameAction.C_PLAY_CARD</a> This function act also as a referee by checking if the user movement does not break any rules.

| Name | Description |
| ---- | ----------- |
| name | *System.String*<br>The name of the user who sent the command |
| card | *CardGameResources.Game.Card*<br>The card that the user want to play |

### PlayCard(name, card)

This function will play a specific <a href="#cardgameresources.game.card">CardGameResources.Game.Card</a> for name. It'll also update all the needed variables and trigger all the changes to the users.

| Name | Description |
| ---- | ----------- |
| name | *System.String*<br>The name of the user who sent the command |
| card | *CardGameResources.Game.Card*<br>The card that the user want to play |

### PlayPhase

Entry point for the main gameplay phase. Manage the core of the game.

### Reconnect(name)

Reconnect a player who's been previously disconnect from the server.

| Name | Description |
| ---- | ----------- |
| name | *System.String*<br>The name of the player who just registered |

### Reset

This method is designed to totally reset the game logic.

### Run

Start the GamePlay phases: TrumpPhase, PlayPhase ; then end the game

### Send(type, data)

Send some data of type "type" to all registered users.

| Name | Description |
| ---- | ----------- |
| type | *CardGameResources.Net.PacketType*<br>The type of data which will be send |
| data | *System.Object*<br>The data which will be send |

#### Returns



### Send(name, type, data)

Send some data of type "type" to name.

| Name | Description |
| ---- | ----------- |
| name | *System.String*<br>The username who will receive the request |
| type | *CardGameResources.Net.PacketType*<br>The type of the data which will be send |
| data | *System.Object*<br>The data which will be send |

### StartGame

Entry point of the game. Called by the referee when 4 players are connected

### TakeTrump_callback(name, ans)

Callback method when the server receive a <a href="#cardgameresources.net.gameaction.c_take_trump">CardGameResources.Net.GameAction.C_TAKE_TRUMP</a> request

| Name | Description |
| ---- | ----------- |
| name | *System.String*<br>The name of the user who sent the request |
| ans | *System.Boolean*<br>The answer of the user |

### TakeTrumpAs_callback(name, color)

Callback method when the server receive a <a href="#cardgameresources.net.gameaction.c_take_trump_as">CardGameResources.Net.GameAction.C_TAKE_TRUMP_AS</a> request

| Name | Description |
| ---- | ----------- |
| name | *System.String*<br>The name of the user who sent the request |
| color | *System.String*<br>A string containing the color of the trump, or nothing |

### TrumpDecision

This method will chose a card for the trump randomly and will then notify all the clients about it

### TrumpPhase

Manage the trump phase: init, request, chose, assign and notify

#### Returns



### TrumpPhaseInitLock(phase)

Manage the locks for the trump phase

| Name | Description |
| ---- | ----------- |
| phase | *System.Int32*<br> |

### TrumpPhaseWait(phase)

Lock the actions for trump phase

| Name | Description |
| ---- | ----------- |
| phase | *System.Int32*<br> |


## Referee

The mission of the referee is to regulate the flow of request handled by the <a href="#network.server">Network.Server</a>. It also act as a router and can interact directly with the <a href="#referee.game">Referee.Game</a>.

### CheckRegisterValidity(name)

Check if the client can register to the server

| Name | Description |
| ---- | ----------- |
| name | *System.String*<br>The name of the client |

### EntryPoint(obj)

This method is triggered when the server receive an object. It Will redirect the object trhough the different routes

| Name | Description |
| ---- | ----------- |
| obj | *System.Object*<br>The <a href="#cardgameresources.net.packet">CardGameResources.Net.Packet</a> object serialized as an <a href="#system.object">System.Object</a> |

### GameEntryPoint(p)
GAME FUNCTIONS 
This function is called by the main entry point when the received object is of type <a href="#cardgameresources.net.packettype.game">CardGameResources.Net.PacketType.GAME</a>

| Name | Description |
| ---- | ----------- |
| p | *CardGameResources.Net.Packet*<br>The <a href="#cardgameresources.net.packet">CardGameResources.Net.Packet</a> received from the server |

### Instance

A getter and a setter for the Referee singleton instance

### PokeHandling(key, name)

This method is used to handle synchronous events. It will unlock the <a href="#network.lock.locker">Network.Lock.Locker</a> associated to the key of the <a href="#cardgameresources.net.packet">CardGameResources.Net.Packet</a>

| Name | Description |
| ---- | ----------- |
| key | *System.UInt32*<br>The key of the <a href="#network.lock.locker">Network.Lock.Locker</a> |
| name | *System.String*<br>The name of the client who try to unlock the <a href="#network.lock.locker">Network.Lock.Locker</a> |

### Register(name, evt)

Try to register a user into the server. If the server went full, it'll start the game.

| Name | Description |
| ---- | ----------- |
| name | *System.String*<br>The name of the client who try to register |
| evt | *CardGameResources.Net.Syscall*<br>The content of the <a href="#cardgameresources.net.syscall">CardGameResources.Net.Syscall</a> event contained in the received <a href="#cardgameresources.net.packet">CardGameResources.Net.Packet</a> |

### SysEntryPoint(p)
SYS FUNCTIONS 
This function is called by the main entry point when the received object is of type <a href="#cardgameresources.net.packettype.sys">CardGameResources.Net.PacketType.SYS</a>

| Name | Description |
| ---- | ----------- |
| p | *CardGameResources.Net.Packet*<br>The <a href="#cardgameresources.net.packet">CardGameResources.Net.Packet</a> received from the server |
