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
using KPCore.KPException;

namespace KPComponents
{
    /// <summary>
    /// <para>Authors: Juliano Tiago Rinaldi and 
    /// Tiago Antonio Jacobi</para>
    /// </summary>
    [ToolboxData(@"<{0}:KPZoomField runat=""server"" FieldName=""FIELD_NAME"" />")]
    public class KPZoomField : WebControl, IKPComponentData
    {
        #region IdComponents
        private string TEXTBOX_ZOOM_DESC_ID;
        private string KPTEXTBOX_ZOOM_FIELD_ID;
        private string KPHIDDEN_ZOOM_FIELD_ID;
        private string KPIMG_BUTTOM_ZOOM_ID;
        #endregion

        #region Properties
        public KPFormBaseControl FormBaseControl { get; private set; }

        public KPFormItemZoom ItemZoom { get; private set; }

        public KPFormZoomModel ZoomConfig { get; private set; }

        public string Value
        {
            get { return HiddenZoomIDField.Value; }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    HiddenZoomIDField.Value = value;
                    GetZoomDataByID(value);
                }
                else
                    SetNullOrEmptyZoomField();
            }
        }

        public string ValueViewed
        {
            get { return TextBoxZoomField.Text; }
            set
            {
                if (!String.IsNullOrEmpty(value))
                    TextBoxZoomField.Text = value;
                else
                    SetNullOrEmptyZoomField();
            }
        }

        public string Description
        {
            get { return DescriptionZoomField.Text; }
            set
            {
                if (!String.IsNullOrEmpty(value))
                    DescriptionZoomField.Text = value;
                else
                    SetNullOrEmptyZoomField();
            }
        }

        public override Unit Width
        {
            get
            {
                if (TextBoxZoomField.Width.Value < 50)
                    TextBoxZoomField.Width = 50;
                return TextBoxZoomField.Width;
            }
            set
            {
                TextBoxZoomField.Width = value;
            }
        }

        public Unit DescriptionWidth
        {
            get
            {
                if (DescriptionZoomField.Width.Value < 50)
                    DescriptionZoomField.Width = 50;
                return DescriptionZoomField.Width;
            }
            set
            {
                DescriptionZoomField.Width = value;
            }
        }

        public string FieldName
        {
            get
            {
                return ItemZoom.FieldName;
            }
            set { }
        }
        #endregion

        #region Components Child Control
        private KPHiddenControl HiddenZoomIDField
        {
            get
            {
                EnsureChildControls();
                KPHiddenControl obj = this.BetterFindControl<KPHiddenControl>(KPHIDDEN_ZOOM_FIELD_ID);
                return obj;
            }
        }

        private KPTextBoxZoomField TextBoxZoomField
        {
            get
            {
                EnsureChildControls();
                KPTextBoxZoomField obj = this.BetterFindControl<KPTextBoxZoomField>(KPTEXTBOX_ZOOM_FIELD_ID);
                return obj;
            }
        }

        private TextBox DescriptionZoomField
        {
            get
            {
                EnsureChildControls();
                TextBox obj = this.BetterFindControl<TextBox>(TEXTBOX_ZOOM_DESC_ID);
                return obj;
            }
        }

        private ImageButton ButtonZoom
        {
            get
            {
                EnsureChildControls();
                ImageButton obj = this.BetterFindControl<ImageButton>(KPIMG_BUTTOM_ZOOM_ID);
                return obj;
            }
        }

        private KPZoomControl ZoomControl
        {
            get
            {
                EnsureChildControls();
                KPZoomControl obj = this.BetterFindControl<KPZoomControl>(String.Format("KPZoomControl_{0}", ZoomConfig.ZoomID));
                return obj;
            }
        }

        #endregion

        #region Contructor
        public KPZoomField(KPFormBaseControl formControl, KPFormItemZoom itemZoom, KPFormZoomModel zoomConfig)
        {
            FormBaseControl = formControl;
            TEXTBOX_ZOOM_DESC_ID = String.Format("TextBox_{0}", zoomConfig.ZoomID);
            KPTEXTBOX_ZOOM_FIELD_ID = String.Format("KPZoomField_{0}", zoomConfig.ZoomID);
            KPHIDDEN_ZOOM_FIELD_ID = String.Format("KPHiddenZoomField_{0}", zoomConfig.ZoomID);
            KPIMG_BUTTOM_ZOOM_ID = String.Format("KPImageButtom_{0}", zoomConfig.ZoomID);
            ItemZoom = itemZoom;
            ZoomConfig = zoomConfig;
            EnsureChildControls();
        }
        #endregion

        #region Override Methods
        protected override void CreateChildControls()
        {
            this.Controls.Clear();
            HtmlGenericControl htmlGenWindow = new HtmlGenericControl("div");
            htmlGenWindow.Attributes.Add("class", "KPZoomField");

            KPHiddenControl hiddenZoomField = new KPHiddenControl()
            {
                ID = KPHIDDEN_ZOOM_FIELD_ID,
                Visible = false,
                FieldName = ItemZoom.FieldName,
            };

            string classMask = ItemZoom.Mask.GetTypeValue().ToString();
            KPTextBoxZoomField textBoxZoomField = new KPTextBoxZoomField()
            {
                ID = KPTEXTBOX_ZOOM_FIELD_ID,
                CssClass = String.Format("{0} {1}", KPCssClass.ControlInput, classMask),
                FieldName = ItemZoom.FieldName,
                Mask = ItemZoom.Mask,
                AutoPostBack = true,
                Enabled = this.Enabled
            };

            if (ItemZoom.KPZoomLostFocusDelegate != null)
            {
                textBoxZoomField.TextChanged += delegate(object senderChanged, EventArgs events)
                {
                    if (!String.IsNullOrEmpty(textBoxZoomField.Text))
                        AutoChoiceSearchZoom(textBoxZoomField.Text);
                    else
                        SetNullOrEmptyZoomField();

                    ItemZoom.KPZoomLostFocusDelegate();
                    if (ButtonZoom != null)
                    {
                        ButtonZoom.Focus();
                    }

                    if (ItemZoom.IndexTab != null)
                    {
                        KPTabControl.SetTabIndex(this.Page, Int32.Parse(ItemZoom.IndexTab.ToString()));
                    }
                };
            }
            else
            {
                textBoxZoomField.TextChanged += delegate(object senderChanged, EventArgs events)
                {
                    if (!String.IsNullOrEmpty(textBoxZoomField.Text))
                        AutoChoiceSearchZoom(textBoxZoomField.Text);
                    else
                        SetNullOrEmptyZoomField();

                    if (ButtonZoom != null)
                    {
                        ButtonZoom.Focus();
                    }

                    if (ItemZoom.IndexTab != null)
                    {
                        KPTabControl.SetTabIndex(this.Page, Int32.Parse(ItemZoom.IndexTab.ToString()));
                    }
                };
            }

            ImageButton buttonZoom = new ImageButton() { ID = KPIMG_BUTTOM_ZOOM_ID };
            buttonZoom.ImageUrl = "~/Assets/Imgs/Themes/Default/Zoom.png";
            buttonZoom.Attributes.Add("class", "KPZoomButton");
            buttonZoom.Attributes.Add("onclick", "setLoading(true);");
            buttonZoom.Enabled = this.Enabled;

            TextBox lblZoom = new TextBox()
            {
                ID = TEXTBOX_ZOOM_DESC_ID,
            };
            lblZoom.Attributes.Add("class", "KPZoomDescription");
            lblZoom.Attributes.Add("disabled", "true");
            if (ItemZoom.DescriptionWidth == 0)
                lblZoom.Visible = false;

            KPZoomControl zoomControl = new KPZoomControl(ZoomConfig, ZoomConfig.KPZoomFieldsConfig) { ID = String.Format("KPZoomControl_{0}", ZoomConfig.ZoomID) };

            buttonZoom.Click += new ImageClickEventHandler(buttonZoom_Click);
            zoomControl.KPEventZoomClickOK += new KPZoomClick(zoomControl_OnKPEventZoomClickOK);
            zoomControl.KPEventZoomClickClose += new KPZoomClick(zoomControl_OnKPEventZoomClickClose);

            htmlGenWindow.Controls.Add(hiddenZoomField);
            htmlGenWindow.Controls.Add(textBoxZoomField);
            htmlGenWindow.Controls.Add(buttonZoom);
            htmlGenWindow.Controls.Add(lblZoom);
            htmlGenWindow.Controls.Add(zoomControl);

            this.Controls.Add(htmlGenWindow);
        }

        public override string CssClass
        {
            get
            {
                return TextBoxZoomField.CssClass;
            }
            set
            {
                TextBoxZoomField.CssClass = value;
            }
        }
        #endregion

        #region Events
        private void buttonZoom_Click(object sender, ImageClickEventArgs e)
        {
            OpenZoomWindow();
            ((KPPageBase)this.Page).SetLoading(false);
        }

        private void zoomControl_OnKPEventZoomClickClose(string fieldReturnId)
        {
            if (ButtonZoom != null)
            {
                ButtonZoom.Focus();
            }

            if (ItemZoom.IndexTab != null)
            {
                KPTabControl.SetTabIndex(this.Page, Int32.Parse(ItemZoom.IndexTab.ToString()));
            }
        }

        private void zoomControl_OnKPEventZoomClickOK(string fieldReturnId)
        {
            RemoveInvalidateMsg();
            int searchId = 0;
            if (Int32.TryParse(fieldReturnId, out searchId))
            {
                GetZoomDataByID(searchId);
            }

            if (ButtonZoom != null)
            {
                ButtonZoom.Focus();
            }

            if (ItemZoom.IndexTab != null)
            {
                KPTabControl.SetTabIndex(this.Page, Int32.Parse(ItemZoom.IndexTab.ToString()));
            }
        }
        #endregion

        #region Methods
        private void OpenZoomWindow()
        {
            EnsureChildControls();
            ZoomControl.Visible = true;
            string scriptKPWindow = "";

            if (ItemZoom.IndexTab != null)
            {
                scriptKPWindow = KPTabControl.GetTabIndexScript(Int32.Parse(ItemZoom.IndexTab.ToString()));
            }

            scriptKPWindow += String.Format("KPZoomWindow(true, '{0}', '{1}')", ZoomConfig.WidthZoom, ZoomConfig.HeightZoom);

            ScriptManager.RegisterClientScriptBlock(this.Page, Page.GetType(), "KPZoomWindow", scriptKPWindow, true);
        }

        /// <summary>
        /// Méthod for clear and set null on field called
        /// </summary>
        private void SetNullOrEmptyZoomField()
        {
            if (HiddenZoomIDField != null)
                HiddenZoomIDField.Value = null;

            if (TextBoxZoomField != null)
                TextBoxZoomField.Text = String.Empty;

            if (DescriptionZoomField != null)
                DescriptionZoomField.Text = String.Empty;

            RemoveInvalidateMsg();
        }

        private void AutoChoiceSearchZoom(string searchValue)
        {
            if (HiddenZoomIDField != null)
            {
                HiddenZoomIDField.Value = String.Empty;
            }
            if (DescriptionZoomField != null)
            {
                DescriptionZoomField.Text = String.Empty;
            }

            RemoveInvalidateMsg();

            bool searchById = IndentifySearchByID();
            int searchId = 0;
            if (Int32.TryParse(searchValue, out searchId) && searchById)
            {
                if (!GetZoomDataByID(searchId))
                {
                    CallZoomCriticize(searchValue);
                }
            }
            else
            {
                CallZoomCriticize(searchValue);
            }
        }

        /// <summary>
        /// Identifica no Zoom se a procura no banco será pelo ID ou pelo SearchByField
        /// </summary>
        /// <returns></returns>
        private bool IndentifySearchByID()
        {
            bool searchById = false;
            if (String.IsNullOrEmpty(ZoomConfig.SearchByField))
                searchById = true;

            return searchById;
        }

        private void CallZoomCriticize(string searchValue)
        {
            if (!GetZoomDataBySearch(searchValue))
            {
                if (ZoomConfig.OpenZoomSearchNotFound)
                    OpenZoomWindow();
                else
                {
                    TextBoxZoomField.CssClass += " " + KPCssClass.InvalidateField;
                    TextBoxZoomField.Attributes["title"] = String.Format("'{0}' não foi encontrado.", searchValue);
                    TextBoxZoomField.Text = String.Empty;
                }
            }
        }

        private bool GetZoomDataBySearch(string searchValue)
        {
            bool hasSearched = false;

            if (TextBoxZoomField != null && !String.IsNullOrEmpty(searchValue))
            {
                if (!String.IsNullOrEmpty(ZoomConfig.SearchByField))
                {
                    PropertyInfo prop = ZoomConfig.TypeEntity.GetProperty(ZoomConfig.SearchByField);
                    if (prop != null)
                    {
                        Type propFieldType = prop.PropertyType;
                        if (typeof(KPActiveRecordBase<>).IsSubclassOfRawGeneric(ZoomConfig.TypeEntity))
                        {

                            MethodInfo method = ZoomConfig.TypeEntity.GetMethodInheritance("SlicedFindAll", new Type[] { typeof(int), typeof(int), typeof(Order[]), typeof(ICriterion[]) });
                            object zoomSearchReturn = null;
                            if (method != null)
                            {
                                object valueTyped = Convert.ChangeType(searchValue, propFieldType);
                                ICriterion criterionSearch = null;
                                if (valueTyped is String)
                                    criterionSearch = new LikeExpression(ZoomConfig.SearchByField, valueTyped.ToString(), MatchMode.Anywhere);
                                else
                                    criterionSearch = Expression.Eq(ZoomConfig.SearchByField, valueTyped);

                                ICriterion filter = null;
                                Order order = null;
                                if (ZoomConfig.KPCriterionFilterDelegate != null)
                                    filter = ZoomConfig.KPCriterionFilterDelegate();
                                if (ZoomConfig.KPOrderDelegate != null)
                                    order = ZoomConfig.KPOrderDelegate();

                                Conjunction conj = new Conjunction();
                                conj.Add(criterionSearch);
                                if (filter != null)
                                    conj.Add(filter);

                                zoomSearchReturn = method.Invoke(null, new object[] { 0, 2, order, new ICriterion[] { conj } });
                            }

                            if (zoomSearchReturn != null)
                            {
                                if (((Array)zoomSearchReturn).Length == 1)
                                {
                                    hasSearched = true;
                                    foreach (var item in (Array)zoomSearchReturn)
                                    {
                                        object returnID = item.GetType().GetProperty(ZoomConfig.FieldReturnId).GetValue(item, null);
                                        if (returnID != null)
                                            HiddenZoomIDField.Value = returnID.ToString();

                                        if (!String.IsNullOrEmpty(ZoomConfig.DisplaySearchField))
                                        {
                                            object returnSearch = item.GetType().GetProperty(ZoomConfig.DisplaySearchField).GetValue(item, null);
                                            if (returnSearch != null)
                                                TextBoxZoomField.Text = returnSearch.ToString();
                                        }
                                        else
                                        {
                                            TextBoxZoomField.Text = HiddenZoomIDField.Value;
                                        }

                                        object labelText = item.GetType().GetProperty(ZoomConfig.FieldReturnText).GetValue(item, null);
                                        if (labelText != null)
                                            DescriptionZoomField.Text = labelText.ToString();
                                    }
                                }
                                else if (((Array)zoomSearchReturn).Length > 1)
                                {
                                    OpenZoomWindow();
                                }
                            }
                        }
                    }
                }
            }

            return hasSearched;
        }

        private bool GetZoomDataByID(object idValue)
        {
            bool hasSearched = false;

            PropertyInfo prop = ZoomConfig.TypeEntity.GetProperty(ZoomConfig.FieldReturnId);
            if (prop != null)
            {
                Type propFieldType = prop.PropertyType;
                if (typeof(KPActiveRecordBase<>).IsSubclassOfRawGeneric(ZoomConfig.TypeEntity))
                {
                    MethodInfo method = ZoomConfig.TypeEntity.GetMethodInheritance("SlicedFindAll", new Type[] { typeof(int), typeof(int), typeof(Order[]), typeof(ICriterion[]) });
                    object zoomReturn = null;
                    if (method != null)
                    {
                        ICriterion filter = null;
                        Order order = null;
                        if (ZoomConfig.KPCriterionFilterDelegate != null)
                            filter = ZoomConfig.KPCriterionFilterDelegate();
                        if (ZoomConfig.KPOrderDelegate != null)
                            order = ZoomConfig.KPOrderDelegate();

                        try
                        {
                            Conjunction conj = new Conjunction();

                            if (FormBaseControl != null 
                                        && !String.IsNullOrWhiteSpace(ZoomConfig.PropertyCompanyEntity)
                                        && !String.IsNullOrWhiteSpace(FormBaseControl.PropertyCompanyEntity) 
                                        && !FormBaseControl.PropertyCompanyEntity.Equals("!")
                                        && !ZoomConfig.PropertyCompanyEntity.Equals("!"))
                            {
                                if (ZoomConfig.TypeEntity != null)
                                {
                                    PropertyInfo propZoomCompany = ZoomConfig.TypeEntity.GetProperty(ZoomConfig.PropertyCompanyEntity);
                                    if (propZoomCompany != null)
                                    {
                                        object dataSource = FormBaseControl.DataSourceAltered;
                                        if (dataSource != null)
                                        {
                                            object company = dataSource.GetType().GetProperty(FormBaseControl.PropertyCompanyEntity).GetValue(dataSource, null);
                                            if (company != null)
                                                conj.Add(Expression.Eq(propZoomCompany.Name, company));
                                        }
                                    }
                                }
                            }

                            ICriterion filterID = Expression.Eq(prop.Name, Convert.ChangeType(idValue, propFieldType));
                            conj.Add(filterID);
                            if (filter != null)
                                conj.Add(filter);

                            zoomReturn = method.Invoke(null, new object[] { 0, 1, order, new ICriterion[] { conj } });
                        }
                        catch
                        { return false; }
                    }

                    if (zoomReturn != null && ((Array)zoomReturn).Length > 0)
                    {
                        zoomReturn = ((Array)zoomReturn).GetValue(0);
                        hasSearched = true;

                        if (HiddenZoomIDField != null)
                            HiddenZoomIDField.Value = idValue.ToString();

                        if (TextBoxZoomField != null)
                        {
                            string propertyValue = String.Empty;
                            if (!String.IsNullOrEmpty(ZoomConfig.DisplaySearchField))
                                propertyValue = ZoomConfig.DisplaySearchField;
                            else
                                propertyValue = ZoomConfig.FieldReturnId;

                            object returnSearch = zoomReturn.GetType().GetProperty(propertyValue).GetValue(zoomReturn, null);
                            if (returnSearch != null)
                            {
                                string oldValue = TextBoxZoomField.Text;
                                TextBoxZoomField.Text = returnSearch.ToString();

                                if (ItemZoom.KPZoomLostFocusDelegate != null)
                                {
                                    if (!oldValue.Equals(TextBoxZoomField.Text, StringComparison.InvariantCultureIgnoreCase))
                                    {
                                        ItemZoom.KPZoomLostFocusDelegate();

                                        if (ItemZoom.IndexTab != null)
                                        {
                                            KPTabControl.SetTabIndex(this.Page, Int32.Parse(ItemZoom.IndexTab.ToString()));
                                        }
                                    }
                                }
                            }
                        }

                        if (!String.IsNullOrWhiteSpace(ZoomConfig.FieldReturnText))
                        {
                            PropertyInfo propField = zoomReturn.GetType().GetProperty(ZoomConfig.FieldReturnText);
                            if (propField == null)
                                throw new KPExceptionSuggestion(String.Format("Não foi encontrado a propriedade '{0}'. Verificar nome da propriedade na Entidade {1}.", ZoomConfig.FieldReturnText, ZoomConfig.TypeEntityNamespace));

                            object labelText = propField.GetValue(zoomReturn, null);
                            if (labelText != null)
                                DescriptionZoomField.Text = labelText.ToString();
                        }
                    }
                }
            }

            return hasSearched;
        }

        public void SetInvalidateMsg(string errorMsg)
        {
            this.Attributes.Add("title", errorMsg);
            this.CssClass += " " + KPCssClass.InvalidateField;
        }

        public void RemoveInvalidateMsg()
        {
            this.Attributes.Remove("title");
            this.CssClass = this.CssClass.Replace(KPCssClass.InvalidateField, String.Empty);
        }
        #endregion
    }
}
