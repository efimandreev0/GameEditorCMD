using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bootEditor.Java.DoomRPG
{
    internal class BSP
    {
        public class Data
        {
            public string fileName { get; set; }
            public byte[] beforeText { get; set; }
            public int count { get; set; }
            public string[] strings { get; set; }
            public byte[] afterText { get; set; }
        }
        public static void Read(string file)
        {
            string nameWithoutExt = Path.GetFileNameWithoutExtension(file);
            var reader = new BinaryReader(File.OpenRead(file));
            Data data = new Data();
            data.fileName = Utils.Utils.ReadString(reader, Encoding.UTF8);
            Utils.Utils.AlignPosition(reader);
            int pos = 0;
            if (data.fileName.Contains("Entrance"))
            {
                pos = 0x2D1E;
            }
            if (data.fileName.Contains("Junction"))
            {
                pos = 0x1872;
            }
            if (file.Contains("Junction_destroyed"))
            {
                pos = 0x2689;
            }
            if (data.fileName.Contains("Sector 1"))
            {
                pos = 0x2719;
            }
            if (data.fileName.Contains("Sector 2"))
            {
                pos = 0x2F72;
            }
            if (data.fileName.Contains("Sector 3"))
            {
                pos = 0x2DED;
            }
            if (data.fileName.Contains("Sector 4"))
            {
                pos = 0x3452;
            }
            if (data.fileName.Contains("Sector 5"))
            {
                pos = 0x3C91;
            }
            if (data.fileName.Contains("Sector 6"))
            {
                pos = 0x3C15;
            }
            if (data.fileName.Contains("Sector 7"))
            {
                pos = 0x3A75;
            }
            if (data.fileName.Contains("Reactor"))
            {
                pos = 0x2A1C;
            }
            reader.BaseStream.Position = 0x10;
            data.beforeText = reader.ReadBytes(pos - 0x10);
            data.count = reader.ReadUInt16();
            data.strings = new string[data.count];
            for (int i = 0; i < data.count; i++)
            {
                int len = reader.ReadInt16();
                data.strings[i] = Encoding.GetEncoding("Windows-1252").GetString(reader.ReadBytes(len)).Replace("\n", "<lf>");
            }
            data.afterText = reader.ReadBytes((int)reader.BaseStream.Length - (int)reader.BaseStream.Position);
            string json = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(nameWithoutExt + ".json", json);
        }
        public static void Write(string json)
        {
            string jsonT = File.ReadAllText(json);

            Data data = JsonConvert.DeserializeObject<Data>(jsonT);
            //writing
            var writer = new BinaryWriter(File.Create(Path.GetFileNameWithoutExtension(json) + ".bsp"));
            writer.Write(Encoding.GetEncoding("Windows-1252").GetBytes(data.fileName));
            Utils.Utils.AlignPosition(writer, 0x10);
            writer.Write(data.beforeText);
            writer.Write((short)data.strings.Length);
            for (int i = 0; i < data.count; i++)
            {
                writer.Write((short)Encoding.GetEncoding("Windows-1252").GetBytes(data.strings[i]).Length);
                writer.Write(Encoding.GetEncoding("Windows-1252").GetBytes(data.strings[i]));
            }
            writer.Write(data.afterText);
        }
    }
}
