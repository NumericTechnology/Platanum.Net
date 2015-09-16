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
using KPComponents.KPForm;
using KPExtension;
using System;
using System.Reflection;
using System.Web.UI.WebControls;

namespace KPComponents
{
    /// <summary>
    /// <para>Authors: Juliano Tiago Rinaldi and 
    /// Tiago Antonio Jacobi</para>
    /// </summary>
    public abstract class KPCompositeControlBase : CompositeControl
    {
        protected ActiveRecordBase SetDetailsKeys(ActiveRecordBase entityMaster, ActiveRecordBase entityDetail, KPFormItemKeyFieldsCollection keyFieldsConfig)
        {
            if (keyFieldsConfig != null)
            {
                foreach (KPMappingKeyModel key in keyFieldsConfig)
                {
                    PropertyInfo propMaster = entityMaster.GetType().GetProperty(key.KeyMaster);
                    PropertyInfo propDetail = entityDetail.GetType().GetProperty(key.KeyDetail);
                    if (propMaster != null && propDetail != null)
                    {
                        object value = propMaster.GetValue(entityMaster, null);

                        if (value == null)
                            continue;

                        int idKey = 0;
                        if (Int32.TryParse(value.ToString(), out idKey))
                        {
                            // Proteção para não trazer nada quando novo
                            if (idKey == 0)
                                continue;
                        }

                        if (propMaster.PropertyType == propDetail.PropertyType)
                        {
                            propDetail.SetValue(entityDetail, value, null);
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
                                        propDetail.SetValue(entityDetail, objValue, null);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return entityDetail;
        }
    }
}
