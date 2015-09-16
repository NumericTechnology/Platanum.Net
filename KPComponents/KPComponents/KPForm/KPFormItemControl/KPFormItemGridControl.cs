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
using System.Web.UI.WebControls;
using KPExtension;
using System.Web.UI;
using System.Diagnostics;
using System.Web.UI.HtmlControls;

namespace KPComponents
{
    /// <summary>
    /// <para>Authors: Juliano Tiago Rinaldi and 
    /// Tiago Antonio Jacobi</para>
    /// </summary>
    public class KPFormItemGridControl : KPFormItemControlBase<KPFormItemGrid>
    {
        private string ControlFieldID;
        private HtmlGenericControl KPActionBarTop = new HtmlGenericControl("div");
        private HtmlGenericControl KPActionBarBottom = new HtmlGenericControl("div");

        private List<KPFormButtonSimpleControl> KPFormButtonSimpleControlList = new List<KPFormButtonSimpleControl>();

        public override KPFormItemGrid FormItem
        {
            get;
            protected set;
        }

        public override bool Enabled
        {
            get
            {
                if (GridViewField != null)
                    return GridViewField.Enabled;

                return false;
            }
            set
            {
                if (GridViewField != null)
                    GridViewField.Enabled = value;
            }
        }

        public KPFormGridModelCollection KPFormGridConfig { get; private set; }

        public KPGridViewControl GridViewField
        {
            get
            {
                EnsureChildControls();
                KPGridViewControl obj = this.BetterFindControl<KPGridViewControl>(ControlFieldID);
                if (obj != null)
                    return obj;

                return null;
            }
        }

        public List<KPFormButtonSimpleControl> KPFormButtonSimpleControls
        {
            get { return KPFormButtonSimpleControlList; }
        }

        public KPFormItemGridControl(KPFormBaseControl formControl, KPFormItemGrid formItem, object objValue)
            : base(formControl)
        {
            this.ID = formItem.ID;
            ControlFieldID = CreateIDField(formItem.ID);

            KPFormGridConfig = formControl.KPFormGridConfig;
            FormItem = formItem;
            ItemValue = objValue;
            EnsureChildControls();
        }

        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            foreach (KPFormGridModel gridConfig in KPFormGridConfig)
            {
                if (FormItem.GridIDConfig.Equals(gridConfig.GridID))
                {
                    #region Buttons GridTop
                    if (gridConfig.KPButtonsModelTop != null && gridConfig.KPButtonsModelTop.Count > 0)
                    {
                        KPActionBarTop.Attributes.Add("class", "KPActionBarTop");

                        foreach (KPFormButtonModel obj in gridConfig.KPButtonsModelTop)
                        {
                            if (obj is KPFormButtonSimple)
                            {
                                KPFormButtonSimpleControl KPBtnSimpleTop = new KPFormButtonSimpleControl(this.FormControl, obj as KPFormButtonSimple);
                                KPActionBarTop.Controls.Add(KPBtnSimpleTop);
                                KPFormButtonSimpleControlList.Add(KPBtnSimpleTop);
                            }
                        }

                        if (KPActionBarTop.Controls.Count > 0)
                            this.Controls.Add(KPActionBarTop);
                    }
                    #endregion Buttons GridTop

                    #region GridView
                    KPGridViewControl grid = new KPGridViewControl(FormControl.Page, FormItem, gridConfig)
                    {
                        ID = ControlFieldID,
                        Width = FormItem.Width,
                        CssClass = "KPGridView"
                    };

                    grid.HeaderStyle.CssClass = "KPGridViewHeader unselectable";
                    grid.RowStyle.CssClass = "KPGridViewRow";


                    if (FormItem.KPGridViewDeleteDelegate != null)
                    {
                        grid.RowDeleting += new GridViewDeleteEventHandler(FormItem.KPGridViewDeleteDelegate);
                    }

                    // grid
                    this.Controls.Add(grid);
                    #endregion GridView

                    #region Buttons GridBottom
                    if (gridConfig.KPButtonsModelBottom != null && gridConfig.KPButtonsModelBottom.Count > 0)
                    {
                        KPActionBarBottom.Attributes.Add("class", "KPActionBarBottom");

                        foreach (KPFormButtonModel obj in gridConfig.KPButtonsModelBottom)
                        {
                            if (obj is KPFormButtonSimple)
                            {
                                KPFormButtonSimpleControl KPBtnSimpleBottom = new KPFormButtonSimpleControl(this.FormControl, obj as KPFormButtonSimple);
                                KPActionBarBottom.Controls.Add(KPBtnSimpleBottom);
                                KPFormButtonSimpleControlList.Add(KPBtnSimpleBottom);
                            }
                        }

                        if (KPActionBarBottom.Controls.Count > 0)
                        {
                            this.Controls.Add(KPActionBarBottom);
                        }
                    }
                    #endregion Buttons GridBottom

                    break;
                }
            }
        }
    }
}
