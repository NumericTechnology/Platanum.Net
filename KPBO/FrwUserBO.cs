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
using KPEntity.ORM;
using KPCore.KPSecurity;
using KPAttributes;
using KPBO.Validator;
using NHibernate.Validator.Engine;
using NHibernate.Criterion;
using KPEnumerator.KPSecurity;
using KPCore.KPUtil;
using KPGlobalization;

namespace KPBO
{
    /// <summary>
    /// <para>Authors: Juliano Tiago Rinaldi and 
    /// Tiago Antonio Jacobi</para>
    /// </summary>
    public class FrwUserBO : BaseBO<FrwUser>
    {
        public const string DEFAULT_PSWD = "@#$%¨&*5ha";
        private bool PasswordChanged = true;
        public FrwUserBO(FrwUser frwUser)
            : base(frwUser)
        {
        }

        public override FrwUser SaveEntity()
        {
            if (this.PasswordChanged)
            {
                EntityField.UserPassword = ComputeHash(String.Concat(EntityField.UserLogin.ToLower(), EntityField.UserPassword));
            }
            EntityField.Save();
            return EntityField;
        }

        private string ComputeHash(string password)
        {
            return KPCryptography.ComputeHash(password, KPAlgorithmEnum.MD5, KPGenericUtil.GetDefaultEncoding().GetBytes("KPFramework"));
        }

        public override bool Validate()
        {
            PasswordChanged = true;
            if (this.IsUpdate)
            {
                this.ValidateOnUpdate();
            }
            else
            {
                if (!this.ValidateOnCreate())
                {
                    return false;
                }
            }

            if (EntityField.UserLogin.Contains(" "))
            {
                InvalidValues.Add(new InvalidValue(KPGlobalizationLanguage.GetString("Message_ValidateLoginEmpty"), EntityField.GetType(), "UserLogin", null, EntityField, null));
            }

            if (InvalidEntityHeader.Count > 0 || InvalidValues.Count > 0)
            {
                return false;
            }

            return true;
        }

        public bool ValidateOnCreate()
        {
            FrwUser frwUser = FrwUser.FindOne(Expression.Eq("UserLogin", EntityField.UserLogin));
            if (frwUser != null)
            {
                InvalidValues.Add(new InvalidValue(KPGlobalizationLanguage.GetString("Message_ValidateExistLogin"), EntityField.GetType(), "UserLogin", null, EntityField, null));
                return false;
            }

            return true;
        }

        public bool ValidateOnUpdate()
        {
            FrwUser frwUser = FrwUser.Find(EntityField.IdUser);
            if (frwUser.UserPassword.Equals(EntityField.UserPassword))
                this.PasswordChanged = false;
            else if (String.IsNullOrWhiteSpace(EntityField.UserPassword) || EntityField.UserPassword.Equals(DEFAULT_PSWD))
            {
                this.PasswordChanged = false;
                EntityField.UserPassword = frwUser.UserPassword;
            }

            return true;
        }
    }
}
