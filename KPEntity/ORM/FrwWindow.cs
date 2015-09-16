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
    [KPEntityTable("FRW_WINDOW")]
    public class FrwWindow : KPActiveRecordBase<FrwWindow>
    {

        #region ID_WINDOW
        [KPDisplayName("FRWEntity_ID")]
        [PrimaryKey(Column = "ID_WINDOW")]
        public virtual Int32 IdWindow { get; set; }
        #endregion ID_WINDOW

        #region WINDOW_TITLE
        [NotNull, NotEmpty]
        [KPDisplayName("FRWEntity_WindowTitle")]
        [Length(Max = 128)]
        [Property(Column = "WINDOW_TITLE")]
        public virtual String WindowTitle { get; set; }
        #endregion WINDOW_TITLE

        #region WINDOW_DESCRIPTION
        [KPDisplayName("FRWEntity_WindowDescription")]
        [Length(Max = 255)]
        [Property(Column = "WINDOW_DESCRIPTION")]
        public virtual String WindowDescription { get; set; }
        #endregion WINDOW_DESCRIPTION

        #region WINDOW_PATH
        [NotNull, NotEmpty]
        [KPDisplayName("FRWEntity_WindowPath")]
        [Length(Max = 255)]
        [Property(Column = "WINDOW_PATH")]
        public virtual String WindowPath { get; set; }
        #endregion WINDOW_PATH

        #region WINDOW_ENUM
        [NotEmpty, NotNull]
        [KPDisplayName("FRWEntity_WindowEnum")]
        [Length(Max = 255)]
        [Property(Column = "WINDOW_ENUM")]
        public virtual String WindowEnum { get; set; }
        #endregion WINDOW_ENUM

        #region WINDOW_WIDTH
        [KPDisplayName("FRWEntity_Width")]
        [Property(Column = "WINDOW_WIDTH")]
        public virtual Int32? WindowWidth { get; set; }
        #endregion WINDOW_WIDTH

        #region WINDOW_HEIGHT
        [KPDisplayName("FRWEntity_Height")]
        [Property(Column = "WINDOW_HEIGHT")]
        public virtual Int32? WindowHeight { get; set; }
        #endregion WINDOW_HEIGHT

        #region WINDOW_ALIAS
        [KPDisplayName("FRWEntity_WindowAlias")]
        [Length(Max = 10)]
        [Property(Column = "WINDOW_ALIAS")]
        public virtual String WindowAlias { get; set; }
        #endregion WINDOW_ALIAS

    }
}
