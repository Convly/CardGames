using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGameResources.Net
{
    /// <summary>
    /// Enum for the ENV part of the communication protocol.
    /// </summary>
    public enum EnvInfos
    {
        /// <summary>
        /// Communication direction: Server => Client.
        /// Give to the client the list of connected users
        /// The object associated to the command must a List of string of size 1 to 4
        /// </summary>
        S_USER_LIST,

        /// <summary>
        /// Communication direction: Server => Client.
        /// Give to the client the scores for the two teams.
        /// The object associated to the command must a List of integer of size 2
        /// </summary>
        S_SCORES,

        /// <summary>
        /// Communication direction: Server => Client.
        /// Give to the clients the name of the current player.
        /// The object associated to the command must be a <see cref="string"/>
        /// </summary>
        S_SET_TOUR,

        /// <summary>
        /// Communication direction: Server => Client.
        /// Give to the client the remaining time before the end of the round.
        /// The object associated to the command must an <see cref="int"/>
        /// </summary>
        S_SET_REMAINING_TIME,

        /// <summary>
        /// Communication direction: Server => Client.
        /// Give to the client the informations about the teams composition.
        /// The object associated to the command must a Dictionnary which link a player name with its team number
        /// </summary>
        S_SET_TEAM,
    }

    /// <summary>
    /// This class have to be used as the <see cref="Object"/> parameter for a <see cref="Packet"/> with a <see cref="PacketType "/> of value <see cref="PacketType.ENV"/>.
    /// </summary>
    public class Envcall
    {
        private EnvInfos type;
        private Object data;

        /// <summary>
        /// Complete constructor for <see cref="Envcall"/>
        /// </summary>
        public Envcall(EnvInfos type_, Object data_)
        {
            this.Type = type_;
            this.Data = data_;
        }

        /// <summary>
        /// Getter and Setter for the type of the <see cref="Envcall"/>
        /// </summary>
        public EnvInfos Type { get => type; set => type = value; }

        /// <summary>
        /// Getter and Setter for the data <see cref="Object"/> of the <see cref="Envcall"/>
        /// </summary>
        public object Data { get => data; set => data = value; }
    }
}
