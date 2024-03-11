using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bootEditor.Java.DoomRPG
{
    internal class BSP
    {
        public static void Read(string file)
        {
            string nameWithoutExt = Path.GetFileNameWithoutExtension(file);
            var reader = new BinaryReader(File.OpenRead(file));
            Console.WriteLine(nameWithoutExt);
            if (file.Contains("intro"))
            {
                reader.BaseStream.Position = 0x2D1E;
            }
            if (file.Contains("junction"))
            {
                reader.BaseStream.Position = 0x1872;
            }
            if (file.Contains("junction_destroyed"))
            {
                reader.BaseStream.Position = 0x2689;
            }
            if (file    .Contains("level01"))
            {
                reader.BaseStream.Position = 0x2719;
            }
            if (file.Contains("level02"))
            {
                reader.BaseStream.Position = 0x2F72;
            }
            if (file    .Contains("level03"))
            {
                reader.BaseStream.Position = 0x2DED;
            }
            if (file.Contains("level04"))
            {
                reader.BaseStream.Position = 0x3452;
            }
            if (file.Contains("level05"))
            {
                reader.BaseStream.Position = 0x3C91;
            }
            if (file.Contains("level06"))
            {
                reader.BaseStream.Position = 0x3C15;
            }
            if (file.Contains("level07"))
            {
                reader.BaseStream.Position = 0x3A75;
            }
            if (file.Contains("reactor"))
            {
                reader.BaseStream.Position = 0x2A1C;
            }
            int count = reader.ReadUInt16();
            string[] strings = new string[count];
            for (int i = 0; i < count; i++)
            {
                int len = reader.ReadInt16();
                strings[i] = Encoding.GetEncoding("Windows-1252").GetString(reader.ReadBytes(len)).Replace("\n", "<lf>");
            }
            File.WriteAllLines(nameWithoutExt + ".txt", strings);
        }
        public static void Write(string file)
        {
            //skipping blocks & get ost info
            string[] strings = File.ReadAllLines(file);
            string nameWithoutExt = Path.GetFileNameWithoutExtension(file);
            var reader = new BinaryReader(File.OpenRead(nameWithoutExt + ".bsp"));
            var pos = 0;
            if (file.Contains("intro"))
            {
                 pos = 0x2D1E;
            }
            if (file.Contains("junction"))
            {
                pos = 0x1872;
            }
            if (file    .Contains("junction_destroyed"))
            {
                pos = 0x2689;
            }
            if (file.Contains("level01"))
            {
                pos = 0x2719;
            }
            if (file    .Contains("level02"))
            {
                pos = 0x2F72;
            }
            if (file.Contains("level03"))
            {
                pos = 0x2DED;
            }
            if (file.Contains("level04"))
            {
                pos = 0x3452;
            }
            if (file.Contains("level05"))
            {
                pos = 0x3C91;
            }
            if (file.Contains("level06"))
            {
                pos = 0x3C15;
            }
            if (file.Contains("level07"))
            {
                pos = 0x3A75;
            }
            if (file.Contains("reactor"))
            {
                pos  = 0x2A1C;
            }
            reader.BaseStream.Position = pos;
            int count = reader.ReadUInt16();
            for (int i = 0; i < count; i++)
            {
                int len = reader.ReadInt16();
                reader.ReadBytes(len);
            }
            byte[] bytes = reader.ReadBytes((int)(reader.BaseStream.Length - reader.BaseStream.Position));
            reader.Close();
            //writing
            var writer = new BinaryWriter(File.OpenWrite(nameWithoutExt + ".bsp"));
            writer.BaseStream.Position = pos;
            writer.Write((short)strings.Length);
            for (int i = 0; i < strings.Length; i++)
            {
                strings[i] = strings[i].Replace("<lf>", "\n");
                writer.Write((short)strings[i].Length);
                writer.Write(Encoding.GetEncoding("Windows-1252").GetBytes(strings[i]));
            }
            writer.Write(bytes);
            Console.WriteLine($"File {nameWithoutExt + ".bsp"} was succesfully writed!");
        }
    }
}
