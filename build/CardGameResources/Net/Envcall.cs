using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGameResources.Net
{
    public enum EnvInfos
    {
        USER_LIST
    }
    public class Envcall
    {
        private EnvInfos type;
        Object data;

        public Envcall(EnvInfos type_, Object data_)
        {
            this.Type = type_;
            this.Data = data_;
        }

        public EnvInfos Type { get => type; set => type = value; }
        public object Data { get => data; set => data = value; }
    }
}
