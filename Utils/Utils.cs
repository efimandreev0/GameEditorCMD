using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using Brotli;

namespace bootEditor.Utils
{
    public static class Utils
    {
        
        public static int FindBytes(byte[] fileBytes, byte[] bytesToSearch)
        {
            int maxPosition = fileBytes.Length - bytesToSearch.Length;
            for (int i = 0; i <= maxPosition; i++)
            {
                bool match = true;
                for (int j = 0; j < bytesToSearch.Length; j++)
                {
                    if (fileBytes[i + j] != bytesToSearch[j])
                    {
                        match = false;
                        break;
                    }
                }

                if (match)
                {
                    return i;
                }
            }

            return -1; // Возвращаем -1, если байты не найдены
        }
        public static int GetRelativePointerOffset(BinaryReader reader, int offset)
        {
            int result = -1;
            reader.BaseStream.Position = offset - 4;
            while (reader.BaseStream.Position > 0)
            {
                int check = reader.ReadInt32();
                int difference = (int)(offset - reader.BaseStream.Position);
                //Console.WriteLine("Pos: {0}/Diff: {1}/Value: {2}", reader.BaseStream.Position, difference, check);
                if (check == difference + 4 && difference != 0)
                    return (int)(reader.BaseStream.Position - 4);
                else
                    reader.BaseStream.Position -= 8;
            }
            return result;
        }
        public static string PersonaEncoding(byte[] namebuff)
        {
            StringBuilder sb = new StringBuilder();
            if (namebuff[0] == 0x80)
            {
                for (int i = 0; i < namebuff.Length - 1; i += 2)
                {
                    sb.Append((char)(namebuff[i + 1] - 0x60));
                }
                return sb.ToString();
            }
            else if (namebuff[0] == 0x81)
            {
                for (int i = 0; i < namebuff.Length - 1; i += 2)
                {
                    sb.Append((char)(namebuff[i + 1] + 0x20));
                }
                return sb.ToString();
            }
            else if (namebuff[0] == 0x82)
            {
                for (int i = 0; i < namebuff.Length - 1; i += 2)
                {
                    sb.Append((char)(namebuff[i + 1] - 0x100));
                }
                return sb.ToString();
            }
            else
            {
                return Utils.ReadString(namebuff, Encoding.UTF8);
            }
        }
        public static string ApplicationDirectory { get; } = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
        public static string AutoFitLine(string text, int maxLength)
        {
            string[] splitted = text.Split();
            string result = "";
            int symbols = 0;
            int line_count = 0;
            foreach (var word in splitted)
            {
                if (word.Length + symbols > maxLength)
                {
                    result += "\n";
                    symbols = word.Length;
                    result += word;
                    line_count++;
                }
                else
                {
                    if (symbols > 0)
                    {
                        result += " ";
                        symbols++;
                    }
                    result += word;
                    symbols += word.Length;
                }
            }
            return result;
        }

        public static byte[] ReadByteArray(BinaryReader reader, int offset, int size)
        {
            byte[] result = new byte[size];
            var savepos = reader.BaseStream.Position;
            reader.BaseStream.Position = offset;
            result = reader.ReadBytes(size);
            reader.BaseStream.Position = savepos;
            return result;
        }
        public static byte[] DecompressZlib(byte[] data)
        {
            using (var compressedStream = new MemoryStream(data))
            using (var zipStream = new GZipStream(compressedStream, System.IO.Compression.CompressionMode.Decompress))
            using (var resultStream = new MemoryStream())
            {
                zipStream.CopyTo(resultStream);
                return resultStream.ToArray();
            }
        }
        public static byte[] CompressBytes(byte[] inputBytes)
        {
            byte[] compressedBytes;

            using (MemoryStream outputStream = new MemoryStream())
            {
                using (BrotliStream brotliStream = new BrotliStream(outputStream, System.IO.Compression.CompressionMode.Compress))
                {
                    brotliStream.Write(inputBytes, 0, inputBytes.Length);
                }

                compressedBytes = outputStream.ToArray();
            }

            return compressedBytes;
        }

        public static string[] SortNames(string[] names)
        {
            // Используем метод Array.Sort для сортировки массива имен
            Array.Sort(names, StringComparer.InvariantCultureIgnoreCase);
            return names;
        }


        public static string Reverse(string s)
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }
        public static string ReadSubtitle(BinaryReader reader, int offset, bool return2pos)
        {
            string sub = string.Empty;
            long savepos = reader.BaseStream.Position;
            reader.BaseStream.Position = offset;
            sub = ReadString(reader, Encoding.UTF8);
            if (return2pos)
                reader.BaseStream.Position = savepos;
            return sub;
        }


        public static void AlignPosition(BinaryReader reader)
        {
            long pos = reader.BaseStream.Position;
            if (pos % 0x10 != 0)
                reader.BaseStream.Position = (0x10 - pos % 0x10) + pos;
        }

        public static void AlignPosition(BinaryReader reader, int align)
        {
            long pos = reader.BaseStream.Position;
            if (pos % align != 0)
                reader.BaseStream.Position = (align - pos % align) + pos;
        }

        public static long GetAlignLength(BinaryWriter writer, long align)
        {
            long length = 0;
            long pos = writer.BaseStream.Position;
            if (pos % align != 0)
                length = ((align - pos % align) + pos) - pos;
            return length;
        }

        public static long GetAlignLength(BinaryWriter writer)
        {
            long length = 0;
            long pos = writer.BaseStream.Position;
            if (pos % 0x10 != 0)
                length = ((0x10 - pos % 0x10) + pos) - pos;
            return length;
        }

        public static void AlignPosition(BinaryWriter writer, int align)
        {
            long pos = writer.BaseStream.Position;
            if (pos % align != 0)
                writer.BaseStream.Position = (align - pos % align) + pos;
        }

        public static long GetAlignLength(BinaryReader reader)
        {
            long length = 0;
            long pos = reader.BaseStream.Position;
            if (pos % 0x10 != 0)
                length = ((0x10 - pos % 0x10) + pos) - pos;
            return length;
        }

        public static string ReadString(byte[] namebuf, Encoding encoding)
        {
            BinaryReader binaryReader = new BinaryReader(new MemoryStream(namebuf));
            if (encoding == null) throw new ArgumentNullException("encoding");

            List<byte> data = new List<byte>();

            while (binaryReader.BaseStream.Position < binaryReader.BaseStream.Length)
            {
                data.Add(binaryReader.ReadByte());

                string partialString = encoding.GetString(data.ToArray(), 0, data.Count);

                if (partialString.Length > 0 && partialString.Last() == '\0')
                    return encoding.GetString(data.SkipLast(encoding.GetByteCount("\0")).ToArray()).TrimEnd('\0');
            }
            throw new InvalidDataException("Hit end of stream while reading null-terminated string.");
        }
        public static string ReadString(this BinaryReader binaryReader, Encoding encoding)
        {
            if (binaryReader == null) throw new ArgumentNullException("binaryReader");
            if (encoding == null) throw new ArgumentNullException("encoding");

            List<byte> data = new List<byte>();

            while (binaryReader.BaseStream.Position < binaryReader.BaseStream.Length)
            {
                data.Add(binaryReader.ReadByte());

                string partialString = encoding.GetString(data.ToArray(), 0, data.Count);

                if (partialString.Length > 0 && partialString.Last() == '\0')
                    return encoding.GetString(data.SkipLast(encoding.GetByteCount("\0")).ToArray()).TrimEnd('\0');
            }
            throw new InvalidDataException("Hit end of stream while reading null-terminated string.");
        }
        private static IEnumerable<TSource> SkipLast<TSource>(this IEnumerable<TSource> source, int count)
        {
            if (source == null) throw new ArgumentNullException("source");

            Queue<TSource> queue = new Queue<TSource>();

            foreach (TSource item in source)
            {
                queue.Enqueue(item);

                if (queue.Count > count) yield return queue.Dequeue();
            }
        }
    }
}
