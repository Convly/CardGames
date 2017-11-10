namespace Network.Lock
{
    public class Locker
    {
        private uint key;
        private string username;
        private bool state = true;
        private int duration = -1;

        public uint Key { get => key; set => key = value; }
        public string Username { get => username; set => username = value; }
        public bool State { get => state; set => state = value; }
        public int Duration { get => duration; set => duration = value; }

        public Locker(uint key_, string username_)
        {
            this.Key = key_;
            this.Username = username_;
        }

        public Locker(uint key_, string username_, int duration_)
        {
            this.Key = key_;
            this.Username = username_;
            this.Duration = duration_;
        }
    }
}
