using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

namespace bootEditor.Nintendo._3DS.BlasterMasterZero
{
    internal class IRARC
    {
        public static void Read(string arc, string tbl)
        {
            var tocReader = new BinaryReader(File.OpenRead(tbl));
            var arcReader = new BinaryReader(File.OpenRead(arc));
            int count = tocReader.ReadInt32();
            string dir = Path.GetFileNameWithoutExtension(tbl) + "\\";
            Directory.CreateDirectory(Path.GetFileNameWithoutExtension(tbl));
            if (File.Exists(dir + "unk.txt"))
            {
                File.Delete(dir + "unk.txt");
            }
            for (int i = 0; i < count; i++)
            {
                int ID = tocReader.ReadInt32();
                int offset = tocReader.ReadInt32();
                int len = tocReader.ReadInt32();
                int unk = tocReader.ReadInt32();
                arcReader.BaseStream.Position = offset;
                byte[] bytes = arcReader.ReadBytes(len);
                File.WriteAllBytes(dir + i + "_" + ID + ".bin", bytes);
                File.AppendAllText(dir + "unk.txt", unk.ToString() + "\n");
            }
        }
        public static void Write(string dir)
        {
            if (!File.Exists(dir + "\\unk.txt"))
            {
                Console.WriteLine($"unk.txt not contains in {dir}");
                return;
            }
            var arcwriter = new BinaryWriter(File.Create(dir + ".irarc"));
            var tocwriter = new BinaryWriter(File.Create(dir + ".irlst"));
            string[] files = Directory.GetFiles(dir, "*.bin", SearchOption.TopDirectoryOnly);
            int[] ID = new int[files.Length];
            int[] pointers = new int[files.Length];
            int[] len = new int[files.Length];
            string[] unkTable = File.ReadAllLines(dir + "\\unk.txt");
            tocwriter.Write((int)files.Length);
            for (int i = 0; i < files.Length; i++)
            {
                int index = Path.GetFileNameWithoutExtension(files[i]).IndexOf('_');
                ID[i] = int.Parse(Path.GetFileNameWithoutExtension(files[i]).Substring(index + 1));
                byte[] bytes = File.ReadAllBytes(files[i]);
                pointers[i] = (int)arcwriter.BaseStream.Position;
                len[i] = bytes.Length;
                arcwriter.Write(bytes);
                tocwriter.Write(ID[i]);
                tocwriter.Write(pointers[i]);
                tocwriter.Write(len[i]);
                tocwriter.Write(int.Parse(unkTable[i]));
            }
            tocwriter.Close();
            arcwriter.Close();
            Console.WriteLine("File has been created");
        }
    }
}
