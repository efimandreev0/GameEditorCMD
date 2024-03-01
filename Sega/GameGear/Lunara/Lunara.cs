using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bootEditor.Sega.GameGear.Lunara
{
    internal class Lunara
    {
        public static void Read(BinaryReader reader)
        {
            string charset = "charset.txt";
            reader.BaseStream.Position = 0x80000;
            CEGET swapper = new CEGET(charset);
            byte[] byteArray = new byte[] { 0x10, 0x20, 0x30 };
            string character = swapper.FromBytes(byteArray);
            Console.WriteLine("Символ для массива байтов: " + character);

        }
    }
}
