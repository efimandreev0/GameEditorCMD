using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace bootEditor.Nintendo._3DS.BlasterMasterZero
{
    internal class IRARC
    {
        public static void Read(string arc, string tbl)
        {
            var tocReader = new BinaryReader(File.OpenRead(tbl));
            var arcReader = new BinaryReader(File.OpenRead(arc));
            int count = tocReader.ReadInt32();
            string dir = Path.GetFileNameWithoutExtension(tbl) + "\\";
            Directory.CreateDirectory(Path.GetFileNameWithoutExtension(tbl));
            if (File.Exists(dir + "unk.txt"))
            {
                File.Delete(dir + "unk.txt");
            }
            for (int i = 0; i < count; i++)
            {
                int ID = tocReader.ReadInt32();
                int offset = tocReader.ReadInt32();
                int len = tocReader.ReadInt32();
                int unk = tocReader.ReadInt32();
                arcReader.BaseStream.Position = offset;
                byte[] bytes = arcReader.ReadBytes(len);
                File.WriteAllBytes(dir + i + "_" + ID + ".bin", bytes);
                File.AppendAllText(dir + "unk.txt", unk.ToString() + "\n");
            }
        }
    }
}
