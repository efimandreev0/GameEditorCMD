using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bootEditor.Nintendo.Switch.LuigisMansion3
{
    internal class TEXT
    {
        public static void Read(string file)
        {
            var reader = new BinaryReader(File.OpenRead(file));
            reader.ReadInt32();
            int count = reader.ReadInt32();
            reader.ReadInt32();
            int[] pointers = new int[count];
            int[] unk = new int[count];
            string[] strings = new string[count];
            for (int i = 0; i < count; i++)
            {
                unk[i] = reader.ReadInt32();
                pointers[i] = reader.ReadInt32();
            }
        }
    }
}
