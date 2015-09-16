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
using System.Web.UI;
using System.ComponentModel;
using KPGlobalization;

namespace KPComponents
{
    /// <summary>
    /// This is the Buttom item component developed to be used inner the forms.
    /// <para>Authors: Juliano Tiago Rinaldi and 
    /// Tiago Antonio Jacobi</para>
    /// </summary>
    /// <seealso cref="KPFormItemModel"/>
    [ToolboxData(@"<{0}:KPFormItemButton runat=""server"" />")]
    public class KPFormItemButton : KPFormItemModel
    {
        
        #region Properties

        /// <summary>
        /// The button Title/Label.
        /// </summary>
        public string Title
        {
            get
            {
                object o = ViewState["Title"];
                return o == null ? null : (String)o;
            }
            set { ViewState["Title"] = KPGlobalizationLanguage.GetString(value); }
        }

        #endregion Properties

        #region Events

        /// <summary>
        /// Only Get the KPClickDelegate to the Button. Internally used.
        /// </summary>
        /// <seealso cref="System.EventHandler"/>
        [Browsable(false)]
        public EventHandler KPClickDelegate
        {
            private set;
            get;
        }

        /// <summary>
        /// The Click Event handler to the Button.
        /// </summary>
        /// <seealso cref="System.EventHandler"/>
        public event EventHandler KPClick
        {
            add { KPClickDelegate += value; }
            remove { KPClickDelegate -= value; }
        }

        #endregion Events

    }
}
