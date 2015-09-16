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
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using KPComponents.KPSecurity;

namespace KPComponents
{
    /// <summary>
    /// Master Page base KPFramework
    /// <para>Authors: Juliano Tiago Rinaldi and 
    /// Tiago Antonio Jacobi</para>
    /// </summary>
    public class KPMasterPage : System.Web.UI.MasterPage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public KPMasterPage()
        {
            KPFrwSecurity.SecurityFramework();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!IsPostBack)
            {
                if (!this.Page.ClientScript.IsClientScriptBlockRegistered("cssBasicKPComponents"))
                {
                    Assembly assemblyKPDocuments = Assembly.Load("KPComponents");
                    Stream cssStream = assemblyKPDocuments.GetManifestResourceStream("KPComponents.Asset.Styles.KPComponentsBasic.css");
                    StreamReader sr = new StreamReader(cssStream);
                    String style = String.Format(@"<style type=""text/css"">{0}</style>", sr.ReadToEnd());
                    this.Page.ClientScript.RegisterStartupScript(this.GetType(), "cssBasicKPComponents", style);
                }
            }
        }
    }
}
