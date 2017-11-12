# Client Documentation

<table>
<tbody>
<tr>
<td><a href="#app">App</a></td>
<td><a href="#app">App</a></td>
</tr>
<tr>
<td><a href="#mainwindow">MainWindow</a></td>
<td><a href="#resources">Resources</a></td>
</tr>
<tr>
<td><a href="#gameboard">GameBoard</a></td>
<td><a href="#lobby">Lobby</a></td>
</tr>
</tbody>
</table>


## App

App

### InitializeComponent

InitializeComponent

### Main

Application Entry Point.


## App

Interaction logic for App.xaml

### OnExit(e)

Method called when the application quit

| Name | Description |
| ---- | ----------- |
| e | *System.Windows.ExitEventArgs*<br> |

### OnStartup(e)

Method called when the application start

| Name | Description |
| ---- | ----------- |
| e | *System.Windows.StartupEventArgs*<br> |

### Client.GameClient.ChatEntryPoint(msg)

Function trigered when a chat msg is recieved

| Name | Description |
| ---- | ----------- |
| msg | *System.String*<br> |

#### Returns



### Client.GameClient.EntryPoint(data)

Function who check the type of the event server

| Name | Description |
| ---- | ----------- |
| data | *System.Object*<br> |

#### Returns



### Client.GameClient.EnvEntryPoint(data)

Function called when the server send an Env info

| Name | Description |
| ---- | ----------- |
| data | *CardGameResources.Net.Packet*<br> |

### Client.GameClient.ErrorEntryPoint(data)

Function called when the server send an Error

| Name | Description |
| ---- | ----------- |
| data | *CardGameResources.Net.Packet*<br> |

### Client.GameClient.GameEntryPoint(data)

Function called when the server send a Game info

| Name | Description |
| ---- | ----------- |
| data | *CardGameResources.Net.Packet*<br> |

### Client.GameClient.SysEntryPoint(data)

Function called when the server send a System info

| Name | Description |
| ---- | ----------- |
| data | *CardGameResources.Net.Packet*<br> |


## MainWindow

Interaction logic for MainWindow.xaml

MainWindow

### Constructor

Default constructor for the <a href="#mainwindow">MainWindow</a>

### connectButton_Click(sender, e)

Connect the client to the server when he click on this button

| Name | Description |
| ---- | ----------- |
| sender | *System.Object*<br> |
| e | *System.Windows.RoutedEventArgs*<br> |

### InitializeComponent

InitializeComponent

### Instance

Getter and Setter for the singleton instance of <a href="#mainwindow">MainWindow</a>

### ProcessConnect

Getter and Setter for the ProcessConnect state which indicate if the client is actually trying to connect to a server or not.

### Window_KeyDown(sender, e)

Quit the program when you press Escape

| Name | Description |
| ---- | ----------- |
| sender | *System.Object*<br> |
| e | *System.Windows.Input.KeyEventArgs*<br> |


## Resources

Une classe de ressource fortement typée destinée, entre autres, à la consultation des chaînes localisées.

### Culture

Remplace la propriété CurrentUICulture du thread actuel pour toutes les recherches de ressources à l'aide de cette classe de ressource fortement typée.

### ResourceManager

Retourne l'instance ResourceManager mise en cache utilisée par cette classe.


## GameBoard

Interaction logic for GameBoard.xaml

GameBoard

### Constructor

Default constructor for the <a href="#gameboard">GameBoard</a>

### boardCard1_Drop(System.Object,System.Windows.DragEventArgs)

Set the card dropped previously to the boardCard

### boardCard2_Drop(System.Object,System.Windows.DragEventArgs)

Set the card dropped previously to the boardCard

### boardCard3_Drop(System.Object,System.Windows.DragEventArgs)

Set the card dropped previously to the boardCard

### boardCard4_Drop(System.Object,System.Windows.DragEventArgs)

Set the card dropped previously to the boardCard

### Button_Click_1(System.Object,System.Windows.RoutedEventArgs)

Action to send request of <a href="#cardgameresources.net.gameaction.c_take_trump_as">CardGameResources.Net.GameAction.C_TAKE_TRUMP_AS</a> as null

### Button_Click(System.Object,System.Windows.RoutedEventArgs)

Action to send request of <a href="#cardgameresources.net.gameaction.c_take_trump_as">CardGameResources.Net.GameAction.C_TAKE_TRUMP_AS</a> as a color

### chat_boxe_KeyDown(sender, e)

Send a msg from the chat to the server

| Name | Description |
| ---- | ----------- |
| sender | *System.Object*<br> |
| e | *System.Windows.Input.KeyEventArgs*<br> |

### InitializeComponent

InitializeComponent

### Instance

Getter and Setter for the singleton instance of the <a href="#gameboard">GameBoard</a>

### userCard1_MouseLeftButtonDown(System.Object,System.Windows.Input.MouseButtonEventArgs)

Get the card for the drop with the mouse

### userCard2_MouseLeftButtonDown(System.Object,System.Windows.Input.MouseButtonEventArgs)

Get the card for the drop with the mouse

### userCard3_MouseLeftButtonDown(System.Object,System.Windows.Input.MouseButtonEventArgs)

Get the card for the drop with the mouse

### userCard4_MouseLeftButtonDown(System.Object,System.Windows.Input.MouseButtonEventArgs)

Get the card for the drop with the mouse

### userCard5_MouseLeftButtonDown(System.Object,System.Windows.Input.MouseButtonEventArgs)

Get the card for the drop with the mouse

### userCard6_MouseLeftButtonDown(System.Object,System.Windows.Input.MouseButtonEventArgs)

Get the card for the drop with the mouse

### userCard7_MouseLeftButtonDown(System.Object,System.Windows.Input.MouseButtonEventArgs)

Get the card for the drop with the mouse

### userCard8_MouseLeftButtonDown(System.Object,System.Windows.Input.MouseButtonEventArgs)

Get the card for the drop with the mouse

### Window_KeyDown(sender, e)

Quit the program when you press Escape

| Name | Description |
| ---- | ----------- |
| sender | *System.Object*<br> |
| e | *System.Windows.Input.KeyEventArgs*<br> |


## Lobby

Interaction logic for Lobby.xaml

Lobby

### Constructor

Default constructor of the <a href="#lobby">Lobby</a>

### InitializeComponent

InitializeComponent

### Instance

Getter and Setter for the singleton instance of <a href="#lobby">Lobby</a>

### Window_KeyDown(sender, e)

Quit the program when you press Escape

| Name | Description |
| ---- | ----------- |
| sender | *System.Object*<br> |
| e | *System.Windows.Input.KeyEventArgs*<br> |
