﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bootEditor.Nintendo._3DS.PMSS
{
    internal class BIN
    {
        public static void Read(string table, string archive)
        {
            var reader = new BinaryReader(File.OpenRead(table));
            Int32 count = reader.ReadInt32();
            int[] nameOffset = new int[count];
            int[] dataOffset = new int[count];
            int[] unknown = new int[count];
            int[] dataSize = new int[count];

            for (int i = 0; i < count; i++)
            {
                nameOffset[i] = reader.ReadInt32();
                dataOffset[i] = reader.ReadInt32();
                unknown[i] = reader.ReadInt32();
                dataSize[i] = reader.ReadInt32();
            }

            int namesPos = count * 0x10 + 4;
            string[] fileNames = new string[count];
            for (int i = 0; i < count; i++)
            {
                reader.BaseStream.Position = namesPos + nameOffset[i];
                fileNames[i] = Utils.Utils.ReadString(reader, Encoding.ASCII);
            }
            reader.Close();
            reader = new BinaryReader(File.OpenRead(archive));
            string outPath = Path.GetFileNameWithoutExtension(archive) + "\\";
            Directory.CreateDirectory(outPath);
            for (int i = 0; i < count; i++)
            {
                reader.BaseStream.Position = dataOffset[i];
                byte[] data = reader.ReadBytes(dataSize[i]);
                Console.WriteLine("unpacked " + fileNames[i] + ".bcrez");
                File.WriteAllBytes(outPath + fileNames[i] + ".bcrez", data);
            }
            File.WriteAllLines(outPath + "unkTable.txt", unknown.ToList().Select(t => t.ToString()));
        }
        public static void Write(string inputDirectory)
        {
            if (!File.Exists(inputDirectory + "\\unkTable.txt"))
            {
                Console.WriteLine("Долбоёб, ты где unkTable.txt проебал?");
                return;
            }
            string[] files = Directory.GetFiles(inputDirectory, "*.bcrez", SearchOption.TopDirectoryOnly);

            string[] unkTable = File.ReadAllLines(inputDirectory + "\\unkTable.txt");

            if (files.Length != unkTable.Length)
            {
                Console.WriteLine("Не, ты всё ещё идёшь нахуй, слишком много файлов.");
                return;
            }

            using (BinaryWriter tocWriter = new BinaryWriter(File.Create(inputDirectory + "_info.bin")))
            using (BinaryWriter arcWriter = new BinaryWriter(File.Create(inputDirectory + ".bin")))
            {
                tocWriter.Write(files.Length);

                int namesPos = files.Length * 0x10 + 4;

                int[] namesPtr = new int[files.Length];
                tocWriter.BaseStream.Position = namesPos;


                for (int i = 0; i < files.Length; i++)
                {
                    string name = Path.GetFileNameWithoutExtension(Path.GetFileName(files[i]));
                    namesPtr[i] = (int)(tocWriter.BaseStream.Position - namesPos);
                    tocWriter.Write(Encoding.ASCII.GetBytes(name));
                    tocWriter.Write(Int32.Parse(unkTable[i]));

                    byte[] file = File.ReadAllBytes(files[i]);
                    tocWriter.Write(file.Length);
                    arcWriter.Write(file);
                }
                tocWriter.BaseStream.Position = 4;
                for (int i = 0; i < files.Length; i++)
                {
                    tocWriter.Write(namesPtr[i]);
                    tocWriter.Write((int)arcWriter.BaseStream.Position);

                }
            }
        }
    }
}
