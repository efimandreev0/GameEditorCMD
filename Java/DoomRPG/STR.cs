using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bootEditor.Java.DoomRPG
{
    internal class STR
    {
        public static void Read(string file)
        {
            var reader = new BinaryReader(File.OpenRead(file));
            int count = reader.ReadInt16();
            string[] strings = new string[count];
            for (int i = 0; i < count; i++)
            {
                short len = reader.ReadInt16();
                strings[i] = Encoding.GetEncoding("Windows-1252").GetString(reader.ReadBytes(len));
            }
            File.WriteAllLines(Path.GetFileNameWithoutExtension(file) + ".txt", strings);
        }
        public static void Write(string file)
        {
            var writer = new BinaryWriter(File.Create(Path.GetFileNameWithoutExtension(file) + ".str"));
            string[] strings = (File.ReadAllLines(file));
            writer.Write((short)strings.Length);
            for (int i = 0; i < strings.Length; i++)
            {
                writer.Write((short)Encoding.GetEncoding("Windows-1252").GetBytes(strings[i]).Length);
                writer.Write(Encoding.GetEncoding("Windows-1252").GetBytes(strings[i]));
            }
        }
    }
}
