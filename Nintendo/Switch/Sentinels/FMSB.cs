using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace bootEditor.Nintendo.Switch.Sentinels
{
    internal class FMSB
    {
        public class Data
        {
            public string headerMagic { get; set; }
            public int headerSize { get; set; }
            public int size { get; set; }
            public int count {  get; set; }
            public int blockCount { get; set; }
            public string[] strings { get; set; }
            public string footerMagic { get; set; }
            public int footerSize { get; set; }
        }
        public static void Read(string file)
        {
            var reader = new BinaryReader(File.OpenRead(file));
            Data data = new Data();
            data.headerMagic = Encoding.UTF8.GetString(reader.ReadBytes(4));
            data.size = reader.ReadInt32() + 0x30;
            data.headerSize = reader.ReadInt32();
            reader.BaseStream.Position = 0x14;
            data.count = reader.ReadInt32();
            data.blockCount = reader.ReadInt32();
            data.strings = new string[data.count];
            reader.BaseStream.Position = data.headerSize + data.count * 8;
            for (int i = 0; i < data.count; i++)
            {
                data.strings[i] = Utils.Utils.ReadString(reader, Encoding.UTF8);
            }
            Utils.Utils.AlignPosition(reader);
            data.footerMagic = Encoding.UTF8.GetString(reader.ReadBytes(4));
            reader.ReadInt32();
            data.footerSize = reader.ReadInt32();
            string json = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(Path.GetFileNameWithoutExtension(file) + ".json", json);
        }
        public static void Write(string json)
        {
            string jsonT = File.ReadAllText(json);
            Data data = JsonConvert.DeserializeObject<Data>(jsonT);
            var writer = new BinaryWriter(File.Create(Path.GetFileNameWithoutExtension(json) + ".fms"));
            writer.Write(Encoding.UTF8.GetBytes(data.headerMagic));
            writer.Write(new byte[4]);
            writer.Write(data.headerSize);
            writer.Write(new byte[8]);
            writer.Write(data.count);
            writer.Write(data.blockCount);
            writer.Write(new byte[4]);
            writer.Write(new byte[data.count * 8]);
            for (int i = 0; i < data.count; i++)
            {
                writer.Write(Encoding.UTF8.GetBytes(data.strings[i]));
                writer.Write(new byte());
            }
            Utils.Utils.AlignPosition(writer, 0x10);
            writer.Write(Encoding.UTF8.GetBytes(data.footerMagic));
            writer.Write(new byte[4]);
            writer.Write(data.footerSize);
            writer.Write(new byte[4]);
            writer.BaseStream.Position = 0x4;
            writer.Write((int)writer.BaseStream.Length - data.headerSize - data.footerSize);
        }
    }
}
