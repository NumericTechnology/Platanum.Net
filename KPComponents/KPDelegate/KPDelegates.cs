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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using KPBO;
using KPComponents.KPEvent;
using NHibernate.Criterion;

namespace KPComponents.KPDelegate
{
    /// <summary>
    /// <para>Authors: Juliano Tiago Rinaldi and 
    /// Tiago Antonio Jacobi</para>
    /// </summary>

    // ******** Padrão de Nomenclatura
    // Todos Delagete do KPFramework deverá começar com "KP"
    // Todos evento criado pelos delegates do KPFramework deverão começar com KPEvent

    // KPFormControl
    public delegate void KPSenderEntityBO(object entityBO);
    public delegate void KPSenderEntityBefore(object entity, KPButtonEventsArgs e);
    public delegate void KPSenderEntity(object entity);
    public delegate void KPFormClosing(object sender, KPFormEventArgs e);

    // KPFormItemCombo
    public delegate void KPComboSelectChange();
    public delegate IEnumerable KPGetComboItems();

    // KPFormItemCheckBox
    public delegate void KPCheckBoxSelect();

    // KPFormItemPassword
    public delegate void KPTextPasswordLostFocus();

    // KPFormItemText
    public delegate void KPTextLostFocus();

    // KPFormItemZoom
    public delegate void KPZoomLostFocus();

    // KPFormItemZoom
    public delegate void KPZoomClick(string fieldReturnId);

    // KPGridControl
    public delegate void KPAfterControlsCreated(object sender, EventArgs e);

    // KPGridControl; KPFormZoomModel
    public delegate ICriterion KPCriterionFilter();
    public delegate Order KPOrder();

    public delegate IEnumerable KPGetObjectDataSource();
}
