using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Network.Lock
{
    /// <summary>
    /// Class used to store, delete, lock and unlock <see cref="Locker"/>
    /// </summary>
    public class LockManager
    {
        private Dictionary<uint, Locker> locks = new Dictionary<uint, Locker>();

        /// <summary>
        /// Getter and Setter for the Dictionnary of <see cref="Locker"/>s, which assign an <see cref="uint"/> key to its <see cref="Locker"/>
        /// </summary>
        public Dictionary<uint, Locker> Locks { get => locks; set => locks = value; }

        /// <summary>
        /// Add a new <see cref="Locker"/> in the manager
        /// </summary>
        /// <param name="username">The owner of the <see cref="Locker"/></param>
        /// <returns>This method return the generated key of the <see cref="Locker"/></returns>
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

        /// <summary>
        /// Lock the <see cref="Locker"/> linked to the key given in parameter and wait for a call to <see cref="Unlock(uint)"/>
        /// </summary>
        /// <param name="key">The key of the <see cref="Locker"/></param>
        /// <returns>False if no <see cref="Locker"/> has been found with this key, true otherwise</returns>
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
                if (sw.ElapsedMilliseconds > this.Locks[key].Duration)
                {
                    Console.WriteLine("Abort locker " + key);
                    this.Locks[key].State = false;
                }
            }
            this.Delete(key);
            return true;
        }

        /// <summary>
        /// Unlock the <see cref="Locker"/> linked to the key given in parameter.
        /// </summary>
        /// <param name="key">The key of the <see cref="Locker"/></param>
        /// <returns>False if the manager does not contain a <see cref="Locker"/> with this key, true otherwise</returns>
        public bool Unlock(uint key)
        {
            if (!(this.Locks.ContainsKey(key)))
                return false;
            this.Locks[key].State = false;
            return true;
        }
        
        /// <summary>
        /// Delete the locker associated to the key given in parameter
        /// </summary>
        /// <param name="key">The key of the <see cref="Locker"/></param>
        /// <returns>True if the <see cref="Locker"/> has been successfully deleted, false otherwise.</returns>
        public bool Delete(uint key)
        {
            if (!(this.Locks.ContainsKey(key)))
                return false;
            this.Locks.Remove(key);
            return true;
        }
    }
}
