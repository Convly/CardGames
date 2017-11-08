using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGameResources.Net
{
    public enum Err
    {
        UNKNOWN_ERROR,
        BAD_ARGUMENT,
        SERVER_FULL,
        FORBIDDEN_CARD,
        FORBIDDEN_ACTION,
        BROKEN_RULE
    }
    public class Errcall
    {
        private Err type = Err.UNKNOWN_ERROR;
        private string message = "An unexpected error has been thrown";

        public Errcall(Err type_, string message_)
        {
            this.Type = type_;
            this.Message = message_;
        }

        public Errcall(Err type_)
        {
            this.Type = type_;
        }

        public Errcall()
        {
        }

        public Err Type { get => type; set => type = value; }
        public string Message { get => message; set => message = value; }
    }
}
