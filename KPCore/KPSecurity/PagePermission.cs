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
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace KPCore.KPSecurity
{
    /// <summary>
    /// <para>Authors: Juliano Tiago Rinaldi and 
    /// Tiago Antonio Jacobi</para>
    /// </summary>
    [Serializable]
    public class PagePermission
    {
        private const string UNKNOW = "_UNKNOW_";

        public PagePermission(int pageId, string pageEnum, string pageTitle)
        {
            PageId = pageId;
            PageTitle = pageTitle;
            if (PageId != 0)
                ExistPage = true;
            if (!String.IsNullOrWhiteSpace(pageEnum))
                PageEnum = pageEnum;
            else
                PageEnum = PagePermission.UNKNOW;
        }

        public int PageId { get; private set; }
        public bool ExistPage { get; private set; }
        public string PageEnum { get; private set; }
        public string PageTitle { get; private set; }
        public bool IsReadOnly { get; set; }
        public bool IsPreview { get; set; }
    }
}
