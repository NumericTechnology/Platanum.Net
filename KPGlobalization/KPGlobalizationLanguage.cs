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
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Web.Compilation;

namespace KPGlobalization
{
    // Retonar o namespace completo com a classe do resouce de tradução
    public delegate string KPNamespaceResource();

    /// <summary>
    /// <para>Authors: Juliano Tiago Rinaldi and 
    /// Tiago Antonio Jacobi</para>
    /// </summary>
    public class KPGlobalizationLanguage
    {
        public static event KPNamespaceResource KPEventNamespaceResource;
        private const string ResourceName = "WebProject.Properties.GlobalizationResource";

        protected static ResourceManager Resource { get; set; }

        protected static ResourceManager GetResource(Assembly assembly)
        {
            if (Resource != null)
                return Resource;

            string resource = ResourceName;
            if (KPEventNamespaceResource != null)
                resource = KPEventNamespaceResource();

            if (String.IsNullOrWhiteSpace(resource))
                resource = ResourceName;

            Resource = new ResourceManager(resource, assembly);
            return Resource;
        }

        public static string GetString(string key)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(key))
                    return key;

                var resource = GetResource(BuildManager.GetGlobalAsaxType().BaseType.Assembly);
                if (resource != null)
                {
                    string keyTranslated = resource.GetString(key);
                    if (String.IsNullOrWhiteSpace(keyTranslated))
                        return String.Format("#{0}#", key);
                    return keyTranslated;
                }
            }
            catch (Exception)
            {
                throw;
            }

            return String.Empty;
        }
    }
}
