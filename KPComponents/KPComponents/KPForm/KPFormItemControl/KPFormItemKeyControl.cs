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

namespace KPComponents
{
    /// <summary>
    /// <para>Authors: Juliano Tiago Rinaldi and 
    /// Tiago Antonio Jacobi</para>
    /// </summary>
    public class KPFormItemKeyControl : KPFormItemControlBase<KPFormItemKey>
    {
        private string ControlFieldID;

        public override KPFormItemKey FormItem
        {
            get;
            protected set;
        }

        public override bool Enabled
        {
            get
            {
                return true;
            }
            set { }
        }

        public KPHiddenControl ControlField
        {
            get
            {
                EnsureChildControls();
                KPHiddenControl obj = this.BetterFindControl<KPHiddenControl>(ControlFieldID);
                return obj;
            }
        }

        public KPFormItemKeyControl(KPFormBaseControl formControl, KPFormItemKey formItem, object objValue)
            : base(formControl)
        {
            this.Attributes.Add("class", "Key");
            this.ID = formItem.ID;
            ControlFieldID = CreateIDField(formItem.ID);

            FormItem = formItem;
            ItemValue = objValue;
            EnsureChildControls();
        }

        protected override void CreateChildControls()
        {
            base.CreateChildControls();
            this.Controls.Add(new KPHiddenControl()
            {
                ID = ControlFieldID,
                FieldName = FormItem.FieldName,
                Visible = false,
                Value = ItemValue == null ? String.Empty : ItemValue.ToString()
            });
        }
    }
}
