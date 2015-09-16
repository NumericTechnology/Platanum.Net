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

namespace KPComponents
{
    /// <summary>
    /// <para>Authors: Juliano Tiago Rinaldi and 
    /// Tiago Antonio Jacobi</para>
    /// </summary>
    public class KPFormItemButtonControl : KPFormItemControlBase<KPFormItemButton>
    {
        private string ControlFieldID;

        public override KPFormItemButton FormItem
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

        public KPButtonControl ControlField
        {
            get
            {
                EnsureChildControls();
                KPButtonControl obj = this.BetterFindControl<KPButtonControl>(ControlFieldID);
                return obj;
            }
        }

        public KPFormItemButtonControl(KPFormBaseControl formControl, KPFormItemButton formItem)
            : base(formControl)
        {
            this.ID = formItem.ID;
            ControlFieldID = CreateIDField(formItem.ID);
            FormItem = formItem;
            EnsureChildControls();
        }

        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            KPButtonControl buttonField = new KPButtonControl()
            {
                ID = ControlFieldID,
                Text = FormItem.Title,
                Width = FormItem.Width,
                CssClass = KPCssClass.ControlButton,
            };

            if (FormItem.KPClickDelegate != null)
            {
                buttonField.Click += delegate(object sender, EventArgs e)
                {
                    FormItem.KPClickDelegate(buttonField, EventArgs.Empty);
                };
            }

            this.Controls.Add(buttonField);
        }

    }
}
