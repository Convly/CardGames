using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGameResources.Net
{
    public enum SysCommand
    {
        C_REGISTER, // void
        C_QUIT, // void
        S_DISCONNECTED, // string
        S_CONNECTED, // void
        S_START_GAME, // void
        S_END_GAME, // void
        S_POKE // void
    }
    public class Syscall
    {
        private SysCommand command;
        private Object data;

        public Syscall(SysCommand command_, Object data_)
        {
            this.Command = command_;
            this.Data = data_;
        }

        public SysCommand Command { get => command; set => command = value; }
        public object Data { get => data; set => data = value; }
    }
}
