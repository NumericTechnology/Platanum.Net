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
using System.Reflection;
using System.Text;
using KPAttributes;
using KPEnumerator.KPGlobalization;

namespace KPExtension
{
    /// <summary>
    /// <para>Authors: Juliano Tiago Rinaldi and 
    /// Tiago Antonio Jacobi</para>
    /// </summary>
    public static class KPEnumExtension
    {
        public static string GetTypeDescription(this Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            TypeDescription[] attributes =
                (TypeDescription[])fi.GetCustomAttributes(
                typeof(TypeDescription),
                false);

            if (attributes != null &&
                attributes.Length > 0)
                return attributes[0].Value;
            else
                return value.ToString();
        }

        public static KPLanguageKeyEnum GetTranslatableDescription(this Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            TranslatableDescription[] attributes =
                (TranslatableDescription[])fi.GetCustomAttributes(
                typeof(TranslatableDescription),
                false);

            if (attributes != null &&
                attributes.Length > 0)
                return attributes[0].Value;
            else
                return (KPLanguageKeyEnum) value;
        }

        public static object GetTypeValue(this Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            TypeValue[] attributes =
                (TypeValue[])fi.GetCustomAttributes(
                typeof(TypeValue),
                false);

            if (attributes != null && attributes.Length > 0)
            {
                return attributes[0].Value;
            }
            else
            {
                return value.ToString();
            }
        }

        public static string[] GetMaskCharacteresEnum(this Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            MaskCharacteresEnum[] attributes =
                (MaskCharacteresEnum[])fi.GetCustomAttributes(
                typeof(MaskCharacteresEnum),
                false);

            if (attributes != null &&
                attributes.Length > 0)
                return attributes[0].Characteres;
            else
                return new string[] { };
        }

        public static string GetResourceKey(this Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            ResourceKey[] attributes =
                (ResourceKey[])fi.GetCustomAttributes(
                typeof(ResourceKey),
                false);

            if (attributes != null &&
                attributes.Length > 0)
                return attributes[0].Key;
            else
                return value.ToString();
        }

        public static ChartData GetChartData(this Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            ChartData[] attributes =
                (ChartData[])fi.GetCustomAttributes(
                typeof(ChartData),
                false);

            if (attributes != null &&
                attributes.Length > 0)
                return attributes[0];
            else
                return null;
        }


    }
}
