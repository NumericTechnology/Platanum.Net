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
using System.Web.UI;
using System.Web.UI.WebControls;
using KPComponents.KPSecurity;
using KPComponents.KPSession;
using KPAttributes;
using KPCore.KPSecurity;
using KPEntity.ORM;
using NHibernate.Criterion;
using KPCore.KPException;

namespace KPComponents
{
    /// <summary>
    /// Page with identify Enum Generic Type
    /// <para>Authors: Juliano Tiago Rinaldi and 
    /// Tiago Antonio Jacobi</para>
    /// </summary>
    public class KPPage : KPPageBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="pageEnumForm">Enum Page Form Key</param>
        public KPPage(Enum pageEnumForm)
            : base(pageEnumForm)
        {

        }

        protected PagePermission PagePermission
        {
            get
            {
                object o = ViewState["PagePermission"];
                return o == null ? null : (PagePermission)o;
            }
            private set { ViewState["PagePermission"] = value; }
        }

        protected ComponentPermission[] ComponentsPermission
        {
            get
            {
                object o = ViewState["ComponentsPermission"];
                return o == null ? null : (ComponentPermission[])o;
            }
            private set { ViewState["ComponentsPermission"] = value; }
        }

        /// <summary>
        /// Close Current Page
        /// </summary>
        public void ClosePageForm()
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, Page.GetType(), "CloseWindow",
            String.Format(@"window.parent.closeWindow(""{0}"");",
                            new System.IO.FileInfo(this.Page.Request.CurrentExecutionFilePath).Name), true);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!IsPostBack)
            {
                if (SecuritySession != null)
                {
                    string enumName = PageEnum.ToString();
                    PagePermission = SecuritySession.GetPagePermission(PageEnum);
                    ComponentsPermission = SecuritySession.GetPageComponentsPermission(PageEnum);
                    if (PagePermission != null)
                    {
                        if (!PagePermission.IsPreview)
                        {
                            string errorPermission = String.Empty;
                            try
                            {
                                FrwWindow frwWindow = FrwWindow.TryFind(PagePermission.PageId);
                                if (frwWindow != null)
                                {
                                    errorPermission = String.Format("Permissão Negada! Você não tem acesso a tela {0} - {1}", frwWindow.WindowTitle, frwWindow.WindowDescription);
                                }
                            }
                            catch
                            {
                                errorPermission = "Permissão Negada!";
                            }
                            throw new KPExceptionSecurity(errorPermission, PagePermission);
                        }
                    }
                }
            }
        }
    }
}