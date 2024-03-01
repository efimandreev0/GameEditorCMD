using System;
using System.Collections.Generic;
using System.IO;

namespace bootEditor
{
    internal class CEGET
    {
        private Dictionary<string, string> byteToCharacterDictionary;
        private Dictionary<string, string> characterToByteDictionary;

        public CEGET(string filePath)
        {
            byteToCharacterDictionary = new Dictionary<string, string>();
            characterToByteDictionary = new Dictionary<string, string>();

            // Чтение значений из текстового файла
            string[] lines = File.ReadAllLines(filePath);

            foreach (string line in lines)
            {
                string[] parts = line.Split('=');
                if (parts.Length == 2)
                {
                    string byteString = parts[0].Trim();
                    string character = parts[1];

                    byteToCharacterDictionary[byteString] = character;
                    characterToByteDictionary[character] = byteString;
                }
            }
        }

        public string FromBytes(byte[] byteValue)
        {
            string byteString = BitConverter.ToString(byteValue).Replace("-", "");
            for (int i = 0; i < byteString.Length; i += 2)
            {
                if (byteToCharacterDictionary.ContainsKey(byteString.Substring(i, 2)))
                {
                    byteString = byteString.Replace(byteString.Substring(i, 2), byteToCharacterDictionary[byteString.Substring(i, 2)]);
                }
            }

            return byteString;
        }


        public byte[] ToBytes(string character)
        {
            if (characterToByteDictionary.ContainsKey(character))
            {
                string byteString = characterToByteDictionary[character];
                return StringToByteArray(byteString);
            }
            else
            {
                throw new ArgumentException("Недопустимый символ: " + character);
            }
        }

        private byte[] StringToByteArray(string byteString)
        {
            int length = byteString.Length / 2;
            byte[] byteArray = new byte[length];
            for (int i = 0; i < length; i++)
            {
                byteArray[i] = Convert.ToByte(byteString.Substring(i * 2, 2), 16);
            }
            return byteArray;
        }
    }
}
