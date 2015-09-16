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
using KPAttributes;
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
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
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
    [ToolboxData(@"<{0}:KPGridBaseControl runat=""server"" ></{0}:KPGridBaseControl>")]
    public abstract class KPGridBaseControl : KPCompositeControlBase
    {
        #region ID Names
        private string ID_HiddenKey
        {
            get
            {
                return String.Format("ID_HiddenKey_{0}", this.ID);
            }
        }

        internal string ID_Table
        {
            get
            {
                return String.Format("{0}{1}", this.ClientID, "GridTable");
            }
        }

        private string ID_Pager
        {
            get
            {
                return String.Format("{0}{1}", this.ClientID, "GridPager");
            }
        }

        private string ID_BtnSourceView
        {
            get
            {
                return String.Format("ID_BtnSourceView_{0}", this.ID);
            }
        }

        private string ID_BtnNew
        {
            get
            {
                return String.Format("ID_BtnNew_{0}", this.ID);
            }
        }

        private string ID_BtnEdit
        {
            get
            {
                return String.Format("ID_BtnEdit_{0}", this.ID);
            }
        }

        private string ID_BtnDelete
        {
            get
            {
                return String.Format("ID_BtnDelete_{0}", this.ID);
            }
        }

        #endregion

        internal KPFormMasterDetailModel MasterDetailConfig { get; set; }
        private KPItemModelCollection KPItemsModels = null;
        private KPViewButtonModelCollection KPButtonModelCollection = null;
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public string Text
        {
            get
            {
                String s = (String)ViewState["Text"];
                return ((s == null) ? String.Empty : s);
            }
            set
            {
                ViewState["Text"] = value;
            }
        }

        public KPGridBaseControl()
        {
            this.Caption = String.Empty;
            this.RowNum = KPJqGridRowNumEnum.RowsView_10;
            this.DoubleClickAction = KPJqGridDoubleClickActionEnum.GridViewEdit;
            this.ViewTotalRecordsLabel = true;
            this.OrderRecords = KPJqGridTypeOrderEnum.ASC;
            this.Height = 350;
            this.Width = 1000;
            this.HeightForm = -1;
            this.WidthForm = -1;
            EnableNew = true;
            EnableEdit = true;
            EnableDelete = true;
        }

        #region Events
        public event KPSenderEntityBefore KPEventBeforeNew;
        public event KPSenderEntityBefore KPEventBeforeEdit;
        public event KPSenderEntityBefore KPEventBeforeDelete;
        public event KPSenderEntity KPEventAfterDelete;

        public event KPCriterionFilter KPEventCriterionFilter;
        public event KPOrder KPEventOrder;
        public event EventHandler KPEventAfterControlsCreated;
        #endregion

        #region Fields
        protected HiddenField HiddenKey = null;
        private string typeEntityNamespace;
        private string urlWCF;
        #endregion

        #region Properties


        public string TypeEntityNamespace
        {
            get { return typeEntityNamespace; }
            set
            {
                typeEntityNamespace = value;
                TypeEntity = KPGenericUtil.GetTypeByNamespace(value);
            }
        }
        public Type TypeEntity { get; private set; }
        public string PropertyCompanyEntity { get; set; }

        public string Caption { get; set; }
        public new int Width { get; set; }
        public new int Height { get; set; }

        public int WidthForm { get; set; }
        public int HeightForm { get; set; }
        public KPJqGridRowNumEnum RowNum { get; set; }

        /// <summary>
        /// Determina o modo de controle do Grid, onde será usado.
        /// </summary>
        [Browsable(false)]
        public KPJqGridDoubleClickActionEnum DoubleClickAction { get; set; }

        internal bool ViewTotalRecordsLabel { get; set; }
        public string Url
        {
            get
            { return urlWCF; }
            set
            {
                if (value.Contains("~"))
                    urlWCF = System.Web.VirtualPathUtility.ToAbsolute(value);
                else
                    urlWCF = value;
            }
        }
        public string PageFormUrl { get; set; }
        public KPJqGridTypeOrderEnum OrderRecords { get; set; }
        public string PrimaryKey { get; set; }
        public string RowSelectedKey
        {
            get
            {
                if (HiddenKey != null)
                    return HiddenKey.Value;

                return String.Empty;
            }
        }

        [DefaultValue(true)]
        public bool EnableNew { get; set; }
        [DefaultValue(true)]
        public bool EnableDelete { get; set; }
        [DefaultValue(true)]
        public bool EnableEdit { get; set; }

        public string LabelButtonEdit { get; set; }
        public string LabelButtonDelete { get; set; }
        public string LabelButtonNew { get; set; }

        public string HelpToolTipEdit { get; set; }
        public string HelpToolTipDelete { get; set; }
        public string HelpToolTipNew { get; set; }

        [MergableProperty(false), PersistenceMode(PersistenceMode.InnerProperty)]
        public KPItemModelCollection KPItemsModel
        {
            get
            {
                if (KPItemsModels == null)
                {
                    KPItemsModels = new KPItemModelCollection();
                    if (!base.IsTrackingViewState)
                    {
                        KPItemsModels.TrackViewState();
                    }
                }
                return KPItemsModels;
            }
        }

        [MergableProperty(false), PersistenceMode(PersistenceMode.InnerProperty)]
        public KPViewButtonModelCollection KPButtonsModel
        {
            get
            {
                if (KPButtonModelCollection == null)
                {
                    KPButtonModelCollection = new KPViewButtonModelCollection();
                    if (!base.IsTrackingViewState)
                    {
                        KPButtonModelCollection.TrackViewState();
                    }
                }
                return KPButtonModelCollection;
            }
        }

        #endregion

        public Entity GetSelectedEntity<Entity>()
        {
            try
            {
                if (String.IsNullOrEmpty(HiddenKey.Value))
                    return default(Entity);

                object entity = GetSelectedEntity();
                if (entity != null)
                    return (Entity)entity;

                return default(Entity);
            }
            catch (Exception)
            {
                throw;
            }
        }

        internal object GetSelectedEntity()
        {
            try
            {
                if (String.IsNullOrEmpty(HiddenKey.Value))
                    return null;

                Type propertyType = TypeEntity.GetProperty(PrimaryKey).PropertyType;
                MethodInfo methodFind = TypeEntity.GetMethodInheritance("Find", new Type[] { typeof(object) });
                object entitieObj = methodFind.Invoke(null, new object[] { (Convert.ChangeType(HiddenKey.Value, propertyType)) });
                return entitieObj;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void DeleteSelectedEntity()
        {
            try
            {
                object entitySelected = GetSelectedEntity();
                if (entitySelected == null)
                    return;

                MethodInfo methodFind = entitySelected.GetType().GetMethodInheritance("Delete", new Type[] { });
                methodFind.Invoke(entitySelected, new object[] { });
            }
            catch (Exception ex)
            {
                var exception = KPExceptionHelper.GetCustomException(ex);
                if (exception is KPExceptionSqlForeignKey)
                {
                    ((KPPageBase)this.Page).ShowMessage(KPGlobalizationLanguage.GetString("KPComponents_MessageExistsRelations"));
                }
                else
                    throw exception;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            #region CompanySession
            if (TypeEntity != null && String.IsNullOrWhiteSpace(PropertyCompanyEntity))
            {
                var entityTableAttibutes = TypeEntity.GetCustomAttributes(typeof(KPEntityTable), true);
                if (entityTableAttibutes != null && entityTableAttibutes.Length > 0)
                {
                    var entityTable = entityTableAttibutes.GetValue(0) as KPEntityTable;
                    if (entityTable != null)
                    {
                        PropertyCompanyEntity = entityTable.PropertyCompany;
                    }
                }
            }
            #endregion CompanySession

            #region PrimaryKey
            if (TypeEntity != null && String.IsNullOrWhiteSpace(PrimaryKey))
            {
                var propPrimaryKey = TypeEntity.GetEntityPrimaryKey();
                if (propPrimaryKey != null)
                {
                    PrimaryKey = propPrimaryKey.Name;
                }
            }
            #endregion PrimaryKey
        }

        protected override void CreateChildControls()
        {
            var pageBase = this.Page as KPPageBase;
            if (pageBase != null)
            {
                PagePermission pagePermission = null;
                if (pageBase.SecuritySession != null)
                    pagePermission = pageBase.SecuritySession.GetPagePermission(pageBase.PageEnum);

                if (pagePermission != null)
                {
                    HiddenKey = new HiddenField();
                    HiddenKey.ID = ID_HiddenKey;
                    this.Controls.Add(HiddenKey);

                    HtmlGenericControl htmlTable = new HtmlGenericControl("table");
                    htmlTable.Attributes.Add("id", ID_Table);

                    HtmlGenericControl htmlDiv = new HtmlGenericControl("div");
                    htmlDiv.Attributes.Add("id", ID_Pager);

                    this.Controls.Add(htmlTable);
                    this.Controls.Add(htmlDiv);

                    UpdatePanel upnFormAjax = new UpdatePanel();
                    upnFormAjax.ChildrenAsTriggers = true;
                    upnFormAjax.UpdateMode = UpdatePanelUpdateMode.Conditional;
                    upnFormAjax.Attributes.Add("class", (this.MasterDetailConfig != null ? "KPMasterDetailActionBar" : "KPActionBar"));
                    this.Controls.Add(upnFormAjax);

                    /*
                    #region View Source Code
                    Button btnSourceView = new Button() { ID = ID_BtnSourceView, Text = "Código Fonte", CssClass = "ButtonViewCode" };
                    btnSourceView.Click += new EventHandler(btnSourceView_Click);
                    btnSourceView.Visible = false;
                    upnFormAjax.ContentTemplateContainer.Controls.Add(btnSourceView);
                    #endregion View Source Code
                    */

                    // Chama os métodos para criar os componentes do Grid Detail
                    if (MasterDetailConfig != null)
                        CreateChildOthersControls(pageBase, pagePermission, upnFormAjax);

                    #region Default Buttons
                    if (EnableNew && !pagePermission.IsReadOnly)
                    {
                        ComponentPermission componentPermission = pageBase.SecuritySession.GetComponentPermission(pageBase.PageEnum, "ACTION_GRID_ADD");
                        if (componentPermission.IsVisible)
                        {
                            Button btnNew = new Button() { ID = ID_BtnNew, Text = KPGlobalizationLanguage.GetString("KPComponents_New"), CssClass = "KPGridNewButton" };
                            if (!String.IsNullOrWhiteSpace(LabelButtonNew))
                                btnNew.Text = KPGlobalizationLanguage.GetString(LabelButtonNew);
                            if (!String.IsNullOrWhiteSpace(HelpToolTipNew))
                                btnNew.Attributes.Add("title", KPGlobalizationLanguage.GetString(HelpToolTipNew));
                            btnNew.Enabled = componentPermission.IsEnabled;
                            btnNew.Attributes.Add("onclick", "setLoading(true);");
                            btnNew.Click += new EventHandler(btnNew_Click);
                            upnFormAjax.ContentTemplateContainer.Controls.Add(btnNew);
                        }
                    }

                    #region Addictional Buttons
                    if (KPButtonModelCollection != null && KPButtonModelCollection.Count > 0 && !pagePermission.IsReadOnly)
                    {
                        foreach (KPViewButtonModel obj in KPButtonModelCollection)
                        {
                            if (obj is KPViewButtonSimple)
                            {
                                KPViewButtonSimple btnModel = (KPViewButtonSimple)obj;

                                ComponentPermission componentPermission = pageBase.SecuritySession.GetComponentPermission(pageBase.PageEnum, btnModel.ID);
                                if (componentPermission.IsVisible)
                                {
                                    KPButtonControl btn = new KPButtonControl();
                                    btn.ID = btnModel.ID;
                                    btn.Enabled = componentPermission.IsEnabled;
                                    btn.Text = btnModel.Title;
                                    btn.Click += delegate(object sender, EventArgs e)
                                    {
                                        btnModel.OnClick(sender, e);
                                    };

                                    upnFormAjax.ContentTemplateContainer.Controls.Add(btn);
                                }
                            }
                        }
                    }
                    #endregion

                    if (EnableEdit && !pagePermission.IsReadOnly)
                    {
                        ComponentPermission componentPermission = pageBase.SecuritySession.GetComponentPermission(pageBase.PageEnum, "ACTION_GRID_EDIT");
                        if (componentPermission.IsVisible)
                        {
                            Button btnEdit = new Button() { ID = ID_BtnEdit, Text = KPGlobalizationLanguage.GetString("KPComponents_Edit"), CssClass = "KPGridEditButton" };
                            if (!String.IsNullOrWhiteSpace(LabelButtonEdit))
                                btnEdit.Text = KPGlobalizationLanguage.GetString(LabelButtonEdit);
                            if (!String.IsNullOrWhiteSpace(HelpToolTipEdit))
                                btnEdit.Attributes.Add("title", KPGlobalizationLanguage.GetString(HelpToolTipEdit));
                            btnEdit.Enabled = componentPermission.IsEnabled;
                            btnEdit.Attributes.Add("onclick", "return validateGridSelection(event, '" + ID_Table + "', true, true);");
                            btnEdit.Click += new EventHandler(btnEdit_Click);
                            upnFormAjax.ContentTemplateContainer.Controls.Add(btnEdit);
                        }
                    }

                    if (EnableDelete && !pagePermission.IsReadOnly)
                    {
                        ComponentPermission componentPermission = pageBase.SecuritySession.GetComponentPermission(pageBase.PageEnum, "ACTION_GRID_DELETE");
                        if (componentPermission.IsVisible)
                        {
                            Button btnDelete = new Button() { ID = ID_BtnDelete, Text = KPGlobalizationLanguage.GetString("KPComponents_Delete"), CssClass = "KPGridDeleteButton" };
                            if (!String.IsNullOrWhiteSpace(LabelButtonDelete))
                                btnDelete.Text = KPGlobalizationLanguage.GetString(LabelButtonDelete);
                            if (!String.IsNullOrWhiteSpace(HelpToolTipDelete))
                                btnDelete.Attributes.Add("title", KPGlobalizationLanguage.GetString(HelpToolTipDelete));
                            btnDelete.Enabled = componentPermission.IsEnabled;
                            string question = KPGlobalizationLanguage.GetString("KPComponents_QuestionDeleteReg");
                            btnDelete.Attributes.Add("onclick", String.Format("return (validateGridSelection(event, '{0}', true, false) ? confirmBox('{1}', 1, true, true) : false);", ID_Table, question));
                            btnDelete.Click += new EventHandler(btnDelete_Click);
                            upnFormAjax.ContentTemplateContainer.Controls.Add(btnDelete);
                        }
                    }
                    #endregion Default Buttons

                    ClearChildViewState();
                    base.CreateChildControls();
                }
            }

            if (KPEventAfterControlsCreated != null)
                KPEventAfterControlsCreated(this, EventArgs.Empty);
        }

        private void btnSourceView_Click(object sender, EventArgs e)
        {
            /*
            //FileInfo fileForm = new FileInfo(this.PageFormUrl);
            //FileInfo fileView = new FileInfo(String.Concat(fileForm.Name.Replace(fileForm.Extension, String.Empty), "View.aspx"));
            FileInfo fileView = new FileInfo(@"E:\Workspace.KP\Application.Net\WebSolution\WebProject\Form\Operacional\Cadastro\FrmItemView.aspx");
            string sourceView = File.ReadAllText(fileView.FullName);

            sourceView = KPGenericUtil.GetSourceViewKPGrid(sourceView);

            if (HttpContext.Current != null)
            {
                HttpContext _context = HttpContext.Current;
                if (_context.Session != null)
                {
                    _context.Session.Add(KPSessionKeyEnum.SESSION_SOURCE_VIEW.ToString(), sourceView);
                    ((KPPageBase)this.Page).KPWindow("Source View", "/SourceView.aspx", true, 800, 600);
                }
            }
             */
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            KPPageBase pageBase = this.Page as KPPageBase;
            if (!String.IsNullOrEmpty(PageFormUrl) && pageBase != null)
            {
                object entitySelected = GetSelectedEntity();
                if (KPEventBeforeNew != null)
                {
                    KPButtonEventsArgs eArgs = new KPButtonEventsArgs();
                    KPEventBeforeNew(entitySelected, eArgs);
                    if (eArgs.Cancel)
                    {
                        pageBase.SetLoading(false);
                        return;
                    }
                }

                KPSessionHelper.SetSessionData(new KPSessionData(this, Activator.CreateInstance(TypeEntity), KPFormStateEnum.New), pageBase.SessionPageID);
                string titlePage = "Carregando ...";
                string pageFormUrlSession = String.Format("{0}?parentID={1}", this.PageFormUrl, pageBase.SessionPageID);

                pageBase.KPWindow(titlePage, pageFormUrlSession, true, WidthForm, HeightForm);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            KPPageBase pageBase = this.Page as KPPageBase;
            if (this.HiddenKey != null && !String.IsNullOrEmpty(HiddenKey.Value) && !String.IsNullOrEmpty(PageFormUrl) && pageBase != null)
            {
                object entitySelected = GetSelectedEntity();
                if (KPEventBeforeEdit != null)
                {
                    KPButtonEventsArgs eArgs = new KPButtonEventsArgs();
                    KPEventBeforeEdit(entitySelected, eArgs);
                    if (eArgs.Cancel)
                    {
                        pageBase.SetLoading(false);
                        return;
                    }
                }

                KPSessionHelper.SetSessionData(new KPSessionData(this, entitySelected, KPFormStateEnum.Edit), pageBase.SessionPageID);
                string titlePage = "Carregando ...";
                string pageFormUrlSession = String.Format("{0}?parentID={1}", this.PageFormUrl, pageBase.SessionPageID);

                pageBase.KPWindow(titlePage, pageFormUrlSession, true, WidthForm, HeightForm);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            object entitySelected = GetSelectedEntity();
            KPPageBase pageBase = this.Page as KPPageBase;
            if (pageBase != null)
            {
                if (KPEventBeforeDelete != null)
                {
                    KPButtonEventsArgs eArgs = new KPButtonEventsArgs();
                    KPEventBeforeDelete(entitySelected, eArgs);
                    if (eArgs.Cancel)
                    {
                        pageBase.SetLoading(false);
                        return;
                    }
                }

                DeleteSelectedEntity();
                DataBind();

                if (KPEventAfterDelete != null)
                {
                    KPEventAfterDelete(entitySelected);
                }
                pageBase.SetLoading(false);
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            base.Render(writer);
            writer.Write(Build());
        }

        public override void DataBind()
        {
            base.DataBind();
            ScriptManager.RegisterClientScriptBlock(this.Page, Page.GetType(), "RefreshData",
                    String.Format(@"$('#{0}').trigger('reloadGrid');", ID_Table), true);
        }

        private string Build()
        {
            ICriterion criterionFilter = null;
            Order order = null;
            if (KPEventCriterionFilter != null)
            {
                criterionFilter = KPEventCriterionFilter();
            }
            if (KPEventOrder != null)
            {
                order = KPEventOrder();
            }

            KPJqGridControl jqGrid = new KPJqGridControl(this.PrimaryKey, this.HiddenKey, this.TypeEntity,
                                                        this.KPItemsModel, this.DoubleClickAction, criterionFilter, order);

            if (MasterDetailConfig != null)
                jqGrid.MasterDetailID = MasterDetailConfig.MasterDetailID;
            jqGrid.Caption = this.Caption;
            jqGrid.RowNum = this.RowNum;
            jqGrid.OrderRecords = this.OrderRecords;
            jqGrid.ViewTotalRecordsLabel = this.ViewTotalRecordsLabel;
            jqGrid.Height = this.Height;
            jqGrid.Width = this.Width;
            jqGrid.PropertyCompanyEntity = PropertyCompanyEntity;
            if (!String.IsNullOrEmpty(Url))
            {
                jqGrid.UrlService = Url;
            }

            return jqGrid.Build(this.ID_Pager, this.ID_Table, this.Page as KPPageBase);
        }

        /// <summary>
        /// This method add KPItemModel on cols Collection
        /// </summary>
        /// <param name="itemModel"></param>
        internal void AddItemsModelCollection(KPItemModel itemModel)
        {
            if (KPItemsModels == null)
                KPItemsModels = new KPItemModelCollection();

            KPItemsModels.Add(itemModel);
        }


        #region Overrides
        protected abstract void CreateChildOthersControls(KPPageBase pageBase, PagePermission pagePermission, UpdatePanel upnFormAjax);
        #endregion
    }
}
