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
using KPComponents.KPGeneric;
using KPComponents.KPJqGrid;
using KPComponents.KPJqGrid.Filter;
using KPComponents.KPSession;
using KPCore;
using KPCore.KPSecurity;
using KPCore.KPUtil;
using KPCore.KPValidator;
using KPEntity.ORM;
using KPEnumerator.KPComponents;
using KPEnumerator.KPGlobalization;
using KPEnumerator.KPSecurity;
using KPExtension;
using KPGlobalization;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace KPComponents.KPJson
{
    /// <summary>
    /// Manages Json JqGrid
    /// <para>Authors: Juliano Tiago Rinaldi and 
    /// Tiago Antonio Jacobi</para>
    /// </summary>
    public class KPJsonJqGrid : KPJsonBase
    {
        /// <summary>
        /// Convert Object Entity to Object Json Serialized
        /// </summary>
        /// <param name="entityList">Array objects</param>
        /// <param name="gridProperties">Properties JqGrid from Page</param>
        /// <seealso cref="KPJqGrid.KPGridJsonProperties"/>
        /// <returns>Object converted by Json Serialized</returns>
        protected string GetJsonToJqGrid(object[] entityList, KPGridJsonProperties gridProperties)
        {
            if (entityList == null || entityList.Length <= 0)
                return "[]";

            KPGridResults result = new KPGridResults();
            List<KPGridRow> rowsList = new List<KPGridRow>();
            foreach (object obj in entityList)
            {
                KPGridRow griRow = new KPGridRow();

                PropertyInfo propertyID = obj.GetType().GetProperty(gridProperties.PropertyOrder);
                griRow.id = propertyID.GetValue(obj, null);
                griRow.cell = new List<string>();
                foreach (string colModel in gridProperties.ColModel)
                {
                    object value = null;
                    JqGridColumnCustom colCustom = GetColumnCustom(colModel);

                    PropertyInfo property = obj.GetType().GetProperty(colCustom.PropertyColumn);
                    if (property != null)
                    {
                        value = property.GetValue(obj, null);

                        if (!String.IsNullOrEmpty(colCustom.PropertyFKName) && value != null)
                            value = value.GetType().GetProperty(colCustom.PropertyFKName).GetValue(value, null);
                        else if (!String.IsNullOrEmpty(colCustom.NamespaceEnum))
                        {
                            Type enumField = KPGenericUtil.GetTypeByNamespace(colCustom.NamespaceEnum);
                            if (enumField != null)
                                value = ((Enum)Enum.Parse(enumField, value.ToString())).GetHashCode();
                        }
                    }

                    griRow.cell.Add(value != null ? value.ToString() : String.Empty);
                }

                rowsList.Add(griRow);
            }
            result.rows = rowsList.ToArray();
            result.page = gridProperties.Page;
            result.total = Convert.ToInt32(Math.Ceiling(((double)gridProperties.TotalFoundRegisters) / ((double)gridProperties.Rows)));
            result.records = gridProperties.TotalFoundRegisters;

            return GetJson(result);
        }

        /// <summary>
        /// Filtering Entities by object properties
        /// </summary>
        /// <typeparam name="EntityType">Object generic Entity Type</typeparam>
        /// <param name="gridProperties">Properties JqGrid from Page</param>
        /// <returns></returns>
        protected EntityType[] FilterEntities<EntityType>(KPGridJsonProperties gridProperties) where EntityType : ActiveRecordBase<EntityType>
        {
            Type typeEntity = typeof(ActiveRecordBase<EntityType>);
            MethodInfo methodFindAll = null;

            Order[] orders = null;
            if (gridProperties.InitialOrder != null)
                orders = new Order[] { gridProperties.InitialOrder };

            #region Responsável por Filtrar por Company
            ICriterion filterCompany = Expression.Sql(" 1=1 "); // Para trazer todos os dados
            if (!String.IsNullOrEmpty(gridProperties.PropertyCompanyEntity) 
                    && !gridProperties.PropertyCompanyEntity.Equals("!") 
                    && gridProperties.Company.HasValue)
            {
                filterCompany = Expression.Sql(" 1=2 "); // Para não trazer dados quando existe a propriedade
                PropertyInfo propEntity = typeof(EntityType).GetProperty(gridProperties.PropertyCompanyEntity);
                if (propEntity != null)
                {
                    PropertyInfo propCompany = null;
                    if (propEntity.PropertyType.Equals(typeof(FrwCompany)))
                        propCompany = propEntity;
                    else
                        // TODO: Encontrar uma forma de descobrir o objIdCompany sem deixar fixo no código.
                        propCompany = propEntity.PropertyType.GetProperty("objIdCompany");

                    if (propCompany != null)
                    {
                        object companyInstance = Activator.CreateInstance(propCompany.PropertyType);
                        if (companyInstance != null)
                        {
                            PropertyInfo idCompany = companyInstance.GetType().GetEntityPrimaryKey();
                            if (idCompany != null)
                            {
                                idCompany.SetValue(companyInstance, gridProperties.Company.Value, null);
                                MethodInfo methodFindAllEntity = propEntity.PropertyType.GetMethodInheritance("FindOne", new Type[] { typeof(ICriterion[]) });

                                if (methodFindAllEntity != null)
                                {
                                    ICriterion filterEntity = null;
                                    if (propEntity.PropertyType.Equals(typeof(FrwCompany)))
                                        filterEntity = Expression.Eq("IdCompany", gridProperties.Company.Value);
                                    else
                                        filterEntity = Expression.Eq("objIdCompany", companyInstance);

                                    object objEntityFiltered = methodFindAllEntity.Invoke(null, new object[] { new ICriterion[] { filterEntity } });
                                    if (objEntityFiltered != null)
                                    {
                                        filterCompany = Expression.Eq(gridProperties.PropertyCompanyEntity, objEntityFiltered);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            #endregion

            List<ICriterion> criterionList = new List<ICriterion>();
            if (gridProperties.InitialFilter != null)
                criterionList.Add(gridProperties.InitialFilter);
            if (filterCompany != null)
                criterionList.Add(filterCompany);

            int indexInit = gridProperties.Page == 1 ? 0 : (gridProperties.Page * gridProperties.Rows) - gridProperties.Rows;
            int countReturn = gridProperties.Rows;

            if (gridProperties.JqGridFilter == null)
            {
                // Retorna o total encontrado dos registros
                gridProperties.TotalFoundRegisters = ActiveRecordMediator.Count(typeof(EntityType), criterionList.ToArray());

                methodFindAll = typeEntity.GetMethod("SlicedFindAll", new Type[] { typeof(int), typeof(int), typeof(Order[]), typeof(ICriterion[]) });

                object entitiesObj = methodFindAll.Invoke(null, new object[] { indexInit, countReturn, orders, criterionList.ToArray() });

                if (entitiesObj != null)
                {
                    EntityType[] entities = (EntityType[])entitiesObj;
                    // Esta variável só será preenchida quando a coluna for um Objeto FK
                    string propFKOrder = String.Empty;
                    JqGridColumnCustom propertyFound = gridProperties.ColumnsCustom.FirstOrDefault(x => x.PropertyColumn.Equals(gridProperties.PropertyOrder));
                    if (propertyFound != null)
                        propFKOrder = propertyFound.PropertyFKName;
                    try
                    {
                        switch (gridProperties.OrderType)
                        {
                            case KPJqGridTypeOrderEnum.ASC:
                                return entities.OrderBy(x => x.GetType().GetProperty(gridProperties.PropertyOrder).GetValue(x, null), new GenericComparer(propFKOrder)).ToArray();
                            case KPJqGridTypeOrderEnum.DESC:
                                return entities.OrderByDescending(x => x.GetType().GetProperty(gridProperties.PropertyOrder).GetValue(x, null), new GenericComparer(propFKOrder)).ToArray();
                        }
                    }
                    catch (NullReferenceException ex)
                    {
                        StringBuilder sbError = new StringBuilder();
                        sbError.AppendFormat(KPGlobalizationLanguageEnum.GetString(KPLanguageKeyEnum.ERROR_SOLUTION_SUGGESTION), Environment.NewLine);
                        sbError.AppendFormat(KPGlobalizationLanguageEnum.GetString(KPLanguageKeyEnum.SUGGESTION_VERIFY_PROPERTY_NAME), gridProperties.PropertyOrder, Environment.NewLine);
                        throw new NullReferenceException(sbError.ToString(), ex);
                    }
                    catch
                    {
                        throw;
                    }
                }

                return null;
            }

            Junction juctionExpress = null;
            if (gridProperties.JqGridFilter.groupOp.ToUpper() == "AND")
                juctionExpress = Expression.Conjunction();
            else if (gridProperties.JqGridFilter.groupOp.ToUpper() == "OR")
                juctionExpress = Expression.Disjunction();

            foreach (KPJqGridRule rule in gridProperties.JqGridFilter.rules)
            {
                KPJqGridTypeFilterEnum filterType = KPJqGridFilter.GetFilter(rule.op);
                if (filterType.Equals(KPJqGridTypeFilterEnum.Error))
                    return null;

                try
                {
                    JqGridColumnCustom columnCustom = gridProperties.ColumnsCustom.Single<JqGridColumnCustom>
                                            (x => x.PropertyColumn.Equals(rule.field));

                    Type EntityPropertyType = typeof(EntityType).GetProperty(rule.field).PropertyType;

                    // Pesquisa quando o Campo é uma FK
                    if (columnCustom.KPItemsModelType == typeof(KPEntityModel))
                    {
                        if (typeof(KPActiveRecordBase<>).IsSubclassOfRawGeneric(EntityPropertyType))
                        {
                            if (columnCustom != null)
                            {
                                MethodInfo methodFindAllFK = EntityPropertyType.GetMethodInheritance("FindAll", new Type[] { typeof(ICriterion[]) });
                                if (methodFindAllFK != null)
                                {
                                    ICriterion filterFK = GetCriterionFilter(filterType, columnCustom.PropertyFKName, rule.data);
                                    object entityArray = methodFindAllFK.Invoke(null, new object[] { criterionList.ToArray() });
                                    if (entityArray != null)
                                    {
                                        List<object> ObjEntityIN = new List<object>();
                                        PropertyInfo propKey = EntityPropertyType.GetEntityPrimaryKey();
                                        foreach (var item in (Array)entityArray)
                                            ObjEntityIN.Add(item.GetType().GetProperty(propKey.Name).GetValue(item, null));

                                        if (ObjEntityIN.Count > 0)
                                        {
                                            filterType = KPJqGridTypeFilterEnum.In;
                                            rule.data = ObjEntityIN.ToArray();
                                        }
                                        else
                                            return null;
                                    }
                                }
                            }
                        }
                    }
                    else if (columnCustom.KPItemsModelType == typeof(KPEnumModel))
                    {
                        if (rule.data != null)
                        {
                            Type typeEnumConvert = null;
                            if (EntityPropertyType.IsGenericType && EntityPropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                                typeEnumConvert = EntityPropertyType.GetGenericArguments().First();
                            else
                                typeEnumConvert = EntityPropertyType;

                            string nameEnumValue = Enum.GetName(typeEnumConvert, Convert.ToInt32(rule.data));
                            rule.data = Enum.Parse(typeEnumConvert, nameEnumValue, false);
                        }
                    }
                    else
                        rule.data = Convert.ChangeType(rule.data, EntityPropertyType);
                }
                catch
                {
                    return null;
                }

                ICriterion criterionFilter = GetCriterionFilter(filterType, rule.field, rule.data);
                if (criterionFilter != null)
                    juctionExpress.Add(criterionFilter);
            }

            if (juctionExpress != null)
                criterionList.Add(juctionExpress);

            gridProperties.TotalFoundRegisters = ActiveRecordMediator.Count(typeof(EntityType), criterionList.ToArray());
            methodFindAll = typeEntity.GetMethod("SlicedFindAll", new Type[] { typeof(int), typeof(int), typeof(Order[]), typeof(ICriterion[]) });
            object entitiesObjFilter = methodFindAll.Invoke(null, new object[] { indexInit, countReturn, orders, criterionList.ToArray() });

            if (entitiesObjFilter != null)
            {
                EntityType[] entities = (EntityType[])entitiesObjFilter;

                string propFKOrder = String.Empty;
                JqGridColumnCustom propertyFound = gridProperties.ColumnsCustom.FirstOrDefault(x => x.PropertyColumn.Equals(gridProperties.PropertyOrder));
                if (propertyFound != null)
                    propFKOrder = propertyFound.PropertyFKName;
                switch (gridProperties.OrderType)
                {
                    case KPJqGridTypeOrderEnum.ASC:
                        return entities.OrderBy(x => x.GetType().GetProperty(gridProperties.PropertyOrder).GetValue(x, null), new GenericComparer(propFKOrder)).ToArray();
                    case KPJqGridTypeOrderEnum.DESC:
                        return entities.OrderByDescending(x => x.GetType().GetProperty(gridProperties.PropertyOrder).GetValue(x, null), new GenericComparer(propFKOrder)).ToArray();
                }
            }

            return null;
        }

        /// <summary>
        /// Execute Filter on Entities
        /// </summary>
        /// <param name="page">Page number</param>
        /// <param name="rows">Rows per page</param>
        /// <param name="sidx">Column order</param>
        /// <param name="sord">Type order</param>
        /// <param name="_search">If true, was executed the search</param>
        /// <param name="filters">FilterJson JqGrid</param>
        /// <param name="sessionUser">User logged</param>
        /// <param name="entity">Entity name</param>
        /// <param name="company">Company logged</param>
        /// <param name="propertyCompany">Property company</param>
        /// <param name="initialFilter">Base static Filter</param>
        /// <param name="initialOrder">Base static Order</param>
        /// <param name="colModel">Struct columns JqGrid</param>
        /// <returns></returns>
        public string GetGenericEntityFilter(int page, int rows, string sidx, string sord, bool _search, string filters,
                                             string sessionUser, string propertyCompany,
                                            string initialFilter, string initialOrder, object[] colModel)
        {
            string propertyCompanyDecript = String.Empty;
            if (!String.IsNullOrEmpty(propertyCompany))
                propertyCompanyDecript = KPCryptography.DecryptStringAES(propertyCompany);
            ICriterion filter = null;
            if (!String.IsNullOrEmpty(initialFilter))
            {
                try
                {
                    filter = SerializerHelper.DeserializationObj<ICriterion>(Convert.FromBase64String(initialFilter));
                }
                catch { }
            }
            Order order = null;
            if (!String.IsNullOrEmpty(initialOrder))
            {
                try
                {
                    order = SerializerHelper.DeserializationObj<Order>(Convert.FromBase64String(initialOrder));
                }
                catch { }
            }
            KPSessionJQGrid sessionJQGrid = null;
            if (!String.IsNullOrEmpty(sessionUser))
            {
                try
                {
                    sessionJQGrid = SerializerHelper.DeserializationObj<KPSessionJQGrid>(Convert.FromBase64String(sessionUser));
                }
                catch { }
            }

            if (sessionJQGrid == null)
                return KPJsonJqGrid.JsonEmpty;

            KPGridJsonProperties gridProperties = new KPGridJsonProperties(_search, page, rows, sidx, sord, filters, sessionJQGrid,
                                                                            propertyCompanyDecript, filter, order, colModel);

            Type entityType = sessionJQGrid.TypeEntity;
            if (entityType != null)
            {
                object entities = Array.CreateInstance(entityType, 0);
                object entitiesJson = KPJsonJqGrid.JsonEmpty;

                MethodInfo methodFilterEntities = this.GetType().GetMethod("FilterEntities", BindingFlags.Instance | BindingFlags.NonPublic);
                MethodInfo methodGenericFilterEntities = methodFilterEntities.MakeGenericMethod(entityType);
                entities = methodGenericFilterEntities.Invoke(this, new object[] { gridProperties });

                MethodInfo methodFilterGetJsonToJqGrid = this.GetType().GetMethod("GetJsonToJqGrid", BindingFlags.Instance | BindingFlags.NonPublic);
                entitiesJson = methodFilterGetJsonToJqGrid.Invoke(this, new object[] { entities, gridProperties });

                if (!String.IsNullOrWhiteSpace(sessionJQGrid.MasterDetailID))
                {
                    DetailSession detailSession = KPSessionHelper.GetSessionMasterDetailList(sessionJQGrid.SessionPageID, sessionJQGrid.MasterDetailID);
                    if (entities != null && detailSession == null)
                    {
                        detailSession = new DetailSession();
                        detailSession.Entities.AddRange((entities as Array));
                        KPSessionHelper.SetSessionMasterDetailList(sessionJQGrid.SessionPageID, sessionJQGrid.MasterDetailID, detailSession);
                    }
                    else
                    {
                        entitiesJson = methodFilterGetJsonToJqGrid.Invoke(this, new object[] { detailSession.Entities.ToArray(), gridProperties });
                    }
                }

                return entitiesJson.ToString();
            }

            return KPJsonJqGrid.JsonEmpty;
        }

        /// <summary>
        /// Create ICriterion Filter
        /// </summary>
        /// <param name="filterEnum">Enum type filter</param>
        /// <param name="field">Column or Property target</param>
        /// <param name="data">Value for comparer</param>
        /// <returns>Instance ICriterion for Filter</returns>
        internal ICriterion GetCriterionFilter(KPJqGridTypeFilterEnum filterEnum, string field, object data)
        {
            ICriterion criterionFilter = null;
            switch (filterEnum)
            {
                case KPJqGridTypeFilterEnum.Eq:
                    criterionFilter = Expression.Eq(field, data);
                    break;
                case KPJqGridTypeFilterEnum.Like:
                    if (data.GetType() == typeof(String))
                        criterionFilter = Expression.Like(field, data.ToString(), MatchMode.Anywhere);
                    else
                        criterionFilter = Expression.Eq(field, data);
                    break;
                case KPJqGridTypeFilterEnum.InsensitiveLike:
                    criterionFilter = Expression.InsensitiveLike(field, data);
                    break;
                case KPJqGridTypeFilterEnum.IsNull:
                    criterionFilter = Expression.IsNull(field);
                    break;
                case KPJqGridTypeFilterEnum.IsNotNull:
                    criterionFilter = Expression.IsNotNull(field);
                    break;
                case KPJqGridTypeFilterEnum.LikeInit:
                    if (data.GetType() == typeof(String))
                        criterionFilter = Expression.Like(field, data.ToString(), MatchMode.Start);
                    else
                        criterionFilter = Expression.Eq(field, data);
                    break;
                case KPJqGridTypeFilterEnum.LikeEnd:
                    if (data.GetType() == typeof(String))
                        criterionFilter = Expression.Like(field, data.ToString(), MatchMode.End);
                    else
                        criterionFilter = Expression.Eq(field, data);
                    break;
                case KPJqGridTypeFilterEnum.In:
                    criterionFilter = Expression.In(field, (object[])data);
                    break;
                default:
                    break;
            }

            return criterionFilter;
        }

        /// <summary>
        /// Manages columns types on JqGrid
        /// </summary>
        /// <param name="colModel">Struct column</param>
        /// <returns>Object JqGridColumnCustom converted</returns>
        internal static JqGridColumnCustom GetColumnCustom(string colModel)
        {
            JqGridColumnCustom gridColumnCustom = null;

            // Pegar dados do KPEnumModel
            if (colModel.Contains("_"))
            {
                gridColumnCustom = new JqGridColumnCustom(typeof(KPEnumModel));

                string[] paramArray = colModel.Split('_');
                if (paramArray.Length != 2)
                    return null;

                gridColumnCustom.PropertyColumn = paramArray[0];
                gridColumnCustom.NamespaceEnum = paramArray[1];
            }
            // Pegar dados do KPEntityModel - FK
            else if (colModel.Contains("."))
            {
                gridColumnCustom = new JqGridColumnCustom(typeof(KPEntityModel));

                string[] paramArray = colModel.Split('.');
                if (paramArray.Length != 2)
                    return null;

                gridColumnCustom.PropertyColumn = paramArray[0];
                gridColumnCustom.PropertyFKName = paramArray[1];
            }
            else
            {
                gridColumnCustom = new JqGridColumnCustom(typeof(KPColumnModel));
                gridColumnCustom.PropertyColumn = colModel;
            }

            return gridColumnCustom;
        }
    }

    /// <summary>
    /// Struct for Column Custom JqGrid
    /// </summary>
    internal class JqGridColumnCustom
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="itemModelType">Type inherits from KPItemModel</param>
        public JqGridColumnCustom(Type itemModelType)
        {
            KPItemsModelType = itemModelType;
        }

        /// <summary>
        /// Type inherits from KPItemModel
        /// </summary>
        public Type KPItemsModelType { get; private set; }

        /// <summary>
        /// Column name
        /// </summary>
        public string PropertyColumn { get; set; }

        /// <summary>
        /// Namespace Enumerator
        /// </summary>
        public string NamespaceEnum { get; set; }

        /// <summary>
        /// Column/Property name Foreign Key
        /// </summary>
        public string PropertyFKName { get; set; }
    }
}
