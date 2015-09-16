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
using KPEnumerator.KPComponents;

namespace KPComponents
{
    /// <summary>
    /// <para>Authors: Juliano Tiago Rinaldi and 
    /// Tiago Antonio Jacobi</para>
    /// </summary>
    public class KPFormItemDateTimeControl : KPFormItemControlBase<KPFormItemDateTime>
    {
        private string ControlFieldID;
        private string ControlLabelID;

        public override KPFormItemDateTime FormItem
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

        public KPTextBoxField ControlField
        {
            get
            {
                EnsureChildControls();
                KPTextBoxField obj = this.BetterFindControl<KPTextBoxField>(ControlFieldID);
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

        public KPFormItemDateTimeControl(KPFormBaseControl formControl, KPFormItemDateTime formItem, object objValue)
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

            #region Field
            string classType = FormItem.Type.GetTypeValue().ToString();
            KPTextBoxField kpTextBoxField = new KPTextBoxField()
            {
                ID = ControlFieldID,
                Text = ItemValue == null ? String.Empty : ItemValue.ToString(),
                Width = FormItem.Width,
                FieldName = FormItem.FieldName,
                CssClass = String.Format("{0} {1}", KPCssClass.ControlInput, classType),
                Mask = KPMaskTypeClassEnum.ALPHANUMERIC,
                Enabled = FormItem.Enabled
            };
            #endregion

            #region Delegates
            if (FormItem.KPTextLostFocusDelegate != null)
            {
                kpTextBoxField.AutoPostBack = true;
                kpTextBoxField.TextChanged += delegate(object sender, EventArgs e)
                {
                    FormItem.KPTextLostFocusDelegate();

                    if (FormItem.IndexTab != null)
                    {
                        KPTabControl.SetTabIndex(this.Page, Int32.Parse(FormItem.IndexTab.ToString()));
                    }
                };
            }
            #endregion

            this.Controls.Add(kpTextBoxField);
        }
    }
}
