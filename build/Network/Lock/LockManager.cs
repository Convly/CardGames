using System;
using System.Collections.Generic;
using System.Diagnostics;

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

            Stopwatch sw = new Stopwatch();
            sw.Start();

            while (this.Locks[key].State)
            {
                if (sw.ElapsedMilliseconds > 1000)
                {
                    Console.WriteLine("Abort locker " + key);
                    this.Locks[key].State = false;
                }
            }
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
