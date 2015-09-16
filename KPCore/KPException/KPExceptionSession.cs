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
using KPEnumerator.KPSecurity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KPExtension;

namespace KPCore.KPException
{
    /// <summary>
    /// <para>Authors: Juliano Tiago Rinaldi and 
    /// Tiago Antonio Jacobi</para>
    /// </summary>
    [Serializable]
    public class KPExceptionSession : Exception
    {
        public string SessionKey { get; private set; }

        public KPExceptionSession(string sessionKey, string message, Exception ex)
            : base(message, ex)
        {
            SessionKey = sessionKey;
        }

        public KPExceptionSession(KPSessionKeyEnum sessionKey, string message, Exception ex)
            : this(sessionKey.GetTypeValue().ToString(), message, ex)
        {
        }

        public KPExceptionSession(KPSessionKeyEnum sessionKey, string message)
            : this(sessionKey, message, null)
        {
        }

    }
}
