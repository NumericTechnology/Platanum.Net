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

using Castle.ActiveRecord;
using KPComponents.KPComponents.KPJqGrid;
using KPComponents.KPForm;
using KPComponents.KPSession;
using KPEnumerator.KPComponents;
using KPExtension;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace KPComponents
{
    /// <summary>
    /// <para>Authors: Juliano Tiago Rinaldi and 
    /// Tiago Antonio Jacobi</para>
    /// </summary>
    public class KPMasterDetailGridControl : WebControl
    {
        public KPFormBaseControl FormControl { get; private set; }
        public KPFormMasterDetailModel MasterDetailConfig { get; private set; }

        #region Name ID
        private string ID_HiddenKey
        {
            get
            {
                return String.Format("ID_HiddenKey_{0}", MasterDetailConfig.MasterDetailID);
            }
        }

        private string ID_KPGridDetailControl
        {
            get
            {
                return String.Format("ID_KPGridDetailControl_{0}", MasterDetailConfig.MasterDetailID);
            }
        }
        #endregion

        internal KPGridDetailControl KPGridControl
        {
            get
            {
                EnsureChildControls();
                KPGridDetailControl obj = this.BetterFindControl<KPGridDetailControl>(ID_KPGridDetailControl);
                return obj;
            }
        }

        private HiddenField HiddenKey;
        private KPJqGridControl JqGridControl;
        private string tableID;
        private string pagerID;

        public KPMasterDetailGridControl(KPFormBaseControl formControl, KPFormMasterDetailModel formMasterDetailConfig)
        {
            FormControl = formControl;
            MasterDetailConfig = formMasterDetailConfig;
            EnsureChildControls();
        }

        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            if (MasterDetailConfig != null && MasterDetailConfig.ViewFieldsConfig != null)
            {
                HtmlGenericControl htmlWindow = new HtmlGenericControl("div");
                htmlWindow.Attributes.Add("class", "KPMasterDetailGridControl");

                KPGridDetailControl gridControl = new KPGridDetailControl(MasterDetailConfig)
                {
                    ID = ID_KPGridDetailControl
                };

                foreach (KPItemModel item in MasterDetailConfig.ViewFieldsConfig)
                {
                    gridControl.AddItemsModelCollection(item);
                }

                gridControl.KPEventCriterionFilter += gridControl_KPEventCriterionFilter;

                htmlWindow.Controls.Add(gridControl);
                this.Controls.Add(htmlWindow);
            }
        }

        private ICriterion gridControl_KPEventCriterionFilter()
        {
            if (MasterDetailConfig != null)
            {
                if (MasterDetailConfig.ViewFieldsConfig != null
                        && MasterDetailConfig.KeyFieldsConfig != null)
                {
                    return GetFiltering(MasterDetailConfig.KeyFieldsConfig, FormControl.DataSourceAltered as ActiveRecordBase);
                }
            }

            return null;
        }

        private ICriterion GetFiltering(KPFormItemKeyFieldsCollection keys, ActiveRecordBase entity)
        {
            Conjunction conjuntion = new Conjunction();

            if (entity != null && MasterDetailConfig.TypeEntityDetail != null)
            {
                foreach (KPMappingKeyModel key in keys)
                {
                    PropertyInfo propMaster = entity.GetType().GetProperty(key.KeyMaster);
                    PropertyInfo propDetail = MasterDetailConfig.TypeEntityDetail.GetProperty(key.KeyDetail);
                    if (propMaster != null && propDetail != null)
                    {
                        object value = propMaster.GetValue(entity, null);

                        int idKey = 0;
                        if (Int32.TryParse(value.ToString(), out idKey))
                        {
                            // Proteção para não trazer nada quando novo
                            if (idKey == 0)
                            {
                                conjuntion.Add(Expression.Sql(" 1 = 2"));
                                return conjuntion;
                            }

                        }

                        if (propMaster.PropertyType == propDetail.PropertyType)
                        {
                            conjuntion.Add(Expression.Eq(key.KeyDetail, value));
                        }
                        else
                        {
                            if (typeof(ActiveRecordBase).IsSubclassOfRawGeneric(propDetail.PropertyType))
                            {
                                MethodInfo methodFind = propDetail.PropertyType.GetMethodInheritance("Find", new Type[] { typeof(object) });
                                if (methodFind != null)
                                {
                                    object objValue = methodFind.Invoke(null, new object[] { value });
                                    if (objValue != null)
                                    {
                                        conjuntion.Add(Expression.Eq(key.KeyDetail, objValue));
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return conjuntion;
        }

    }
}
