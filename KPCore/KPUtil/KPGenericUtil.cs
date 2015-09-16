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
using System.Reflection;
using System.IO;
using System.Text.RegularExpressions;

namespace KPCore.KPUtil
{
    /// <summary>
    /// <para>Authors: Juliano Tiago Rinaldi and 
    /// Tiago Antonio Jacobi</para>
    /// </summary>
    public class KPGenericUtil
    {
        public static Type GetTypeByNamespace(string typeEntityNamespace)
        {
            Type typeEntity = null;

            string extractAssemblyByNamespace = typeEntityNamespace.Split('.')[0];
            try
            {
                typeEntity = Assembly.Load(extractAssemblyByNamespace).GetType(typeEntityNamespace);
            }
            catch { }

            if (typeEntity != null)
                return typeEntity;

            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                try
                {
                    typeEntity = assembly.GetType(typeEntityNamespace);
                }
                catch { continue; }

                if (typeEntity != null)
                    return typeEntity;
            }

            return null;
        }

        public static bool IsFileLocked(FileInfo file, int tryAgain = 10)
        {
            if (!File.Exists(file.FullName))
                return false;

            FileStream stream = null;
            try
            {
                stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            }
            catch (Exception)
            {
                System.Threading.Thread.Sleep(500);
                if (tryAgain >= 0)
                    IsFileLocked(file, tryAgain--);

                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }

            return false;
        }

        public static string GetSourceViewKPGrid(string source)
        {
            Regex regexKPGrid = new Regex(@"<KP:KPGridControl [\w\s="".:/<>%-]+</KP:KPGridControl>");

            var extract = regexKPGrid.Match(source);
            if (extract.Success)
                return extract.Value;

            return String.Empty;
        }

        public static string GetSourceViewKPForm(string source)
        {
            Regex regexKPForm = new Regex(@"<KP:KPFormControl [\w\s="".:/<>%-]+</KP:KPFormControl>");

            var extract = regexKPForm.Match(source);
            if (extract.Success)
                return extract.Value;

            return String.Empty;
        }

        /// <summary>
        /// Return the Default System Encoding (UTF8 with BOM)
        /// </summary>
        /// <returns></returns>
        public static Encoding GetDefaultEncoding()
        {
            UTF8Encoding encoding = new UTF8Encoding(true);
            return encoding;
        }

    }
}
