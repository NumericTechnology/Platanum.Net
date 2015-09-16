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
using KPBO;
using KPBO.Validator;
using KPComponents.KPData;
using KPComponents.KPDelegate;
using KPComponents.KPEvent;
using KPComponents.KPForm;
using KPComponents.KPSecurity;
using KPComponents.KPSession;
using KPCore.KPException;
using KPCore.KPSecurity;
using KPCore.KPUtil;
using KPCore.KPValidator;
using KPEntity.ORM;
using KPEnumerator.KPComponents;
using KPEnumerator.KPGlobalization;
using KPEnumerator.KPSecurity;
using KPExtension;
using KPGlobalization;
using NHibernate.Criterion;
using NHibernate.Validator.Engine;
using NHibernate.Validator.Event;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace KPComponents
{
    /// <summary>
    /// <para>Authors: Juliano Tiago Rinaldi and 
    /// Tiago Antonio Jacobi</para>
    /// </summary>
    public abstract class KPFormBaseControl : KPCompositeControlBase
    {
        #region Containers Controls
        private KPFormTabModelCollection KPFormTabModels = null;
        private KPFormItemModelCollection KPFormItemModels = null;
        private KPFormButtonModelCollection KPFormButtonsModel = null;
        private KPFormZoomModelCollection KPFormZoomModels = null;
        private KPFormGridModelCollection KPFormGridModels = null;
        private KPFormMasterDetailModelCollection KPFormMasterDetailModels = null;

        private HtmlGenericControl KPActionBar = new HtmlGenericControl("div");
        private HtmlGenericControl ItemsFieldset = new HtmlGenericControl("ul");
        #endregion

        #region Fields
        private string typeBONamespace;
        internal protected HtmlGenericControl ErrorsView = new HtmlGenericControl("div") { ID = "errorsView" };
        #endregion

        #region Private Properties
        private UpdatePanel UpdatePanelFormAjax { get; set; }
        private ControlCollection ControlsFieldset { get { return ItemsFieldset.Controls; } }
        #endregion

        #region Events
        public event KPSenderEntityBO KPEventSaveClick;
        public event KPSenderEntityBefore KPEventBeforeSaveClick;
        public event KPSenderEntityBO KPEventAfterSaveClick;
        public event KPSenderEntity KPEventAfterLoad;
        public event KPAfterControlsCreated KPEventAfterControlsCreated;
        public event KPFormClosing KPEventFormClosing;
        #endregion

        public KPFormBaseControl()
        {
            UpdatePanelFormAjax = new UpdatePanel() { ID = "updatePanel_Form" };
            EnableSave = true;
            EnableClose = false;
        }

        #region Properties
        public KPPageBase PageBase { get; protected set; }

        [Browsable(false)]
        public KPFormStateEnum FormActionState
        {
            get
            {
                if (ViewState["FormActionStatus"] != null)
                {
                    return (KPFormStateEnum)ViewState["FormActionStatus"];
                }
                return KPFormStateEnum.New;
            }
            private set
            {
                ViewState["FormActionStatus"] = value;
            }
        }

        public bool EnableSave { get; set; }
        public bool EnableClose { get; set; }

        public string HelpToolTipSave { get; set; }
        public string HelpToolTipClose { get; set; }

        public override ControlCollection Controls
        {
            get
            {
                EnsureChildControls();
                return base.Controls;
            }
        }

        public string PropertyCompanyEntity { get; set; }

        public string TypeBONamespace
        {
            get { return typeBONamespace; }
            set
            {
                typeBONamespace = value;
                TypBOEntity = KPGenericUtil.GetTypeByNamespace(value);
                if (typeof(BaseBO<>).IsSubclassOfRawGeneric(TypBOEntity))
                {
                    PropertyInfo prop = TypBOEntity.GetProperty("EntityField");
                    TypeEntity = prop.PropertyType;
                    TypeEntityNamespace = String.Format("{0}.{1}", TypeEntity.Namespace, TypeEntity.Name);
                }
                else
                    throw new Exception(String.Format("A BO {0} não está na estrutura esperada.", typeBONamespace));
            }
        }

        /// <summary>
        /// Propriedade que recebe código javascript para ser executado no Parent ao fechar/salvar tela
        /// </summary>
        public string OnCloseScriptTargetParent
        {
            get
            {
                object o = ViewState["OnCloseScriptTargetParent"];
                return o == null ? String.Empty : o.ToString();
            }
            set { ViewState["OnCloseScriptTargetParent"] = value; }
        }

        [Browsable(false)]
        internal new int Width { get; set; }

        [Browsable(false)]
        internal new int Height { get; set; }

        public string TypeEntityNamespace { get; private set; }

        public Type TypeEntity { get; private set; }

        public Type TypBOEntity { get; private set; }

        public string ErrorsViewer
        {
            get { return this.ErrorsView.InnerText; }
        }

        [Browsable(false)]
        public object DataSource
        {
            get
            {
                object o = ViewState["DataSource"];
                return o == null ? null : o;
            }
            set
            {
                if (value == null)
                    value = Activator.CreateInstance(TypeEntity);
                if (TypeEntity != value.GetType())
                    throw new Exception(
                            String.Format("O tipo de dados esperado é: {0} e não {1}",
                                TypeEntity.ToString(), value.GetType().ToString()));
                else
                    ViewState["DataSource"] = value;
            }
        }

        [Browsable(false)]
        public object DataSourceAltered
        {
            get
            {
                object o = ViewState["DataSourceAltered"];
                return o == null ? null : o;
            }
            internal protected set { ViewState["DataSourceAltered"] = value; }
        }

        [Browsable(false)]
        public KPSessionData KPSessionData
        {
            get
            {
                object o = ViewState["KPSessionDataViewState"];
                return o == null ? null : (KPSessionData)o;
            }
            private set { ViewState["KPSessionDataViewState"] = value; }
        }

        [MergableProperty(false), PersistenceMode(PersistenceMode.InnerProperty)]
        public KPFormItemModelCollection KPColumnsModel
        {
            get
            {
                if (KPFormItemModels == null)
                {
                    KPFormItemModels = new KPFormItemModelCollection();
                    if (!base.IsTrackingViewState)
                    {
                        KPFormItemModels.TrackViewState();
                    }
                }
                return KPFormItemModels;
            }
        }

        [MergableProperty(false), PersistenceMode(PersistenceMode.InnerProperty)]
        public KPFormTabModelCollection KPTabs
        {
            get
            {
                if (KPFormTabModels == null)
                {
                    KPFormTabModels = new KPFormTabModelCollection();
                    if (!base.IsTrackingViewState)
                    {
                        KPFormTabModels.TrackViewState();
                    }
                }
                return KPFormTabModels;
            }
        }

        [MergableProperty(false), PersistenceMode(PersistenceMode.InnerProperty)]
        public KPFormButtonModelCollection KPButtonsModel
        {
            get
            {
                if (KPFormButtonsModel == null)
                {
                    KPFormButtonsModel = new KPFormButtonModelCollection();
                    if (!base.IsTrackingViewState)
                    {
                        KPFormButtonsModel.TrackViewState();
                    }
                }
                return KPFormButtonsModel;
            }
        }

        [MergableProperty(false), PersistenceMode(PersistenceMode.InnerProperty)]
        public KPFormMasterDetailModelCollection KPFormMasterDetailConfig
        {
            get
            {
                if (KPFormMasterDetailModels == null)
                {
                    KPFormMasterDetailModels = new KPFormMasterDetailModelCollection();
                    if (!base.IsTrackingViewState)
                    {
                        KPFormMasterDetailModels.TrackViewState();
                    }
                }
                return KPFormMasterDetailModels;
            }
        }

        [MergableProperty(false), PersistenceMode(PersistenceMode.InnerProperty)]
        public KPFormZoomModelCollection KPFormZoomConfig
        {
            get
            {
                if (KPFormZoomModels == null)
                {
                    KPFormZoomModels = new KPFormZoomModelCollection();
                    if (!base.IsTrackingViewState)
                    {
                        KPFormZoomModels.TrackViewState();
                    }
                }
                return KPFormZoomModels;
            }
        }

        [MergableProperty(false), PersistenceMode(PersistenceMode.InnerProperty)]
        public KPFormGridModelCollection KPFormGridConfig
        {
            get
            {
                if (KPFormGridModels == null)
                {
                    KPFormGridModels = new KPFormGridModelCollection();
                    if (!base.IsTrackingViewState)
                    {
                        KPFormGridModels.TrackViewState();
                    }
                }
                return KPFormGridModels;
            }
        }

        [MergableProperty(false), PersistenceMode(PersistenceMode.InnerProperty)]
        public Panel KPCustomContent
        {
            get;
            set;
        }
        #endregion

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

            if (PageBase == null)
                this.PageBase = this.Page as KPPageBase;

            this.ErrorsView.InnerText = String.Empty;

            KPSessionData obj = null;
            if (PageBase != null)
                obj = KPSessionHelper.GetSessionData(PageBase.ParentSessionPageID);
            if (obj != null)
            {
                KPSessionData = obj;
                FormActionState = KPSessionData.FormState;
            }

            if (DataSource == null && KPSessionData != null)
            {
                DataSource = KPSessionData.Entity;
            }

            if (DataSource == null)
            {
                DataSource = Activator.CreateInstance(TypeEntity);
            }

            if (Page != null && !Page.IsPostBack)
            {
                if (this.FormActionState == KPFormStateEnum.New)
                    SetCurrentCompany(DataSource as ActiveRecordBase);
            }

            if (DataSourceAltered == null)
            {
                DataSourceAltered = CloneDataSource(DataSource, TypeEntity);
            }

            String script = "window.parent.setLoading(false);";
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "parentLoadingFalse", script, true);
        }

        internal object CloneDataSource(object dataSource, Type typeDataSource)
        {
            object dataSourceCloned = null;

            if (dataSource != null)
            {
                if (dataSourceCloned == null)
                {
                    dataSourceCloned = Activator.CreateInstance(dataSource.GetType());
                    foreach (PropertyInfo propertyInfo in dataSource.GetType().GetProperties())
                    {
                        if (dataSourceCloned.GetType().GetProperty(propertyInfo.Name).GetSetMethod() != null)
                        {
                            dataSourceCloned.GetType().
                                GetProperty(propertyInfo.Name).
                                    SetValue(dataSourceCloned, propertyInfo.GetValue(dataSource, null), null);
                        }
                    }
                }
            }
            else
            {
                return Activator.CreateInstance(typeDataSource);
            }

            return dataSourceCloned;
        }

        public void AddErrorView(string errorMessage)
        {
            StringBuilder erroMsg = new StringBuilder();
            if (!String.IsNullOrWhiteSpace(ErrorsView.InnerText))
                erroMsg.AppendFormat("{0}", ErrorsView.InnerText);

            if (!String.IsNullOrWhiteSpace(errorMessage))
                erroMsg.AppendFormat(" - {0} <br>", errorMessage);

            ErrorsView.InnerHtml = erroMsg.ToString();
        }

        /// <summary>
        /// Pega os dados da tela e preenche o DataSource
        /// </summary>
        internal void RefreshDataSourceAltered()
        {
            Control[] ControlsComponentData = GetControlsComponentData(this.Controls);
            foreach (Control item in ControlsComponentData)
            {
                if (item is IKPComponentData)
                {
                    IKPComponentData componentData = item as IKPComponentData;
                    if (String.IsNullOrEmpty(((IKPComponentData)componentData).FieldName))
                    {
                        continue;
                    }

                    object objValue = null;

                    #region KPTextBoxField
                    if (item is KPTextBoxField)
                    {
                        KPTextBoxField textBoxField = (KPTextBoxField)item;

                        Type typeProperty = DataSourceAltered.GetType().GetProperty(componentData.FieldName).PropertyType;
                        if (!String.IsNullOrEmpty(textBoxField.Text))
                        {
                            try
                            {
                                objValue = TypeDescriptor.GetConverter(typeProperty).ConvertFrom(textBoxField.Text);
                            }
                            catch
                            {
                                textBoxField.SetInvalidateMsg(KPGlobalizationLanguageEnum.GetString(KPLanguageKeyEnum.INVALID_VALUE));
                            }
                        }
                        else
                        {
                            if (typeProperty.Equals(typeof(String)))
                            {
                                objValue = String.Empty;
                            }
                        }
                    }
                    #endregion KPTextBoxField

                    #region KPCheckBoxField
                    else if (item is KPCheckBoxField)
                    {
                        Type typeProperty = DataSourceAltered.GetType().GetProperty(componentData.FieldName).PropertyType;
                        objValue = ((KPCheckBoxField)item).Checked;
                    }
                    #endregion

                    #region KPComboBoxField
                    else if (item is KPComboBoxField)
                    {
                        KPComboBoxField comboBoxItem = (KPComboBoxField)item;
                        if (comboBoxItem.Items.Count > 0 && comboBoxItem.SelectedIndex > 0
                            && (!String.IsNullOrWhiteSpace(comboBoxItem.SelectedValue)))
                        {
                            Type typeProperty = DataSourceAltered.GetType().GetProperty(componentData.FieldName).PropertyType;
                            if (typeof(KPActiveRecordBase<>).IsSubclassOfRawGeneric(typeProperty))
                            {
                                PropertyInfo prop = typeProperty.GetEntityPrimaryKey();
                                if (prop != null)
                                {
                                    object primaryKeyObj = TypeDescriptor.GetConverter(prop.PropertyType).ConvertFrom(comboBoxItem.SelectedValue);
                                    MethodInfo method = typeProperty.GetMethodInheritance("Find", new Type[] { typeof(object) });
                                    if (method != null)
                                        objValue = method.Invoke(null, new object[] { primaryKeyObj });
                                }
                            }
                            else
                            {
                                objValue = TypeDescriptor.GetConverter(typeProperty).ConvertFrom(comboBoxItem.SelectedValue);
                            }
                        }
                    }
                    #endregion KPComboBoxField

                    #region KPZoomField
                    else if (item is KPZoomField)
                    {
                        KPZoomField textZoomField = (KPZoomField)item;

                        if (!String.IsNullOrEmpty(textZoomField.Value))
                        {
                            Type typeProperty = DataSourceAltered.GetType().GetProperty(componentData.FieldName).PropertyType;
                            if (typeof(KPActiveRecordBase<>).IsSubclassOfRawGeneric(typeProperty))
                            {
                                PropertyInfo prop = typeProperty.GetEntityPrimaryKey();
                                if (prop != null)
                                {
                                    object primaryKeyObj = TypeDescriptor.GetConverter(prop.PropertyType).ConvertFrom(textZoomField.Value);
                                    MethodInfo method = typeProperty.GetMethodInheritance("Find", new Type[] { typeof(object) });
                                    if (method != null)
                                    {
                                        objValue = method.Invoke(null, new object[] { primaryKeyObj });
                                    }
                                }
                            }
                        }
                        else
                        {
                            DataSourceAltered.GetType().GetProperty(componentData.FieldName).SetValue(DataSourceAltered, null, null);
                        }
                    }
                    #endregion KPZoomField

                    if (objValue != null)
                    {
                        DataSourceAltered.GetType().GetProperty(componentData.FieldName).SetValue(DataSourceAltered, objValue, null);
                    }
                }
            }
        }

        private Control[] GetControlsComponentData(ControlCollection controls)
        {
            List<Control> controlsData = new List<Control>();
            foreach (Control item in controls)
            {
                if (item.Controls.Count > 0)
                {
                    controlsData.AddRange(GetControlsComponentData(item.Controls));
                }

                if (item is IKPComponentData)
                {
                    controlsData.Add(item);
                }
            }

            return controlsData.ToArray();
        }

        protected override void OnPreRender(EventArgs e)
        {
            // Atualizar os valores da Tela
            RefreshDataSourceAltered();

            base.OnPreRender(e);

            if (String.IsNullOrEmpty(TypeEntityNamespace))
            {
                throw new Exception("É obrigatório atribuir um tipo ao Form, o tipo deve ser uma entidade.");
            }

            if (Page != null && Page.IsPostBack)
            {
                if (!String.IsNullOrEmpty(Page.ErrorPage))
                {
                    AddErrorView(Page.ErrorPage);
                }

                this.ErrorsView.Visible = (!String.IsNullOrEmpty(this.ErrorsView.InnerHtml));
            }

            if (KPEventAfterControlsCreated != null)
            {
                KPEventAfterControlsCreated(this, EventArgs.Empty);
            }
        }

        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            if (PageBase == null)
                PageBase = this.Page as KPPageBase;

            PagePermission pagePermission = null;
            if (PageBase.SecuritySession != null)
                pagePermission = PageBase.SecuritySession.GetPagePermission(PageBase.PageEnum);

            if (pagePermission != null)
            {

                UpdatePanelFormAjax.ChildrenAsTriggers = true;
                UpdatePanelFormAjax.UpdateMode = UpdatePanelUpdateMode.Conditional;
                UpdatePanelFormAjax.Attributes.Add("class", "KPFormBody");

                HtmlGenericControl htmlGenWindow = new HtmlGenericControl("div");
                htmlGenWindow.Attributes.Add("class", "KPInternalFormWindow");

                this.ErrorsView.Attributes.Add("class", "KPFormErrorView");
                if (String.IsNullOrEmpty(this.ErrorsView.InnerHtml))
                    this.ErrorsView.Visible = false;

                HtmlGenericControl htmlGenFieldSet = new HtmlGenericControl("fieldset");
                htmlGenFieldSet.Attributes.Add("class", "KPFormControl");

                UpdatePanelFormAjax.ContentTemplateContainer.Controls.Add(htmlGenFieldSet);

                htmlGenFieldSet.Controls.Add(this.ErrorsView);

                #region Create the Form Fields.

                KPFormItemFactory formItemFactory = this.CreateFormItemFactory(this.KPColumnsModel, null);

                if (formItemFactory.ControlsFieldset.Count > 0)
                {
                    htmlGenFieldSet.Controls.Add(formItemFactory.ItemsFieldset);
                }

                #region Create tabs if Exists

                if (KPTabs.Count > 0)
                {
                    htmlGenFieldSet.Controls.Add(new KPFormTabControl(this, KPTabs));
                }
                #endregion Create tabs if Exists

                if (KPCustomContent != null && KPCustomContent.Controls.Count > 0)
                {
                    htmlGenFieldSet.Controls.Add(KPCustomContent);
                }
                #endregion End form fields creation.

                #region Create the ActionBar
                KPActionBar.Attributes.Add("class", "KPActionBar");

                if (EnableSave && !pagePermission.IsReadOnly)
                {
                    ComponentPermission componentPermission = PageBase.SecuritySession.GetComponentPermission(PageBase.PageEnum, ComponentPermission.COMPONENT_ACTION_FORM_SAVE);
                    if (componentPermission.IsVisible)
                    {
                        KPActionBar.Controls.Add(GetSaveButton(componentPermission));
                    }
                }

                if (!pagePermission.IsReadOnly)
                {
                    foreach (KPFormButtonModel obj in KPButtonsModel)
                    {
                        if (obj is KPFormButtonSimple)
                        {
                            KPFormButtonSimple objButton = obj as KPFormButtonSimple;
                            ComponentPermission componentPermission = PageBase.SecuritySession.GetComponentPermission(PageBase.PageEnum, objButton.ID);
                            if (componentPermission.IsVisible)
                            {
                                KPFormButtonSimpleControl KPBtnSimpleTop = new KPFormButtonSimpleControl(this, objButton);
                                KPBtnSimpleTop.ControlButton.Enabled = componentPermission.IsEnabled;
                                KPActionBar.Controls.Add(KPBtnSimpleTop);
                            }
                        }
                    }
                }

                var btnClose = GetCloseButton();
                if (!EnableClose)
                {
                    btnClose.Attributes.Add("style", "display: none;");
                }
                KPActionBar.Controls.Add(btnClose);

                ClearChildViewState();
                UpdatePanelFormAjax.ContentTemplateContainer.Controls.Add(KPActionBar);

                #endregion End action bar creation

                htmlGenWindow.Controls.Add(UpdatePanelFormAjax);

                this.Controls.Add(htmlGenWindow);

                // Só chama na entrada do formulário
                if (Page != null && !Page.IsPostBack)
                {
                    if (this.FormActionState == KPFormStateEnum.New)
                    {
                        SetCurrentCompany(DataSource as ActiveRecordBase);
                    }
                    if (KPEventAfterLoad != null)
                    {
                        KPEventAfterLoad(DataSource);
                    }
                }

                String script = @"generateFormDefaults();";
                ScriptManager.RegisterStartupScript(UpdatePanelFormAjax, UpdatePanelFormAjax.GetType(), "generateFormDefaults", script, true);

                // TODO Juliano irá fazer funcionar...
                // TODO Erro é quando da refresh no UpdatePanel perde máscara, bom exemplo é zoom de CEP tela de Cliente.
                // Em algumas situações o programa fica fazendo vários requests, ou seja ele entra em loop. Testar isto em alterações, para verifira basta ver na aba "Network" do google Chrome.
                // ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "generateFormDefaults", script, true);
                // ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "generateMaskValidators", "generateMaskValidators();", true);
                // ScriptManager.RegisterStartupScript(UpdatePanelFormAjax, this.GetType(), "generateMaskValidators", "generateMaskValidators();", true);

                KPSessionHelper.RemoveSessionData(PageBase.ParentSessionPageID);
            }
        }

        /// <summary>
        /// Responsável por setar a Company na entrada do formulário.
        /// Usar apenas quando é um novo cadastro.
        /// </summary>
        /// <param name="entityData"></param>
        private void SetCurrentCompany(ActiveRecordBase entityData)
        {
            #region Responsável por Setar Company
            if (entityData == null)
                return;

            KPSecuritySession session = KPFormsAuthentication.SecuritySession;

            if (!String.IsNullOrEmpty(this.PropertyCompanyEntity) && !PropertyCompanyEntity.Equals("!") && session != null)
            {
                PropertyInfo propCompany = entityData.GetType().GetProperty(this.PropertyCompanyEntity);
                if (propCompany != null && propCompany.PropertyType.Equals(typeof(FrwCompany)))
                {
                    FrwCompany companyInstance = FrwCompany.Find(session.FrwCompany);
                    propCompany.SetValue(entityData, companyInstance, null);
                }
            }
            #endregion
        }

        protected override void RenderContents(HtmlTextWriter output)
        {
            base.RenderContents(output);
            try
            {
                ValidateSchemaEntity();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Faz as validações entre os componentes e a Entidade
        /// </summary>
        private void ValidateSchemaEntity()
        {
            try
            {
                Type typeEntity = TypeEntity;

                StringBuilder sbErro = new StringBuilder();
                foreach (KPFormItemModel obj in KPColumnsModel)
                {
                    if (obj is KPFormItemModelField)
                    {
                        if (!String.IsNullOrEmpty(((KPFormItemModelField)obj).FieldName))
                        {
                            PropertyInfo property = typeEntity.GetProperty(((KPFormItemModelField)obj).FieldName);
                            if (property == null)
                            {
                                if (sbErro.Length != 0)
                                {
                                    sbErro.Append(Environment.NewLine);
                                }
                                sbErro.AppendFormat(@"O tipo entidade ""{0}"" não possui a propriedade ""{1}""", TypeEntity, ((KPFormItemModelField)obj).FieldName);
                            }
                        }
                    }
                }

                if (sbErro.Length != 0)
                {
                    throw new Exception(sbErro.ToString());
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Executa o NHibernate Validator
        /// </summary>
        /// <returns></returns>
        internal protected InvalidValue[] ValidateEntity()
        {
            if (NHibernate.Validator.Cfg.Environment.SharedEngineProvider == null)
                NHibernate.Validator.Cfg.Environment.SharedEngineProvider = new NHibernateSharedEngineProvider();

            ValidatorEngine ve = NHibernate.Validator.Cfg.Environment.SharedEngineProvider.GetEngine();
            InvalidValue[] errors = ve.Validate(DataSourceAltered, null);
            return errors;
        }

        internal protected bool Validate(InvalidValue[] invalidValues)
        {
            this.ErrorsView.Visible = false;
            InvalidValue[] errors = invalidValues;
            bool foundComponent = false;
            if (errors != null)
            {
                int firstTabIndex = 9999;
                int tabIndex = 0;

                WebControl[] componentDataWebControls = this.GetWebControlsType(typeof(IKPComponentData));
                if (componentDataWebControls != null)
                {
                    foreach (WebControl componentData in componentDataWebControls)
                    {
                        if (String.IsNullOrEmpty(((IKPComponentData)componentData).FieldName))
                        {
                            continue;
                        }

                        ((IKPComponentData)componentData).RemoveInvalidateMsg();

                        foreach (InvalidValue error in errors)
                        {
                            if (((IKPComponentData)componentData).FieldName.Equals(error.PropertyName))
                            {
                                foundComponent = true;
                                ((IKPComponentData)componentData).SetInvalidateMsg(error.Message);

                                if (firstTabIndex > 0)
                                {
                                    tabIndex = this.GetTabIndexFromFieldName(error.PropertyName);
                                    if (tabIndex >= 0 && firstTabIndex > tabIndex)
                                    {
                                        firstTabIndex = tabIndex;
                                    }
                                }
                            }
                        }
                    }
                }

                if (errors.Length > 0)
                {
                    string scriptString = "generateMaskValidators();";

                    if (this.KPTabs.Count > 0)
                    {
                        scriptString += KPTabControl.GetTabIndexScript(firstTabIndex);
                    }

                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "scriptWhenErrorOccur", scriptString, true);

                    if (!foundComponent)
                    {
                        this.AddErrorView("Atenção! Existem componentes não visíveis em tela.");
                        foreach (var errorView in errors)
                        {
                            PropertyInfo prop = errorView.EntityType.GetProperty(errorView.PropertyName);
                            this.AddErrorView(String.Format("{0} - {1}", prop.GetTranslate(), errorView.Message));
                        }
                    }

                    return false;
                }
            }

            return true;
        }

        private int GetTabIndexFromFieldName(string fieldName)
        {
            int tabIndex = -1;
            int currIndex = 0;

            if (fieldName != null && this.KPTabs.Count > 0)
            {
                foreach (KPFormTabModel modelColl in this.KPTabs)
                {
                    foreach (KPFormItemModelField componentData in modelColl.KPColumnsModel)
                    {
                        if (componentData.FieldName.Equals(fieldName))
                        {
                            tabIndex = currIndex;
                            break;
                        }
                    }

                    if (tabIndex >= 0)
                    {
                        break;
                    }
                    currIndex++;
                }
            }

            return tabIndex;
        }

        private object GetValue(KPFormItemModel columEntity)
        {
            if (!String.IsNullOrEmpty(TypeEntityNamespace) && DataSource != null && columEntity is KPFormItemModelField)
            {
                try
                {
                    if (!String.IsNullOrEmpty(((KPFormItemModelField)columEntity).FieldName))
                    {
                        PropertyInfo propField = DataSource.GetType().GetProperty(((KPFormItemModelField)columEntity).FieldName);
                        if (propField == null)
                            throw new KPExceptionSuggestion(String.Format("Não foi encontrado a propriedade '{0}'. Verificar nome da propriedade na Entidade.", ((KPFormItemModelField)columEntity).FieldName));

                        object objGet = propField.GetValue(DataSource, null);

                        if (objGet != null && typeof(KPActiveRecordBase<>).IsSubclassOfRawGeneric(objGet.GetType()))
                        {
                            if (columEntity is KPFormItemModelEntityField &&
                                !String.IsNullOrEmpty(((KPFormItemModelEntityField)columEntity).FieldNameDescription))
                            {
                                PropertyInfo propertyDescription = objGet.GetType().GetProperty(((KPFormItemModelEntityField)columEntity).FieldNameDescription);
                                if (propertyDescription != null)
                                    return propertyDescription.GetValue(objGet, null);
                            }
                            else
                            {
                                PropertyInfo propKey = objGet.GetType().GetEntityPrimaryKey();
                                return propKey.GetValue(objGet, null);
                            }
                        }

                        return objGet;
                    }
                }
                catch
                {
                    throw;
                }
            }

            return null;
        }

        private Control GetSaveButton(ComponentPermission componentPermission)
        {
            Button btnSave = new Button() { Text = KPGlobalizationLanguage.GetString("FRWBtnSave") };
            btnSave.Enabled = componentPermission.IsEnabled;
            btnSave.Click += new EventHandler(btnSave_Click);
            if (!String.IsNullOrWhiteSpace(HelpToolTipSave))
                btnSave.Attributes.Add("title", KPGlobalizationLanguage.GetString(HelpToolTipSave));

            return btnSave;
        }

        private Button GetCloseButton()
        {
            Button btnClose = new Button() { Text = KPGlobalizationLanguage.GetString("FRWBtnClose") };
            btnClose.Attributes.Add("class", "KPFormCloseButton");
            btnClose.Click += new EventHandler(btnClose_Click);
            if (!String.IsNullOrWhiteSpace(HelpToolTipClose))
                btnClose.Attributes.Add("title", KPGlobalizationLanguage.GetString(HelpToolTipClose));

            return btnClose;
        }

        private void btnSourceView(object sender, EventArgs e)
        {
            /*
            FileInfo fileView = new FileInfo(@"E:\Workspace.KP\Application.Net\WebSolution\WebProject\Form\Operacional\Cadastro\FrmItemView.aspx");
            string sourceView = File.ReadAllText(fileView.FullName);

            sourceView = KPGenericUtil.GetSourceViewKPForm(sourceView);

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

        protected virtual void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                SaveEntity();
            }
            catch
            {
                throw;
            }

        }

        public bool SaveEntity()
        {
            try
            {
                // Reset DataSource
                DataSourceAltered = CloneDataSource(DataSource, TypeEntity);

                //Pega os dados da tela e preenche o DataSource
                RefreshDataSourceAltered();

                if (DataSourceAltered != null)
                {
                    if (Validate(ValidateEntity()))
                    {
                        if (KPEventBeforeSaveClick != null)
                        {
                            KPButtonEventsArgs eventArgs = new KPButtonEventsArgs();
                            KPEventBeforeSaveClick(DataSourceAltered, eventArgs);
                            if (eventArgs.Cancel)
                                return false;
                        }

                        ConstructorInfo entityBO = TypBOEntity.GetConstructor(new[] { TypeEntity });
                        if (entityBO == null)
                        {
                            throw new Exception(String.Format("Could not find a valid Constructor to this Entity ({0})", TypeEntity.FullName));
                        }
                        object entityBOInstance = entityBO.Invoke(new object[] { DataSourceAltered });

                        MethodInfo methodValidate = TypBOEntity.GetMethod("Validate");
                        if (methodValidate == null)
                        {
                            throw new Exception(String.Format("Could not find a valid 'Validate' method to this EntityBO ({0})", TypBOEntity.FullName));
                        }
                        object validateReturn = methodValidate.Invoke(entityBOInstance, null);

                        if (validateReturn != null && !((bool)validateReturn))
                        {
                            PropertyInfo propInvalidValues = TypBOEntity.GetProperty("InvalidValues");
                            List<InvalidValue> invalidValues = propInvalidValues.GetValue(entityBOInstance, null) as List<InvalidValue>;
                            if (invalidValues.Count > 0)
                            {
                                if (!Validate(invalidValues.ToArray()))
                                    return false;
                            }

                            PropertyInfo propInvalidEntity = TypBOEntity.GetProperty("InvalidEntityHeader");
                            List<InvalidValueBO> invalidEntityValues = propInvalidEntity.GetValue(entityBOInstance, null) as List<InvalidValueBO>;
                            if (invalidEntityValues.Count > 0)
                            {
                                this.ErrorsView.Visible = true;
                                StringBuilder erroMsg = new StringBuilder();
                                foreach (InvalidValueBO invalidValueBO in invalidEntityValues)
                                {
                                    erroMsg.AppendFormat(" - {0}<br>", invalidValueBO.Error);
                                }
                                this.ErrorsView.InnerHtml = erroMsg.ToString();
                                return false;
                            }
                        }

                        if (KPEventSaveClick != null)
                        {
                            KPEventSaveClick(entityBOInstance);
                        }
                        else
                        {
                            MethodInfo methodSave = TypBOEntity.GetMethod("SaveEntityBase");
                            object entitySaved = methodSave.Invoke(entityBOInstance, null);
                            DataSource = entitySaved;
                            AfterSaveEntity(DataSource as ActiveRecordBase);

                            if (KPEventAfterSaveClick != null)
                            {
                                KPEventAfterSaveClick(entityBOInstance);
                            }
                        }

                        var eventFormArgs = new KPFormEventArgs();
                        eventFormArgs.Entity = this.DataSourceAltered;
                        if (KPEventFormClosing != null)
                            KPEventFormClosing(this, eventFormArgs);

                        ClosePage();
                    }
                    else
                    {
                        return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null && ex.InnerException is KPExceptionValidator)
                {
                    KPExceptionValidator exception = ex.InnerException as KPExceptionValidator;
                    this.ErrorsView.Visible = true;
                    StringBuilder erroMsg = new StringBuilder();
                    foreach (InvalidValue error in exception.Erros)
                    {
                        erroMsg.AppendFormat(" - {0} <br>", error.Message);
                    }
                    this.ErrorsView.InnerHtml = erroMsg.ToString();

                    return false;
                }

                throw ex;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            var eventArgs = new KPFormEventArgs();
            if (KPEventFormClosing != null)
            {
                eventArgs.Entity = this.DataSourceAltered;
                KPEventFormClosing(this, eventArgs);
            }

            if (!eventArgs.Cancel)
                ClosePage();
        }

        public KPFormItemFactory CreateFormItemFactory(KPFormItemModelCollection columnsModel, int? tabIndex)
        {
            KPFormItemFactory formItemFactory = new KPFormItemFactory(this);

            foreach (KPFormItemModel obj in columnsModel)
            {
                obj.IndexTab = tabIndex - 1;
                formItemFactory.AddFormItem(obj, GetValue(obj));
            }

            return formItemFactory;
        }

        public void ClosePage()
        {
            if (KPSessionData != null)
            {
                ClosePageAndReloadView();
            }
            else
            {
                string script = String.Format(@"window.parent.closeWindow(""{0}"");",
                    new System.IO.FileInfo(this.Page.Request.CurrentExecutionFilePath).Name);

                if (!String.IsNullOrWhiteSpace(OnCloseScriptTargetParent))
                    script += OnCloseScriptTargetParent;

                ScriptManager.RegisterClientScriptBlock(this.Page, Page.GetType(), "CloseWindow", script, true);
            }

            try
            {
                if (PageBase != null)
                {
                    var pagePermission = PageBase.SecuritySession.GetPagePermission(PageBase.PageEnum);
                    LogHelper.Log(String.Format("Usuário [{0}] fechou a página [{1}]", PageBase.SecuritySession.Login, pagePermission.PageTitle), PageBase.SecuritySession.Login, pagePermission.PageId);
                }
            }
            catch { }
        }

        public void ScriptReloadGrid()
        {
            string refreshGrid = String.Empty;

            if (KPSessionData != null && !String.IsNullOrEmpty(KPSessionData.GridTableID))
            {
                refreshGrid = String.Format(@"$('#{0}').trigger('reloadGrid');", KPSessionData.GridTableID);
                ScriptManager.RegisterClientScriptBlock(this.Page, Page.GetType(), "ReloadGrid", refreshGrid, true);
            }
        }

        private void ClosePageAndReloadView()
        {
            if (KPSessionData != null)
            {
                string refreshGrid = String.Format(@"$('#{0}').trigger('reloadGrid');", KPSessionData.GridTableID);

                if (!String.IsNullOrWhiteSpace(OnCloseScriptTargetParent))
                    refreshGrid += OnCloseScriptTargetParent;

                ScriptManager.RegisterClientScriptBlock(this.Page, Page.GetType(), "CloseWindow",
                            String.Format(@"window.parent.closeWindow(""{0}"", ""{1}"");",
                                            new System.IO.FileInfo(this.Page.Request.CurrentExecutionFilePath).Name,
                                            refreshGrid), true);
            }
        }

        protected virtual void AfterSaveEntity(ActiveRecordBase entity)
        {
            try
            {
                if (KPFormMasterDetailConfig != null && KPFormMasterDetailConfig.Count > 0)
                {
                    foreach (KPFormMasterDetailModel item in KPFormMasterDetailConfig)
                    {
                        string masterDetailID = item.MasterDetailID;
                        DetailSession detailSession = KPSessionHelper.GetSessionMasterDetailList(PageBase.SessionPageID, masterDetailID);
                        if (detailSession != null)
                        {
                            foreach (DetailEntity detailEntity in detailSession.DetailsEntity)
                            {
                                detailEntity.Entity.Delete();
                            }

                            foreach (var objEntity in detailSession.Entities)
                            {
                                var entityDetail = (objEntity as ActiveRecordBase);
                                if (entityDetail != null)
                                {
                                    var entitySaving = SetDetailsKeys(entity, entityDetail, item.KeyFieldsConfig);
                                    PropertyInfo propPK = entitySaving.GetType().GetEntityPrimaryKey();
                                    object valuePK = propPK.GetValue(entitySaving, null);
                                    int valuePKInt = 0;
                                    if (Int32.TryParse(valuePK.ToString(), out valuePKInt))
                                    {
                                        if (valuePKInt < 0)
                                            propPK.SetValue(entitySaving, 0, null);

                                        entitySaving.Save();
                                    }
                                }
                            }
                        }
                        KPSessionHelper.RemoveSessionMasterDetailList(PageBase.SessionPageID, masterDetailID);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected List<PropertyInfo> GetFieldNameKeys()
        {
            List<PropertyInfo> fieldNameKeyList = new List<PropertyInfo>();
            var formItemKeys = this.KPColumnsModel.Where(x => x.GetType() == typeof(KPFormItemKey));
            if (formItemKeys != null && formItemKeys.Count() > 0)
            {
                foreach (var item in formItemKeys)
                {
                    string fieldName = (item as KPFormItemKey).FieldName;
                    if (!String.IsNullOrWhiteSpace(fieldName))
                    {
                        var prop = TypeEntity.GetProperty(fieldName);
                        fieldNameKeyList.Add(prop);
                    }
                }
            }

            return fieldNameKeyList;
        }



    }
}
