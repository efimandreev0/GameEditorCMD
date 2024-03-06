using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace bootEditor.Nintendo._3DS.SMDH
{
    class SMDH
    {
        public static void Read(BinaryReader reader)
        {
            string magic = Encoding.UTF8.GetString(reader.ReadBytes(4));
            reader.ReadInt32();
            string[] name1 = new string[12];
            string[] name2 = new string[12];
            string[] developer = new string[12];
            for (int i = 0; i < 12; i++)
            {
                name1[i] = bootEditor.Utils.Utils.ReadString(reader, Encoding.Unicode);
                reader.BaseStream.Position += 128 - (Encoding.Unicode.GetBytes(name1[i])).Length - i;
				name2[i] = bootEditor.Utils.Utils.ReadString(reader, Encoding.Unicode);
                reader.BaseStream.Position += 256 - (Encoding.Unicode.GetBytes(name2[i])).Length - i;
                developer[i] = bootEditor.Utils.Utils.ReadString(reader, Encoding.Unicode);
                reader.BaseStream.Position += 128 - (Encoding.Unicode.GetBytes(developer[i])).Length - i;
            }
        }
    }
}
