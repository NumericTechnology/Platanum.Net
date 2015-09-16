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

using KPComponents.KPJqGrid;
using KPComponents.KPSession;
using KPCore;
using KPCore.KPException;
using KPCore.KPSecurity;
using KPCore.KPUtil;
using KPEnumerator.KPComponents;
using KPEnumerator.KPSecurity;
using KPExtension;
using NHibernate.Criterion;
using System;
using System.ComponentModel;
using System.Text;
using System.Web.UI.WebControls;

namespace KPComponents.KPComponents.KPJqGrid
{
    /// <summary>
    /// <para>Authors: Juliano Tiago Rinaldi and 
    /// Tiago Antonio Jacobi</para>
    /// </summary>
    public class KPJqGridControl
    {
        private HiddenField HiddenKey = null;
        private string urlWCF;

        internal KPItemModelCollection KPItemsModel { get; private set; }
        internal string PrimaryKey { get; private set; }
        internal string UrlService
        {
            get
            { return urlWCF; }
            set
            {
                if (value.Contains("~"))
                    urlWCF = System.Web.VirtualPathUtility.ToAbsolute(value);
                else
                    urlWCF = value;
            }
        }

        internal string Caption { get; set; }
        internal Type TypeEntity { get; private set; }
        internal string PropertyCompanyEntity { get; set; }
        internal ICriterion InitialFilter { get; private set; }
        internal Order InitialOrder { get; private set; }

        internal int Width { get; set; }
        internal int Height { get; set; }
        internal KPJqGridRowNumEnum RowNum { get; set; }
        internal KPJqGridTypeOrderEnum OrderRecords { get; set; }
        internal KPJqGridDoubleClickActionEnum DoubleClickAction { get; set; }
        internal bool ViewTotalRecordsLabel { get; set; }
        internal string RowSelectedKey
        {
            get
            {
                if (HiddenKey != null)
                    return HiddenKey.Value;

                return String.Empty;
            }
        }
        internal string MasterDetailID { get; set; }

        public KPJqGridControl(string primaryKey, HiddenField hiddenKey, Type typeEntity, KPItemModelCollection KPitemsModel,
                              KPJqGridDoubleClickActionEnum DoubleClickAction, ICriterion filter, Order order)
        {
            #region Filter/Order
            this.InitialFilter = filter;
            this.InitialOrder = order;
            #endregion

            this.HiddenKey = hiddenKey == null ? new HiddenField() : hiddenKey;
            this.Caption = String.Empty;
            this.PrimaryKey = primaryKey;
            this.UrlService = System.Web.VirtualPathUtility.ToAbsolute("~/WCF/EntityService.svc/GetEntityFilter");
            this.TypeEntity = typeEntity;
            this.KPItemsModel = KPitemsModel;
            this.DoubleClickAction = DoubleClickAction;

            this.RowNum = KPJqGridRowNumEnum.RowsView_10;
            this.OrderRecords = KPJqGridTypeOrderEnum.ASC;
            this.ViewTotalRecordsLabel = true;
            this.Height = 100;
            this.Width = 300;
        }

        public string Build(string pagerID, string tableID, KPPageBase page)
        {
            StringBuilder sbColNames = new StringBuilder();
            StringBuilder sbColModel = new StringBuilder();
            StringBuilder sbSortName = new StringBuilder();
            sbSortName.Append(this.PrimaryKey);

            string fieldNameTryException = null;
            try
            {
                foreach (KPItemModel item in KPItemsModel)
                {
                    fieldNameTryException = item.FieldName;

                    #region Create Display Name
                    if (sbColNames.Length != 0)
                        sbColNames.Append(", ");

                    if (!String.IsNullOrEmpty(item.HeaderName))
                        sbColNames.AppendFormat("'{0}'", item.HeaderName);
                    else
                    {
                        DisplayNameAttribute[] displayName = TypeEntity.GetProperty(item.FieldName).GetCustomAttributes(typeof(DisplayNameAttribute), false) as DisplayNameAttribute[];
                        if (displayName.Length > 0)
                            sbColNames.AppendFormat("'{0}'", displayName[0].DisplayName);
                        else
                            sbColNames.AppendFormat("'{0}'", item.FieldName);
                    }
                    #endregion

                    #region Create colModels
                    if (sbColModel.Length != 0)
                    {
                        sbColModel.AppendFormat(",{0}", Environment.NewLine);
                    }

                    sbColModel.Append("{");

                    if (item is KPEntityModel)
                    {
                        sbColModel.AppendFormat(" name: '{0}.{1}', index: '{0}', ", item.FieldName, ((KPEntityModel)item).FieldNameDescription);
                    }
                    else if (item is KPEnumModel)
                    {
                        Type typeEnum = KPGenericUtil.GetTypeByNamespace(((KPEnumModel)item).NamespaceEnum);
                        string itemsComboBoxSearch = this.GetEnumeratorsSearchJqGrid(typeEnum);
                        sbColModel.AppendFormat(" name: '{0}_{1}', index: '{0}', formatter:'select', stype: 'select', ", item.FieldName, ((KPEnumModel)item).NamespaceEnum);
                        sbColModel.AppendFormat("edittype:'select', editoptions: {{ value: '{0}' }}, ", itemsComboBoxSearch);
                        sbColModel.AppendFormat("searchoptions:{{ sopt:['eq'], value: '{0}' }}, ", itemsComboBoxSearch);
                    }
                    else if (item is KPBooleanModel)
                    {
                        KPBooleanModel itemBoolean = item as KPBooleanModel;
                        string itemsComboBoxSearch = this.GetBooleanSearchJqGrid(itemBoolean.CustomTrue, itemBoolean.CustomFalse);
                        sbColModel.AppendFormat(" name: '{0}', index: '{0}', formatter:'select', stype: 'select', ", item.FieldName);
                        sbColModel.AppendFormat("edittype:'select', editoptions: {{ value: '{0}' }}, ", itemsComboBoxSearch);
                        sbColModel.AppendFormat("searchoptions:{{ sopt:['eq'], value: '{0}' }}, ", itemsComboBoxSearch);
                    }
                    else
                    {
                        if (!item.FieldName.Equals(PrimaryKey))
                            sbColModel.AppendFormat(" name: '{0}', index: '{0}', ", item.FieldName);
                    }

                    if (item.FieldName.Equals(PrimaryKey))
                        sbColModel.AppendFormat(" name: '{0}', index: '{0}', key: true, ", item.FieldName);

                    sbColModel.AppendFormat("width: '{0}', sortable: '{1}', editable: '{2}', hidden: {3} ",
                                                item.Width,
                                                item.Sortable.ToString().ToLower(),
                                                item.Editable.ToString().ToLower(),
                                                (!item.Visible).ToString().ToLower());

                    if (item is KPItemTextModel)
                    {
                        var itemText = item as KPItemTextModel;
                        if (!itemText.Mask.Equals(KPMaskTypeClassEnum.ALPHANUMERIC))
                        {
                            sbColModel.Append(@",cellattr: function(rowId, val, rawObject) { return "" class='" + itemText.Mask.GetTypeValue() + @"'""; }");
                        }
                    }

                    sbColModel.Append("}");
                    #endregion
                }
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format("Ocorreu um erro com o Campo: {0}. Verifica se a propriedade está de acordo com a Entidade {1}.", fieldNameTryException, this.TypeEntity), ex);
            }

            //Artigo interessante:
            //http://www.codeproject.com/Articles/58357/Using-jqGrid-s-search-toolbar-with-multiple-filter

            //Verifica posição da tela Find
            //http://www.ok-soft-gmbh.com/jqGrid/multisearchfilter.htm

            //http://trirand.com/blog/jqgrid/jqgrid.html

            KPSessionJQGrid sessionJQGrid = new KPSessionJQGrid()
            {
                SecuritySession = page.SecuritySession,
                TypeEntity = TypeEntity,
                SessionPageID = page.SessionPageID,
                MasterDetailID = MasterDetailID
            };
            string sessionUser = String.Empty;
            if (sessionJQGrid != null)
            {
                sessionUser = Convert.ToBase64String(SerializerHelper.SerializationObj(sessionJQGrid));
            }
            string propertyCompanyCrypt = String.Empty;
            if (!String.IsNullOrEmpty(PropertyCompanyEntity))
            {
                propertyCompanyCrypt = KPCryptography.EncryptStringAES(PropertyCompanyEntity);
            }
            string initialFilter = String.Empty;
            if (InitialFilter != null)
            {
                initialFilter = Convert.ToBase64String(SerializerHelper.SerializationObj(InitialFilter));
            }
            string initialOrder = String.Empty;
            if (InitialOrder != null)
            {
                initialOrder = Convert.ToBase64String(SerializerHelper.SerializationObj(InitialOrder));
            }

            StringBuilder buildGrid = new StringBuilder();
            buildGrid.Append(
            @"<script type=""text/javascript"">
            function GetcolModels() {
                var colModels = new Array();
                var colModelsObj = $('#" + tableID + @"').jqGrid('getGridParam', 'colModel');
                for (i=0;i<colModelsObj.length;i++)
                {colModels[i] = colModelsObj[i].name;}

                return colModels;
            }

            function successFunction(jsondata, stat) { 
                var thegrid = jQuery('#" + tableID + @"')[0];
                thegrid.addJSONData(JSON.parse(jsondata.d));
                setLoading(false);
            }

            function errorFunction(jsondata, stat) { 
                alertBox('Ops! Algum problema aconteceu com nosso servidor, por favor feche a tela e abra novamente.');
                setLoading(false);
            }

            function getGridData(pdata) {
                setLoading(true);
                $('#" + HiddenKey.ClientID + @"').val(null);
                var params = new Object();

                params.page = pdata.page;
                params.rows = pdata.rows;
                params.sidx = pdata.sidx;
                params.sord = pdata.sord;
                params._search = pdata._search;
                if (pdata.searchField != 'undefined')
                    params.searchField = pdata.searchField;
                if (pdata.searchOper != 'undefined')
                    params.searchOper = pdata.searchOper;
                if (pdata.searchString != 'undefined')
                    params.searchString = pdata.searchString;
                if (pdata.filters != 'undefined')
                    params.filters = pdata.filters;

                params.colModel = GetcolModels();
                params.user = '" + sessionUser + @"';
                params.propertyCompany = '" + propertyCompanyCrypt + @"';
                params.initialFilter = '" + initialFilter + @"';
                params.initialOrder = '" + initialOrder + @"';

                $.ajax(
                {
                    url: '" + UrlService + @"',
                    data: JSON.stringify(params),
                    dataType: 'json',
                    type: 'POST',
                    contentType: 'application/json; charset=utf-8',
                    success: successFunction,
                    error: errorFunction
                });
            }
            
            var lastSel;

            function loadGrid" + tableID + @"(){
                $('#" + tableID + @"').jqGrid({
                    datatype: function (pdata) {getGridData(pdata);},
                    mtype : 'POST',
                    height: " + Height + @",
                    width: " + Width + @",
                    colNames: [" + sbColNames.ToString() + @"],
                    colModel: [" + sbColModel.ToString() + @"],
                    rowNum: " + RowNum.GetTypeValue() + @",
                    rowList: [" + GetRowList() + @"],
                    pager: '#" + pagerID + @"',
                    sortname: '" + sbSortName.ToString() + @"',
                    viewrecords: " + ViewTotalRecordsLabel.ToString().ToLower() + @",
                    sortorder: '" + OrderRecords.ToString().ToLower() + @"',
                    caption: '" + Caption + @"',
                    gridComplete: function() { generateMaskValidators(); },
                    onSelectRow: function(rowId)
                                 {
                                    if(rowId && rowId!==lastSel)
                                    { 
                                        jQuery(this).restoreRow(lastSel); 
                                        lastSel=rowId; 
                                    }
                                    $('#" + HiddenKey.ClientID + @"').val(rowId);
                                 },
                    ondblClickRow: function(rowId) 
                                {
                                    $('#" + HiddenKey.ClientID + @"').val(rowId);
                                    $('" + DoubleClickAction.GetTypeValue() + @"').click();
                                },

                });

                $('#" + tableID + @"').jqGrid('navGrid', '#" + pagerID + @"', { edit: false, add: false, del: false, search: true },{},{},{},
                    {
                        sopt:[" + this.JqGridSortType() + @"],
                        groupOps: [ { op: 'AND', text: 'E' }, { op: 'OR', text: 'OU' } ], 
                        multipleSearch: true, 
                        showQuery: false,
                        searchOnEnter: true,
                        closeOnEscape: false,
                        Reset: 'Limpar filtros'
                    }
                  );

                $('#" + tableID + @"').jqGrid('filterToolbar',{stringResult: true, searchOnEnter: true});

                $(window).bind('resize', function () {");

            if (string.IsNullOrWhiteSpace(this.MasterDetailID)) {
                buildGrid.Append(@"
                    $('#" + tableID + @"').jqGrid('setGridWidth', ($('#" + tableID + @"').closest('.Content').width()-2));
                    $('#" + tableID + @"').jqGrid('setGridHeight', ($('#" + tableID + @"').closest('.Content').height()-114));");
            } else {
                buildGrid.Append(@"
                    $('#" + tableID + @"').jqGrid('setGridWidth', ($('#" + tableID + @"').closest('.KPMasterDetailField').width()-2));
                    $('#" + tableID + @"').jqGrid('setGridHeight', ($('#" + tableID + @"').closest('.KPMasterDetailField').height()-105));");
            }

            buildGrid.Append(@"
                }).trigger('resize');
            }

            $(function () {
                loadGrid" + tableID + @"();
                var updatePanel = Sys.WebForms.PageRequestManager.getInstance();
                updatePanel.add_pageLoaded(loadGrid" + tableID + @");
            });

           </script>");
            return buildGrid.ToString();
            /*

            // Verificar o problema no Grid de sumir ao clicar em salvar
            // Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function(){alert('teste');});

            */
        }

        private string GetEnumeratorsSearchJqGrid(Type enumType)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(":Todos");
            foreach (var item in Enum.GetValues(enumType))
                sb.AppendFormat(";{0}:{1}", item.GetHashCode(), ((Enum)item).GetTypeDescription());

            return sb.ToString();
        }

        private string GetBooleanSearchJqGrid(string customTrue, string customFalse)
        {
            if (String.IsNullOrEmpty(customTrue))
                customTrue = "Sim";
            if (String.IsNullOrEmpty(customFalse))
                customFalse = "Não";

            StringBuilder sb = new StringBuilder();
            sb.Append(":Todos");
            sb.AppendFormat(";{0}:{1}", true, customTrue);
            sb.AppendFormat(";{0}:{1}", false, customFalse);

            return sb.ToString();
        }

        private string GetRowList()
        {
            StringBuilder sb = new StringBuilder();
            KPJqGridRowNumEnum[] gridRowNums = Enum.GetValues(typeof(KPJqGridRowNumEnum)) as KPJqGridRowNumEnum[];

            foreach (KPJqGridRowNumEnum item in gridRowNums)
            {
                if (sb.Length != 0)
                    sb.Append(",");

                sb.Append(item.GetTypeValue());
            }

            return sb.ToString();
        }

        private string JqGridSortType()
        {
            StringBuilder sbSort = new StringBuilder();
            KPJqGridTypeFilterEnum[] filterTypes = Enum.GetValues(typeof(KPJqGridTypeFilterEnum)) as KPJqGridTypeFilterEnum[];
            foreach (KPJqGridTypeFilterEnum item in filterTypes)
            {
                if (sbSort.Length != 0)
                {
                    sbSort.Append(",");
                }
                sbSort.AppendFormat("'{0}'", item.GetTypeDescription());
            }

            return sbSort.ToString();
        }
    }
}
