using bootEditor.Java.DoomRPG;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace bootEditor.Nintendo.Switch.LuigisMansion3
{
    internal class ARC
    {
        public class Data
        {
            public byte[] beforeToc { get; set; }
            public string[] files { get; set; }
            public string DICTmagic { get; set; }
            public int count { get; set; }
            public int DICTunk { get; set; }
            public int DICTunk2 { get; set; }
            public int DICTunk3 { get; set; }
            public byte[] bytes { get; set; }
            public bool isCompressed { get; set; }
            public System.Collections.Generic.List<int> offset { get; set; }
            public System.Collections.Generic.List<int> len { get; set; }
            public System.Collections.Generic.List<int> decSize { get; set; }
            public System.Collections.Generic.List<int> unk2 { get; set; }
        }
        public static void Read(string arc, string toc)
        {
            var arcReader = new BinaryReader(File.OpenRead(arc));
            var tocReader = new BinaryReader(File.OpenRead(toc));
            string dir = Path.GetFileNameWithoutExtension(toc) + "\\";
            Directory.CreateDirectory(Path.GetFileNameWithoutExtension(toc));
            Data data = new Data();
            data.DICTmagic = Encoding.UTF8.GetString(tocReader.ReadBytes(4));
            if (data.DICTmagic != "X$�")
            {
                Console.WriteLine("It's not a Luigi's Mansion 3 archive.");
                throw new Exception("Aborting...");
            }
            data.DICTunk = tocReader.ReadInt16();
            int b = tocReader.ReadByte();
            if (b == 1)
            {
                data.isCompressed = true;
            }
            else
            {
                data.isCompressed = false;
            }
            data.DICTunk2 = tocReader.ReadInt32();
            data.DICTunk2 = tocReader.ReadByte();
            data.count = tocReader.ReadByte();
            tocReader.BaseStream.Position = 0x0;
            data.beforeToc = tocReader.ReadBytes(0x4F0);
            tocReader.BaseStream.Position = 0x4F0;
            data.offset = new List<int>();
            data.len = new List<int>();
            data.decSize = new List<int>();
            data.unk2 = new List<int>();
            data.files = new string[data.count];
            while (tocReader.BaseStream.Position < tocReader.BaseStream.Length - 0x13)
            {
                data.offset.Add(tocReader.ReadInt32());
                data.decSize.Add(tocReader.ReadInt32());
                data.len.Add(tocReader.ReadInt32());
                data.unk2.Add(tocReader.ReadInt32());
            }
            for (int i = 0; i < data.offset.Count; i++)
            {
                arcReader.BaseStream.Position = data.offset[i];
                ushort Magic = arcReader.ReadUInt16();
                arcReader.BaseStream.Position -= 2;
                byte[] bytes = arcReader.ReadBytes(data.len[i]);
                if (Magic == 0x9C78 || Magic == 0xDA78)
                {
                    byte[] dec = Utils.Utils.DecompressZlib(bytes);
                    File.WriteAllBytes(dir + i + "_dec.bin", dec);
                    data.files[i] = dir + i + "_dec.bin";
                }
                else
                {
                    File.WriteAllBytes(dir + i + "_cmp.bin", bytes);
                    data.files[i] = dir + i + "_cmp.bin";
                }
            }
            string json = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(dir + "parameters.json", json);
        }
        public static void Write(string dir)
        {
            string jsonT = File.ReadAllText(dir + "\\parameters.json");
            Data data = JsonConvert.DeserializeObject<Data>(jsonT);
            var arcWriter = new BinaryWriter(File.Create(dir + ".data"));
            var tocWriter = new BinaryWriter(File.Create(dir + ".dict"));
            for (int i = 0; i < data.count; i++)
            {
                if (data.files[i].Contains("_dec"))
                {
                    byte[] bytes = File.ReadAllBytes(data.files[i]);
                    byte[] comp = Utils.Utils.CompressBytes(bytes);
                    data.decSize[i] = bytes.Length;
                    data.len[i] = comp.Length;
                    data.offset[i] = (int)arcWriter.BaseStream.Position;
                    arcWriter.Write(comp);
                }
                else
                {
                    byte[] bytes = File.ReadAllBytes(data.files[i]);
                    data.offset[i] = (int)arcWriter.BaseStream.Position;
                    arcWriter.Write(bytes);
                }
            }
            tocWriter.Write(data.beforeToc);
            for (int i = 0; i < data.count; i++)
            {
                tocWriter.Write(data.offset[i]);
                tocWriter.Write(data.decSize[i]);
                tocWriter.Write(data.len[i]);
                tocWriter.Write(data.unk2[i]);
            }
        }
    }
}
