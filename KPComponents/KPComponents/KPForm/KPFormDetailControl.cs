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
using KPBO.Validator;
using KPComponents.KPData;
using KPComponents.KPSession;
using KPCore.KPValidator;
using KPEnumerator.KPComponents;
using KPExtension;
using NHibernate.Validator.Engine;
using NHibernate.Validator.Event;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KPComponents
{
    /// <summary>
    /// <para>Authors: Juliano Tiago Rinaldi and 
    /// Tiago Antonio Jacobi</para>
    /// </summary>
    [DefaultProperty("Text")]
    [ToolboxData(@"<{0}:KPFormDetailControl runat=server TypeBONamespace=""SpecialistBO.EntityBO""></{0}:KPFormDetailControl>")]
    public class KPFormDetailControl : KPFormBaseControl
    {
        private int KeyNumberTemporary
        {
            get
            {
                return KPSessionHelper.GetSessionDetailTemporayId(PageBase.ParentSessionPageID, this.ID);
            }
        }


        protected override void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                // Reset DataSource
                DataSourceAltered = CloneDataSource(DataSource, TypeEntity);

                //Pega os dados da tela e preenche o DataSource
                RefreshDataSourceAltered();

                if (DataSourceAltered != null)
                {
                    if (Validate(ValidateEntityDetail()))
                    {
                        ConstructorInfo entityBO = TypBOEntity.GetConstructor(new[] { TypeEntity });
                        if (entityBO == null)
                        {
                            throw new Exception(String.Format("Could not find a valid Constructor to this Entity ({0})", TypeEntity.FullName));
                        }
                        object entityBOInstance = entityBO.Invoke(new object[] { DataSourceAltered });

                        MethodInfo methodValidate = TypBOEntity.GetMethod("Validate");
                        if (methodValidate == null)
                        {
                            throw new Exception(String.Format("Could not find a valid 'Validate' method to this EntityBO ({0})", TypBOEntity.FullName));
                        }
                        object validateReturn = methodValidate.Invoke(entityBOInstance, null);

                        if (validateReturn != null && !((bool)validateReturn))
                        {
                            PropertyInfo propInvalidValues = TypBOEntity.GetProperty("InvalidValues");
                            List<InvalidValue> invalidValues = propInvalidValues.GetValue(entityBOInstance, null) as List<InvalidValue>;
                            if (invalidValues.Count > 0)
                            {
                                if (!Validate(invalidValues.ToArray()))
                                    return;
                            }

                            PropertyInfo propInvalidEntity = TypBOEntity.GetProperty("InvalidEntityHeader");
                            List<InvalidValueBO> invalidEntityValues = propInvalidEntity.GetValue(entityBOInstance, null) as List<InvalidValueBO>;
                            if (invalidEntityValues.Count > 0)
                            {
                                this.ErrorsView.Visible = true;
                                StringBuilder erroMsg = new StringBuilder();
                                foreach (InvalidValueBO invalidValueBO in invalidEntityValues)
                                {
                                    erroMsg.AppendFormat(" - {0}<br>", invalidValueBO.Error);
                                }
                                this.ErrorsView.InnerHtml = erroMsg.ToString();
                                return;
                            }
                        }

                        if (KPSessionData != null)
                        {
                            #region New Id Generator for new register in memory
                            // Foi considerado apenas chaves do tipo inteiras
                            List<PropertyInfo> fieldNameKeys = GetFieldNameKeys();
                            foreach (PropertyInfo propKey in fieldNameKeys)
                            {
                                int idKey = Int32.Parse(propKey.GetValue(DataSourceAltered, null).ToString());
                                if (idKey == 0)
                                    propKey.SetValue(DataSourceAltered, KeyNumberTemporary, null);
                            }
                            #endregion

                            SetEntityInMemory(DataSourceAltered);
                            ClosePage();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null && ex.InnerException is KPExceptionValidator)
                {
                    KPExceptionValidator exception = ex.InnerException as KPExceptionValidator;
                    this.ErrorsView.Visible = true;
                    StringBuilder erroMsg = new StringBuilder();
                    foreach (InvalidValue error in exception.Erros)
                    {
                        erroMsg.AppendFormat(" - {0} <br>", error.Message);
                    }
                    this.ErrorsView.InnerHtml = erroMsg.ToString();

                    return;
                }

                throw ex;
            }
        }

        private InvalidValue[] ValidateEntityDetail()
        {
            List<String> propValidateList = new List<String>();
            WebControl[] componentDataWebControls = this.GetWebControlsType(typeof(IKPComponentData));
            if (componentDataWebControls != null)
            {
                foreach (WebControl componentData in componentDataWebControls)
                {
                    string fieldName = ((IKPComponentData)componentData).FieldName;
                    if (!String.IsNullOrEmpty(fieldName))
                    {
                        if (!propValidateList.Exists(x => x.Equals(fieldName)))
                            propValidateList.Add(((IKPComponentData)componentData).FieldName);
                    }
                }
            }

            if (NHibernate.Validator.Cfg.Environment.SharedEngineProvider == null)
                NHibernate.Validator.Cfg.Environment.SharedEngineProvider = new NHibernateSharedEngineProvider();

            ValidatorEngine ve = NHibernate.Validator.Cfg.Environment.SharedEngineProvider.GetEngine();
            List<InvalidValue> errors = new List<InvalidValue>();
            foreach (string prop in propValidateList)
            {
                errors.AddRange(ve.ValidatePropertyValue(DataSourceAltered, prop, null));
            }
            return errors.ToArray();
        }

        private void SetEntityInMemory(object entity)
        {
            if (PageBase == null)
                this.PageBase = this.Page as KPPageBase;

            if (PageBase != null && KPSessionData != null)
            {
                string masterDetailID = (KPSessionData as KPSessionDetailData).MasterDetailID;
                DetailSession detailSession = KPSessionHelper.GetSessionMasterDetailList(PageBase.ParentSessionPageID, masterDetailID);
                if (detailSession != null)
                {
                    if (this.FormActionState == KPFormStateEnum.Edit)
                    {
                        var propKeys = GetFieldNameKeys();
                        foreach (PropertyInfo propKey in propKeys)
                        {
                            var objKey = propKey.GetValue(entity, null);

                            for (int i = 0; i < detailSession.Entities.Count; i++)
                            {
                                var entityMemory = detailSession.Entities[i];
                                if (entityMemory != null)
                                {
                                    var objMemoryKey = propKey.GetValue(entityMemory, null);
                                    if (objMemoryKey != null)
                                    {
                                        if (objMemoryKey.ToString() == objKey.ToString())
                                        {
                                            detailSession.Entities[i] = entity;
                                        }
                                    }
                                }
                            }
                        }
                        KPSessionHelper.SetSessionMasterDetailList(PageBase.ParentSessionPageID, masterDetailID, detailSession);
                    }
                    else if (this.FormActionState == KPFormStateEnum.New)
                    {
                        detailSession.Entities.Add(entity);
                    }
                }
            }
        }
    }
}
