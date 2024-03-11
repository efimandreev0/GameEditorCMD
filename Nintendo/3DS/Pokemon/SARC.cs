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
        public static void Read(string file)
        {
            int position = 0;
            byte[] bytesToSearch = { 0x53, 0x41, 0x52, 0x43 };
            // Открываем бинарный файл для чтения в байтовом режиме
            using (var fileStream = new FileStream(file, FileMode.Open, FileAccess.Read))
            {
                byte[] fileBytes = new byte[fileStream.Length];
                fileStream.Read(fileBytes, 0, fileBytes.Length);

                // Выполняем поиск указанных байтов
                position = Utils.Utils.FindBytes(fileBytes, bytesToSearch);

                if (position >= 0)
                {
                    Console.WriteLine("Байты найдены на позиции {0}", position);
                }
                else
                {
                    Console.WriteLine("Байты не найдены");
                }
            }
            var reader = new BinaryReader(File.OpenRead(file));
            reader.BaseStream.Position = position;
            //S
            int magic = reader.ReadInt32();
            int headerLen = reader.ReadInt16();
            int byteOrder = reader.ReadInt16(); //Byte-order marker (0xFEFF = big, 0xFFFE = little)
            int fileLen = reader.ReadInt32();
            int dataOffset = reader.ReadInt32() + 0xC + position;
            reader.ReadInt32(); //unk -- always \x00\x01\x00\x00
            //SFAT
            int magicSFAT = reader.ReadInt32();
            int headerSFAT = reader.ReadInt32();
        }
    }
}
