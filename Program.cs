using bootEditor.Nintendo._3DS.SMDH;
using bootEditor.Nintendo.DS.Coropata;
using bootEditor.Sega.GameGear.Lunara;
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
            if (args.Length == 0)
            {
                Console.WriteLine("USAGE: Tool.exe {-e(extract) or -w(write)} {game or format of file} {gamefile}\nSupported Games:\nCoropata (DS)\nDoomRPG (Java)\nSupported files:\nBMG");
                Console.ReadKey();
            }
            else
            {
                if (args[0].Contains("-e"))
                {
                    if (args[1].Contains("Coropata"))
                    {
                        var reader = new BinaryReader(File.OpenRead(args[2]));
                        Coropata.Read(reader);
                    }
                    if (args[1].Contains("DoomRPG"))
                    {
                        Java.DoomRPG.STR.Read(args[2]);
                    }
                    if (args[1].Contains("BMG"))
                    {
                        Nintendo.DS.BMG.BMG.Read(args[2]);
                    }
                }
                if (args[0].Contains("-w"))
                {
                    if (args[1].Contains("Coropata"))
                    {
                        var writer = new BinaryWriter(File.OpenRead(args[2]));
                        Coropata.Write(writer);
                    }
                    if (args[1].Contains("DoomRPG"))
                    {
                        Java.DoomRPG.STR.Write(args[2]);
                    }
                }
            }
        }
    }
}
