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
using KPAttributes;
using System.ComponentModel;

namespace KPBO.Parameters
{
    /// <summary>
    /// <para>Authors: Juliano Tiago Rinaldi and 
    /// Tiago Antonio Jacobi</para>
    /// </summary>
    public enum FrwParamEnum
    {
        [TypeDescription("FRW_REPORT_PAGE")]
        [TypeValue(typeof(String))]
        [DefaultValue(@"~/ReportFrw/ReportViewer.aspx")]
        FRW_REPORT_PAGE,

        [TypeDescription("FRW_DIR_PDF_REPORT")]
        [TypeValue(typeof(System.String))]
        [DefaultValue(@"\PDF\")]
        FRW_DIR_PDF_REPORT,

        [TypeDescription("FRW_NAMESPACE_ENUM_WINDOW")]
        [TypeValue(typeof(System.String))]
        [DefaultValue(@"SpecialistEnumerator.WindowEnum")]
        FRW_NAMESPACE_ENUM_WINDOW,

        [TypeDescription("FRW_ASSEMBLY_NAME_ENTITY")]
        [TypeValue(typeof(System.String))]
        [DefaultValue(@"SpecialistEntity")]
        FRW_ASSEMBLY_NAME_ENTITY,
    }
}
