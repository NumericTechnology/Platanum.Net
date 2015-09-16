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

using KPComponents.Generic;
using KPEnumerator.KPComponents;
using System;
using System.ComponentModel;
using System.Web.UI;

namespace KPComponents
{
    /// <summary>
    /// <para>Authors: Juliano Tiago Rinaldi and 
    /// Tiago Antonio Jacobi</para>
    /// </summary>
    [Serializable]
    public abstract class KPItemTextModel : KPItemModel
    {
        #region Properties
        [DefaultValue(KPMaskTypeClassEnum.ALPHANUMERIC)]
        public KPMaskTypeClassEnum Mask
        {
            get;
            set;
        }
        #endregion
    }
}
