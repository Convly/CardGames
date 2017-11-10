using System;
using System.Collections.Generic;

namespace Network.Lock
{
    public class LockManager
    {
        private Dictionary<uint, Locker> locks = new Dictionary<uint, Locker>();

        public Dictionary<uint, Locker> Locks { get => locks; set => locks = value; }

        public uint Add(string username)
        {
            Random rand = new Random();
            uint key = (uint)rand.Next(1, 999999999);

            while (this.Locks.ContainsKey(key))
            {
                key = (uint)rand.Next(1, 999999999);
            }

            this.Locks.Add(key, new Locker(key, username));
            return key;
        }

        public bool Lock(uint key)
        {
            if (!(this.Locks.ContainsKey(key)))
            {
                return false;
            }
            while (this.Locks[key].State) ;
            this.Delete(key);
            return true;
        }

        public bool Unlock(uint key)
        {
            if (!(this.Locks.ContainsKey(key)))
                return false;
            this.Locks[key].State = false;
            return true;
        }
        

        public bool Delete(uint key)
        {
            if (!(this.Locks.ContainsKey(key)))
                return false;
            this.Locks.Remove(key);
            return true;
        }
    }
}
