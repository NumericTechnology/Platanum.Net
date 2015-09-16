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

using KPCore.KPUtil;
using KPEnumerator.KPComponents;
using KPExtension;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization.Json;
using System.Text;

namespace KPComponents.KPJqGrid.Filter
{
    /// <summary>
    /// <para>Authors: Juliano Tiago Rinaldi and 
    /// Tiago Antonio Jacobi</para>
    /// </summary>
    public struct KPGridResults
    {
        public int page;
        public int total;
        public int records;
        public KPGridRow[] rows;
    }

    /// <summary>
    /// <para>Authors: Juliano Tiago Rinaldi and 
    /// Tiago Antonio Jacobi</para>
    /// </summary>
    public struct KPGridRow
    {
        public object id;
        public List<string> cell;
    }

    /// <summary>
    /// <para>Authors: Juliano Tiago Rinaldi and 
    /// Tiago Antonio Jacobi</para>
    /// </summary>
    public class KPJqGridRule
    {
        public string field { get; set; }
        public string op { get; set; }
        public object data { get; set; }
    }

    /// <summary>
    /// <para>Authors: Juliano Tiago Rinaldi and 
    /// Tiago Antonio Jacobi</para>
    /// </summary>
    public class KPJqGridFilter
    {
        public string groupOp { get; set; }
        public List<KPJqGridRule> rules { get; set; }

        public static KPJqGridFilter Create(string jsonJqGridFilters)
        {
            try
            {
                var serializer =
                  new DataContractJsonSerializer(typeof(KPJqGridFilter));
                System.IO.StringReader reader =
                  new System.IO.StringReader(jsonJqGridFilters);
                System.IO.MemoryStream ms =
                  new System.IO.MemoryStream(
                  KPGenericUtil.GetDefaultEncoding().GetBytes(jsonJqGridFilters));
                return serializer.ReadObject(ms) as KPJqGridFilter;
            }
            catch
            {
                return null;
            }
        }

        public static KPJqGridTypeFilterEnum GetFilter(string operJqGrid)
        {
            KPJqGridTypeFilterEnum[] filterTypeList = (KPJqGridTypeFilterEnum[])
                                                    Enum.GetValues(typeof(KPJqGridTypeFilterEnum));

            foreach (KPJqGridTypeFilterEnum filterType in filterTypeList)
            {
                if (filterType.GetTypeDescription().Equals(operJqGrid))
                    return filterType;
            }

            return KPJqGridTypeFilterEnum.Error;
        }

        public static KPJqGridTypeOrderEnum GetOrder(string orderJqGrid)
        {
            KPJqGridTypeOrderEnum[] orderTypeList = (KPJqGridTypeOrderEnum[])
                                        Enum.GetValues(typeof(KPJqGridTypeOrderEnum));

            foreach (KPJqGridTypeOrderEnum orderType in orderTypeList)
            {
                if (orderType.GetTypeDescription().Equals(orderJqGrid))
                    return orderType;
            }

            return KPJqGridTypeOrderEnum.ASC;
        }
    }
}
