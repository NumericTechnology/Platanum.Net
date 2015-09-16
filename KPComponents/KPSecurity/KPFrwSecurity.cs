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
using KPCore.KPSecurity;
using KPGlobalization;
using KPEnumerator.KPGlobalization;

namespace KPComponents.KPSecurity
{
    /// <summary>
    /// KPFrwSecurity Framework Security Control
    /// <para>Authors: Juliano Tiago Rinaldi and 
    /// Tiago Antonio Jacobi</para>
    /// </summary>
    internal static class KPFrwSecurity
    {
        internal static bool securityFramework = true;

        /// <summary>
        /// Verify Security Control
        /// </summary>
        internal static void SecurityFramework()
        {
            if (securityFramework)
            {
                if (HttpContext.Current != null && HttpContext.Current.Request != null && HttpContext.Current.Request.UrlReferrer != null)
                {
                    string referrer = HttpContext.Current.Request.UrlReferrer.ToString();
                    string[] list = new string[] { "EAAAALjZH10s+a1+pvZXvvRaEg744AyX92CZJ8DTurSPPO0pnGBpgM01RKYELXRGWkW3Jw==", 
                                                      "EAAAAOKuYPr94rp1oixs8v5ANVPBY1Qtp/z1hsLa963GllE3iZfNJ/9Wpp1ZobwVo/8oPA==", 
                                                      "EAAAAEqaiMo9w7X6BGGwHpGcBQX8PVGNsRnByIflCcLeiBXy",
                                                      "EAAAACDcCibHKYMFcbkIJtv7nX1SOSdWWlzqiBcE0HUPCh3ZP1zT+I9iCPtXnru5ebsfYg==",
                                                      "EAAAAOgNRTFb/B4GtojI9dYla36kFQpAGU6BfDoaDSdCEn4x", 
                                                      "EAAAACRRNRulmmb/Dch0MMA818EgV7JsEAf0H7x9mq5CztnTV+bepVeZ/FN2mGcXYh0qpQ=="
                                                    };
                    bool flag = true;
                    foreach (string item in list)
                    {
                        if (referrer.ToLower().Contains(KPCryptography.DecryptStringAES(item)))
                        {
                            flag = false;
                            break;
                        }
                    }

                    //if (flag)
                    //    throw new Exception(KPGlobalizationLanguageEnum.GetString(KPLanguageKeyEnum.SECURITY_FAILURE));
                }
            }
        }
    }
}
