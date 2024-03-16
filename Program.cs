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
        //Оставь надежду, всяк сюда входящий.
        //Как говорил один мой знакомый:
        //"Через месяц весь код, который
        //я написал, становится чужим. А
        //после нового года моя память
        //стирается, и приходится учить
        //шарп заново".
        static void Main(string[] args)
        {
            /*if (args.Length == 0)
            {
                Console.WriteLine("USAGE: Tool.exe {-e(extract) or -w(write)} {game or format of file} {gamefile}\n" +
                    "\n" +
                    "Supported Games:\n" +
                    "Blaster Master Zero (3DS)(TEXT, ARC) (Command - BMZ. TEXT -t, ARC -a (file.irarc file.irlst))\n" +
                    "Paper Mario Sticker Star (3DS) (BIN-image archive) (Command - PMSS)\n" +
                    "Coropata (DS) (Command - Coropata)\n" +
                    "Gangstar Vegas (DS) (Command - Gangstar)\n" +
                    "DoomRPG (Java(STR), Brew(BSP)) (Command - DoomRPG (if brew point the after DoomRPG -b))\n" +
                    "Doom II RPG (IOS) (Command - DoomIIRPG)\n" +
                    "Jack and Daxter: The precursor Legacy (PC, PS2, PSV) (Command - Legacy)\n" +
                    "Hyrule Warriors: Age of Calamity (NSW) (Command - HyruleCalamity)\n" +
                    "\n" +
                    "Supported files:\nB" +
                    "MG (DS) (Full Support)\n");
                Console.ReadKey();
            }
            else
            {
                if (args[0].Contains("-e"))
                {
                    if (args[1].Contains("BMZ"))
                    {
                        if (args[2].Contains("-t"))
                        {
                            Nintendo._3DS.BlasterMasterZero.TEXT.Read(args[3]);
                        }
                        if (args[2].Contains("-a"))
                        {
                            if (args[3].Contains(".irlst"))
                            {
                                Nintendo._3DS.BlasterMasterZero.IRARC.Read(args[3].Replace(".irlst", ".irarc"), args[3]);
                                return;
                            }
                            else
                            {
                                Nintendo._3DS.BlasterMasterZero.IRARC.Read(args[3], args[3].Replace(".irarc", ".irlst"));
                                return;
                            }
                        }
                    }
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
                        if (args[2].Contains(".db"))
                        {
                            Java.DoomRPG.DB.Read(args[2]);
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
                    if (args[1].Contains("BMG"))
                    {
                        Nintendo.DS.BMG.BMG.Write(args[3]);
                    }
                    if (args[1].Contains("BMZ"))
                    {
                        if (args[2].Contains("-t"))
                        {
                            Nintendo._3DS.BlasterMasterZero.TEXT.Write(args[3]);
                        }
                        if (args[2].Contains("-a"))
                        {
                            Nintendo._3DS.BlasterMasterZero.IRARC.Write(args[3]);
                        }
                    }
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
                        if (args[2].Contains("-b"))
                        {
                            Java.DoomRPG.BSP.Write(args[3]);
                        }
                        if (args[2].Contains(".json"))
                        {
                            Java.DoomRPG.DB.Write(args[2]);
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
            }*/
            //Nintendo.DS.OrcsElves.ARC.Read("UI_Shapes.bin", "UI_Shape_Offset.bin");
            //Java.DoomRPG.BSP.Write("level01.json");
            Nintendo.Switch.LuigisMansion3.ARC.Write("init");
        }
    }
}
