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
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace KPCore.KPSecurity
{
    /// <summary>
    /// <para>Authors: Juliano Tiago Rinaldi and 
    /// Tiago Antonio Jacobi</para>
    /// </summary>
    [Serializable]
    public class KPSecuritySession
    {
        public KPSecuritySession(string sessionID, string login, int frwCompany, int idUser,
                                PagePermission[] pagePermissions, ComponentPermission[] componentPermissions)
        {
            FrwCompany = frwCompany;
            IdUser = idUser;
            SessionID = sessionID;
            Login = login;
            PagePermissions = pagePermissions;
            ComponentPermissions = componentPermissions;
        }

        public string SessionID { get; private set; }
        public string Login { get; private set; }
        public int FrwCompany { get; set; }

        public int IdUser { get; private set; }

        [EditorBrowsable(EditorBrowsableState.Never)]
        internal PagePermission[] PagePermissions { get; private set; }

        [EditorBrowsable(EditorBrowsableState.Never)]
        internal ComponentPermission[] ComponentPermissions { get; private set; }

        public PagePermission GetPagePermission(Enum pageEnum)
        {

            if (pageEnum == null)
                return null;

            PagePermission permission = null;
            if (PagePermissions != null)
            {
                permission = PagePermissions.FirstOrDefault(x => x.PageEnum.Equals(pageEnum.ToString(), StringComparison.InvariantCultureIgnoreCase));

                if (permission != null)
                    return permission;
            }

            permission = new PagePermission(0, pageEnum.ToString(), null)
            {
                IsPreview = true,
                IsReadOnly = false
            };

            return permission;
        }

        public PagePermission GetPagePermission(int pageId)
        {
            PagePermission permission = null;
            if (PagePermissions != null)
            {
                permission = PagePermissions.FirstOrDefault(x => x.PageId.Equals(pageId));

                if (permission != null)
                    return permission;
            }

            return permission;
        }

        public ComponentPermission GetComponentPermission(Enum pageEnum, string componentNameId)
        {
            ComponentPermission permission = null;
            if (ComponentPermissions != null)
            {
                permission = ComponentPermissions.FirstOrDefault(x => x.PageEnum.Equals(pageEnum.ToString(), StringComparison.InvariantCultureIgnoreCase)
                                                                   && x.ComponentNameId.Equals(componentNameId, StringComparison.InvariantCultureIgnoreCase));

                if (permission != null)
                    return permission;
            }

            permission = new ComponentPermission(0, pageEnum.ToString(), 0, componentNameId)
            {
                IsEnabled = true,
                IsVisible = true
            };

            return permission;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public ComponentPermission[] GetPageComponentsPermission(Enum pageEnum)
        {
            if (ComponentPermissions != null)
            {
                var permissions = ComponentPermissions.Where(x => x.PageEnum.Equals(pageEnum.ToString(), StringComparison.InvariantCultureIgnoreCase));

                if (permissions != null)
                    return permissions.ToArray();
            }

            return new ComponentPermission[0];
        }

        #region ValidatePermissions
        public bool IsReadOnly(Enum pageEnum)
        {
            var permission = GetPagePermission(pageEnum);
            if (permission != null)
            {
                return permission.IsReadOnly;
            }

            return false;
        }

        public bool IsPreviewAllowed(Enum pageEnum)
        {
            var permission = GetPagePermission(pageEnum);
            if (permission != null)
            {
                return permission.IsPreview;
            }

            return false;
        }

        public bool IsEnabled(Enum pageEnum, string componentNameId)
        {
            var permission = GetComponentPermission(pageEnum, componentNameId);
            if (permission != null)
            {
                return permission.IsEnabled;
            }

            return true;
        }

        public bool IsVisible(Enum pageEnum, string componentNameId)
        {
            var permission = GetComponentPermission(pageEnum, componentNameId);
            if (permission != null)
            {
                return permission.IsVisible;
            }

            return true;
        }
        #endregion


    }
}
