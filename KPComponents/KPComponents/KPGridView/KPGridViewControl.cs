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
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KPComponents.KPJqGrid;
using KPCore.KPUtil;
using System.Reflection;
using KPAttributes;
using KPExtension;
using KPEnumerator.KPComponents;

namespace KPComponents
{
    /// <summary>
    /// <para>Authors: Juliano Tiago Rinaldi and 
    /// Tiago Antonio Jacobi</para>
    /// </summary>
    public class KPGridViewControl : GridView
    {
        private string SESSION_DATASOURCE = "SESSION_DATASOURCE";

        #region Properties
        public KPFormItemGrid ItemGrid { get; private set; }

        public KPFormGridModel GridConfig { get; private set; }
        #endregion

        public KPGridViewControl(Page page, KPFormItemGrid itemGrid, KPFormGridModel gridConfig)
        {
            this.ID = "kpgrid_" + itemGrid.GridIDConfig;
            SESSION_DATASOURCE += "_" + itemGrid.GridIDConfig;
            this.Page = page;
            if (Page != null && Page.Session != null && !Page.IsPostBack)
            {
                Page.Session.Remove(SESSION_DATASOURCE);
            }

            this.Width = itemGrid.Width;
            this.Height = itemGrid.Height;
            ItemGrid = itemGrid;
            GridConfig = gridConfig;
            AutoGenerateColumns = GridConfig.AutoGenerateColumns;

            if (GridConfig.EnableMemoryDataSource)
            {
                if (Page != null && Page.Session != null && !Page.IsPostBack)
                {
                    if (GridConfig.KPGetObjectDataSourceDelegate != null)
                        Page.Session[SESSION_DATASOURCE] = GridConfig.KPGetObjectDataSourceDelegate();
                }
                DataSource = Page.Session[SESSION_DATASOURCE];
            }
            else
            {
                if (GridConfig.KPGetObjectDataSourceDelegate != null)
                    DataSource = GridConfig.KPGetObjectDataSourceDelegate();
            }

            if (this.HeaderRow != null)
            {
                this.HeaderRow.TableSection = TableRowSection.TableHeader;
            }

            EnsureChildControls();
        }

        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            if (ItemGrid.GridIDConfig == GridConfig.GridID)
            {
                if (!GridConfig.AutoGenerateColumns)
                {
                    ButtonField btDelete = new ButtonField()
                    {
                        CommandName = "Delete",
                        ButtonType = ButtonType.Image,
                        ImageUrl = "/Assets/Imgs/Themes/Default/delete_line.png"
                    };

                    //TODO Jacobi neste ponto o width deve ser igual ao MaxWidth no style.
                    btDelete.ItemStyle.Width = 18;
                    btDelete.HeaderStyle.Width = 18;
                    btDelete.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    btDelete.HeaderStyle.CssClass = "KPGridViewColumnHeader";
                    btDelete.ItemStyle.CssClass = "KPGridViewColumnRow";
                    btDelete.ControlStyle.CssClass = "KPGridViewDeleteRow";

                    this.Columns.Add(btDelete);

                    foreach (KPFormGridField gridField in GridConfig.KPGridFieldsConfig)
                    {
                        string itemCssClass = "KPGridViewColumnRow";
                        string headerName = gridField.HeaderName;

                        if (String.IsNullOrEmpty(headerName))
                        {
                            headerName = gridField.Field;
                        }

                        BoundField fieldColl = new BoundField()
                        {
                            DataField = gridField.Field,
                            HeaderText = headerName,
                        };

                        if (gridField.Width > 0)
                        {
                            //TODO Jacobi neste ponto o width deve ser igual ao MaxWidth no style.
                            fieldColl.ItemStyle.Width = gridField.Width;
                            fieldColl.HeaderStyle.Width = gridField.Width;
                        }

                        if (!KPMaskTypeClassEnum.ALPHANUMERIC.Equals(gridField.Mask))
                        {
                            itemCssClass += " " + gridField.Mask.GetTypeValue();
                        }

                        fieldColl.HeaderStyle.CssClass = "KPGridViewColumnHeader";
                        fieldColl.ItemStyle.CssClass = itemCssClass;

                        this.Columns.Add(fieldColl);
                    }
                }

                if (this.Columns.Count > 0)
                {
                    this.ShowHeaderWhenEmpty = true;
                    this.SelectedRowStyle.BackColor = System.Drawing.Color.Blue;
                    this.DataBind();
                }
            }
        }

        #region Methods Memory DataSource
        public void AddEntityInMemoryDataSource(object entity)
        {
            if (GridConfig.EnableMemoryDataSource)
            {
                if (Page.Session[SESSION_DATASOURCE] != null)
                {
                    Type entityListType = Page.Session[SESSION_DATASOURCE].GetType();
                    MethodInfo addMethod = entityListType.GetMethod("Add");
                    if (addMethod != null)
                    {
                        addMethod.Invoke(Page.Session[SESSION_DATASOURCE], new object[] { entity });
                        DataSource = Page.Session[SESSION_DATASOURCE];
                        DataBind();
                    }
                }
            }
        }

        public void RemoveEntityInMemoryDataSource(object entity)
        {
            if (GridConfig.EnableMemoryDataSource)
            {
                if (Page.Session[SESSION_DATASOURCE] != null)
                {
                    Type entityListType = Page.Session[SESSION_DATASOURCE].GetType();
                    MethodInfo removeMethod = entityListType.GetMethod("Remove");
                    if (removeMethod != null)
                    {
                        removeMethod.Invoke(Page.Session[SESSION_DATASOURCE], new object[] { entity });
                        DataSource = Page.Session[SESSION_DATASOURCE];
                        DataBind();
                    }
                }
            }
        }

        public void RemoveEntityInMemoryDataSource<Entity>(int index)
        {
            List<Entity> entities = this.GetEntitiesInMemoryDataSource<Entity>();
            entities = entities.GetRange(index, 1);

            this.RemoveEntityInMemoryDataSource(entities.First());
        }

        public Entity GetEntitySelectedInMemoryDataSource<Entity>()
        {
            if (GridConfig.EnableMemoryDataSource)
            {
                if (Page.Session[SESSION_DATASOURCE] != null)
                {
                    int selecteRow = this.SelectedIndex;
                    return ((IList<Entity>)Page.Session[SESSION_DATASOURCE])[selecteRow];
                }
            }

            return default(Entity);
        }

        public List<Entity> GetEntitiesInMemoryDataSource<Entity>()
        {
            if (GridConfig.EnableMemoryDataSource)
            {
                if (Page.Session[SESSION_DATASOURCE] != null)
                    return (List<Entity>)Page.Session[SESSION_DATASOURCE];
            }

            return default(List<Entity>);
        }
        #endregion
    }
}
