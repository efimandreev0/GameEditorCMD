using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bootEditor.Nintendo.DS.BMG
{
    internal class BMG
    {
        public static void Read(string file)
        {
            var reader = new BinaryReader(File.OpenRead(file));
            string magicBMG = Encoding.UTF8.GetString(reader.ReadBytes(8));
            int fullSize = reader.ReadInt32();
            Console.WriteLine($"Magic: {magicBMG}, fullsize: {fullSize}");
            reader.BaseStream.Position = 0x20;
            string magicINF1 = Encoding.UTF8.GetString(reader.ReadBytes(4));
            int INF1Size = reader.ReadInt32() + 0x20;
            int stringCount = reader.ReadInt16();
            int[] pos = new int[stringCount];
            string[] strings = new string[stringCount];
            Console.WriteLine($"Magic: {magicINF1}, fullsize: {INF1Size}, string count: {stringCount}");
            reader.BaseStream.Position += 6;
            for (int i = 0; i < stringCount; i++)
            {
                pos[i] = reader.ReadInt32();
            }
            reader.BaseStream.Position = INF1Size;
            string magicDAT1 = Encoding.UTF8.GetString(reader.ReadBytes(4));
            int DAT1Size = reader.ReadInt32();
            var position = reader.BaseStream.Position;
            for (int i = 0; i < stringCount; i++)
            {
                reader.BaseStream.Position = position;
                reader.BaseStream.Position += pos[i];
                strings[i] = Utils.ReadString(reader, Encoding.Unicode).Replace("\n", "<lf>").Replace("\r", "<br>");
            }
            File.WriteAllLines("File.txt", strings);
        }
    }
}
