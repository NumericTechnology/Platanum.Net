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

using KPComponents.Asset;
using KPComponents.KPData;
using KPEnumerator.KPComponents;
using KPExtension;
using System;

namespace KPComponents
{
    /// <summary>
    /// <para>Authors: Juliano Tiago Rinaldi and 
    /// Tiago Antonio Jacobi</para>
    /// </summary>
    public class KPTextBoxZoomField : System.Web.UI.WebControls.TextBox, IKPComponentData
    {

        #region Properties
        public string FieldName
        {
            get
            {
                object o = ViewState["FieldName"];
                return o == null ? null : (String)o;
            }
            set { ViewState["FieldName"] = value; }
        }

        public KPMaskTypeClassEnum Mask
        {
            get;
            set;
        }

        public override string Text
        {
            get
            {
                string returnText = base.Text;
                foreach (string item in Mask.GetMaskCharacteresEnum())
                    returnText = returnText.Replace(item, String.Empty);

                return returnText;
            }
            set
            {
                base.Text = value;
            }
        }
        #endregion

        #region Methods

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
        
        #region Events
        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            if (!KPMaskTypeClassEnum.ALPHANUMERIC.Equals(this.Mask))
            {
                string classMask = Mask.GetTypeValue().ToString();
                this.Attributes.Add("onfocus", ("maskIt(this, '" + classMask + "');"));
            }
        }
        #endregion
    }
}
