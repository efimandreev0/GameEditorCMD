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
            if (nameWithoutExt == "intro")
            {
                reader.BaseStream.Position = 0x2D1E;
            }
            if (nameWithoutExt == "junction")
            {
                reader.BaseStream.Position = 0x1872;
            }
            if (nameWithoutExt == "junction_destroyed")
            {
                reader.BaseStream.Position = 0x2689;
            }
            if (nameWithoutExt == "level01")
            {
                reader.BaseStream.Position = 0x2719;
            }
            if (nameWithoutExt == "level02")
            {
                reader.BaseStream.Position = 0x2F72;
            }
            if (nameWithoutExt == "level03")
            {
                reader.BaseStream.Position = 0x2DED;
            }
            if (nameWithoutExt == "level04")
            {
                reader.BaseStream.Position = 0x3452;
            }
            if (nameWithoutExt == "level05")
            {
                reader.BaseStream.Position = 0x3C91;
            }
            if (nameWithoutExt == "level06")
            {
                reader.BaseStream.Position = 0x3C15;
            }
            if (nameWithoutExt == "level07")
            {
                reader.BaseStream.Position = 0x3A75;
            }
            if (nameWithoutExt == "reactor")
            {
                reader.BaseStream.Position = 0x2A1C;
            }
            else
            {
                Console.WriteLine("This file format is not supported!");
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
            string[] strings = File.ReadAllLines(file);
            string nameWithoutExt = Path.GetFileNameWithoutExtension(file);
            var writer = new BinaryWriter(File.OpenWrite(nameWithoutExt + ".bsp"));
            if (nameWithoutExt == "intro.bsp")
            {
                writer.BaseStream.Position = 0x2D1E;
            }
            if (nameWithoutExt == "junction")
            {
                writer.BaseStream.Position = 0x1872;
            }
            if (nameWithoutExt == "junction_destroyed")
            {
                writer.BaseStream.Position = 0x2689;
            }
            if (nameWithoutExt == "level01")
            {
                writer.BaseStream.Position = 0x2719;
            }
            if (nameWithoutExt == "level02")
            {
                writer.BaseStream.Position = 0x2F72;
            }
            if (nameWithoutExt == "level03")
            {
                writer.BaseStream.Position = 0x2DED;
            }
            if (nameWithoutExt == "level04")
            {
                writer.BaseStream.Position = 0x3452;
            }
            if (nameWithoutExt == "level05")
            {
                writer.BaseStream.Position = 0x3C91;
            }
            if (nameWithoutExt == "level06")
            {
                writer.BaseStream.Position = 0x3C15;
            }
            if (nameWithoutExt == "level07")
            {
                writer.BaseStream.Position = 0x3A75;
            }
            if (nameWithoutExt == "reactor")
            {
                writer.BaseStream.Position = 0x2A1C;
            }
            else
            {
                Console.WriteLine("File is not searched in database!");
            }
            writer.Write((short)strings.Length);
            for (int i = 0; i < strings.Length; i++)
            {
                strings[i] = strings[i].Replace("<lf>", "\n");
                writer.Write((short)strings[i].Length);
                writer.Write(Encoding.GetEncoding("Windows-1252").GetBytes(strings[i]));
            }
        }
    }
}
