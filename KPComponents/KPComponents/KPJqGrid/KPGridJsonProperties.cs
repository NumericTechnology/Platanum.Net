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

using KPComponents.KPJqGrid.Filter;
using KPComponents.KPJson;
using KPComponents.KPSession;
using KPEnumerator.KPComponents;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;

namespace KPComponents.KPJqGrid
{
    /// <summary>
    /// <para>Authors: Juliano Tiago Rinaldi and 
    /// Tiago Antonio Jacobi</para>
    /// </summary>
    public class KPGridJsonProperties
    {
        private string fieldFilterJason;
        private string orderJason;


        public KPGridJsonProperties(bool isSearch, int page, int rows,
                                    string propertyOrder, string orderTypeJason,
                                    string filterJson,  KPSessionJQGrid sessionJQGrid, string propertyCompany,
                                    ICriterion initialFilter, Order initialOrder, object[] colModel)
        {
            IsSearch = isSearch;
            Page = page;
            Rows = rows;
            PropertyOrder = propertyOrder;
            OrderTypeJson = orderTypeJason;
            User = sessionJQGrid.SecuritySession.Login;
            Company = sessionJQGrid.SecuritySession.FrwCompany;
            PropertyCompanyEntity = propertyCompany;
            FilterJson = filterJson;
            ColModel = Array.ConvertAll<object, string>(colModel, Convert.ToString);
            List<JqGridColumnCustom> jqGridColumnCustomList = new List<JqGridColumnCustom>();
            foreach (string item in ColModel)
                jqGridColumnCustomList.Add(KPJsonJqGrid.GetColumnCustom(item));
            ColumnsCustom = jqGridColumnCustomList.ToArray();
            InitialFilter = initialFilter;
            InitialOrder = initialOrder;
        }

        //_search
        public bool IsSearch { get; set; }

        //page
        public int Page { get; set; }

        //rows
        public int Rows { get; set; }

        /// <summary>
        /// Column Order
        /// </summary>
        public string PropertyOrder { get; set; }

        /// <summary>
        /// Type Order
        /// </summary>
        public string OrderTypeJson
        {
            get { return this.orderJason; }
            set
            {
                OrderType = KPJqGridFilter.GetOrder(value);
                this.orderJason = value;
            }
        }


        /// <summary>
        /// Type Order Enum
        /// </summary>
        public KPJqGridTypeOrderEnum OrderType { get; set; }

        /// <summary>
        /// User logged
        /// </summary>
        public string User { get; set; }

        /// <summary>
        /// Company logged
        /// </summary>
        public int? Company { get; set; }

        public string PropertyCompanyEntity { get; set; }

        //filters
        public string FilterJson
        {
            get { return this.fieldFilterJason; }
            set
            {
                JqGridFilter = KPJqGridFilter.Create(value);
                this.fieldFilterJason = value;
            }
        }

        public ICriterion InitialFilter { get; set; }

        public Order InitialOrder { get; set; }

        public KPJqGridFilter JqGridFilter { get; set; }

        //colModel
        public string[] ColModel { get; set; }

        public int TotalFoundRegisters { get; set; }

        internal JqGridColumnCustom[] ColumnsCustom { get; private set; }
    }
}
