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
using System.Web.UI.WebControls;
using System.Web.UI;
using KPEntity.ORM;
using NHibernate.Criterion;
using KPGlobalization;

namespace KPComponents
{
    /// <summary>
    /// <para>Authors: Juliano Tiago Rinaldi and 
    /// Tiago Antonio Jacobi</para>
    /// </summary>
    [ToolboxData(@"<{0}:KPTabControl runat=server ></{0}:KPTabControl>")]
    public class KPTabControl : WebControl
    {
        public string DashboardTitle
        {
            get
            {
                String s = (String)ViewState["DashboardTitle"];
                return ((s == null) ? String.Empty : s);
            }
            set
            {
                ViewState["DashboardTitle"] = KPGlobalizationLanguage.GetString(value);
            }
        }
        public string DashboardUrl { get; set; }

        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            // string TabId = "KPTabControl";
            StringBuilder sbTab = new StringBuilder();

            sbTab.Append(@"<div id=""KPTabControl"">");
            sbTab.Append(@"    <ul>");
            sbTab.AppendFormat(@"        <li><a href=""#tab-1"">{0}</a></li>", String.IsNullOrEmpty(DashboardTitle) ? KPGlobalizationLanguage.GetString("Translate_Dashboard") : DashboardTitle);
            sbTab.Append(@"    </ul>");
            sbTab.Append(@"    <div class=""KPTabsContent"">");
            sbTab.AppendFormat(@"        <iframe id=""tab-1"" scrolling=""no"" frameBorder=""no"" class=""KPIframeTab"" src=""{0}"" ></iframe>", String.IsNullOrEmpty(DashboardUrl) ? "Dashboard.aspx" : DashboardUrl);
            sbTab.Append(@"    </div>");
            sbTab.Append(@"</div>");

            // sbTab.AppendFormat(@"<script type=""text/javascript""> $(function () { $(""#KPTabControl"").KPTab({}, true); }); </script>", TabId);

            writer.Write(sbTab.ToString());

            // string script = String.Format(@"<script type=""text/javascript""> $(function () { $(""#{0}"").KPTab({}, true); }); </script>", TabId);
            // ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "KPTabControl", script, false);

        }

        public static void SetTabIndex(Page page, int index)
        {
            if (page != null)
            {
                string scriptActiveTab = GetTabIndexScript(index);
                ScriptManager.RegisterClientScriptBlock(page, page.GetType(), "KPFormTabSetActive", scriptActiveTab, true);
            }
        }

        public static string GetTabIndexScript(int index)
        {
            return "$('.KPFormTab.ui-tabs').tabs('option', 'active'," + index + ");";
        }
    }
}
