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

using KPComponents.Generic;
using KPEnumerator.KPComponents;
using KPGlobalization;
using System;
using System.ComponentModel;
using System.Web.UI;

namespace KPComponents
{
    /// <summary>
    /// <para>Authors: Juliano Tiago Rinaldi and 
    /// Tiago Antonio Jacobi</para>
    /// </summary>
    [Serializable]
    public class KPItemModel : StateManagedItem
    {
        public KPItemModel()
        {
            Editable = false;
            Sortable = true;
            Width = 100;
        }

        #region Properties
        public int Width
        {
            get
            {
                object o = ViewState["Width"];
                return o == null ? 100 : (int)o;
            }
            set { ViewState["Width"] = value; }
        }

        public bool Sortable
        {
            get
            {
                object o = ViewState["Sortable"];
                return o == null ? false : (bool)o;
            }
            set { ViewState["Sortable"] = value; }
        }

        // TODO: Não será implementado neste momento
        // http://www.trirand.com/jqgridwiki/doku.php?id=wiki:common_rules
        [Browsable(false)]
        public bool Editable
        {
            get
            {
                object o = ViewState["Editable"];
                return o == null ? false : (bool)o;
            }
            set { ViewState["Editable"] = value; }
        }

        public string FieldName
        {
            get
            {
                object o = ViewState["FieldName"];
                return o == null ? null : (String)o;
            }
            set { ViewState["FieldName"] = value; }
        }

        public string HeaderName
        {
            get
            {
                object o = ViewState["HeaderName"];
                return o == null ? null : (String)o;
            }
            set { ViewState["HeaderName"] = KPGlobalizationLanguage.GetString(value); }
        }

        public bool Visible
        {
            get
            {
                object o = ViewState["Visible"];
                return o == null ? true : (bool)o;
            }
            set { ViewState["Visible"] = value; }
        }

        #endregion
    }
}
