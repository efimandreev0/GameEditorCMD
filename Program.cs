using bootEditor.Nintendo.DS.Coropata;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bootEditor
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var writer = new BinaryWriter(File.OpenWrite("arm9.bin"));
            Coropata.Write(writer);
        }
    }
}
