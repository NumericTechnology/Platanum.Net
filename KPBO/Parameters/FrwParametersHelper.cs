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

using KPAttributes;
using KPEntity.ORM;
using KPEnumerator.KPComponents;
using NHibernate.Criterion;
using System;
using System.Globalization;
using System.Linq;

namespace KPBO.Parameters
{
    /// <summary>
    /// <para>Authors: Juliano Tiago Rinaldi and 
    /// Tiago Antonio Jacobi</para>
    /// </summary>
    public class FrwParametersHelper
    {
        public static T GetValueParam<T>(Enum enumeratorParam, FrwCompany company = null)
        {
            try
            {
                object returnParamValue = null;
                FrwParamValue paramValue = GetParamValue(enumeratorParam, company);
                if (paramValue != null)
                {
                    KPDataTypeEnum typeEnum = (KPDataTypeEnum)paramValue.objIdParam.objIdDatatype.IdDatatype;
                    switch (typeEnum)
                    {
                        case KPDataTypeEnum.INT:
                            returnParamValue = Convert.ToInt32(paramValue.Value);
                            break;
                        case KPDataTypeEnum.STRING:
                            returnParamValue = paramValue.Value;
                            break;
                        case KPDataTypeEnum.DECIMAL:
                            returnParamValue = Convert.ToDecimal(paramValue.Value, CultureInfo.InvariantCulture);
                            break;
                        case KPDataTypeEnum.BOOL:
                            returnParamValue = Convert.ToBoolean(paramValue.Value);
                            break;
                        case KPDataTypeEnum.ARRAY:
                            throw new Exception("Ainda não implementado - Verificar como será tratado.");
                    }
                }
                if (returnParamValue != null)
                    return (T)returnParamValue;

                return default(T);
            }
            catch
            {
                throw;
            }
        }

        public static void UpdateDefaultParamValueFrw()
        {
            FrwParamValue[] paramsValue = FrwParamValue.FindAll(Expression.IsNull("objIdCompany"));
            Conjunction conj = new Conjunction();
            conj.Add(Expression.Not(Expression.In("IdParam", paramsValue.Select(x => x.objIdParam.IdParam).ToArray())));
            conj.Add(Expression.Eq("IsCompanyRequired", false));
            FrwParam[] paramsNew = FrwParam.FindAll(conj);
            string[] paramsNames = paramsNew.Select(x => x.IdParam).ToArray();
            FrwParamEnum enumTryParse;
            foreach (string item in paramsNames)
            {
                if (Enum.TryParse<FrwParamEnum>(item, out enumTryParse))
                    FrwParametersHelper.UpdateDefaultParamValue(enumTryParse);
            }
        }

        protected static void UpdateDefaultParamValue(Enum enumeratorParam, FrwCompany company = null)
        {
            try
            {
                FrwParamValue paramValue = FrwParametersHelper.GetParamValue(enumeratorParam, company);
                if (paramValue == null)
                {
                    FrwParam param = GetParam(enumeratorParam);
                    paramValue = new FrwParamValue()
                    {
                        objIdCompany = company,
                        objIdParam = param,
                        Value = param.DefaultValue
                    };
                    paramValue.Save();
                }
            }
            catch
            {
                throw;
            }
        }

        public static FrwParamValue GetParamValue(Enum enumeratorParam, FrwCompany company = null)
        {
            try
            {
                FrwParam param = GetParam(enumeratorParam);
                if (param == null)
                    throw new Exception(String.Format("Não foi encontrado o parâmetro '{0}' no banco de dados.", enumeratorParam.ToString()));

                if (param.IsCompanyRequired.Value)
                {
                    if (company == null)
                        throw new Exception(String.Format("O parâmetro enumerador {0} obrigatoriamente necessita de uma empresa.", enumeratorParam.ToString()));
                }
                else
                    company = null;

                Conjunction conj = new Conjunction();
                conj.Add(Expression.Eq("objIdParam", param));
                if (company != null)
                    conj.Add(Expression.Eq("objIdCompany", company));

                FrwParamValue paramValue = FrwParamValue.FindFirst(conj);

                return paramValue;
            }
            catch
            {
                throw;
            }
        }

        private static FrwParam GetParam(Enum enumeratorParam)
        {
            try
            {
                Type enumType = enumeratorParam.GetType();

                object[] typeDescriptionArray = enumType.GetField(enumeratorParam.ToString()).GetCustomAttributes(typeof(TypeDescription), true);
                object[] typeValue = enumType.GetField(enumeratorParam.ToString()).GetCustomAttributes(typeof(TypeValue), true);

                if (typeDescriptionArray != null && typeDescriptionArray.Length > 0 && typeValue != null && typeValue.Length > 0)
                {
                    FrwParam param = FrwParam.Find(((TypeDescription)typeDescriptionArray[0]).Value);

                    if (param == null)
                        throw new Exception(String.Format("Não foi encontrado o parâmetro '{0}' no banco de dados.", typeDescriptionArray[0].ToString()));

                    return param;
                }
                else
                    throw new Exception(String.Format("Não foi encontrado atributo [TypeDescription, TypeValue] no enumerador '{0}'.", enumeratorParam.ToString()));
            }
            catch
            {
                throw;
            }
        }
    }
}
