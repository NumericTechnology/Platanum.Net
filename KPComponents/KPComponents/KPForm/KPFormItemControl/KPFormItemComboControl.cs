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
using KPExtension;
using KPComponents.Asset;
using System.Web.UI.WebControls;
using System.Reflection;
using KPCore.KPValidator;
using KPCore.KPUtil;
using NHibernate.Criterion;
using KPCore.KPSecurity;
using KPEntity.ORM;
using KPComponents.KPSecurity;

namespace KPComponents
{
    /// <summary>
    /// <para>Authors: Juliano Tiago Rinaldi and 
    /// Tiago Antonio Jacobi</para>
    /// </summary>
    public class KPFormItemComboControl : KPFormItemControlBase<KPFormItemCombo>
    {
        private string ControlFieldID;
        private string ControlLabelID;

        public override KPFormItemCombo FormItem { get; protected set; }

        public override bool Enabled
        {
            get
            {
                if (ControlField != null)
                    return ControlField.Enabled;

                return false;
            }
            set
            {
                if (ControlField != null)
                    ControlField.Enabled = value;
            }
        }

        public KPComboBoxField ControlField
        {
            get
            {
                EnsureChildControls();
                KPComboBoxField obj = this.BetterFindControl<KPComboBoxField>(ControlFieldID);
                return obj;
            }
        }

        public KPLabelControl ControlLabel
        {
            get
            {
                EnsureChildControls();
                KPCaptionControl obj = this.BetterFindControl<KPCaptionControl>(ControlLabelID);
                if (obj != null)
                    return obj.ControlLabel;

                return null;
            }
        }

        public KPFormItemComboControl(KPFormBaseControl formControl, KPFormItemCombo formItem, object objValue)
            : base(formControl)
        {
            this.ID = formItem.ID;
            ControlFieldID = CreateIDField(formItem.ID);
            ControlLabelID = CreateIDLabel(formItem.ID);

            FormItem = formItem;
            ItemValue = objValue;
            EnsureChildControls();
        }

        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            #region Caption
            string titleCaption = KPFormItemControlHelper.GetTitleNamePropertyEntity(FormItem.Title,
                                                                                     FormItem.FieldName, TypeEntity);
            KPCaptionControl captionControl = new KPCaptionControl(ControlLabelID, titleCaption, FormItem);

            this.Controls.Add(captionControl);
            #endregion


            KPComboBoxField comboBoxField = new KPComboBoxField();
            comboBoxField.ID = ControlFieldID;
            comboBoxField.FieldName = FormItem.FieldName;
            comboBoxField.Width = FormItem.Width;
            comboBoxField.CssClass = KPCssClass.ControlComboBox;
            comboBoxField.Enabled = FormItem.Enabled;

            // Use When property is FK Object Entity
            if (!String.IsNullOrEmpty(FormItem.DataName) && !String.IsNullOrEmpty(FormItem.DataValue))
            {
                PropertyInfo prop = TypeEntity.GetProperty(FormItem.FieldName);
                if (prop != null)
                {
                    Type propType = prop.PropertyType;
                    if (typeof(KPActiveRecordBase<>).IsSubclassOfRawGeneric(propType))
                    {
                        MethodInfo method = propType.GetMethodInheritance("FindAll", new Type[] { typeof(ICriterion[]) });
                        if (method != null)
                        {
                            object[] filterCompany = new object[] { new ICriterion[] { } };
                            if (!String.IsNullOrWhiteSpace(FormControl.PropertyCompanyEntity) && !FormControl.PropertyCompanyEntity.Equals("!"))
                            {
                                PropertyInfo propCompany = propType.GetProperty(FormControl.PropertyCompanyEntity);
                                if (propCompany != null)
                                {
                                    KPSecuritySession session = KPFormsAuthentication.SecuritySession;
                                    FrwCompany companyInstance = new FrwCompany() { IdCompany = session.FrwCompany };
                                    filterCompany = new object[] { new ICriterion[] { Expression.Eq(propCompany.Name, companyInstance) } };
                                }
                            }

                            object entityArray = method.Invoke(null, filterCompany);
                            comboBoxField.DataSource = entityArray;
                            comboBoxField.DataTextField = FormItem.DataName;
                            comboBoxField.DataValueField = FormItem.DataValue;
                            comboBoxField.DataBind();
                        }
                    }
                }
            }
            else if (!String.IsNullOrEmpty(FormItem.NamespaceEnum))
            {
                comboBoxField.Items.Clear();
                Type enumType = KPGenericUtil.GetTypeByNamespace(FormItem.NamespaceEnum);

                List<ListItem> listItem = new List<ListItem>();
                listItem.Add(new ListItem());
                foreach (var item in Enum.GetValues(enumType))
                {
                    listItem.Add(new ListItem(((Enum)item).GetTypeDescription(), item.GetHashCode().ToString()));
                }
                comboBoxField.Items.AddRange(listItem.ToArray());
            }
            else if (FormItem.KPGetComboItemsDelegate != null)
            {
                var objItems = FormItem.KPGetComboItemsDelegate();
                comboBoxField.DataSource = objItems;
                comboBoxField.DataBind();
            }

            if (ItemValue != null)
            {
                if (ItemValue is Enum)
                    comboBoxField.SelectedValue = ItemValue.GetHashCode().ToString();
                else
                    comboBoxField.SelectedValue = ItemValue.ToString();
            }

            if (FormItem.KPComboSelectChangeDelegate != null)
            {
                comboBoxField.AutoPostBack = true;
                comboBoxField.SelectedIndexChanged += delegate(object sender, EventArgs e)
                {
                    FormItem.KPComboSelectChangeDelegate();
                };
            }

            this.Controls.Add(comboBoxField);
        }
    }
}
