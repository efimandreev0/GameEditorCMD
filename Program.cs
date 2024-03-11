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
                Console.WriteLine("USAGE: Tool.exe {-e(extract) or -w(write)} {game or format of file} {gamefile}\nSupported Games:\nPaper Mario Sticker Star (3DS)(BIN-image archive) (Command - PMSS)\nCoropata (DS) (Command - Coropata)\nGangstar Vegas (DS) (Command - Gangstar)\nDoomRPG (Java(STR), Brew(BSP)) (Command - DoomRPG)\nDoom II RPG (IOS) (Command - DoomIIRPG)\nJack and Daxter: The precursor Legacy (PC, PS2, PSV) (Command - Legacy)\nHyrule Warriors: Age of Calamity (NSW) (Command - HyruleCalamity)\nSupported files:\nBMG\n");
                Console.ReadKey();
            }
            else
            {
                if (args[0].Contains("-e"))
                {
                    if (args[1].Contains("PMSS"))
                    {
                        FileAttributes attr = File.GetAttributes(args[2]);
                        bool isDir = false;
                        if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
                            isDir = true;
                        if (!isDir)
                        {
                            if (args[2].Contains("_info"))
                            {
                                Nintendo._3DS.PMSS.BIN.Read(args[2], args[2].Replace("_info", ""));
                                return;
                            }
                            else
                            {
                                Nintendo._3DS.PMSS.BIN.Read(Path.GetFileNameWithoutExtension(args[2]) + "_info.bin", args[2]);
                                return;
                            }
                        }
                        else
                        {
                            Nintendo._3DS.PMSS.BIN.Write(args[2]);
                            return;
                        }
                    }
                    if (args[1].Contains("Gangstar"))
                    {
                        Nintendo.DS.GangstarVegas.LNG.Read(args[2]);
                    }
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
                    if (args[1].Contains("DoomIIRPG"))
                    {
                        Console.WriteLine("WARNING: To extract you need specify the folder in args");
                        IOS.DoomIIRPG.BIN.Read(args[2]);
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
                    if (args[1].Contains("Gangstar"))
                    {
                        FileAttributes attr = File.GetAttributes(args[2]);
                        bool isDir = false;
                        if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
                            isDir = true;
                        if (!isDir)
                        {
                            Console.WriteLine("WARNING: tool need directory with txt-files to build the full file");
                        }
                        else
                        {
                            Nintendo.DS.GangstarVegas.LNG.Write(args[2]);
                        }
                    }
                    if (args[1].Contains("Coropata"))
                    {
                        var writer = new BinaryWriter(File.OpenRead(args[2]));
                        Coropata.Write(writer);
                    }
                    if (args[1].Contains("DoomRPG"))
                    {
                        Console.WriteLine("WARNING: in the BREW-version file can't be greater by fixed size. Be careful!");
                        if (args[2].Contains(".txt"))
                        {
                            Java.DoomRPG.STR.Write(args[2]);
                        }
                        if (args[2].Contains("level01") || args[2].Contains("level02") || args[2].Contains("level03") || args[2].Contains("level04") || args[2].Contains("level05") || args[2].Contains("level06") || args[2].Contains("level07") || args[2].Contains("reactor") || args[2].Contains("junction") || args[2].Contains("intro") || args[2].Contains("junction_destroyed"))
                        {
                            Console.WriteLine("WARNING: file.bsp need to be in the one folder with file.txt and have one name, but \".bsp\" extension");
                            Java.DoomRPG.BSP.Write(args[2]);
                            
                        }
                    }
                    if (args[1].Contains("DoomIIRPG"))
                    {
                        Console.WriteLine("WARNING: To rebuild you need specify the folder in args");
                        IOS.DoomIIRPG.BIN.Write(args[2]);
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
            //IOS.DoomIIRPG.BIN.Write("C:\\Users\\zZz\\source\\repos\\bootEditor\\bin\\Debug\\text_DoomIIRPG-RE");
        }
    }
}
