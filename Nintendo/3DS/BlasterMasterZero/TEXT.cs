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
    }
}
