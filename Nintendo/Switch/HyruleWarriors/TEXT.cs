using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bootEditor.Nintendo.Switch.HyruleWarriors
{
    internal class TEXT
    {
        public static void Read(string file)
        {
            var reader = new BinaryReader(File.OpenRead(file));
            int count = reader.ReadInt32();
            int textBlockSize = reader.ReadInt32() - 0x1;
            List<int> pointers = new List<int>();
            List<string> strings = new List<string>();
            for (int i = 0; i < count; i++)
            {
                if (i == 0 || i == 1)
                {
                    reader.ReadInt32();
                }
                else
                {
                    pointers.Add((int)reader.BaseStream.Position + reader.ReadInt32());
                    var pos = reader.BaseStream.Position;
                    reader.BaseStream.Position = pointers[i - 2];
                    strings.Add(bootEditor.Utils.Utils.ReadString(reader, Encoding.UTF8).Replace("\n", "<lf>").Replace("\r", "<br>"));
                    if (strings[i - 2].Length == 0)
                    {
                        strings[i - 2] = "<EMPTY>";
                    }
                    reader.BaseStream.Position = pos;
                }
            }
            File.WriteAllLines(Path.GetFileNameWithoutExtension(file) + ".txt", strings);
        }
        public static void Write(string text)
        {
            var writer = new BinaryWriter(File.Create(Path.GetFileNameWithoutExtension(text) + ".cnk"));
            string[] strings = File.ReadAllLines(text);
            int[] pointers = new int[strings.Length];
            writer.Write(strings.Length + 2);
            writer.Write(new byte[(strings.Length + 2) * 4]);
            writer.Write(new byte[4]);
            for (int i = 0; i < strings.Length; i++) 
            {
                if (strings[i] == "<EMPTY>")
                {
                    pointers[i] = (int)writer.BaseStream.Position;
                    writer.Write(new byte());
                }
                else
                {
                    pointers[i] = (int)writer.BaseStream.Position;
                    writer.Write(Encoding.UTF8.GetBytes(strings[i].Replace("<lf>", "\n").Replace("<br>", "\r")));
                    writer.Write(new byte());
                }
            }
            int strBlockSize = (int)writer.BaseStream.Length - (strings.Length + 2) * 4;
            writer.BaseStream.Position = 0x4;
            writer.Write(strBlockSize);
            writer.BaseStream.Position += 0x8;
            for (int i = 0; i < strings.Length; i++)
            {
                writer.Write((int)(pointers[i] - (int)writer.BaseStream.Position));
            }
        }
    }
}
