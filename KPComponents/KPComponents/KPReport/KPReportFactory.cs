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

using KPBO.Parameters;
using KPComponents.KPSession;
using KPComponents.Report.KPReport;
using KPEnumerator.KPSecurity;
using System;

namespace KPComponents.KPReport
{
    /// <summary>
    /// Manages Factory Reports
    /// Generate Microsoft Report Viewer
    /// <para>Authors: Juliano Tiago Rinaldi and 
    /// Tiago Antonio Jacobi</para>
    /// </summary>
    public class KPReportFactory
    {
        /// <summary>
        /// Complex object for configuration Report
        /// </summary>
        /// <seealso cref="Report.KPReport.KPReportConfig"/>
        public KPReportConfig ReportConfig { get; private set; }

        /// <summary>
        /// Object Page
        /// </summary>
        public KPPageBase Page { get; private set; }

        /// <summary>
        /// Construtor Default
        /// </summary>
        /// <param name="page">Object Page</param>
        /// <param name="reportConfig">Object Report Configuration</param>
        public KPReportFactory(KPPageBase page, KPReportConfig reportConfig)
        {
            Page = page;
            ReportConfig = reportConfig;
        }

        /// <summary>
        /// Set configuration into Session and Redirect for Report View
        /// </summary>
        public void BuildReportViewer()
        {
            try
            {
                KPSessionHelper.SetSession(KPSessionKeyEnum.SESSION_REPORT_VIEWER, ReportConfig);
                string linkReportPage = FrwParametersHelper.GetValueParam<String>(FrwParamEnum.FRW_REPORT_PAGE, null);
                Page.Response.Redirect(linkReportPage);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
