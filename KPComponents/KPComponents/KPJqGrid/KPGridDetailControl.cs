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

using Castle.ActiveRecord;
using KPComponents.KPComponents.KPJqGrid;
using KPComponents.KPDelegate;
using KPComponents.KPEvent;
using KPComponents.KPForm;
using KPComponents.KPJqGrid;
using KPComponents.KPSession;
using KPCore.KPException;
using KPCore.KPSecurity;
using KPCore.KPUtil;
using KPEntity.ORM;
using KPEnumerator.KPComponents;
using KPEnumerator.KPEntity;
using KPExtension;
using KPGlobalization;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace KPComponents
{
    /// <summary>
    /// <para>Authors: Juliano Tiago Rinaldi and 
    /// Tiago Antonio Jacobi</para>
    /// </summary>
    [DefaultProperty("Text")]
    [ToolboxData(@"<{0}:KPGridDetailControl runat=""server"" ></{0}:KPGridDetailControl>")]
    public class KPGridDetailControl : KPGridBaseControl
    {

        #region Properties
        [DefaultValue(true)]
        internal bool EnableNewDetail { get; set; }
        [DefaultValue(true)]
        internal bool EnableEditDetail { get; set; }
        [DefaultValue(true)]
        internal bool EnableDeleteDetail { get; set; }
        #endregion

        #region ID Controls
        private string ID_BtnNewDetail
        {
            get
            {
                return String.Format("ID_BtnNewDetail_{0}", this.ID);
            }
        }

        private string ID_BtnEditDetail
        {
            get
            {
                return String.Format("ID_BtnEditDetail_{0}", this.ID);
            }
        }

        private string ID_BtnDeleteDetail
        {
            get
            {
                return String.Format("ID_BtnDeleteDetail_{0}", this.ID);
            }
        }
        #endregion

        #region Constructor
        public KPGridDetailControl(KPFormMasterDetailModel masterDetailConfig)
            : base()
        {
            MasterDetailConfig = masterDetailConfig;
            EnableDelete = false;
            EnableNew = false;
            EnableEdit = false;
            EnableNewDetail = true;
            EnableEditDetail = true;
            EnableDeleteDetail = true;
            TypeEntityNamespace = MasterDetailConfig.TypeEntityDetailNamespace;
            RowNum = MasterDetailConfig.RowNum;
            PrimaryKey = MasterDetailConfig.PrimaryKeyDetail;
            PropertyCompanyEntity = MasterDetailConfig.PropertyCompanyEntity;
            Width = MasterDetailConfig.WidthGrid;
            Height = MasterDetailConfig.HeightGrid;
            WidthForm = MasterDetailConfig.WidthFormDetail;
            HeightForm = MasterDetailConfig.HeightFormDetail;
            PageFormUrl = MasterDetailConfig.PageFormUrl;
        }
        #endregion

        #region Methods
        protected override void CreateChildOthersControls(KPPageBase pageBase, PagePermission pagePermission, UpdatePanel upnFormAjax)
        {
            if (EnableNewDetail && !pagePermission.IsReadOnly)
            {
                ComponentPermission componentPermission = pageBase.SecuritySession.GetComponentPermission(pageBase.PageEnum, "ACTION_MASTER_GRID_ADD");
                if (componentPermission.IsVisible)
                {
                    Button btnNewDetail = new Button() { ID = ID_BtnNewDetail, Text = KPGlobalizationLanguage.GetString("KPComponents_New"), CssClass = "KPGridNewDetailButton" };
                    btnNewDetail.Enabled = componentPermission.IsEnabled;
                    btnNewDetail.Attributes.Add("onclick", "setLoading(true);");

                    if (MasterDetailConfig != null)
                    {
                        if (!String.IsNullOrWhiteSpace(MasterDetailConfig.HelpToolTipNew))
                            btnNewDetail.Attributes.Add("title", KPGlobalizationLanguage.GetString(MasterDetailConfig.HelpToolTipNew));
                    }

                    btnNewDetail.Click += new EventHandler(btnNewDetail_Click);
                    upnFormAjax.ContentTemplateContainer.Controls.Add(btnNewDetail);
                }
            }

            if (EnableEditDetail && !pagePermission.IsReadOnly)
            {
                ComponentPermission componentPermission = pageBase.SecuritySession.GetComponentPermission(pageBase.PageEnum, "ACTION_MASTER_GRID_EDIT");
                if (componentPermission.IsVisible)
                {
                    Button btnEditDetail = new Button() { ID = ID_BtnEditDetail, Text = KPGlobalizationLanguage.GetString("KPComponents_Edit"), CssClass = "KPGridEditDetailButton" };
                    btnEditDetail.Enabled = componentPermission.IsEnabled;
                    btnEditDetail.Attributes.Add("onclick", "return validateGridSelection(event, '" + ID_Table + "', true, true);");
                    if (MasterDetailConfig != null)
                    {
                        if (!String.IsNullOrWhiteSpace(MasterDetailConfig.HelpToolTipEdit))
                            btnEditDetail.Attributes.Add("title", KPGlobalizationLanguage.GetString(MasterDetailConfig.HelpToolTipEdit));
                    }
                    btnEditDetail.Click += new EventHandler(btnEditDetail_Click);
                    upnFormAjax.ContentTemplateContainer.Controls.Add(btnEditDetail);
                }
            }
            if (EnableDeleteDetail && !pagePermission.IsReadOnly)
            {
                ComponentPermission componentPermission = pageBase.SecuritySession.GetComponentPermission(pageBase.PageEnum, "ACTION_MASTER_GRID_DELETE");
                if (componentPermission.IsVisible)
                {
                    Button btnDeleteDetail = new Button() { ID = ID_BtnDeleteDetail, Text = KPGlobalizationLanguage.GetString("KPComponents_Delete"), CssClass = "KPGridDeleteDetailButton" };
                    btnDeleteDetail.Enabled = componentPermission.IsEnabled;
                    string question = KPGlobalizationLanguage.GetString("KPComponents_QuestionDeleteReg");
                    btnDeleteDetail.Attributes.Add("onclick", String.Format("return (validateGridSelection(event, '{0}', true, false) ? confirmBox('{1}', 1, true, true) : false);", ID_Table, question));
                    if (MasterDetailConfig != null)
                    {
                        if (!String.IsNullOrWhiteSpace(MasterDetailConfig.HelpToolTipDelete))
                            btnDeleteDetail.Attributes.Add("title", KPGlobalizationLanguage.GetString(MasterDetailConfig.HelpToolTipDelete));
                    }
                    btnDeleteDetail.Click += new EventHandler(btnDeleteDetail_Click);
                    upnFormAjax.ContentTemplateContainer.Controls.Add(btnDeleteDetail);
                }
            }
        }

        internal object GetSelectedEntityDetail()
        {
            try
            {
                if (String.IsNullOrEmpty(HiddenKey.Value))
                    return null;

                KPPageBase pageBase = this.Page as KPPageBase;
                DetailSession detailSession = KPSessionHelper.GetSessionMasterDetailList(pageBase.SessionPageID, this.MasterDetailConfig.MasterDetailID);
                if (detailSession != null)
                {
                    var propKey = TypeEntity.GetProperty(this.PrimaryKey);
                    foreach (var item in detailSession.Entities)
                    {
                        if (propKey.GetValue(item, null).ToString() == HiddenKey.Value)
                            return item;
                    }
                }

                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region EventsDetail

        private void btnNewDetail_Click(object sender, EventArgs e)
        {
            KPPageBase pageBase = this.Page as KPPageBase;
            if (!String.IsNullOrEmpty(PageFormUrl) && pageBase != null)
            {
                ActiveRecordBase entityDetail = Activator.CreateInstance(TypeEntity) as ActiveRecordBase;
                if (entityDetail != null &&
                        this.NamingContainer != null &&
                            this.NamingContainer is KPFormControl)
                {
                    KPFormBaseControl formControl = this.NamingContainer as KPFormControl;
                    entityDetail = SetDetailsKeys(formControl.DataSourceAltered as ActiveRecordBase, entityDetail, MasterDetailConfig.KeyFieldsConfig);
                }

                if (MasterDetailConfig.KPEventBeforeNewDetailDelegate != null)
                {
                    KPButtonEventsArgs eArgs = new KPButtonEventsArgs();
                    MasterDetailConfig.KPEventBeforeNewDetailDelegate(entityDetail, eArgs);
                    if (eArgs.Cancel)
                    {
                        pageBase.SetLoading(false);
                        return;
                    }
                }

                KPSessionHelper.SetSessionData(new KPSessionDetailData(this, entityDetail, KPFormStateEnum.New, this.MasterDetailConfig.MasterDetailID), pageBase.SessionPageID);
                string titlePage = "Carregando ...";
                string pageFormUrlSession = String.Format("{0}?parentID={1}", this.PageFormUrl, pageBase.SessionPageID);
                pageBase.KPWindow(titlePage, pageFormUrlSession, true, WidthForm, HeightForm);
            }
        }

        private void btnEditDetail_Click(object sender, EventArgs e)
        {
            KPPageBase pageBase = this.Page as KPPageBase;
            if (this.HiddenKey != null && !String.IsNullOrEmpty(HiddenKey.Value) && !String.IsNullOrEmpty(PageFormUrl) && pageBase != null)
            {
                object entitySelected = GetSelectedEntityDetail();

                if (MasterDetailConfig.KPEventBeforeEditDetailDelegate != null)
                {
                    KPButtonEventsArgs eArgs = new KPButtonEventsArgs();
                    MasterDetailConfig.KPEventBeforeEditDetailDelegate(entitySelected, eArgs);
                    if (eArgs.Cancel)
                    {
                        pageBase.SetLoading(false);
                        return;
                    }
                }

                KPSessionHelper.SetSessionData(new KPSessionDetailData(this, entitySelected, KPFormStateEnum.Edit, this.MasterDetailConfig.MasterDetailID), pageBase.SessionPageID);
                string titlePage = "Carregando ...";
                string pageFormUrlSession = String.Format("{0}?parentID={1}", this.PageFormUrl, pageBase.SessionPageID);
                pageBase.KPWindow(titlePage, pageFormUrlSession, true, WidthForm, HeightForm);
            }
        }

        private void btnDeleteDetail_Click(object sender, EventArgs e)
        {
            KPPageBase pageBase = this.Page as KPPageBase;
            if (this.HiddenKey != null && !String.IsNullOrEmpty(HiddenKey.Value) && !String.IsNullOrEmpty(PageFormUrl) && pageBase != null)
            {
                object entitySelected = GetSelectedEntityDetail();

                if (MasterDetailConfig.KPEventBeforeDeleteDetailDelegate != null)
                {
                    KPButtonEventsArgs eArgs = new KPButtonEventsArgs();
                    MasterDetailConfig.KPEventBeforeDeleteDetailDelegate(entitySelected, eArgs);
                    if (eArgs.Cancel)
                    {
                        pageBase.SetLoading(false);
                        return;
                    }
                }

                DetailSession detailSession = KPSessionHelper.GetSessionMasterDetailList(pageBase.SessionPageID, this.MasterDetailConfig.MasterDetailID);
                int valueKey = 0;
                if (Int32.TryParse(HiddenKey.Value, out valueKey))
                {
                    if (valueKey > 0)
                        detailSession.DetailsEntity.Add(new DetailEntity(entitySelected as ActiveRecordBase, StateDetailEntity.Deleted));
                }
                detailSession.Entities.Remove(entitySelected);
                DataBind();

                if (MasterDetailConfig.KPEventAfterDeleteDetailDelegate != null)
                {
                    MasterDetailConfig.KPEventAfterDeleteDetailDelegate(entitySelected);
                }
            }

            pageBase.SetLoading(false);
        }
        #endregion
    }
}
