using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGameResources.Net
{
    public enum SysCommand
    {
        REGISTER = 0,
        QUIT
    }
    public class Syscall
    {
        private SysCommand command;
        private List<string> args;

        public Syscall(SysCommand command_, List<string> args_)
        {
            this.Command = command_;
            this.Args = args_;
        }

        public SysCommand Command { get => command; set => command = value; }
        public List<string> Args { get => args; set => args = value; }
    }
}
