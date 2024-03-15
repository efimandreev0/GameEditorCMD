using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace bootEditor.Nintendo.DS.OrcsElves
{
    internal class ARC
    {
        public static void Read(string arc, string toc)
        {
            var arcReader = new BinaryReader(File.OpenRead((arc)));
            var tocReader = new BinaryReader(File.OpenRead((toc)));
            string dir = Path.GetFileNameWithoutExtension(arc) + "\\";
            Directory.CreateDirectory(Path.GetFileNameWithoutExtension(arc));
            List<int> len = new List<int>();
            while(tocReader.BaseStream.Position < tocReader.BaseStream.Length)
            {
                len.Add(tocReader.ReadInt32());
            }
            for (int i = 0; i < len.Count; i++)
            {
                if (i == 0)
                {
                    arcReader.BaseStream.Position = 0;
                    byte[] bytes = arcReader.ReadBytes(len[i]);
                    File.WriteAllBytes(dir + i + ".bin", bytes);
                }
                if (i == len.Count - 1)
                {
                    arcReader.BaseStream.Position = len[i];
                    byte[] bytes = arcReader.ReadBytes((int)arcReader.BaseStream.Length - len[i]);
                    File.WriteAllBytes(dir + i + ".bin", bytes);
                }
                else
                {
                    arcReader.BaseStream.Position = len[i];
                    byte[] bytes = arcReader.ReadBytes(len[i + 1] - len[i]);
                    File.WriteAllBytes(dir + i + ".bin", bytes);
                }
            }
        }
    }
}
