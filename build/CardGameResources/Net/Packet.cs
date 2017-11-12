using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGameResources.Net
{
    /// <summary>
    /// Enum used to defined the type of the <see cref="Packet"/>'s <see cref="Object"/>
    /// </summary>
    public enum PacketType
    {
        /// <summary>
        /// The associated object must be an <see cref="Errcall"/>
        /// </summary>
        ERR = -1,

        /// <summary>
        /// The associated object must be a <see cref="Syscall"/>
        /// </summary>
        SYS = 0,

        /// <summary>
        /// The associated object must be a <see cref="Gamecall"/>
        /// </summary>
        GAME,

        /// <summary>
        /// The associated object must be an <see cref="Envcall"/>
        /// </summary>
        ENV
    }

    /// <summary>
    /// Class used to transfer data across the network.
    /// </summary>
    public class Packet
    {
        private string name;
        private uint key;
        private PacketType type;
        private Object data;
        private bool registration;


        /// <summary>
        /// Default constructor for a <see cref="Packet"/>
        /// </summary>
        public Packet()
        {

        }

        /// <summary>
        /// Main constructor for client side. The key is initialized to 0 and the registration state to false.
        /// </summary>
        /// <param name="name_">The name of the emitter.</param>
        /// <param name="type_">The type of the <see cref="Object"/> member</param>
        /// <param name="data_">The <see cref="Object"/> which contain the data</param>
        public Packet(string name_, PacketType type_, Object data_)
        {
            this.Name = name_;
            this.Key = 0;
            this.Type = type_;
            this.Data = data_;
            this.Registration = false;
        }

        /// <summary>
        /// Registration constructor for client side. The key is initialized to 0.
        /// </summary>
        /// <param name="name_">The name of the emitter.</param>
        /// <param name="type_">The type of the <see cref="Object"/> member</param>
        /// <param name="data_">The <see cref="Object"/> which contain the data</param>
        /// <param name="registration_">This boolean must be set to tru only if the type is <see cref="PacketType.SYS"/> and the data's command is of value <see cref="SysCommand.C_REGISTER"/></param>
        public Packet(string name_, PacketType type_, Object data_, bool registration_)
        {
            this.Name = name_;
            this.Key = 0;
            this.Type = type_;
            this.Data = data_;
            this.Registration = registration_;
        }
        
        /// <summary>
        /// Main constructor for server side, the key must be initialized with a value.
        /// </summary>
        /// <param name="name_">The name of the emitter. For server side, it will be "root"</param>
        /// <param name="key_">The value of the Locker key</param>
        /// <param name="type_">The type of the <see cref="Object"/> member</param>
        /// <param name="data_">The <see cref="Object"/> which contain the data</param>
        public Packet(string name_, uint key_, PacketType type_, Object data_)
        {
            this.Name = name_;
            this.Key = key_;
            this.Type = type_;
            this.Data = data_;
            this.Registration = false;
        }

        /// <summary>
        /// Getter and Setter for the Packet's emitter name.
        /// </summary>
        public string Name { get => name; set => name = value; }
        /// <summary>
        /// Getter and Setter for the Packet's type.
        /// </summary>
        public PacketType Type { get => type; set => type = value; }
        /// <summary>
        /// Getter and Setter for the Packet's data.
        /// </summary>
        public object Data { get => data; set => data = value; }
        /// <summary>
        /// Getter and Setter for the registration state
        /// </summary>
        public bool Registration { get => registration; set => registration = value; }
        /// <summary>
        /// Getter and Setter for the Packet's reference on a Locker key
        /// </summary>
        public uint Key { get => key; set => key = value; }
    }
}
