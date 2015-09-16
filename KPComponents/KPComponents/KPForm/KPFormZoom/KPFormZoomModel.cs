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
using KPComponents.KPJqGrid;
using KPCore.KPUtil;
using KPGlobalization;

namespace KPComponents
{
    /// <summary>
    /// <para>Authors: Juliano Tiago Rinaldi and 
    /// Tiago Antonio Jacobi</para>
    /// </summary>
    [ToolboxData(@"<{0}:KPFormZoomModel runat=""server"" ID="""" ZoomID=""""><{0}:KPFormZoomModel>")]
    public class KPFormZoomModel : KPFormBaseModel
    {
        #region Delegates
        [Browsable(false)]
        public KPCriterionFilter KPCriterionFilterDelegate
        {
            private set;
            get;
        }

        [Browsable(false)]
        public KPOrder KPOrderDelegate
        {
            private set;
            get;
        }
        #endregion

        #region Events
        public event KPCriterionFilter KPEventCriterionFilter
        {
            add { KPCriterionFilterDelegate += value; }
            remove { KPCriterionFilterDelegate -= value; }
        }

        public event KPOrder KPEventOrder
        {
            add { KPOrderDelegate += value; }
            remove { KPOrderDelegate -= value; }
        }
        #endregion

        private KPItemModelCollection KPzoomFieldsConfig = null;

        [MergableProperty(false), PersistenceMode(PersistenceMode.InnerProperty)]
        public KPItemModelCollection KPZoomFieldsConfig
        {
            get
            {
                if (KPzoomFieldsConfig == null)
                {
                    KPzoomFieldsConfig = new KPItemModelCollection();
                    if (!base.IsTrackingViewState)
                    {
                        KPzoomFieldsConfig.TrackViewState();
                    }
                }
                return KPzoomFieldsConfig;
            }
        }

        public override string ID
        {
            get
            {
                if (String.IsNullOrEmpty(base.ID))
                    return ZoomID;
                return base.ID;
            }
            set
            {
                base.ID = value;
            }
        }

        public string ZoomID
        {
            get
            {

                object o = ViewState["ZoomID"];
                return o == null ? null : (String)o;
            }
            set { ViewState["ZoomID"] = value; }
        }

        public string TypeEntityNamespace
        {
            get
            {
                object o = ViewState["TypeEntityNamespace"];
                return o == null ? null : (String)o;
            }
            set
            {
                ViewState["TypeEntityNamespace"] = value;
                TypeEntity = KPGenericUtil.GetTypeByNamespace(value);
                if (TypeEntity == null)
                {
                    //TODO: Quando fizer Refactor, transformar a exceções sugestivas em um exceção do KPFramework
                    throw new Exception(String.Format("Não foi encontrado a entidade \"{0}\". Verifique a propriedade TypeEntityNamespace.", value));
                }
            }
        }

        public int HeightZoom
        {
            get
            {
                object o = ViewState["HeightZoom"];
                return o == null ? 200 : (int)o;
            }
            set { ViewState["HeightZoom"] = value; }
        }

        public int WidthZoom
        {
            get
            {
                object o = ViewState["WidthZoom"];
                return o == null ? 300 : (int)o;
            }
            set { ViewState["WidthZoom"] = value; }
        }

        [Browsable(false)]
        public Type TypeEntity { get; private set; }

        public string FieldReturnId
        {
            get
            {
                object o = ViewState["FieldReturnId"];
                return o == null ? null : (String)o;
            }
            set { ViewState["FieldReturnId"] = value; }
        }

        public string FieldReturnText
        {
            get
            {
                object o = ViewState["FieldReturnText"];
                return o == null ? null : (String)o;
            }
            set { ViewState["FieldReturnText"] = value; }
        }

        public string SearchByField
        {
            get
            {
                object o = ViewState["FinderByField"];
                return o == null ? null : (String)o;
            }
            set { ViewState["FinderByField"] = value; }
        }

        public string DisplaySearchField
        {
            get
            {
                object o = ViewState["DisplaySearchField"];
                return o == null ? null : (String)o;
            }
            set { ViewState["DisplaySearchField"] = value; }
        }

        public string WindowTitle
        {
            get
            {
                object o = ViewState["WindowTitle"];
                return o == null ? null : (String)o;
            }
            set { ViewState["WindowTitle"] = KPGlobalizationLanguage.GetString(value); }
        }

        public string PropertyCompanyEntity
        {
            get
            {
                object o = ViewState["PropertyCompanyEntity"];
                return o == null ? null : (String)o;
            }
            set { ViewState["PropertyCompanyEntity"] = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the Zoom should open when Search Not Found
        /// </summary>
        public bool OpenZoomSearchNotFound
        {
            get
            {
                object o = ViewState["OpenZoomSearchNotFound"];
                return o == null ? false : (bool)o;
            }
            set { ViewState["OpenZoomSearchNotFound"] = value; }
        }
    }
}
