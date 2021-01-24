using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTPApp_Server.Misc
{
    class Tracking
    {

        private static int count;

        public void IncreaseCount()
        {
            count++;
        }

        public void DecreaseCount()
        {
            count--;
        }

        public void ResetCount()
        {
            count = 0;
        }

        public int GetCount()
        {
            return count;
        }

    }
}
