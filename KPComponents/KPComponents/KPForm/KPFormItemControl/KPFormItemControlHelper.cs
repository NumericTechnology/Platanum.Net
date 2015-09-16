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
using System.ComponentModel;
using KPGlobalization;

namespace KPComponents
{
    /// <summary>
    /// <para>Authors: Juliano Tiago Rinaldi and 
    /// Tiago Antonio Jacobi</para>
    /// </summary>
    internal class KPFormItemControlHelper
    {
        public static string CreateIDField(string controlFieldID)
        {
            return String.Format("{0}_Field", controlFieldID);
        }

        public static string CreateIDLabel(string controlFieldID)
        {
            return String.Format("{0}_Label", controlFieldID);
        }

        /// <summary>
        /// Resgatar o Título do Label do Controle
        /// Get the Title of the control label
        /// </summary>
        /// <param name="captionField"></param>
        /// <param name="fieldName"></param>
        /// <param name="typeEntity"></param>
        /// <returns></returns>
        public static string GetTitleNamePropertyEntity(string captionField, string fieldName, Type typeEntity)
        {
            string titleField = captionField;
            if (String.IsNullOrEmpty(titleField))
            {
                if (String.IsNullOrEmpty(fieldName))
                {
                    titleField = "&nbsp";
                }
                else
                {
                    DisplayNameAttribute[] displayName = typeEntity.GetProperty(fieldName).GetCustomAttributes(typeof(DisplayNameAttribute), false) as DisplayNameAttribute[];
                    if (displayName.Length > 0)
                        titleField = displayName[0].DisplayName;
                    else
                        titleField = fieldName;
                }
            }

            return titleField;
        }
    }
}
