namespace Network.Lock
{
    /// <summary>
    /// Class designed to lock and unlock asynchronous event. Usually managed by a <see cref="LockManager"/>.
    /// </summary>
    public class Locker
    {
        private uint key;
        private string username;
        private bool state = true;
        private int duration = -1;

        /// <summary>
        /// Getter and Setter for the <see cref="Locker"/> key
        /// </summary>
        public uint Key { get => key; set => key = value; }
        /// <summary>
        /// Getter and Setter for the Username of the owner of the <see cref="Locker"/>
        /// </summary>
        public string Username { get => username; set => username = value; }
        /// <summary>
        /// Getter and Setter for the current State of the <see cref="Locker"/>
        /// </summary>
        public bool State { get => state; set => state = value; }
        /// <summary>
        /// Getter and Setter for the timeout duration of the <see cref="Locker"/>
        /// </summary>
        public int Duration { get => duration; set => duration = value; }

        /// <summary>
        /// Main constructor of the <see cref="Locker"/>
        /// </summary>
        /// <param name="key_">The unique key code of the <see cref="Locker"/></param>
        /// <param name="username_">The username of the <see cref="Locker"/>'s owner</param>
        public Locker(uint key_, string username_)
        {
            this.Key = key_;
            this.Username = username_;
            this.Duration = 1000;
        }

        /// <summary>
        /// Second constructor of the <see cref="Locker"/>, allowing the user to customize the timeout duration.
        /// </summary>
        /// <param name="key_">The unique key code of the <see cref="Locker"/></param>
        /// <param name="username_">The username of the <see cref="Locker"/>'s owner</param>
        /// <param name="duration_">The duration of the lock period timeout.</param>
        public Locker(uint key_, string username_, int duration_)
        {
            this.Key = key_;
            this.Username = username_;
            this.Duration = duration_;
        }
    }
}
