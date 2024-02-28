using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace bootEditor.Nintendo.DS.Coropata
{
    internal class Coropata
    {
        public static void Read(BinaryReader reader)
        {
            reader.BaseStream.Position = 0xB38E8;
            string check = Encoding.UTF8.GetString(reader.ReadBytes(0x8));
            if (check != "COROPATA")
            {
                Console.WriteLine("It's not a Coropata arm9.bin");
            }
            else
            {
                reader.BaseStream.Position = 0x26980;
                int sysPoint = reader.ReadInt32() - 0x2000000;
                reader.BaseStream.Position = sysPoint;
                string system = Utils.ReadString(reader, Encoding.GetEncoding("Shift-jis")).Replace("\n\n", "<end>").Replace("\n", "<lf>");
                Directory.CreateDirectory("Coropata");
                File.WriteAllText("Coropata" + "\\" + "System.txt", system);
                reader.BaseStream.Position = 0xB3428;
                List<string> itemsSTR = new List<string>();
                for (int i = 0; i < 80; i++)
                {
                    int itemsP = reader.ReadInt32() - 0x2000000;
                    var pos = reader.BaseStream.Position;
                    reader.BaseStream.Position = itemsP;
                    itemsSTR.Add(Utils.ReadString(reader, Encoding.GetEncoding("Shift-jis")).Replace("\n\n", "<end>").Replace("\n", "<lf>"));
                    reader.BaseStream.Position = pos;
                }
                File.WriteAllLines("Coropata" + "\\" + "Items.txt", itemsSTR.ToArray());
                List<int> levelsP = new List<int>();
                List<string> levelsSTR = new List<string>();
                reader.BaseStream.Position = 0xB6CA8;
                for (int i = 0; i < 193; i++)
                {
                    levelsP.Add(reader.ReadInt32() - 0x2000000);
                    levelsP.Add(reader.ReadInt32() - 0x2000000);
                    levelsP.Add(reader.ReadInt32() - 0x2000000);
                    reader.BaseStream.Position += 12;
                }
                for (int i = 0; i < levelsP.Count; i++)
                {
                    reader.BaseStream.Position = levelsP[i];
                    levelsSTR.Add(Utils.ReadString(reader, Encoding.GetEncoding("Shift-jis")).Replace("\n\n", "<end>").Replace("\n", "<lf>").Replace("\b", "<b>").Replace("\u0010", "<10>"));
                }
                File.WriteAllLines("Coropata" + "\\" + "Levels.txt", levelsSTR.ToArray());
                reader.BaseStream.Position = 0x60F64;
                List<int> wifiP = new List<int>();
                List<string> wifiSTR = new List<string>();
                for (int i = 0; i < 18; i++)
                {
                    wifiP.Add(reader.ReadInt32() - 0x2000000);
                    var pos = reader.BaseStream.Position;
                    reader.BaseStream.Position = wifiP[i];
                    wifiSTR.Add(Utils.ReadString(reader, Encoding.GetEncoding("Shift-jis")).Replace("\n\n", "<end>").Replace("\n", "<lf>").Replace("\b", "<b>").Replace("\u0010", "<10>").Replace("\u0018", "<18>").Replace("\u001e", "<1E>").Replace("\u000e", "<0E>").Replace("\u001a", "<1A>"));
                    reader.BaseStream.Position = pos;
                }
                File.WriteAllLines("Coropata" + "\\" + "wifi.txt", wifiSTR.ToArray());
                reader.BaseStream.Position = 0xB8490;
                List<int> system2P = new List<int>();
                List<string> system2STR = new List<string>();
                for (int i = 0; i < 28; i++)
                {
                    system2P.Add(reader.ReadInt32() - 0x2000000);
                    var pos = reader.BaseStream.Position;
                    reader.BaseStream.Position = system2P[i];
                    system2STR.Add(Utils.ReadString(reader, Encoding.GetEncoding("Shift-jis")).Replace("\n\n", "<end>").Replace("\n", "<lf>").Replace("\b", "<b>").Replace("\u0010", "<10>").Replace("\u0018", "<18>").Replace("\u001e", "<1E>").Replace("\u000e", "<0E>").Replace("\u001a", "<1A>"));
                    reader.BaseStream.Position = pos;
                }
                File.WriteAllLines("Coropata" + "\\" + "system2.txt", system2STR.ToArray());
            }
        }
        public static void Write(BinaryWriter writer)
        {
            string[] system = File.ReadAllLines("Coropata" + "\\" + "System.txt");
            string[] system2 = File.ReadAllLines("Coropata" + "\\" + "system2.txt");
            int[] system2P = new int[system2.Length];
            string[] levels = File.ReadAllLines("Coropata" + "\\" + "levels.txt");
            int[] levelsP = new int[levels.Length];
            string[] items = File.ReadAllLines("Coropata" + "\\" + "items.txt");
            int[] itemsP = new int[items.Length];
            string[] wifi = File.ReadAllLines("Coropata" + "\\" + "wifi.txt");
            int[] wifiP = new int[wifi.Length];
            //system1
            writer.BaseStream.Position = 0xAAB74;
            int systemP = (int)writer.BaseStream.Position + 0x2000000;
            writer.Write(Encoding.GetEncoding("Shift-jis").GetBytes(system[0].Replace("<lf>", "\n")));
            writer.Write(new byte());
            writer.BaseStream.Position = 0x26980;
            writer.Write(systemP);
            writer.BaseStream.Position = 0x26A54;
            writer.Write(systemP);
            writer.BaseStream.Position = 0x5EA2C;
            writer.Write(systemP);
            //system2
            writer.BaseStream.Position = 0xB8500;
            for (int i = 0; i < system2.Length; i++)
            {
                system2P[i] = (int)writer.BaseStream.Position + 0x2000000;
                writer.Write(Encoding.GetEncoding("Shift-jis").GetBytes(system2[i].Replace("<lf>", "\n")));
                writer.Write(new byte());
            }
            writer.BaseStream.Position = 0xB8490;
            for (int i = 0; i < system2.Length; i++)
            {
                writer.Write(system2P[i]);
            }
            //items
            writer.BaseStream.Position = 0xB1CE0;
            for (int i = 0; i < items.Length; i++)
            {
                itemsP[i] = (int)writer.BaseStream.Position + 0x2000000;
                writer.Write(Encoding.GetEncoding("Shift-jis").GetBytes(items[i].Replace("<lf>", "\n").Replace("<end>", "\n\n")));
                writer.Write(new byte());
            }
            writer.BaseStream.Position = 0xB3428;
            for (int i = 0; i < items.Length; i++)
            {
                writer.Write(itemsP[i]);
            }
            //levels
            writer.BaseStream.Position = 0xB392C;
            for (int i = 0; i < levels.Length; i++)
            {
                levelsP[i] = (int)writer.BaseStream.Position + 0x2000000;
                writer.Write(Encoding.GetEncoding("Shift-jis").GetBytes(levels[i].Replace("<lf>", "\n").Replace("<end>", "\n\n").Replace("<b>", "\b").Replace("<10>", "\u0010")));
                writer.Write(new byte());
            }
            writer.BaseStream.Position = 0xB6CA8;
            for (int i = 0; i < levels.Length / 3; i++)
            {
                writer.Write(levelsP[i]);
                writer.Write(levelsP[i+1]);
                writer.Write(levelsP[i+2]);
                writer.BaseStream.Position += 12;
            }
            //wifi
            writer.BaseStream.Position = 0xB8024;
            for (int i = 0; i < wifi.Length; i++)
            {
                wifiP[i] = (int)writer.BaseStream.Position + 0x2000000;
                writer.Write(Encoding.GetEncoding("Shift-jis").GetBytes(wifi[i].Replace("<lf>", "\n").Replace("<end>", "\n\n").Replace("<b>", "\b").Replace("<10>", "\u0010").Replace("<18>", "\u0018").Replace("<1E>", "\u001E").Replace("<0E>", "\u000E").Replace("<1A>","\u001A")));
                writer.Write(new byte());
            }
            writer.BaseStream.Position = 0x60F64;
            for (int i = 0; i < wifi.Length; i++)
            {
                writer.Write(wifiP[i]);
            }
            //end
        }
    }
}
