using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bootEditor.IOS.DoomIIRPG
{
    internal class BIN
    {
        public static void Read(string dir)
        {
            string[] files = Directory.GetFiles(dir, "*strings*.bin");
            string direct = "text_" + Path.GetFileNameWithoutExtension(dir) + "\\";
            Directory.CreateDirectory("text_" + Path.GetFileNameWithoutExtension(dir));
            for (int i = 0; i < files.Length; i++)
            {
                var reader = new BinaryReader(File.OpenRead(files[i]));
                List<string> strings = new List<string>();
                if (i != 0)
                {
                    reader.ReadByte();
                    while (reader.BaseStream.Position < reader.BaseStream.Length)
                    {
                        strings.Add(Utils.Utils.ReadString(reader, Encoding.GetEncoding("Windows-1252")));

                    }
                }
                else
                {
                    while (reader.BaseStream.Position < reader.BaseStream.Length)
                    {
                        strings.Add(Utils.Utils.ReadString(reader, Encoding.GetEncoding("Windows-1252")));

                    }
                }
                reader.Close();
                File.WriteAllLines(direct + Path.GetFileNameWithoutExtension(files[i]) + ".txt", strings);
            }
        }
        public static void Write(string dir)
        {
            string[] files = Directory.GetFiles(dir, "*strings*.txt");
            string direct = Path.GetFileNameWithoutExtension(dir).Substring(5) + "\\";
            Directory.CreateDirectory(Path.GetFileNameWithoutExtension(dir).Substring(5));
            for (int i = 0; i < files.Length; i++)
            {
                var writer = new BinaryWriter(File.Create(direct + Path.GetFileNameWithoutExtension(files[i]) + ".bin"));
                string[] strings = File.ReadAllLines(files[i]);
                if (files[i].Contains("00"))
                {
                    for (int a = 0; a < strings.Length; a++)
                    {
                        writer.Write(Encoding.GetEncoding("Windows-1252").GetBytes(strings[a]));
                        writer.Write(new byte());
                    }
                }
                else
                {
                    writer.Write(new byte());
                    for (int a = 0; a < strings.Length; a++)
                    {
                        writer.Write(Encoding.GetEncoding("Windows-1252").GetBytes(strings[a]));
                        writer.Write(new byte());
                    }
                }
                writer.Close();
            }
        }
    }
}