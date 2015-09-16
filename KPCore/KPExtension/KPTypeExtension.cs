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
using Castle.ActiveRecord;
using KPAttributes;
using KPGlobalization;

namespace KPExtension
{
    /// <summary>
    /// <para>Authors: Juliano Tiago Rinaldi and 
    /// Tiago Antonio Jacobi</para>
    /// </summary>
    public static class KPTypeExtension
    {
        public static string GetEntityColumnNameDB(this Type type, string propertyEntity)
        {
            PropertyInfo property = type.GetProperty(propertyEntity);
            if (property != null)
            {
                PrimaryKeyAttribute[] attributeKey = property.GetCustomAttributes(
                                                        typeof(PrimaryKeyAttribute), false)
                                                            as PrimaryKeyAttribute[];
                if (attributeKey != null && attributeKey.Length > 0)
                    return attributeKey[0].Column;

                PropertyAttribute[] attributeProp = property.GetCustomAttributes(
                                                       typeof(PropertyAttribute), false)
                                                           as PropertyAttribute[];
                if (attributeProp != null && attributeProp.Length > 0)
                    return attributeProp[0].Column;
            }
            return null;
        }

        // Retornar a Propriedade PrimaryKey da Entity
        public static PropertyInfo GetEntityPrimaryKey(this Type type)
        {
            foreach (PropertyInfo property in type.GetProperties())
            {
                PrimaryKeyAttribute[] attributeKey = property.GetCustomAttributes(
                                        typeof(PrimaryKeyAttribute), false)
                                            as PrimaryKeyAttribute[];
                if (attributeKey != null && attributeKey.Length > 0)
                    return property;
            }

            return null;
        }

        // Extensão tenso que vai até o pai tentando pegar o Método
        public static MethodInfo GetMethodInheritance(this Type type, string name, Type[] types)
        {
            MethodInfo methodFind = type.GetMethod(name, types);
            if (methodFind != null)
                return methodFind;
            else
                if (type.BaseType != null)
                    return type.BaseType.GetMethodInheritance(name, types);
                else
                    return null;
        }

        // Extensão tenso que vai até o pai tentando pegar a Propriedade
        public static PropertyInfo GetPropertyInheritance(this Type type, string name)
        {
            PropertyInfo propInfo = type.GetProperty(name);
            if (propInfo != null)
                return propInfo;
            else
                if (type.BaseType != null)
                    return type.BaseType.GetPropertyInheritance(name);
                else
                    return null;
        }

        // Extensão verifica se o Tipo implementa / herda da classe
        public static bool IsSubclassOfRawGeneric(this Type generic, Type toCheck)
        {
            while (toCheck != null && toCheck != typeof(object))
            {
                var cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
                if (generic == cur)
                {
                    return true;
                }
                toCheck = toCheck.BaseType;
            }
            return false;
        }

        // Extensão que busca um KPDisplayName para traduzir a título da propriedade
        public static string GetTranslate(this PropertyInfo propertyInfo)
        {
            KPDisplayName[] attributes = propertyInfo.GetCustomAttributes(
                                                                   typeof(KPDisplayName), false)
                                                                       as KPDisplayName[];
            if (attributes != null && attributes.Length > 0)
                return KPGlobalizationLanguage.GetString(attributes[0].DisplayName);

            return propertyInfo.Name;
        }
    }
}
