using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGameResources.Net
{
    public enum EnvInfos
    {
        S_USER_LIST, // List<string> (size: 0-4)
        S_SCORES, // List<int> (size: 2)
        S_SET_TOUR, // string
        S_SET_REMAINING_TIME, // int
        S_SET_TEAM, // Dictionary<string, int>

    }
    public class Envcall
    {
        private EnvInfos type;
        private Object data;

        public Envcall(EnvInfos type_, Object data_)
        {
            this.Type = type_;
            this.Data = data_;
        }

        public EnvInfos Type { get => type; set => type = value; }
        public object Data { get => data; set => data = value; }
    }
}
