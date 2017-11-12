using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGameResources.Net
{
    /// <summary>
    /// Enum for the ERROR part of the communication protocol.
    /// </summary>
    public enum Err
    {
        /// <summary>
        /// Communication direction: Server => Client.
        /// Inform the client that an unexpected error has been thrown on the server.
        /// The object associated to the command must be null
        /// </summary>
        UNKNOWN_ERROR,

        /// <summary>
        /// Communication direction: Server => Client.
        /// Inform the client that the server has received a bad argument on one of the client's request.
        /// The object associated to the command must be null
        /// </summary>
        BAD_ARGUMENT,

        /// <summary>
        /// Communication direction: Server => Client.
        /// Inform the client that he can't register to the server because it has no remaining slot.
        /// The object associated to the command must be null
        /// </summary>
        SERVER_FULL,

        /// <summary>
        /// Communication direction: Server => Client.
        /// Inform the client that the card he played was not valid.
        /// The object associated to the command must be null
        /// </summary>
        FORBIDDEN_CARD,

        /// <summary>
        /// Communication direction: Server => Client.
        /// Inform the client that the action he did was not allowed.
        /// The object associated to the command must be null
        /// </summary>
        FORBIDDEN_ACTION,

        /// <summary>
        /// Communication direction: Server => Client.
        /// Inform the client that he broke a rule by one of it's action.
        /// The object associated to the command must be null
        /// </summary>
        BROKEN_RULE
    }

    /// <summary>
    /// This class have to be used as the <see cref="Object"/> parameter for a <see cref="Packet"/> with a <see cref="PacketType "/> of value <see cref="PacketType.ERR"/>.
    /// </summary>
    public class Errcall
    {
        private Err type = Err.UNKNOWN_ERROR;
        private string message = "An unexpected error has been thrown";


        /// <summary>
        /// Complete constructor for <see cref="Errcall"/>
        /// </summary>
        public Errcall(Err type_, string message_)
        {
            this.Type = type_;
            this.Message = message_;
        }


        /// <summary>
        /// Constructor with type defined only for <see cref="Errcall"/>
        /// </summary>
        public Errcall(Err type_)
        {
            this.Type = type_;
        }

        /// <summary>
        /// Default constructor for <see cref="Errcall"/>
        /// </summary>

        public Errcall()
        {
        }

        /// <summary>
        /// Getter and Setter for the type of the <see cref="Errcall"/>
        /// </summary>
        public Err Type { get => type; set => type = value; }

        /// <summary>
        /// Getter and Setter for the data <see cref="Object"/> of the <see cref="Errcall"/>
        /// </summary>
        public string Message { get => message; set => message = value; }
    }
}
