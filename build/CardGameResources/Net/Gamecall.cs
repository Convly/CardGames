using CardGameResources.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGameResources.Net
{
    /// <summary>
    /// Enum for the GAME part of the communication protocol.
    /// </summary>
    public enum GameAction
    {
        /// <summary>
        /// Communication direction: Client => Server.
        /// Inform the server that the client want to play a specific <see cref="Card"/> of its <see cref="Deck"/>.
        /// The object associated to the command must be a <see cref="Card"/>
        /// </summary>
        C_PLAY_CARD,

        /// <summary>
        /// Communication direction: Client => Server.
        /// Answer for the frst lap of the trump phase. The client use this command to inform if want (or not) to take the trump.
        /// The object associated to the command must be a <see cref="bool"/>
        /// </summary>
        C_TAKE_TRUMP,

        /// <summary>
        /// Communication direction: Client => Server.
        /// Answer for the second lap of the trump phase. The client use this command to inform if want (or not) to take the trump as the color of its choice.
        /// The object associated to the command must be a <see cref="string"/> (either empty or with the name of the wanted color)
        /// </summary>
        C_TAKE_TRUMP_AS,
        
        /// <summary>
        /// Communication direction: Server => Client.
        /// The server use this command to send the updated user <see cref="Deck"/> to the client.
        /// The object associated to the command must be a <see cref="Deck"/>.
        /// </summary>
        S_SET_USER_DECK,

        /// <summary>
        /// Communication direction: Server => Client.
        /// The server use this command to send the updated board <see cref="Deck"/>.
        /// The object associated to the command must be a <see cref="Deck"/>.
        /// </summary>
        S_SET_BOARD_DECK,

        /// <summary>
        /// Communication direction: Server => Client.
        /// The server use this command to send the updated <see cref="Deck"/> for the last played round.
        /// The object associated to the command must be a Dictionary(string, Card) of size 4 where each key represent the name of the player which play the card in Value.
        /// </summary>
        S_SET_LASTROUND_DECK,

        /// <summary>
        /// Communication direction: Server => Client.
        /// The server use this command to send the informations about the trump to clients.
        /// The object associated to the command must be a <see cref="TrumpInfos"/>
        /// </summary>
        S_SET_TRUMP,

        /// <summary>
        /// Communication direction: Server => Client.
        /// The server use this command to ask if a client want to take the trump or not.
        /// Following the lap number (1 or 2), the answer might be either <see cref="GameAction.C_TAKE_TRUMP"/> or <see cref="GameAction.C_TAKE_TRUMP_AS"/>.
        /// The object associated to the command must be a KeyValuePair(int, string) where the key is the lap number and the string the player targeted.
        /// </summary>
        S_REQUEST_TRUMP_FROM
    }

    /// <summary>
    /// This class have to be used as the <see cref="Object"/> parameter for a <see cref="Packet"/> with a <see cref="PacketType "/> of value <see cref="PacketType.GAME"/>.
    /// </summary>
    public class Gamecall
    {
        private GameAction action;
        private Object data;

        /// <summary>
        /// Main constructor for <see cref="Gamecall"/>
        /// </summary>
        /// <param name="action_">The type of the action you want to send. See also <seealso cref="SysCommand"/></param>
        /// <param name="data_">The object associated to the <see cref="SysCommand"/></param>
        public Gamecall(GameAction action_, Object data_)
        {
            this.Action = action_;
            this.Data = data_;
        }

        /// <summary>
        /// Getter and Setter for the action of the <see cref="Gamecall"/>
        /// </summary>
        public GameAction Action { get => action; set => action = value; }

        /// <summary>
        /// Getter and Setter for the data <see cref="Object"/> of the <see cref="Gamecall"/>
        /// </summary>
        public object Data { get => data; set => data = value; }
    }
}
