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

using KPComponents.KPComponents.KPJqGrid;
using KPComponents.KPDelegate;
using KPComponents.KPJqGrid;
using KPEnumerator.KPComponents;
using KPGlobalization;
using NHibernate.Criterion;
using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace KPComponents
{
    /// <summary>
    /// <para>Authors: Juliano Tiago Rinaldi and 
    /// Tiago Antonio Jacobi</para>
    /// </summary>
    public class KPZoomControl : WebControl
    {
        public event KPZoomClick KPEventZoomClickOK;
        public event KPZoomClick KPEventZoomClickClose;

        private HiddenField HiddenKey;
        private KPJqGridControl JqGridControl;
        private string tableID;
        private string pagerID;
        private KPItemModelCollection KPColumnsModel { get; set; }
        private KPFormZoomModel ZoomConfig { get; set; }

        public KPZoomControl(KPFormZoomModel KPFormZoomConfig, KPItemModelCollection KPcolumnsModel)
        {
            ZoomConfig = KPFormZoomConfig;
            KPColumnsModel = KPcolumnsModel;
            Visible = false;
        }

        protected override void OnInit(EventArgs e)
        {
            if (KPColumnsModel != null)
            {

                HtmlGenericControl htmlWindow = new HtmlGenericControl("div");
                htmlWindow.Attributes.Add("class", "KPWindow KPFormWindow KPFormZoom");

                #region Hidden Key
                HiddenKey = new HiddenField();
                HiddenKey.ID = String.Format("hiddenKey_{0}", ZoomConfig.ZoomID);
                #endregion Hidden Key

                htmlWindow.Controls.Add(GenerateTitle(ZoomConfig.WindowTitle));
                htmlWindow.Controls.Add(HiddenKey);

                #region Zoom Content
                tableID = String.Concat(this.ClientID, "GridTable");
                pagerID = String.Concat(this.ClientID, "GridPager");

                HtmlGenericControl htmlContent = new HtmlGenericControl("div");
                htmlContent.Attributes.Add("class", "Content");

                HtmlGenericControl htmlTable = new HtmlGenericControl("table");
                htmlTable.Attributes.Add("id", tableID);

                HtmlGenericControl htmlPager = new HtmlGenericControl("div");
                htmlPager.Attributes.Add("id", pagerID);

                htmlContent.Controls.Add(htmlTable);
                htmlContent.Controls.Add(htmlPager);

                #endregion

                htmlWindow.Controls.Add(htmlContent);

                #region Zoom Action Bar
                HtmlGenericControl htmlKPActionBar = new HtmlGenericControl("div");
                htmlKPActionBar.Attributes.Add("class", "KPActionBar");

                Button btnOK = new Button()
                {
                    ID = String.Format("KPGridControlBtnOK_{0}", ZoomConfig.ZoomID),
                    Text = KPGlobalizationLanguage.GetString("KPComponents_Select"),
                    CssClass = "KPZoomSelectButton"
                };
                btnOK.Attributes.Add("onclick", "return  validateGridSelection(event, '" + tableID + "', true, true);");
                btnOK.Click += new EventHandler(btnOK_Click);

                htmlKPActionBar.Controls.Add(btnOK);
                #endregion

                htmlWindow.Controls.Add(htmlKPActionBar);

                this.Controls.Add(htmlWindow);

            }
            base.OnInit(e);
        }

        private HtmlGenericControl GenerateTitle(string title)
        {
            HtmlGenericControl htmlKPFormTitle = new HtmlGenericControl("div");
            htmlKPFormTitle.Attributes.Add("class", "KPFormTitle");

            HtmlGenericControl htmlKPTitleLabel = new HtmlGenericControl("p");
            htmlKPTitleLabel.Attributes.Add("class", "KPTitleLabel");
            htmlKPTitleLabel.InnerText = title;

            Button htmlKPCloseButton = new Button();
            htmlKPCloseButton.Attributes.Add("class", "KPCloseButton");
            htmlKPCloseButton.Click += new EventHandler(btnClose_Click);

            htmlKPFormTitle.Controls.Add(htmlKPTitleLabel);
            htmlKPFormTitle.Controls.Add(htmlKPCloseButton);

            return htmlKPFormTitle;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (HiddenKey != null && !String.IsNullOrEmpty(HiddenKey.Value))
            {
                if (KPEventZoomClickOK != null)
                {
                    Visible = false;
                    KPEventZoomClickOK(HiddenKey.Value);

                    ScriptManager.RegisterClientScriptBlock(this.Page, Page.GetType(), "CloseZoomWindow",
                        @"closeZoomWindow("""");setLoading(false);", true);
                }
            }
            else
            {
                string message = KPGlobalizationLanguage.GetString("KPComponents_MessageSelectTableValue");
                ScriptManager.RegisterClientScriptBlock(this.Page, Page.GetType(), "SelectZoomWindowMessage",
                    String.Format("alertBox('{0}');", message), true);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Visible = false;
            if (KPEventZoomClickClose != null)
            {
                KPEventZoomClickClose(HiddenKey.Value);
            }

            ScriptManager.RegisterClientScriptBlock(this.Page, Page.GetType(), "CloseZoomWindow",
                @"closeZoomWindow("""");", true);
        }

        protected override void Render(HtmlTextWriter writer)
        {
            base.Render(writer);
            if (KPColumnsModel != null)
            {
                ICriterion filter = null;
                Order order = null;
                if (ZoomConfig.KPCriterionFilterDelegate != null)
                    filter = ZoomConfig.KPCriterionFilterDelegate();
                if (ZoomConfig.KPOrderDelegate != null)
                    order = ZoomConfig.KPOrderDelegate();

                JqGridControl = new KPJqGridControl(ZoomConfig.FieldReturnId, HiddenKey, ZoomConfig.TypeEntity, KPColumnsModel,
                    KPJqGridDoubleClickActionEnum.GridZoomSelect, filter, order);
                JqGridControl.PropertyCompanyEntity = ZoomConfig.PropertyCompanyEntity;

                string scriptGrid = JqGridControl.Build(pagerID, tableID, this.Page as KPPageBase);
                ScriptManager.RegisterClientScriptBlock(this.Page, Page.GetType(), "GridBuildScript", scriptGrid, false);
                writer.Write(scriptGrid);
            }
        }
    }
}
