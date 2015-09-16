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

using KPEntity.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KPComponents.KPSecurity
{
    /// <summary>
    /// <para>Authors: Juliano Tiago Rinaldi and 
    /// Tiago Antonio Jacobi</para>
    /// </summary>
    internal class Authenticate
    {
        public Authenticate(FrwUser user, bool isAuthenticate)
            : this(user, isAuthenticate, null, null)
        {

        }

        public Authenticate(FrwUser user, bool isAuthenticate, FrwCompany[] companies, FrwCompany defaultCompany)
        {
            User = user;
            IsAuthenticate = isAuthenticate;
            Companies = companies;
            DefaultCompany = defaultCompany;
        }

        public FrwUser User { get; set; }
        public bool IsAuthenticate { get; set; }
        public FrwCompany[] Companies { get; set; }
        public FrwCompany DefaultCompany { get; set; }
    }
}
