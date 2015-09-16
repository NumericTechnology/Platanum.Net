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
using System.Web.UI.WebControls;
using KPComponents.Asset;
using KPComponents.KPForm;
using KPExtension;
using KPGlobalization;

namespace KPComponents
{
    /// <summary>
    /// <para>Authors: Juliano Tiago Rinaldi and 
    /// Tiago Antonio Jacobi</para>
    /// </summary>
    public class KPFormItemCheckBoxControl : KPFormItemControlBase<KPFormItemCheckBox>
    {
        private string ControlFieldID;
        private string ControlLabelID;

        public override KPFormItemCheckBox FormItem
        {
            get;
            protected set;
        }

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

        public KPCheckBoxField ControlField
        {
            get
            {
                EnsureChildControls();
                KPCheckBoxField obj = this.BetterFindControl<KPCheckBoxField>(ControlFieldID);
                return obj;
            }
        }

        public KPFormItemCheckBoxControl(KPFormBaseControl formControl, KPFormItemCheckBox formItem, object objValue)
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

            if (!FormItem.IsGroupStyleDisplay)
            {
                KPCaptionControl captionControl = new KPCaptionControl(ControlLabelID, "&nbsp;", FormItem);
                this.Controls.Add(captionControl);
            }


            string titleCaption = KPFormItemControlHelper.GetTitleNamePropertyEntity(KPGlobalizationLanguage.GetString(FormItem.Description),
                                                                                     FormItem.FieldName, TypeEntity);

            KPCheckBoxField checkBoxField = new KPCheckBoxField()
            {
                ID = ControlFieldID,
                Text = titleCaption,
                Checked = ItemValue != null ? Convert.ToBoolean(ItemValue) : FormItem.DefaultValue,
                Width = FormItem.Width,
                FieldName = FormItem.FieldName,
                CssClass = KPCssClass.ControlCheckBox,
                Height = 20,
            };

            if (FormItem.KPCheckBoxSelectDelegate != null)
            {
                checkBoxField.AutoPostBack = true;
                checkBoxField.CheckedChanged += delegate(object sender, EventArgs e)
                {
                    FormItem.KPCheckBoxSelectDelegate();
                };
            }

            this.Controls.Add(checkBoxField);
        }
    }
}
