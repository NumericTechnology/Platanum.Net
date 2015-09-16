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
    /// This is the Zoom item component developed to be used inner the forms.
    /// <para>Authors: Juliano Tiago Rinaldi and 
    /// Tiago Antonio Jacobi</para>
    /// </summary>
    /// <seealso cref="KPFormItemModelField"/>
    [ToolboxData(@"<{0}:KPFormItemZoom runat=server FieldName="""" Mask=""ALPHANUMERIC"" />")]
    public class KPFormItemZoom : KPFormItemModelField
    {
        
        #region Properties

        public string Title
        {
            get
            {
                object o = ViewState["Title"];
                return o == null ? null : (String)o;
            }
            set { ViewState["Title"] = KPGlobalizationLanguage.GetString(value); }
        }

        public int DescriptionWidth
        {
            get
            {
                object o = ViewState["DescriptionWidth"];
                return o == null ? 60 : (int)o;
            }
            set { ViewState["DescriptionWidth"] = value; }
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

        public string ZoomIDConfig
        {
            get
            {
                object o = ViewState["ZoomIDConfig"];
                return o == null ? null : (String)o;
            }
            set { ViewState["ZoomIDConfig"] = value; }
        }

        [DefaultValue(KPMaskTypeClassEnum.ALPHANUMERIC)]
        public KPMaskTypeClassEnum Mask
        {
            get;
            set;
        }

        #endregion Properties
        
        #region Events

        [Browsable(false)]
        public KPZoomLostFocus KPZoomLostFocusDelegate
        {
            private set;
            get;
        }

        public event KPZoomLostFocus KPEventZoomLostFocus
        {
            add { KPZoomLostFocusDelegate += value; }
            remove { KPZoomLostFocusDelegate -= value; }
        }

        #endregion Events

    }
}
