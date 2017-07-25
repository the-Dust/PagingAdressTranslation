using System.Collections.Generic;

namespace PagingAdressTranslation
{
    class Translator
    {
        //page address - key, value in page address - value
        Dictionary<ulong, ulong> map;
        //address of the root table
        ulong rootAddress;

        public Translator(Dictionary<ulong, ulong> map, ulong rootAddress)
        {
            this.map = map;
            this.rootAddress = rootAddress;
        }

        public string GetPhisycalAddress(ulong logicalAddress)
        {
            ulong mask12 = 4095; //"0000 1111 1111 1111"
            ulong mask9 = 511; //"0000 0001 1111 1111"

            ulong offset = logicalAddress & mask12;
            logicalAddress >>= 12; //see the structure of long mode paging in x86

            ulong table = logicalAddress & mask9;
            logicalAddress >>= 9;

            ulong directory = logicalAddress & mask9;
            logicalAddress >>= 9;

            ulong directoryPtr = logicalAddress & mask9;
            logicalAddress >>= 9;

            ulong PML4 = logicalAddress & mask9;

            ulong[] numbers = { PML4, directoryPtr, directory, table};

            ulong nextLevel = rootAddress;

            for (int i = 0; i < 4; i++)
            {
                bool fail = false;
                nextLevel = GetNextLevelAddress(nextLevel, numbers[i], ref fail);
                if(fail)
                    return "fault";
            }

            ulong address = nextLevel + offset;

            return address.ToString();
        }

        private ulong GetNextLevelAddress(ulong level, ulong number, ref bool fail)
        {
            //we take the address of table (level) and adding number of note * 8 (size of each note in bytes)
            ulong key = level + 8 * number;
            //if the map doesn't contain a key, we take value = 0 (according to condition of the task)
            ulong nextLevel = map.ContainsKey(key) ? map[key] : 0;
            //checking a "p"-bit (see the structure of the note in long mode table in x86)
            //if p==0 the note isn't in use
            if ((nextLevel & 1) == 0)
                fail = true;
            //making align to 12 younger bits
            else
            {
                nextLevel >>= 12;
                nextLevel <<= 12;
            }
            return nextLevel;
        }
    }
}
