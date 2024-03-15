using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;


namespace bootEditor.Java.DoomRPG
{
    internal class DB
    {
        public class Data
        {
            public string[] strings { get; set; }
            public int[] unk1 { get; set; }
            public int[] unk2 { get; set; }
            public int[] unk3 { get; set; }
        }
        public static void Read(string DB)
        {
            var reader = new BinaryReader(File.OpenRead(DB));
            List<int> unk1 = new List<int>();
            List<int> unk2 = new List<int>();
            List<int> unk3 = new List<int>();
            List<string> strings = new List<string>();
            int a = 0;
            while (reader.BaseStream.Position < reader.BaseStream.Length)
            {
                unk1.Add(reader.ReadInt16());
                unk2.Add(reader.ReadInt16());
                unk3.Add(reader.ReadInt16());
                if (a == 0)
                {
                    reader.ReadInt32();
                }
                else
                {
                    reader.ReadInt16();
                }
                strings.Add(Utils.Utils.ReadString(reader, Encoding.GetEncoding("Windows-1252")));
                reader.BaseStream.Position += 15 - strings[a].Length;
                a++;
            }
            var jsonOBJ = new
            {
                strings,
                unk1,
                unk2,
                unk3
            };
            string json = JsonConvert.SerializeObject(jsonOBJ, Formatting.Indented);
            File.WriteAllText(Path.GetFileNameWithoutExtension(DB) + ".json", json);
        }
        public static void Write(string json)
        {
            string jsonT = File.ReadAllText(json);

            Data data = JsonConvert.DeserializeObject<Data>(jsonT);
            var writer = new BinaryWriter(File.Create(Path.GetFileNameWithoutExtension(json) + ".db"));
            for (int i = 0; i < data.strings.Length; i++)
            {
                writer.Write((short)data.unk1[i]);
                writer.Write((short)data.unk2[i]);
                writer.Write((short)data.unk3[i]);
                if (i == 0)
                {
                    writer.Write(new byte[4]);
                }
                else
                {
                    writer.Write(new byte[2]);
                }
                if (Encoding.GetEncoding("Windows-1252").GetBytes(data.strings[i]).Length >= 16)
                {
                    writer.Write(new byte[16]);
                    Console.WriteLine($"String {i + 1} over 16 bytes! It's not been writed.");
                }
                else
                {
                    writer.Write(Encoding.GetEncoding("Windows-1252").GetBytes(data.strings[i]));
                    writer.Write(new byte[16 - Encoding.GetEncoding("Windows-1252").GetBytes(data.strings[i]).Length]);
                    Console.WriteLine($"String {i + 1} succesfully writed!");
                }
            }
            writer.Close();
        }
    }
}
