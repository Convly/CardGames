using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGameResources.Net
{
    /// <summary>
    /// Enum for the SYS part of the communication protocol.
    /// </summary>
    public enum SysCommand
    {
        /// <summary>
        /// Communication direction: Client => Server.
        /// Request a registration to the server from a client. This command must be called only once by game.
        /// The object associated to the command must be null.
        /// </summary>
        C_REGISTER,

        /// <summary>
        /// Communication direction: Client => Server.
        /// Request a registration to the server from a client. This command must be called only once by game.
        /// The object associated to the command must be null.
        /// </summary>
        C_QUIT,

        /// <summary>
        /// Communication direction: Client => Server.
        /// Inform the client that a client is about to be ejected from the server.
        /// The object associated to the command must be the name of a client.
        /// </summary>
        S_DISCONNECTED,

        /// <summary>
        /// Communication direction: Server => Client.
        /// Inform the client that he has been disconnected from the server.
        /// The object associated to the command must be null.
        /// </summary>
        S_CONNECTED,

        /// <summary>
        /// Communication direction: Server => Client.
        /// Inform the client that the game has began.
        /// The object associated to the command must be null.
        /// </summary>
        S_START_GAME,

        /// <summary>
        /// Communication direction: Server => Client.
        /// Inform the client that the game has ended.
        /// The object associated to the command must be null.
        /// </summary>
        S_END_GAME,

        /// <summary>
        /// Communication direction: Client => Server.
        /// Indicate to the server than you have successfully received a message.
        /// It also unlock the next action for the server.
        /// You must send this request less than one second after you received the server message with the same key.
        /// The key of the Packet must match the Locker key you want to unlock.
        /// The object associated to the command must be null.
        /// </summary>
        S_POKE
    }

    /// <summary>
    /// This class have to be used as the <see cref="Object"/> parameter for a <see cref="Packet"/> with a <see cref="PacketType "/> of value <see cref="PacketType.SYS"/>.
    /// </summary>
    public class Syscall
    {
        private SysCommand command;
        private Object data;

        /// <summary>
        /// Main constructor for <see cref="Syscall"/>
        /// </summary>
        /// <param name="command_">The type of the command you want to send. See also <seealso cref="SysCommand"/></param>
        /// <param name="data_">The object associated to the <see cref="SysCommand"/></param>
        public Syscall(SysCommand command_, Object data_)
        {
            this.Command = command_;
            this.Data = data_;
        }

        /// <summary>
        /// Getter and Setter for the command of the <see cref="Syscall"/>
        /// </summary>
        public SysCommand Command { get => command; set => command = value; }
        /// <summary>
        /// Getter and Setter for the data <see cref="Object"/> of the <see cref="Syscall"/>
        /// </summary>
        public object Data { get => data; set => data = value; }
    }
}
