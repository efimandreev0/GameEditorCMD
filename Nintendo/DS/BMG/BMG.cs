using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Net.NetworkInformation;
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
            if (magicBMG != "MESGbmg1")
            {
                throw new Exception("This is not .bmg");
            }
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
                strings[i] = Utils.Utils.ReadString(reader, Encoding.Unicode).Replace("\n", "<lf>").Replace("\r", "<br>");
                if (strings[i] == "")
                {
                    strings[i] = "<EMPTY>";
                }
            }
            File.WriteAllLines(Path.GetFileNameWithoutExtension(file) + ".txt", strings);
        }
        public static void Write(string txt)
        {
            var withoutExt = Path.GetFileNameWithoutExtension(txt);
            string[] strings = File.ReadAllLines(txt);
            int count = strings.Length;
            int[] pointers = new int[count];
            var reader = new BinaryReader(File.OpenRead(withoutExt + ".bmg"));
            string magicBMG = Encoding.UTF8.GetString(reader.ReadBytes(8));
            if (magicBMG != "MESGbmg1")
            {
                throw new Exception("This is not .bmg");
            }
            int position = 0;
            byte[] bytesToSearch = { 0x53, 0x41, 0x52, 0x43 };
            // Открываем бинарный файл для чтения в байтовом режиме
            byte[] fileBytes = new byte[reader.BaseStream.Length];
            reader.Read(fileBytes, 0, fileBytes.Length);
            // Выполняем поиск указанных байтов
            position = Utils.Utils.FindBytes(fileBytes, bytesToSearch);

            if (position >= 0)
            {
                Console.WriteLine("Block  DAT1 was finded on position ", position);
            }
            else
            {
                Console.WriteLine("Байты не найдены");
            }
            position += 8;
            reader.Close();
            var writer = new BinaryWriter(File.OpenWrite(withoutExt + ".bmg"));
            writer.BaseStream.Position = position;
            for (int i = 0; i < count; i++)
            {
                strings[i] = strings[i].Replace("<lf>", "\n").Replace("<br>", "\r");
                if (strings[i] == "<EMPTY>")
                {
                    pointers[i] = (int)writer.BaseStream.Position - position;
                    writer.Write(new byte());
                }
                else
                {
                    pointers[i] = (int)writer.BaseStream.Position - position;
                    writer.Write(Encoding.Unicode.GetBytes(strings[i]));
                    writer.Write(new byte());
                }
            }
            writer.BaseStream.Position = 0x28;
            writer.Write((int)count);
            writer.BaseStream.Position += 6;
            for (int i = 0; i < count; i++)
            {
                writer.Write((int)pointers[i]);
            }
        }
    }
}
