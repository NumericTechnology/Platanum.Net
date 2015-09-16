﻿/*
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
using Castle.ActiveRecord;
using NHibernate.Validator.Engine;

namespace KPCore.KPValidator
{
    /// <summary>
    /// <para>Authors: Juliano Tiago Rinaldi and 
    /// Tiago Antonio Jacobi</para>
    /// </summary>
    [Serializable]
    public class KPExceptionValidator : Exception
    {
        public ActiveRecordBase Entity { get; private set; }
        public InvalidValue[] Erros { get; private set; }

        public KPExceptionValidator(ActiveRecordBase entity, InvalidValue[] errors)
        {
            Entity = entity;
            Erros = errors;
        }
    }
}