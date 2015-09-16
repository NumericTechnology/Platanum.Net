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
using System.Web;

namespace KPComponents.KPJqGrid
{
    /// <summary>
    /// <para>Authors: Juliano Tiago Rinaldi and 
    /// Tiago Antonio Jacobi</para>
    /// </summary>
    public abstract class KPGridHandler : IHttpHandler
    {
        #region Properties
        protected string GridID { get; private set; }
        protected string Search { get; private set; }
        protected int NumberOfRows { get; private set; }
        protected int PageIndex { get; private set; }
        protected string SortColumnName { get; private set; }
        protected string SortOrderBy { get; private set; }
        #endregion


        public bool IsReusable
        {
            get { return false; }
        }

        public void ProcessRequest(HttpContext context)
        {
            HttpRequest request = context.Request;
            HttpResponse response = context.Response;

            if (!String.IsNullOrEmpty(request.QueryString.ToString()))
            {
                GridID = request["guid"];
                Search = request["_search"];

                int tryParse = 0;
                if (Int32.TryParse(request["rows"], out tryParse))
                    NumberOfRows = tryParse;

                if (Int32.TryParse(request["page"], out tryParse))
                    PageIndex = tryParse;

                SortColumnName = request["sidx"];
                SortOrderBy = request["sord"];

                response.Write(JsonData());
            }
        }

        protected abstract string JsonData();
    }


}