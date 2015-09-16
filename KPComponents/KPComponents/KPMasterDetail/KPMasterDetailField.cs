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
using System.Reflection;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using KPComponents.Asset;
using KPComponents.KPData;
using KPComponents.KPDelegate;
using KPComponents.KPJqGrid;
using KPCore.KPValidator;
using KPExtension;
using NHibernate.Criterion;
using Castle.ActiveRecord;
using KPComponents.KPSession;
using KPEnumerator.KPComponents;

namespace KPComponents
{
    /// <summary>
    /// <para>Authors: Juliano Tiago Rinaldi and 
    /// Tiago Antonio Jacobi</para>
    /// </summary>
    [ToolboxData(@"<{0}:KPMasterDetailField runat=""server"" />")]
    public class KPMasterDetailField : WebControl
    {
        #region Names ID
        private string ID_KPMasterDetailGridControl
        {
            get
            {
                return String.Format("KPMasterDetailGridControl_{0}", MasterDetailConfig.MasterDetailID);
            }
        }

        private string ID_BtnFormMasterDetail
        {
            get
            {
                return String.Format("BtnFormMasterDetail_{0}", MasterDetailConfig.MasterDetailID);
            }
        }
        #endregion

        public KPFormBaseControl FormControl { get; private set; }

        public KPFormMasterDetailModel MasterDetailConfig { get; private set; }

        public KPFormItemMasterDetail FormItem { get; private set; }

        private KPMasterDetailGridControl MasterDetailGridControl
        {
            get
            {
                EnsureChildControls();
                KPMasterDetailGridControl obj = this.BetterFindControl<KPMasterDetailGridControl>(ID_KPMasterDetailGridControl);
                return obj;
            }
        }

        public KPMasterDetailField(KPFormBaseControl formControl, KPFormItemMasterDetail formItem, KPFormMasterDetailModel masterDetailConfig)
        {
            FormControl = formControl;
            FormItem = formItem;

            MasterDetailConfig = masterDetailConfig;
            EnsureChildControls();
        }

        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            HtmlGenericControl htmlGenWindow = new HtmlGenericControl("div");
            htmlGenWindow.Attributes.Add("class", "KPMasterDetailField");
            htmlGenWindow.Style.Add(HtmlTextWriterStyle.Width, this.MasterDetailConfig.WidthGrid + "px");
            htmlGenWindow.Style.Add(HtmlTextWriterStyle.Height, this.MasterDetailConfig.HeightGrid + "px");

            KPMasterDetailGridControl msaterDetailGridControl =
                new KPMasterDetailGridControl(FormControl, MasterDetailConfig)
                {
                    ID = ID_KPMasterDetailGridControl
                };

            htmlGenWindow.Controls.Add(msaterDetailGridControl);

            this.Controls.Add(htmlGenWindow);
        }

        private void btnFormMasterDetail_Click(object sender, EventArgs e)
        {
            KPPageBase pageBase = this.Page as KPPageBase;
            if (MasterDetailGridControl != null && MasterDetailGridControl.KPGridControl != null && pageBase != null)
            {
                if (!String.IsNullOrEmpty(MasterDetailGridControl.KPGridControl.PageFormUrl))
                {
                    object entitySelected = MasterDetailGridControl.KPGridControl.GetSelectedEntity();

                    KPSessionHelper.SetSessionData(new KPSessionData(MasterDetailGridControl.KPGridControl, Activator.CreateInstance(MasterDetailGridControl.KPGridControl.TypeEntity), KPFormStateEnum.New), pageBase.SessionPageID);
                    string titlePage = "Carregando ...";

                    string pageFormUrlSession = String.Format("{0}?parentID={1}", MasterDetailGridControl.KPGridControl.PageFormUrl, pageBase.SessionPageID);
                    pageBase.KPWindow(titlePage, pageFormUrlSession, true, 
                                    MasterDetailGridControl.KPGridControl.WidthForm, 
                                            MasterDetailGridControl.KPGridControl.HeightForm);
                }
            }
        }
    }
}
