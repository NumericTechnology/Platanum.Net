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
using System.Web.UI.HtmlControls;
using System.Diagnostics;
using KPGlobalization;

namespace KPComponents
{
    /// <summary>
    /// The control for the KP Simple Form Button
    /// <para>Authors: Juliano Tiago Rinaldi and 
    /// Tiago Antonio Jacobi</para>
    /// </summary>
    /// <seealso cref="System.Web.UI.HtmlControls.HtmlGenericControl"/>
    public class KPFormButtonSimpleControl : HtmlGenericControl
    {
        /// <summary>
        /// The Id of the Control Field
        /// </summary>
        private string ControlFieldID;

        /// <summary>
        /// Only Get the Form Control.
        /// </summary>
        /// <seealso cref="KPFormControl"/>
        protected KPFormBaseControl FormControl { get; private set; }

        /// <summary>
        /// Only Get the Button Control
        /// </summary>
        /// <seealso cref="KPButtonControl"/>
        public KPButtonControl ControlButton
        {
            get
            {
                EnsureChildControls();
                KPButtonControl obj = this.BetterFindControl<KPButtonControl>(ControlFieldID);
                return obj;
            }
        }

        /// <summary>
        /// Only Get the Button Item
        /// </summary>
        /// <seealso cref="KPFormButtonSimple"/>
        public KPFormButtonSimple ButtonItem
        {
            get;
            protected set;
        }

        /// <summary>
        /// Contructor to the Simple Button Form Control
        /// </summary>
        /// <param name="formControl">The form control<see cref="KPFormControl"/></param>
        /// <param name="buttonItem">The Button Item<see cref="KPFormButtonSimple"/></param>
        public KPFormButtonSimpleControl(KPFormBaseControl formControl, KPFormButtonSimple buttonItem)
        {
            FormControl = formControl;
            ButtonItem = buttonItem;
            this.ID = ButtonItem.ID;
            ControlFieldID = String.Concat(ButtonItem.ID, "_control");
        }

        /// <summary>
        /// Called by the ASP.NET page framework to notify server controls that use composition-based implementation to create any child controls they contain in preparation for posting back or rendering.
        /// </summary>
        protected override void CreateChildControls()
        {
            KPButtonControl btn = new KPButtonControl();
            btn.ID = ControlFieldID;
            btn.Text = KPGlobalizationLanguage.GetString(ButtonItem.Title);
            if (!String.IsNullOrWhiteSpace(ButtonItem.HelpToolTip))
                btn.Attributes.Add("title", KPGlobalizationLanguage.GetString(ButtonItem.HelpToolTip));
            btn.Click += delegate(object sender, EventArgs e)
            {
                FormControl.RefreshDataSourceAltered();
                ButtonItem.OnClick(sender, e);
            };

            this.Controls.Add(btn);
        }
    }
}
