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
using KPExtension;
using KPComponents.Asset;
using System.Reflection;
using System.Web.UI.WebControls;

namespace KPComponents
{
    /// <summary>
    /// <para>Authors: Juliano Tiago Rinaldi and 
    /// Tiago Antonio Jacobi</para>
    /// </summary>
    public class KPFormItemEntityControl : KPFormItemControlBase<KPFormItemEntity>
    {
        private string ControlFieldID;
        private string ControlLabelID;

        public override KPFormItemEntity FormItem
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

        public TextBox ControlField
        {
            get
            {
                EnsureChildControls();
                TextBox obj = this.BetterFindControl<TextBox>(ControlFieldID);
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

        public KPFormItemEntityControl(KPFormBaseControl formControl, KPFormItemEntity formItem, object objValue)
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


            TextBox kpTextBoxField = new TextBox()
            {
                ID = ControlFieldID,
                Text = ItemValue == null ? String.Empty : ItemValue.ToString(),
                Width = FormItem.Width,
                CssClass = KPCssClass.ControlInput,
                Enabled = false
            };

            this.Controls.Add(kpTextBoxField);
        }
    }
}
