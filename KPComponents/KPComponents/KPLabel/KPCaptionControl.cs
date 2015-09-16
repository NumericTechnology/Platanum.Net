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
using KPGlobalization;

namespace KPComponents
{
    /// <summary>
    /// <para>Authors: Juliano Tiago Rinaldi and 
    /// Tiago Antonio Jacobi</para>
    /// </summary>
    public class KPCaptionControl : HtmlGenericControl
    {
        private string ControlLabelID;

        public KPLabelControl ControlLabel
        {
            get
            {
                EnsureChildControls();
                KPLabelControl obj = this.BetterFindControl<KPLabelControl>(ControlLabelID);
                return obj;
            }
        }

        public string Title { get; private set; }
        public string FieldName { get; private set; }
        public bool IsRequired { get; private set; }

        private KPFormItemModelField FormItem { get; set; }

        public KPCaptionControl(string id, string title, KPFormItemModelField formItem)
            : this(id, title, formItem.FieldName, formItem.IsRequired)
        {
            FormItem = formItem;
        }

        public KPCaptionControl(string id, string title, string fieldName, bool isRequired)
            : base("span")
        {
            this.Attributes.Add("class", "ControlCaption");
            this.ID = id;
            ControlLabelID = KPFormItemControlHelper.CreateIDLabel(this.ID);
            Title = title;
            FieldName = fieldName;
            IsRequired = isRequired;
            // EnsureChildControls();
        }

        protected override void CreateChildControls()
        {
            base.CreateChildControls();
            KPLabelControl lblField = new KPLabelControl()
            {
                ID = ControlLabelID,
                Text = Title,
                EnableViewState = true,
                FieldName = FieldName,
            };

            this.Controls.Add(lblField);

            if (IsRequired)
            {
                HtmlGenericControl htmlSpanReq = new HtmlGenericControl("span");
                htmlSpanReq.Attributes.Add("class", "Required");
                htmlSpanReq.InnerText = "*";

                this.Controls.Add(htmlSpanReq);
            }

            if (FormItem != null && !String.IsNullOrWhiteSpace(FormItem.HelpToolTip))
            {
                HtmlImage helpImage = new HtmlImage();
                helpImage.Src = "/Assets/Imgs/Themes/Default/help.png";
                helpImage.Attributes.Add("class", "HelpFieldImage");
                helpImage.Attributes.Add("title", KPGlobalizationLanguage.GetString(FormItem.HelpToolTip));

                this.Controls.Add(helpImage);
            }
        }
    }
}
