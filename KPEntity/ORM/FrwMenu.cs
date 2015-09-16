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
using KPAttributes;
using KPCore.KPValidator;
using KPEntity.ORM;
using KPEnumerator.KPEntity;
using NHibernate.Validator.Constraints;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace KPEntity.ORM
{
    /// <summary>
    /// <para>Authors: Juliano Tiago Rinaldi and 
    /// Tiago Antonio Jacobi</para>
    /// </summary>
    [Serializable]
    [KPEntityTable("FRW_MENU")]
    public class FrwMenu : KPActiveRecordBase<FrwMenu>
    {

        #region ID_MENU
        [KPDisplayName("FRWEntity_ID")]
        [PrimaryKey(Column = "ID_MENU")]
        public virtual Int32 IdMenu { get; set; }
        #endregion ID_MENU

        #region ID_PARENT
        [KPDisplayName("FRWEntity_MenuParent")]
        [BelongsTo("ID_PARENT", Lazy = FetchWhen.OnInvoke)]
        public virtual FrwMenu objIdParent { get; set; }
        #endregion ID_PARENT

        #region ID_WINDOW
        [NotNull]
        [KPDisplayName("FRWEntity_Window")]
        [BelongsTo("ID_WINDOW", Lazy = FetchWhen.OnInvoke)]
        public virtual FrwWindow objIdWindow { get; set; }
        #endregion ID_WINDOW

        #region WINDOW_TITLE_MENU
        [NotNull, NotEmpty]
        [KPDisplayName("FRWEntity_WindowTitleMenu")]
        [Length(Max = 128)]
        [Property(Column = "WINDOW_TITLE_MENU")]
        public virtual String WindowTitleMenu { get; set; }
        #endregion WINDOW_TITLE_MENU

        #region SEQUENCE
        [NotNull]
        [KPDisplayName("FRWEntity_Sequence")]
        [Property(Column = "SEQUENCE")]
        public virtual Int32? Sequence { get; set; }
        #endregion SEQUENCE

        #region IS_VISIBLE
        [NotNull]
        [KPDisplayName("FRWEntity_IsVisible")]
        [Property(Column = "IS_VISIBLE")]
        public virtual StateVisibleEnum? IsVisible { get; set; }
        #endregion IS_VISIBLE

        #region IMAGE_URL
        [KPDisplayName("FRWEntity_ImageUrl")]
        [Length(Max = 255)]
        [Property(Column = "IMAGE_URL")]
        public virtual String ImageUrl { get; set; }
        #endregion IMAGE_URL

        #region WIDTH
        [KPDisplayName("FRWEntity_Width")]
        [Property(Column = "WIDTH")]
        public virtual Int32? Width { get; set; }
        #endregion WIDTH

        #region HEIGHT
        [KPDisplayName("FRWEntity_Height")]
        [Property(Column = "HEIGHT")]
        public virtual Int32? Height { get; set; }
        #endregion HEIGHT

        #region MIN_WIDTH
        [KPDisplayName("FRWEntity_MinWidth")]
        [Property(Column = "MIN_WIDTH")]
        public virtual Int32? MinWidth { get; set; }
        #endregion MIN_WIDTH

        #region MIN_HEIGHT
        [KPDisplayName("FRWEntity_MinHeight")]
        [Property(Column = "MIN_HEIGHT")]
        public virtual Int32? MinHeight { get; set; }
        #endregion MIN_HEIGHT

    }
}
