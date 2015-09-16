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
using System.Web.UI.HtmlControls;
using KPComponents.KPForm;
using System.Web.UI;

namespace KPComponents
{
    /// <summary>
    /// <para>Authors: Juliano Tiago Rinaldi and 
    /// Tiago Antonio Jacobi</para>
    /// </summary>
    public class KPFormTabControl : HtmlGenericControl
    {
        private KPFormTabModelCollection KPFormTabModels { get; set; }
        private KPFormBaseControl KPFormControl { get; set; }

        public KPFormTabControl(KPFormBaseControl formBaseControl, KPFormTabModelCollection formTabModels) : base("div")
        {
            this.Attributes.Add("class", "KPFormTab");
            this.KPFormTabModels = formTabModels;
            this.KPFormControl = formBaseControl;
        }

        protected override void CreateChildControls()
        {
            base.CreateChildControls();
            
            HtmlGenericControl kpUlTabs = new HtmlGenericControl("ul");
            HtmlGenericControl kpTabsContent = new HtmlGenericControl("div");
            kpTabsContent.Attributes.Add("class", "KPFormTabsContent");

            int indexId = 0;

            foreach (KPFormTabModel tabModel in KPFormTabModels)
            {
                indexId++;
                
                kpUlTabs.Controls.Add(this.CreateTabHeader(tabModel, indexId));
                kpTabsContent.Controls.Add(this.CreateTabContent(tabModel, indexId));
                
            }
            
            this.Controls.Add(kpUlTabs);
            this.Controls.Add(kpTabsContent);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            EnsureChildControls();

            String script = @"$('.KPFormTab').tabs();";
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "tabCreator", script, true);
        }

        private HtmlGenericControl CreateTabHeader(KPFormTabModel tabModel, int index)
        {
            HtmlGenericControl liTab = new HtmlGenericControl("li");

            HtmlGenericControl linkTab = new HtmlGenericControl("a");
            linkTab.Attributes.Add("href", this.GetTabId(index, true));
            linkTab.InnerText = tabModel.Title;

            liTab.Controls.Add(linkTab);

            return liTab;
        }

        private HtmlGenericControl CreateTabContent(KPFormTabModel tabModel, int index)
        {
            HtmlGenericControl divTab = new HtmlGenericControl("div");
            divTab.Attributes.Add("id", GetTabId(index, false));

            if (tabModel.KPColumnsModel != null)
            {
                KPFormItemFactory formItemFactory = KPFormControl.CreateFormItemFactory(tabModel.KPColumnsModel, index);
                if (formItemFactory.ControlsFieldset.Count > 0)
                {
                    divTab.Controls.Add(formItemFactory.ItemsFieldset);
                }
            }

            return divTab;
        }

        private String GetTabId(int index, bool addSelector)
        {
            return ((addSelector?"#":"") + "tabs-" + index);
        }

        protected override void Render(HtmlTextWriter writer)
        {
            base.Render(writer);

            String script = @"<script type=""text/javascript"">
                                $(document).ready(function () {
                                    $('.KPFormControl').css('overflow-y','hidden');
                                    $('.KPFormTab').tabs();
                                    $('.KPFormControl').css('overflow-y','auto');
                                });
                            </script>";
            writer.Write(script);
        }
    }
}
