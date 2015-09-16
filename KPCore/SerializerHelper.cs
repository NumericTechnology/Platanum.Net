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
using System.Xml.Serialization;
using System.IO;
using System.Xml;
using System.Runtime.Serialization.Formatters.Binary;

namespace KPCore
{
    /// <summary>
    /// <para>Authors: Juliano Tiago Rinaldi and 
    /// Tiago Antonio Jacobi</para>
    /// </summary>
    public class SerializerHelper
    {
        public static string Serialization<T>(T objSerializer, bool omitXmlDeclaration = false, bool indented = false)
        {
            try
            {
                string xmlResult = String.Empty;

                XmlSerializerNamespaces ns = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
                ns.Add("", "");
                XmlWriterSettings writerSettings = new XmlWriterSettings()
                {
                    OmitXmlDeclaration = omitXmlDeclaration,
                    Indent = indented,
                    IndentChars = "\t"
                };

                StringWriter stringWriter = new StringWriter();
                using (XmlWriter xmlWriter = XmlWriter.Create(stringWriter, writerSettings))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(T));
                    serializer.Serialize(xmlWriter, objSerializer, ns);
                    xmlResult = stringWriter.ToString();
                }

                return xmlResult;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static T Deserialization<T>(string xmlString)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            object objSerialized = serializer.Deserialize(new StringReader(xmlString));
            if (objSerialized != null)
                return (T)objSerialized;

            throw new Exception("Não foi possível Deserializar XML");
        }

        public static byte[] SerializationObj(object obj)
        {
            BinaryFormatter binary = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                binary.Serialize(ms, obj);
                return ms.ToArray();
            }
        }

        public static T DeserializationObj<T>(byte[] data)
        {
            BinaryFormatter binary = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                ms.Write(data, 0, data.Length);
                ms.Position = 0;
                return (T)binary.Deserialize(ms);
            }
        }
    }
}
