/*
 * Copyright 2011-2015 Numeric Technology
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using KPCore.KPUtil;
using Ionic.Zip;

namespace KPComponentsCore.Zip
{
    /// <summary>
    /// <para>Authors: Juliano Tiago Rinaldi and 
    /// Tiago Antonio Jacobi</para>
    /// </summary>
    public class ZipUtil
    {
        private static string ReplaceInvalidNameChars(string fileName)
        {
            string fileNameReturn = String.Empty;

            foreach (char item in fileName)
            {
                if (Path.GetInvalidFileNameChars().Contains(item))
                    fileNameReturn += "_";
                else
                    fileNameReturn += item;
            }

            return fileNameReturn;
        }

        public static string GetStringFile(ZipEntry zipEntry)
        {
            if (zipEntry != null)
            {
                using (var stream = zipEntry.OpenReader())
                {
                    using (StreamReader sr = new StreamReader(stream, KPGenericUtil.GetDefaultEncoding()))
                    {
                        string fileString = sr.ReadToEnd();
                        return fileString;
                    }
                }
            }

            return String.Empty;
        }

        public static byte[] ZipFileBytes(FileInfo[] files)
        {
            MemoryStream ms = new MemoryStream();
            using (Ionic.Zip.ZipFile zip = new ZipFile())
            {
                foreach (FileInfo item in files)
                {
                    FileStream fs = new FileStream(item.FullName, FileMode.Open, FileAccess.Read, FileShare.Read);
                    zip.AddEntry(item.Name, fs);
                }

                zip.Save(ms);
            }
            if (ms.Length > 0)
                return ms.ToArray();

            return null;
        }

        public static byte[] ZipFileBytes(byte[] fileBytes)
        {
            MemoryStream ms = new MemoryStream();
            using (Ionic.Zip.ZipFile zip = new ZipFile())
            {
                zip.CompressionLevel = Ionic.Zlib.CompressionLevel.BestCompression;
                zip.AddEntry("Object", fileBytes);
                zip.Save(ms);
            }
            if (ms.Length > 0)
                return ms.ToArray();

            return null;
        }

        public static void ZipFileSave(FileInfo zipFile, FileInfo[] files)
        {
            using (Ionic.Zip.ZipFile zip = new ZipFile())
            {
                foreach (FileInfo item in files)
                {
                    if (!item.Exists)
                    {
                        throw new IOException(String.Format("Arquivo {0} não existente.", item.FullName));
                    }

                    FileStream fs = new FileStream(item.FullName, FileMode.Open, FileAccess.Read, FileShare.Read);
                    zip.AddEntry(item.Name, fs);
                }

                using (FileStream fs = new FileStream(zipFile.FullName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    zip.Save(fs);
                    fs.Flush();
                }
            }
        }

        public static ZipFile GetZipFile(byte[] bytesZip)
        {
            if (!(bytesZip.Length > 0))
                throw new Exception("Não foi possível zipar os bytes pois estava vazio.");

            MemoryStream ms = new MemoryStream(bytesZip);
            return Ionic.Zip.ZipFile.Read(ms);
        }

        public static ZipFile GetZipFile(FileInfo zipFile)
        {
            if (zipFile.Exists)
                return Ionic.Zip.ZipFile.Read(zipFile.FullName);

            return null;
        }
    }

}
