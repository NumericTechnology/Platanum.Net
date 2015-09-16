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
 
using KPEnumerator.KPGlobalization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using KPExtension;

namespace KPGlobalization
{
    /// <summary>
    /// <para>Authors: Juliano Tiago Rinaldi and 
    /// Tiago Antonio Jacobi</para>
    /// </summary>
    public class KPGlobalizationLanguageEnum : KPGlobalizationLanguage
    {
        public static string GetString(KPLanguageKeyEnum key)
        {
            try
            {
                var resource = GetResource(Assembly.GetEntryAssembly());
                if (resource != null)
                    return resource.GetString(key.GetResourceKey());
            }
            catch (Exception)
            {
                throw;
            }

            return String.Concat("$NOTFOUND_", key.GetResourceKey());
        }
    }
}
