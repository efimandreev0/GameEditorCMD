using bootEditor.Java.DoomRPG;
using Newtonsoft.Json;
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
        public class Data
        {
            public string magic { get; set; }
            public int size { get; set; }
            public int unk1 { get; set; }
            public int unk2 { get; set; }
            public string magicINF { get; set; }
            public int INFsize { get; set; }
            public int count { get; set; }
            public int INFunk { get; set; }
            public string magicDAT { get; set; }
            public int DATsize { get; set; }
            public int[] pos { get; set; }
            public string[] strings { get; set; }

        }
        public static void Read(string file)
        {
            Data data = new Data();
            var reader = new BinaryReader(File.OpenRead(file));
            data.magic = Encoding.UTF8.GetString(reader.ReadBytes(8));
            if (data.magic != "MESGbmg1")
            {
                throw new Exception("This is not .bmg");
            }
            data.size = reader.ReadInt32();
            data.unk1 = reader.ReadInt32();
            data.unk2 = reader.ReadInt32();
            Utils.Utils.AlignPosition(reader);
            Console.WriteLine($"Magic: {data.magic}, fullsize: {data.size}");
            data.magicINF = Encoding.UTF8.GetString(reader.ReadBytes(4));
            data.INFsize = reader.ReadInt32();
            data.count = reader.ReadInt16();
            data.INFunk = reader.ReadInt32();
            Utils.Utils.AlignPosition(reader);

            data.pos = new int[data.count];
            data.strings = new string[data.count];
            Console.WriteLine($"Magic: {data.magicINF}, fullsize: {data.INFsize}, string count: {data.count}");
            for (int i = 0; i < data.count; i++)
            {
                data.pos[i] = reader.ReadInt32();
            }
            reader.BaseStream.Position = data.INFsize + 0x20;
            data.magicDAT = Encoding.UTF8.GetString(reader.ReadBytes(4));
            data.DATsize = reader.ReadInt32();
            var position = reader.BaseStream.Position;
            for (int i = 0; i < data.count; i++)
            {
                reader.BaseStream.Position = position;
                reader.BaseStream.Position += data.pos[i];
                data.strings[i] = Utils.Utils.ReadString(reader, Encoding.Unicode);
                if (data.strings[i] == "")
                {
                    data.strings[i] = "<EMPTY>";
                }
            }
            string json = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(Path.GetFileNameWithoutExtension(file) + ".json", json);

        }
        public static void Write(string json)
        {
            string jsonT = File.ReadAllText(json);
            Data data = JsonConvert.DeserializeObject<Data>(jsonT);
            var writer = new BinaryWriter(File.Create(Path.GetFileNameWithoutExtension(json) + ".bmg"));
            writer.Write(Encoding.UTF8.GetBytes(data.magic));
            writer.Write(new byte[4]);
            writer.Write(data.unk1);
            writer.Write(data.unk2);
            Utils.Utils.AlignPosition(writer, 0x10);
            writer.Write(Encoding.UTF8.GetBytes(data.magicINF));
            writer.Write(data.INFsize);
            writer.Write((short)data.count);
            writer.Write(data.INFunk);
            Utils.Utils.AlignPosition(writer, 0x10);
            writer.Write(new byte[data.count * 4]);
            Utils.Utils.AlignPosition(writer, 0x10);
            writer.Write(Encoding.UTF8.GetBytes(data.magicDAT));
            writer.Write(new byte[4]);
            var pos = writer.BaseStream.Position;
            for (int i = 0; i < data.count; i++)
            {
                if (writer.BaseStream.Position - pos == 0)
                {
                    writer.Write(new byte[2]);
                    data.pos[i] = (int)writer.BaseStream.Position - (int)pos;
                    if (data.strings[i] == "<EMPTY>")
                    {
                        writer.Write(new byte[2]);
                    }
                    else
                    {
                        writer.Write(Encoding.Unicode.GetBytes(data.strings[i]));
                        writer.Write(new byte[2]);
                    }
                }
                else
                {
                    data.pos[i] = (int)writer.BaseStream.Position - (int)pos;
                    if (data.strings[i] == "<EMPTY>")
                    {
                        writer.Write(new byte[2]);
                    }
                    else
                    {
                        writer.Write(Encoding.Unicode.GetBytes(data.strings[i]));
                        writer.Write(new byte[2]);
                    }
                }
            }
            Utils.Utils.AlignPosition(writer, 0x10);
            writer.BaseStream.Position = data.magic.Length;
            writer.Write(writer.BaseStream.Length);
            writer.BaseStream.Position = data.INFsize + 0x24;
            writer.Write((int)writer.BaseStream.Length - data.INFsize + 0x20);
            writer.BaseStream.Position = 0x30;
            for (int i = 0; i < data.count; i++)
            {
                writer.Write(data.pos[i]);
            }
        }
    }
}
