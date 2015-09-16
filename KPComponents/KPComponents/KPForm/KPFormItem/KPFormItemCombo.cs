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
    /// This is the ComboBox item component developed to be used inner the forms.
    /// <para>Authors: Juliano Tiago Rinaldi and 
    /// Tiago Antonio Jacobi</para>
    /// </summary>
    /// <seealso cref="KPFormItemModelField"/>
    [ToolboxData(@"<{0}:KPFormItemCombo runat=""server"" FieldName="""" />")]
    public class KPFormItemCombo : KPFormItemModelField
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

        public bool Enabled
        {
            get
            {
                object o = ViewState["Enabled"];
                return o == null ? true : (bool)o;
            }
            set { ViewState["Enabled"] = value; }
        }

        /// <summary>
        /// The data name property for the combo box component
        /// P.S.: This is used when you want to load the combo box items using a table, so this property will be Entity Field to the combo items Label.
        /// </summary>
        public string DataName
        {
            get
            {
                object o = ViewState["DataName"];
                return o == null ? null : (String)o;
            }
            set { ViewState["DataName"] = value; }
        }


        /// <summary>
        /// The data value property for the combo box component
        /// P.S.: This is used when you want to load the combo box items using a table, so this property will be Entity Field to the combo items Value.
        /// </summary>
        public string DataValue
        {
            get
            {
                object o = ViewState["DataValue"];
                return o == null ? null : (String)o;
            }
            set { ViewState["DataValue"] = value; }
        }

        /// <summary>
        /// The namespace enum property for the combo box component.
        /// P.S.: This is used when you want to load the combo box items using an Enum. The label to each item must be set in the Enum using the attribute <see cref="KPAttributes.TypeDescription">TypeDescription</see>.
        /// </summary>
        public string NamespaceEnum
        {
            get
            {
                object o = ViewState["NamespaceEnum"];
                return o == null ? null : (String)o;
            }
            set { ViewState["NamespaceEnum"] = value; }
        }

        #endregion Properties

        #region Events

        /// <summary>
        /// Only get the KP select chage Delegate event.
        /// </summary>
        /// <seealso cref="KPDelegate.KPComboSelectChange"/>
        [Browsable(false)]
        public KPComboSelectChange KPComboSelectChangeDelegate
        {
            private set;
            get;
        }

        /// Only get the KP return items Delegate event.
        /// </summary>
        /// <seealso cref="KPDelegate.KPGetComboItems"/>
        [Browsable(false)]
        public KPGetComboItems KPGetComboItemsDelegate
        {
            private set;
            get;
        }

        /// <summary>
        /// Add or remove the Selection Changed event for the DateTime component
        /// </summary>
        /// <seealso cref="KPDelegate.KPComboSelectChange"/>
        public event KPComboSelectChange KPEventComboSelectChange
        {
            add { KPComboSelectChangeDelegate += value; }
            remove { KPComboSelectChangeDelegate -= value; }
        }

        /// <summary>
        /// Add or remove event
        /// </summary>
        /// <seealso cref="KPDelegate.KPGetComboItems"/>
        public event KPGetComboItems KPEventGetComboItems
        {
            add { KPGetComboItemsDelegate += value; }
            remove { KPGetComboItemsDelegate -= value; }
        }

        #endregion Events
    }
}
