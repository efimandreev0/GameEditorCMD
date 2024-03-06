using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace bootEditor.Nintendo._3DS.Pokemon
{
    internal class SARC
    {
        public static void ExtractSARC(string inputFilePath, string outputFolderPath)
        {
            using (FileStream inputFileStream = new FileStream(inputFilePath, FileMode.Open))
            {
                using (BinaryReader binaryReader = new BinaryReader(inputFileStream))
                {
                    string magic = new string(binaryReader.ReadChars(4));
                    if (magic != "SARC")
                    {
                        throw new Exception("Invalid SARC file!");
                    }

                    binaryReader.ReadUInt16(); // Не используется
                    ushort headerSize = binaryReader.ReadUInt16();
                    binaryReader.ReadUInt32(); // Не используется

                    binaryReader.BaseStream.Position = headerSize;

                    if (binaryReader.ReadInt32() != 0xFEFF4143)
                    {
                        throw new Exception("Incorrect SARC header!");
                    }

                    binaryReader.ReadUInt32(); // Не используется
                    int fileSize = binaryReader.ReadInt32();
                    int dataOffset = binaryReader.ReadInt32();

                    binaryReader.BaseStream.Position += 8; // Пропускаем заголовок

                    DirectoryInfo outputDirectory = Directory.CreateDirectory(outputFolderPath);

                    while (binaryReader.BaseStream.Position < dataOffset)
                    {
                        string fileName = new string(binaryReader.ReadChars(4));
                        int fileLength = binaryReader.ReadInt32();

                        byte[] fileData = binaryReader.ReadBytes(fileLength);

                        File.WriteAllBytes(Path.Combine(outputDirectory.FullName, fileName), fileData);

                        Console.WriteLine("Извлечен файл: " + fileName);
                    }

                    Console.WriteLine("SARC архив разобран успешно!");
                }
            }
        }


    }
}
