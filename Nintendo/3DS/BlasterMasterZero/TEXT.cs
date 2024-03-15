using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace bootEditor.Nintendo._3DS.BlasterMasterZero
{
    internal class TEXT
    {
        public static void Read(string file)
        {
            var reader = new BinaryReader(File.OpenRead(file));
            reader.BaseStream.Position = 0x10;
            int sizeNo1BLOCK = reader.ReadInt32();
            reader.BaseStream.Position = 0x20;
            int sizeNo2BLOCK = reader.ReadInt32();
            int count = reader.ReadInt32();
            List<int> ints = new List<int>();
            List<string> strings = new List<string>();
            for (int i  = 0; i < count; i++)
            {
                ints.Add(reader.ReadInt32());

            }
            int pos = (int)reader.BaseStream.Position;
            for (int i = 0; i < count; i++)
            {
                reader.BaseStream.Position = ints[i] + pos;
                if (i == count - 1)
                {
                    byte[] bytes = reader.ReadBytes((int)reader.BaseStream.Position - ints[i]);
                    for (int a = 0; a < bytes.Length; a++)
                    {
                        if (bytes[a] == 0x5F)
                        {
                            bytes[a] = 0x20;
                        }
                        else
                        {
                            bytes[a] += 0x21;
                        }
                    }
                    strings.Add(Encoding.UTF8.GetString(bytes).Replace("\r", "<br>").Replace("\u001d\u001e\u001f", "<END>").Replace("\"\u0011!", "<START>").Replace("\u0013", "<DIALOG>").Replace("\u001d\u001e", "<next>").Replace("\u001d", "<lf>").Replace("\u0011", "<11>").Replace("\u0001", "<01>"));
                }
                else
                {
                    byte[] bytes = reader.ReadBytes(ints[i + 1] - ints[i]);
                    for (int a = 0; a < bytes.Length; a++)
                    {
                        if (bytes[a] == 0x5F)
                        {
                            bytes[a] = 0x20;
                        }
                        else
                        {
                            bytes[a] += 0x21;
                        }
                    }
                    strings.Add(Encoding.UTF8.GetString(bytes).Replace("\r", "<br>").Replace("\u001d\u001e\u001f", "<END>").Replace("\"\u0011!", "<START>").Replace("\u0013", "<DIALOG>").Replace("\u001d\u001e", "<next>").Replace("\u001d", "<lf>").Replace("\u0011", "<11>").Replace("\u0001", "<01>"));
                }
            }
            File.WriteAllLines(Path.GetFileNameWithoutExtension(file) + ".txt", strings);
        }
        public static void Write(string text)
        {
            string noExt = Path.GetFileNameWithoutExtension(text);
            string[] strings = File.ReadAllLines(text);
            var reader = new BinaryReader(File.OpenRead(noExt + ".bin"));
            reader.BaseStream.Position = 0x24;
            int count = reader.ReadInt32();
            int[] pointers = new int[count];
            for (int i = 0; i < count; i++)
            {
                reader.ReadInt32();
            }
            var pos = reader.BaseStream.Position;
            if (count != strings.Length)
            {
                Console.WriteLine($"In {noExt + ".txt"} more lines in the file than in the {noExt + ".bin"} file");
            }
            else
            {
                reader.Close();
            }
            var writer = new BinaryWriter(File.OpenWrite(noExt + ".bin"));
            writer.BaseStream.Position = pos;
            for (int i = 0; i < count; i++)
            {
                byte[] bytes = Encoding.UTF8.GetBytes(strings[i].Replace("<br>", "\r").Replace("<END>", "\u001d\u001e\u001f").Replace("<START>", "\"\u0011!").Replace("<DIALOG>", "\u0013").Replace("<next>", "\u001d\u001e").Replace("<lf>", "\u001d").Replace("<11>", "\u0011").Replace("<01>", "\u0001"));
                for (int a = 0; a < bytes.Length; a++)
                {
                    if (bytes[a] == 0x20)
                    {
                        bytes[a] = 0x5F;
                    }
                    else
                    {
                        bytes[a] -= 0x21;
                    }
                }
                pointers[i] = (int)writer.BaseStream.Position - (int)pos;
                writer.Write(bytes);
            }
            writer.BaseStream.Position = 0x10;
            writer.Write((int)writer.BaseStream.Length - 0x14);
            writer.BaseStream.Position = 0x28;
            for (int i = 0; i < count; i++)
            {
                writer.Write(pointers[i]);
            }
        }
    }
}
