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
using KPComponents.KPForm;
using KPEnumerator.KPComponents;

namespace KPComponents
{
    /// <summary>
    /// <para>Authors: Juliano Tiago Rinaldi and 
    /// Tiago Antonio Jacobi</para>
    /// </summary>
    [ToolboxData(@"<{0}:KPFormMasterDetailModel runat=""server"" ID=""""><{0}:KPFormMasterDetailModel>")]
    public class KPFormMasterDetailModel : KPFormBaseModel
    {
        private KPItemModelCollection viewFieldsConfig = null;
        private KPFormItemModelCollection formFieldsConfig = null;
        private KPFormItemKeyFieldsCollection keyFieldsConfig = null;

        public string MasterDetailID
        {
            get
            {
                object o = ViewState["MasterDetailID"];
                return o == null ? null : (String)o;
            }
            set { ViewState["MasterDetailID"] = value; }
        }

        public string TypeEntityDetailNamespace
        {
            get
            {
                object o = ViewState["TypeEntityDetailNamespace"];
                return o == null ? null : (String)o;
            }
            set
            {
                ViewState["TypeEntityDetailNamespace"] = value;
                TypeEntityDetail = KPGenericUtil.GetTypeByNamespace(value);
                if (TypeEntityDetail == null)
                {
                    throw new Exception(String.Format("Não foi encontrado a entidade \"{0}\". Verifique a propriedade TypeEntityDetailNamespace.", value));
                }
            }
        }

        public string PrimaryKeyDetail
        {
            get
            {
                object o = ViewState["PrimaryKeyDetail"];
                return o == null ? null : (string)o;
            }
            set { ViewState["PrimaryKeyDetail"] = value; }
        }

        [Browsable(false)]
        public Type TypeEntityDetail { get; private set; }

        public string PropertyCompanyEntity
        {
            get
            {
                object o = ViewState["PropertyCompanyEntity"];
                return o == null ? null : (String)o;
            }
            set { ViewState["PropertyCompanyEntity"] = value; }
        }

        public int WidthFormDetail { get; set; }
        public int HeightFormDetail { get; set; }

        public int WidthGrid { get; set; }
        public int HeightGrid { get; set; }

        public string HelpToolTipNew { get; set; }
        public string HelpToolTipEdit { get; set; }
        public string HelpToolTipDelete { get; set; }

        public string PageFormUrl { get; set; }
        public KPJqGridRowNumEnum RowNum { get; set; }

        public override string ID
        {
            get
            {
                if (String.IsNullOrEmpty(base.ID))
                    return MasterDetailID;
                return base.ID;
            }
            set
            {
                base.ID = value;
            }
        }

        [MergableProperty(false), PersistenceMode(PersistenceMode.InnerProperty)]
        public KPItemModelCollection ViewFieldsConfig
        {
            get
            {
                if (viewFieldsConfig == null)
                {
                    viewFieldsConfig = new KPItemModelCollection();
                    if (!base.IsTrackingViewState)
                    {
                        viewFieldsConfig.TrackViewState();
                    }
                }
                return viewFieldsConfig;
            }
        }

        [MergableProperty(false), PersistenceMode(PersistenceMode.InnerProperty)]
        public KPFormItemKeyFieldsCollection KeyFieldsConfig
        {
            get
            {
                if (keyFieldsConfig == null)
                {
                    keyFieldsConfig = new KPFormItemKeyFieldsCollection();
                    if (!base.IsTrackingViewState)
                    {
                        keyFieldsConfig.TrackViewState();
                    }
                }
                return keyFieldsConfig;
            }
        }

        #region Events
        [Browsable(false)]
        public KPSenderEntityBefore KPEventBeforeNewDetailDelegate
        {
            private set;
            get;
        }

        public event KPSenderEntityBefore KPEventBeforeNewDetail
        {
            add { KPEventBeforeNewDetailDelegate += value; }
            remove { KPEventBeforeNewDetailDelegate -= value; }
        }


        [Browsable(false)]
        public KPSenderEntityBefore KPEventBeforeEditDetailDelegate
        {
            private set;
            get;
        }

        public event KPSenderEntityBefore KPEventBeforeEditDetail
        {
            add { KPEventBeforeEditDetailDelegate += value; }
            remove { KPEventBeforeEditDetailDelegate -= value; }
        }

        [Browsable(false)]
        public KPSenderEntityBefore KPEventBeforeDeleteDetailDelegate
        {
            private set;
            get;
        }

        public event KPSenderEntityBefore KPEventBeforeDeleteDetail
        {
            add { KPEventBeforeDeleteDetailDelegate += value; }
            remove { KPEventBeforeDeleteDetailDelegate -= value; }
        }

        [Browsable(false)]
        public KPSenderEntity KPEventAfterDeleteDetailDelegate
        {
            private set;
            get;
        }

        public event KPSenderEntity KPEventAfterDeleteDetail
        {
            add { KPEventAfterDeleteDetailDelegate += value; }
            remove { KPEventAfterDeleteDetailDelegate -= value; }
        }
        #endregion
    }
}
