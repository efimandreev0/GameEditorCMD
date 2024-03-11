using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace bootEditor.PC.JackDaxterLegacy
{
    internal class TXTB
    {
        public static void Read(string file)
        {
            var reader = new BinaryReader(File.OpenRead(file));
            reader.ReadInt32();
            int block1Size = reader.ReadInt32();
            reader.BaseStream.Position = block1Size;
            reader.ReadInt32();
            int block2Size = reader.ReadInt32();
            for (int i = 0; i <  block2Size; i++)
            {
                reader.ReadInt32();
                reader.ReadInt32();
            }
            reader.ReadInt64();
            reader.ReadInt32();
            reader.ReadInt32();
            List<string> strings = new List<string>();
            while (reader.BaseStream.Position < reader.BaseStream.Length)
            {
                reader.ReadInt32();
                int size = reader.ReadInt32();
                strings.Add(Encoding.UTF8.GetString(reader.ReadBytes(size)).Replace("\n", "<lf>").Replace("\r", "<br>").Replace("\u0011", "<11>").Replace("\u0014", "<14>").Replace("\u0012", "'").Replace("\u001d", "Ç").Replace("\u0015", "<15>").Replace("\u0003", "<color>"));
                bootEditor.Utils.Utils.AlignPosition(reader, 0x10);
            }
            File.WriteAllLines(Path.GetFileNameWithoutExtension(file) + ".dec.txt", strings);
        }
        public static void Write(string text)
        {
            string[] strings = File.ReadAllLines(text);
            //start skipping all blocks without text
            var reader = new BinaryReader(File.OpenRead(text + ".undec"));
            reader.ReadInt32();
            int block1Size = reader.ReadInt32();
            reader.BaseStream.Position = block1Size;
            reader.ReadInt32();
            int block2Size = reader.ReadInt32();
            for (int i = 0; i < block2Size; i++)
            {
                reader.ReadInt32();
                reader.ReadInt32();
            }
            reader.ReadInt64();
            reader.ReadInt32();
            reader.ReadInt32();
            var pos = reader.BaseStream.Position;
            reader.Close(); //we are skipped all blocks
            //start writing new text block 
            var writer = new BinaryWriter(File.OpenWrite(text + ".undec"));
            writer.BaseStream.Position = pos;
            for (int i = 0; i < strings.Length; i++)
            {
                strings[i] = strings[i].Replace("<lf>", "\n").Replace("<br>", "\r").Replace("<11>", "\u0011").Replace("<14>", "\u0014").Replace("'", "\u0012").Replace("Ç", "\u001d").Replace("<15>", "\u0015").Replace("<color>", "\u0003");
                writer.Write((int)-1);
                writer.Write(Encoding.UTF8.GetBytes(strings[i]).Length);
                writer.Write(Encoding.UTF8.GetBytes(strings[i]));
                bootEditor.Utils.Utils.AlignPosition(writer, 16);
            }
        }
    }
}
