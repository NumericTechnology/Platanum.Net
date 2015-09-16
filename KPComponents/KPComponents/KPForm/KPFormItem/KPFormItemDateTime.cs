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

using KPComponents.KPDelegate;
using KPEnumerator.KPComponents;
using KPGlobalization;
using System;
using System.ComponentModel;
using System.Web.UI;

namespace KPComponents
{
    /// <summary>
    /// This is the DateTime item component developed to be used inner the forms.
    /// <para>Authors: Juliano Tiago Rinaldi and 
    /// Tiago Antonio Jacobi</para>
    /// </summary>
    /// <seealso cref="KPFormItemModelField"/>
    [ToolboxData(@"<{0}:KPFormItemDateTime runat=server FieldName="""" Type=""DATE"" />")]
    public class KPFormItemDateTime : KPFormItemModelField
   { 

        #region Properties

        /// <summary>
        /// The title property for the combo box component
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

        /// <summary>
        /// The Type for the Date Time component, defined by the <see cref="KPEnumerator.KPComponents.KPDateTypeEnum">KPDateTypeEnum</see>.
        /// DefaultValue: KPDateTypeEnum.DATE
        /// </summary>
        [DefaultValue(KPDateTypeEnum.DATE)]
        public KPDateTypeEnum Type
        {
            get;
            set;
        }

        public bool Enabled
        {
            get
            {
                object o = ViewState["Enabled"];
                return o == null ? true : (bool)o;
            }
            set { ViewState["Enabled"] = value; }
        }
        #endregion Properties

        #region Events

        /// <summary>
        /// Only get the KP text lost focus Delegate event.
        /// </summary>
        /// <seealso cref="KPDelegate.KPTextLostFocus"/>
        [Browsable(false)]
        public KPTextLostFocus KPTextLostFocusDelegate
        {
            private set;
            get;
        }

        /// <summary>
        /// Add or remove the Lost Focus event for the DateTime component
        /// </summary>
        /// <seealso cref="KPDelegate.KPTextLostFocus"/>
        public event KPTextLostFocus KPEventTextLostFocus
        {
            add { KPTextLostFocusDelegate += value; }
            remove { KPTextLostFocusDelegate -= value; }
        }

        #endregion Events

    }
}
