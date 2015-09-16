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
using KPCore.SerializerMap;
using System.IO;

namespace KPCore.KPConfig
{
    /// <summary>
    /// <para>Authors: Juliano Tiago Rinaldi and 
    /// Tiago Antonio Jacobi</para>
    /// </summary>
    public class KPConfigurationHelper
    {
        private static KPConfiguration kpConfiguration;

        public static KPConfiguration KPConfiguration
        {
            get
            {
                if (kpConfiguration == null)
                    kpConfiguration = GetKPConfiguration();

                return kpConfiguration;
            }
        }

        /// <summary>
        /// Retorna o Objeto de Configuração do KPFramework
        /// </summary>
        /// <returns></returns>
        private static KPConfiguration GetKPConfiguration()
        {
            try
            {
                KPConfiguration config = null;
                string fileConfigXml = Path.Combine(AppDomain.CurrentDomain.RelativeSearchPath, "KPConfiguration.xml");
                FileInfo file = new FileInfo(fileConfigXml);
                if (file.Exists)
                {
                    FileStream fileStream = new FileStream(file.FullName, FileMode.Open);
                    StreamReader sr = new StreamReader(fileStream);
                    config = SerializerHelper.Deserialization<KPConfiguration>(sr.ReadToEnd());
                }

                return config;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
