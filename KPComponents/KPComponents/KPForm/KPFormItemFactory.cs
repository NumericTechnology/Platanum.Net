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
using System.Web.UI.HtmlControls;
using System.Web.UI;
using KPExtension;
using KPComponents.Asset;
using System.Web.UI.WebControls;
using System.Reflection;
using KPCore.KPValidator;
using KPCore.KPUtil;
using System.ComponentModel;
using KPCore.KPSecurity;

namespace KPComponents.KPForm
{
    /// <summary>
    /// Factory - FormControl components creator
    /// <para>Authors: Juliano Tiago Rinaldi and 
    /// Tiago Antonio Jacobi</para>
    /// </summary>
    public class KPFormItemFactory
    {
        private KPFormBaseControl FormBaseControl { get; set; }

        public HtmlGenericControl ItemsFieldset { get; private set; }
        public ControlCollection ControlsFieldset { get { return ItemsFieldset.Controls; } }

        public KPFormItemFactory(KPFormBaseControl formBaseControl)
        {
            FormBaseControl = formBaseControl;
            ItemsFieldset = new HtmlGenericControl("ul");
        }

        public void AddFormItem(KPFormItemModel formItem, object objValue)
        {
            if (formItem is KPFormItemText)
            {
                AddFormItem(formItem as KPFormItemText, objValue);
            }
            else if (formItem is KPFormItemPassword)
            {
                AddFormItem(formItem as KPFormItemPassword, objValue);
            }
            else if (formItem is KPFormItemCombo)
            {
                AddFormItem(formItem as KPFormItemCombo, objValue);
            }
            else if (formItem is KPFormItemZoom)
            {
                AddFormItem(formItem as KPFormItemZoom, objValue);
            }
            else if (formItem is KPFormItemGrid)
            {
                AddFormItem(formItem as KPFormItemGrid, objValue);
            }
            else if (formItem is KPFormItemMasterDetail)
            {
                AddFormItem(formItem as KPFormItemMasterDetail, objValue);
            }
            else if (formItem is KPFormItemKey)
            {
                AddFormItem(formItem as KPFormItemKey, objValue);
            }
            else if (formItem is KPFormItemCheckBox)
            {
                AddFormItem(formItem as KPFormItemCheckBox, objValue);
            }
            else if (formItem is KPFormItemSpacer)
            {
                AddFormItem(formItem as KPFormItemSpacer);
            }
            else if (formItem is KPFormItemButton)
            {
                AddFormItem(formItem as KPFormItemButton);
            }
            else if (formItem is KPFormItemEntity)
            {
                AddFormItem(formItem as KPFormItemEntity, objValue);
            }
            else if (formItem is KPFormItemDateTime)
            {
                AddFormItem(formItem as KPFormItemDateTime, objValue);
            }
        }

        private void AddFormItem(KPFormItemText formItem, object objValue)
        {
            PagePermission pagePermission = FormBaseControl.PageBase.SecuritySession.GetPagePermission(FormBaseControl.PageBase.PageEnum);
            if (!pagePermission.IsReadOnly)
            {
                ComponentPermission componentPermission = FormBaseControl.PageBase.SecuritySession.GetComponentPermission(FormBaseControl.PageBase.PageEnum, formItem.ID);
                if (componentPermission.IsVisible)
                {
                    KPFormItemTextControl formItemControl = new KPFormItemTextControl(this.FormBaseControl, formItem, objValue);
                    if (formItem != null && !String.IsNullOrWhiteSpace(formItem.HelpInfo))
                    {
                        formItemControl.Attributes.Add("data-step", formItem.HelpStep.ToString());
                        formItemControl.Attributes.Add("data-intro", formItem.HelpInfo);
                        //formItemControl.Attributes.Add("onClick", "javascript:introJs().start();");
                    }

                    if (formItemControl.Enabled)
                        formItemControl.Enabled = componentPermission.IsEnabled;
                    ControlsFieldset.Add(formItemControl);
                }
            }
        }

        private void AddFormItem(KPFormItemPassword formItem, object objValue)
        {
            PagePermission pagePermission = FormBaseControl.PageBase.SecuritySession.GetPagePermission(FormBaseControl.PageBase.PageEnum);
            if (!pagePermission.IsReadOnly)
            {
                ComponentPermission componentPermission = FormBaseControl.PageBase.SecuritySession.GetComponentPermission(FormBaseControl.PageBase.PageEnum, formItem.ID);
                if (componentPermission.IsVisible)
                {
                    KPFormItemPasswordControl formItemControl = new KPFormItemPasswordControl(this.FormBaseControl, formItem, objValue);
                    if (formItemControl.Enabled)
                        formItemControl.Enabled = componentPermission.IsEnabled;

                    ControlsFieldset.Add(formItemControl);
                }
            }
        }

        private void AddFormItem(KPFormItemCombo formItem, object objValue)
        {
            PagePermission pagePermission = FormBaseControl.PageBase.SecuritySession.GetPagePermission(FormBaseControl.PageBase.PageEnum);
            if (!pagePermission.IsReadOnly)
            {
                ComponentPermission componentPermission = FormBaseControl.PageBase.SecuritySession.GetComponentPermission(FormBaseControl.PageBase.PageEnum, formItem.ID);
                if (componentPermission.IsVisible)
                {
                    KPFormItemComboControl formItemControl = new KPFormItemComboControl(this.FormBaseControl, formItem, objValue);
                    if (formItemControl.Enabled)
                        formItemControl.Enabled = componentPermission.IsEnabled;

                    ControlsFieldset.Add(formItemControl);
                }
            }
        }

        private void AddFormItem(KPFormItemGrid formItem, object objValue)
        {
            PagePermission pagePermission = FormBaseControl.PageBase.SecuritySession.GetPagePermission(FormBaseControl.PageBase.PageEnum);
            if (!pagePermission.IsReadOnly)
            {
                ComponentPermission componentPermission = FormBaseControl.PageBase.SecuritySession.GetComponentPermission(FormBaseControl.PageBase.PageEnum, formItem.ID);
                if (componentPermission.IsVisible)
                {
                    KPFormItemGridControl formItemControl = new KPFormItemGridControl(this.FormBaseControl, formItem, objValue);
                    if (formItemControl.Enabled)
                        formItemControl.Enabled = componentPermission.IsEnabled;

                    ControlsFieldset.Add(formItemControl);
                }
            }
        }

        private void AddFormItem(KPFormItemMasterDetail formItem, object objValue)
        {
            PagePermission pagePermission = FormBaseControl.PageBase.SecuritySession.GetPagePermission(FormBaseControl.PageBase.PageEnum);
            if (!pagePermission.IsReadOnly)
            {
                ComponentPermission componentPermission = FormBaseControl.PageBase.SecuritySession.GetComponentPermission(FormBaseControl.PageBase.PageEnum, formItem.ID);
                if (componentPermission.IsVisible)
                {
                    KPFormItemMasterDetailControl formItemControl = new KPFormItemMasterDetailControl(this.FormBaseControl, formItem);
                    if (formItemControl.Enabled)
                        formItemControl.Enabled = componentPermission.IsEnabled;

                    ControlsFieldset.Add(formItemControl);
                }
            }
        }

        private void AddFormItem(KPFormItemZoom formItem, object objValue)
        {
            PagePermission pagePermission = FormBaseControl.PageBase.SecuritySession.GetPagePermission(FormBaseControl.PageBase.PageEnum);
            if (!pagePermission.IsReadOnly)
            {
                ComponentPermission componentPermission = FormBaseControl.PageBase.SecuritySession.GetComponentPermission(FormBaseControl.PageBase.PageEnum, formItem.ID);
                if (componentPermission.IsVisible)
                {
                    KPFormItemZoomControl formItemControl = new KPFormItemZoomControl(this.FormBaseControl, formItem, objValue);
                    if (formItemControl.Enabled)
                        formItemControl.Enabled = componentPermission.IsEnabled;

                    ControlsFieldset.Add(formItemControl);
                }
            }
        }

        private void AddFormItem(KPFormItemKey formItem, object objValue)
        {
            PagePermission pagePermission = FormBaseControl.PageBase.SecuritySession.GetPagePermission(FormBaseControl.PageBase.PageEnum);
            if (!pagePermission.IsReadOnly)
            {
                ComponentPermission componentPermission = FormBaseControl.PageBase.SecuritySession.GetComponentPermission(FormBaseControl.PageBase.PageEnum, formItem.ID);
                if (componentPermission.IsVisible)
                {
                    KPFormItemKeyControl formItemControl = new KPFormItemKeyControl(this.FormBaseControl, formItem, objValue);
                    if (formItemControl.Enabled)
                        formItemControl.Enabled = componentPermission.IsEnabled;

                    ControlsFieldset.Add(formItemControl);
                }
            }
        }

        private void AddFormItem(KPFormItemCheckBox formItem, object objValue)
        {
            PagePermission pagePermission = FormBaseControl.PageBase.SecuritySession.GetPagePermission(FormBaseControl.PageBase.PageEnum);
            if (!pagePermission.IsReadOnly)
            {
                ComponentPermission componentPermission = FormBaseControl.PageBase.SecuritySession.GetComponentPermission(FormBaseControl.PageBase.PageEnum, formItem.ID);
                if (componentPermission.IsVisible)
                {
                    KPFormItemCheckBoxControl formItemControl = new KPFormItemCheckBoxControl(this.FormBaseControl, formItem, objValue);
                    if (formItemControl.Enabled)
                        formItemControl.Enabled = componentPermission.IsEnabled;

                    ControlsFieldset.Add(formItemControl);
                }
            }
        }

        private void AddFormItem(KPFormItemSpacer formItem)
        {
            KPFormItemSpacerControl formItemControl = new KPFormItemSpacerControl(this.FormBaseControl, formItem);
            ControlsFieldset.Add(formItemControl);
        }

        private void AddFormItem(KPFormItemButton formItem)
        {
            PagePermission pagePermission = FormBaseControl.PageBase.SecuritySession.GetPagePermission(FormBaseControl.PageBase.PageEnum);
            if (!pagePermission.IsReadOnly)
            {
                ComponentPermission componentPermission = FormBaseControl.PageBase.SecuritySession.GetComponentPermission(FormBaseControl.PageBase.PageEnum, formItem.ID);
                if (componentPermission.IsVisible)
                {
                    KPFormItemButtonControl formItemControl = new KPFormItemButtonControl(this.FormBaseControl, formItem);
                    if (formItemControl.Enabled)
                        formItemControl.Enabled = componentPermission.IsEnabled;

                    ControlsFieldset.Add(formItemControl);
                }
            }
        }

        private void AddFormItem(KPFormItemEntity formItem, object objValue)
        {
            PagePermission pagePermission = FormBaseControl.PageBase.SecuritySession.GetPagePermission(FormBaseControl.PageBase.PageEnum);
            if (!pagePermission.IsReadOnly)
            {
                ComponentPermission componentPermission = FormBaseControl.PageBase.SecuritySession.GetComponentPermission(FormBaseControl.PageBase.PageEnum, formItem.ID);
                if (componentPermission.IsVisible)
                {
                    KPFormItemEntityControl formItemControl = new KPFormItemEntityControl(this.FormBaseControl, formItem, objValue);
                    if (formItemControl.Enabled)
                        formItemControl.Enabled = componentPermission.IsEnabled;

                    ControlsFieldset.Add(formItemControl);
                }
            }
        }

        private void AddFormItem(KPFormItemDateTime formItem, object objValue)
        {
            PagePermission pagePermission = FormBaseControl.PageBase.SecuritySession.GetPagePermission(FormBaseControl.PageBase.PageEnum);
            if (!pagePermission.IsReadOnly)
            {
                ComponentPermission componentPermission = FormBaseControl.PageBase.SecuritySession.GetComponentPermission(FormBaseControl.PageBase.PageEnum, formItem.ID);
                if (componentPermission.IsVisible)
                {
                    KPFormItemDateTimeControl formItemControl = new KPFormItemDateTimeControl(this.FormBaseControl, formItem, objValue);
                    if (formItemControl.Enabled)
                        formItemControl.Enabled = componentPermission.IsEnabled;

                    ControlsFieldset.Add(formItemControl);
                }
            }
        }
    }
}
