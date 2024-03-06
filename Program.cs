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
                Console.WriteLine("USAGE: Tool.exe {-e(extract) or -w(write)} {game or format of file} {gamefile}\nSupported Games:\nCoropata (DS) (Command - Coropata)\nDoomRPG (Java(STR), Brew(BSP)) (Command - DoomRPG)\nJack and Daxter: The precursor Legacy (PC) (Command - Legacy)\nHyrule Warriors: Age of Calamity (Command - HyruleCalamity)\nSupported files:\nBMG");
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
                        if (args[2].Contains(".str"))
                        {
                            Java.DoomRPG.STR.Read(args[2]);
                        }
                        if (args[2].Contains(".bsp"))
                        {
                            Java.DoomRPG.BSP.Read(args[2]);
                        }
                    }
                    if (args[1].Contains("BMG"))
                    {
                        Nintendo.DS.BMG.BMG.Read(args[2]);
                    }
                    if (args[1].Contains("Legacy"))
                    {
                        PC.JackDaxterLegacy.TXTB.Read(args[2]);
                    }
                    if (args[1].Contains("HyruleCalamity"))
                    {
                        Nintendo.Switch.HyruleWarriors.TEXT.Read(args[2]);
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
                        Console.WriteLine("WARNING: in the BREW-version file can't be greater by fixed size. Be careful!");
                        if (args[2].Contains(".str"))
                        {
                            Java.DoomRPG.STR.Write(args[2]);
                        }
                        if (args[2].Contains(".bsp"))
                        {
                            Java.DoomRPG.BSP.Write(args[2]);
                        }
                    }
                    if (args[1].Contains("Legacy"))
                    {
                        Console.WriteLine("WARNING: To rebuild you need speicfy your .txt file with edited text, and file to you want rebuild will be called {your text file name + \".undec\"}");
                        PC.JackDaxterLegacy.TXTB.Write(args[2]);
                    }
                    if (args[1].Contains("HyruleCalamity"))
                    {
                        Nintendo.Switch.HyruleWarriors.TEXT.Write(args[2]);
                    }
                }
            }
        }
    }
}
