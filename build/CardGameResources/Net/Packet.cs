using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGameResources.Net
{
    public enum PacketType
    {
        ERR = -1,
        SYS = 0,
        GAME,
        ENV
    }
    public class Packet
    {
        private string name;
        private PacketType type;
        private Object data;
        private bool registration;

        public Packet()
        {

        }

        public Packet(string name_, PacketType type_, Object data_)
        {
            this.Name = name_;
            this.Type = type_;
            this.Data = data_;
            this.Registration = false;
        }
        public Packet(string name_, PacketType type_, Object data_, bool registration_)
        {
            this.Name = name_;
            this.Type = type_;
            this.Data = data_;
            this.Registration = registration_;
        }

        public string Name { get => name; set => name = value; }
        public PacketType Type { get => type; set => type = value; }
        public object Data { get => data; set => data = value; }
        public bool Registration { get => registration; set => registration = value; }
    }
}
