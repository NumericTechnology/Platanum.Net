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
using System.ComponentModel;
using System.Web.UI;
using KPComponents.Generic;
using KPComponents.KPDelegate;
using KPComponents.KPForm;

namespace KPComponents
{
    /// <summary>
    /// This is a Form grid model, used to comfigure the Grid Component in a form.
    /// <para>Authors: Juliano Tiago Rinaldi and 
    /// Tiago Antonio Jacobi</para>
    /// </summary>
    /// <seealso cref="Generic.StateManagedItem"/>
    public class KPFormGridModel : StateManagedItem
    {
        #region Delegates
        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        public KPGetObjectDataSource KPGetObjectDataSourceDelegate
        {
            get;
            private set;
        }
        #endregion

        #region Events
        public event KPGetObjectDataSource KPEventGetData
        {
            add { KPGetObjectDataSourceDelegate += value; }
            remove { KPGetObjectDataSourceDelegate -= value; }
        }
        #endregion

        private KPFormGridFieldCollection KPgridFieldsConfig = null;
        private KPFormButtonModelCollection KPFormButtonsModelTop = null;
        private KPFormButtonModelCollection KPFormButtonsModelBottom = null;

        [MergableProperty(false), PersistenceMode(PersistenceMode.InnerProperty)]
        public KPFormGridFieldCollection KPGridFieldsConfig
        {
            get
            {
                if (KPgridFieldsConfig == null)
                {
                    KPgridFieldsConfig = new KPFormGridFieldCollection();
                    if (!base.IsTrackingViewState)
                    {
                        KPgridFieldsConfig.TrackViewState();
                    }

                }
                return KPgridFieldsConfig;
            }
        }

        [MergableProperty(false), PersistenceMode(PersistenceMode.InnerProperty)]
        public KPFormButtonModelCollection KPButtonsModelTop
        {
            get
            {
                if (KPFormButtonsModelTop == null)
                {
                    KPFormButtonsModelTop = new KPFormButtonModelCollection();
                    if (!base.IsTrackingViewState)
                    {
                        KPFormButtonsModelTop.TrackViewState();
                    }
                }
                return KPFormButtonsModelTop;
            }
        }

        [MergableProperty(false), PersistenceMode(PersistenceMode.InnerProperty)]
        public KPFormButtonModelCollection KPButtonsModelBottom
        {
            get
            {
                if (KPFormButtonsModelBottom == null)
                {
                    KPFormButtonsModelBottom = new KPFormButtonModelCollection();
                    if (!base.IsTrackingViewState)
                    {
                        KPFormButtonsModelBottom.TrackViewState();
                    }
                }
                return KPFormButtonsModelBottom;
            }
        }

        //[PersistenceMode(PersistenceMode.InnerProperty)]
        //[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        //[NotifyParentProperty(true)]
        //public KPDataSourceModel DataSource { get; set; }

        public string GridID
        {
            get
            {
                object o = ViewState["GridID"];
                return o == null ? null : (String)o;
            }
            set { ViewState["GridID"] = value; }
        }

        [DefaultValue(false)]
        public bool AutoGenerateColumns
        {
            get
            {
                object o = ViewState["AutoGenerateColumns"];
                return o == null ? false : (bool)o;
            }
            set { ViewState["AutoGenerateColumns"] = value; }
        }

        [DefaultValue(false)]
        public bool EnableMemoryDataSource
        {
            get
            {
                object o = ViewState["EnableMemoryDataSource"];
                return o == null ? false : (bool)o;
            }
            set { ViewState["EnableMemoryDataSource"] = value; }
        }
    }
}
