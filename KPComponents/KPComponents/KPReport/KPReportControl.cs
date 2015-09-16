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
using System.Web.UI;
using Microsoft.Reporting.WebForms;

namespace KPComponents
{
    /// <summary>
    /// Web Control Report
    /// <para>Authors: Juliano Tiago Rinaldi and 
    /// Tiago Antonio Jacobi</para>
    /// </summary>
    /// <seealso cref="Microsoft.Reporting.WebForms.ReportViewer"/>
    /// <example>
    /// Example
    /// <code language="html" title="ASPX">
    /// &lt;KP:KPReportControl runat="server" ID="kpReportViewer" Font-Names="Verdana" Height="445px"
    ///    Font-Size="8pt" WaitMessageFont-Names="Verdana" ShowToolBar="true" WaitMessageFont-Size="14pt"&gt;
    /// &lt;/KP:KPReportControl&gt;
    /// </code>
    /// </example>
    [ToolboxData(@"<{0}:KPReportControl runat=""server"" />")]
    public class KPReportControl : ReportViewer
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        public KPReportControl()
        {
            this.Style.Add(HtmlTextWriterStyle.Height, "100%");
            this.Style.Add(HtmlTextWriterStyle.Width, "100%");
            this.ShowRefreshButton = false;
        }
    }
}
