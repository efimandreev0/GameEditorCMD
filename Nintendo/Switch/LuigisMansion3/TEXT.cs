using bootEditor.Java.DoomRPG;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bootEditor.Nintendo.Switch.LuigisMansion3
{
    internal class TEXT
    {
        public class Data
        {
            public int headerUNK { get; set; }
            public int count { get; set; }
            public string[] strings { get; set; }
            public int[] unk1 { get; set; }
            public int[] unk2 { get; set; }
        }
        public static void Read(string file)
        {
            Data data = new Data();
            var reader = new BinaryReader(File.OpenRead(file));
            data.headerUNK = reader.ReadInt32();
            data.count = reader.ReadInt32();
            reader.ReadInt32();
            data.strings = new string[data.count];
            data.unk1 = new int[data.count];
            data.unk2 = new int[data.count];
            for (int i = 0; i < data.count; i++)
            {
                data.unk1[i] = reader.ReadInt32();
                data.unk2[i] = reader.ReadInt32();
            }
            for (int i = 0; i < data.count; i++)
            {
                data.strings[i] = Utils.Utils.ReadString(reader, Encoding.Unicode);
                
            }
            string json = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(Path.GetFileNameWithoutExtension(file) + ".json", json);
        }
        public static void Write(string json)
        {
            string jsonT = File.ReadAllText(json);
            Data data = JsonConvert.DeserializeObject<Data>(jsonT);
            var writer = new BinaryWriter(File.Create(Path.GetFileNameWithoutExtension(json) + ".bin"));
            writer.Write(data.headerUNK);
            writer.Write(data.count);
            writer.Write(data.headerUNK);
            for (int i = 0; i < data.count; i++)
            {
                writer.Write(data.unk1[i]);
                writer.Write(data.unk2[i]);
            }
            for (int i = 0; i < data.count; i++)
            {
                writer.Write(Encoding.Unicode.GetBytes(data.strings[i]));
                writer.Write(new byte[2]);
            }
            writer.Write(new byte[2]);
        }
    }
}
