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
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web.UI;
using KPComponents.KPDelegate;
using KPGlobalization;

namespace KPComponents
{
    /// <summary>
    /// This is the Password item component developed to be used inner the forms.
    /// <para>Authors: Juliano Tiago Rinaldi and 
    /// Tiago Antonio Jacobi</para>
    /// </summary>
    /// <seealso cref="KPFormItemModelField"/>
    [ToolboxData(@"<{0}:KPFormItemPassword runat=server FieldName="""" />")]
    public class KPFormItemPassword : KPFormItemModelField
    {

        #region Properties

        /// <summary>
        /// The Title property for the field
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
        /// Only get the KP text password lost focus Delegate event.
        /// </summary>
        /// <seealso cref="KPDelegate.KPTextPasswordLostFocus"/>
        [Browsable(false)]
        public KPTextPasswordLostFocus KPTextPasswordLostFocusDelegate
        {
            private set;
            get;
        }

        /// <summary>
        /// Add or remove the Password Lost Focus event for the Text Password component
        /// </summary>
        /// <seealso cref="KPDelegate.KPTextPasswordLostFocus"/>
        public event KPTextPasswordLostFocus KPEventTextLostFocus
        {
            add { KPTextPasswordLostFocusDelegate += value; }
            remove { KPTextPasswordLostFocusDelegate -= value; }
        }

        #endregion Events
        
    }
}
