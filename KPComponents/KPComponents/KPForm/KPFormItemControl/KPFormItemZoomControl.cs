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
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using KPComponents.Asset;
using KPComponents.KPForm;
using KPExtension;

namespace KPComponents
{
    /// <summary>
    /// <para>Authors: Juliano Tiago Rinaldi and 
    /// Tiago Antonio Jacobi</para>
    /// </summary>
    public class KPFormItemZoomControl : KPFormItemControlBase<KPFormItemZoom>
    {
        private string ControlFieldID;
        private string ControlLabelID;
        private bool controlsEnabled;

        public override KPFormItemZoom FormItem
        {
            get;
            protected set;
        }

        public KPFormBaseControl FormBaseControl { get; private set; }

        public override bool Enabled
        {
            get { return controlsEnabled; }
            set
            {
                controlsEnabled = value;
                EnableComponents(controlsEnabled);
            }
        }

        public KPFormZoomModelCollection KPFormZoomConfig { get; private set; }

        public KPLabelControl ControlLabel
        {
            get
            {
                EnsureChildControls();
                KPCaptionControl obj = this.BetterFindControl<KPCaptionControl>(ControlLabelID);
                if (obj != null)
                    return obj.ControlLabel;

                return null;
            }
        }

        public KPZoomField ControlField
        {
            get
            {
                EnsureChildControls();
                KPZoomField obj = this.BetterFindControl<KPZoomField>(ControlFieldID);

                return obj;
            }
        }

        public KPFormItemZoomControl(KPFormBaseControl formControl, KPFormItemZoom formItem, object objValue)
            : base(formControl)
        {
            this.ID = formItem.ID;
            ControlFieldID = CreateIDField(formItem.ID);
            ControlLabelID = CreateIDLabel(formItem.ID);
            FormBaseControl = formControl;

            KPFormZoomConfig = formControl.KPFormZoomConfig;
            FormItem = formItem;
            ItemValue = objValue;
            EnsureChildControls();
        }

        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            foreach (KPFormZoomModel zoomConfig in KPFormZoomConfig)
            {
                if (FormItem.ZoomIDConfig.Equals(zoomConfig.ZoomID))
                {
                    #region Caption
                    string titleCaption = KPFormItemControlHelper.GetTitleNamePropertyEntity(FormItem.Title,
                                                                                             FormItem.FieldName, TypeEntity);
                    KPCaptionControl captionControl = new KPCaptionControl(ControlLabelID, titleCaption, FormItem);

                    this.Controls.Add(captionControl);
                    #endregion

                    this.Controls.Add(new KPZoomField(FormBaseControl, FormItem, zoomConfig)
                    {
                        ID = ControlFieldID,
                        Width = FormItem.Width,
                        DescriptionWidth = FormItem.DescriptionWidth,
                        Enabled = FormItem.Enabled,
                        Value = ItemValue == null ? String.Empty : ItemValue.ToString()
                    });

                    break;
                }
            }
        }

        private void EnableComponents(bool enabled)
        {
            foreach (Control item in this.Controls)
            {
                if (item is WebControl)
                    ((WebControl)item).Enabled = enabled;
            }
        }
    }
}
