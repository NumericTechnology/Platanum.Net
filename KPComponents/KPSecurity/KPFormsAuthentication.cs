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

using KPBO;
using KPComponents.KPSession;
using KPCore.KPSecurity;
using KPEntity.ORM;
using KPEnumerator.KPGlobalization;
using KPEnumerator.KPSecurity;
using KPGlobalization;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Authentication;
using System.Web;
using System.Web.Configuration;
using System.Web.Security;
using System.Web.UI;

namespace KPComponents.KPSecurity
{
    /// <summary>
    /// Manages KPFramework forms-authentication services
    /// <para>Authors: Juliano Tiago Rinaldi and 
    /// Tiago Antonio Jacobi</para>
    /// </summary>
    public static class KPFormsAuthentication
    {
        /// <summary>
        /// Credential Authentication and redirect page for main application
        /// </summary>
        /// <param name="page">Object Page</param>
        /// <param name="login">User login</param>
        /// <param name="password">User password</param>
        /// <param name="isPersistent"> true if the ticket will be stored in a persistent cookie (saved across browser sessions); 
        /// otherwise, false. If the ticket is stored in the URL, this value is ignored</param>
        /// <exception cref="System.Security.Authentication.AuthenticationException">Thrown when incorrect User or Password</exception>
        public static void AuthenticateAndRedirect(Page page, string login, string password, bool isPersistent)
        {
            try
            {
                Authenticate authenticate = Authenticate(login, password);
                if (authenticate != null && authenticate.IsAuthenticate)
                {
                    LogHelper.Log(String.Format("Login reconhecido: {0}", login), login);

                    int timeOut = 30;

                    try
                    {
                        SessionStateSection sessionStateSection = (SessionStateSection)ConfigurationManager.GetSection("system.web/sessionState");
                        if (sessionStateSection != null)
                            timeOut = sessionStateSection.Timeout.Minutes;
                    }
                    catch
                    {
                        throw;
                    }

                    FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1,
                            login, DateTime.Now, DateTime.Now.AddMinutes(timeOut),
                            isPersistent, String.Empty, FormsAuthentication.FormsCookiePath);

                    string encTicket = FormsAuthentication.Encrypt(ticket);
                    page.Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));

                    string sessionID = null;
                    if (System.Web.HttpContext.Current != null && System.Web.HttpContext.Current.Session != null)
                        sessionID = System.Web.HttpContext.Current.Session.SessionID;
                    KPSessionHelper.SetSession(KPSessionKeyEnum.SESSION_LOGIN,
                        new KPSecuritySession(sessionID, login, authenticate.DefaultCompany.IdCompany,
                            authenticate.User.IdUser, KPFormsAuthentication.UserPagePermissions(authenticate.User),
                             KPFormsAuthentication.UserComponentPermissions(authenticate.User)));

                    LogHelper.Log("Permissão de acesso ao sistema");

                    KPFormsAuthentication.RedirectMain(page, login);
                }
                else
                {
                    LogHelper.Log(String.Format("Falha na autenticação, tentativa de login: {0}, senha: {1}", login, password), login);
                    throw new AuthenticationException(KPGlobalizationLanguageEnum.GetString(KPLanguageKeyEnum.INCORRECT_USER_OR_PASSWORD));
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        internal static PagePermission[] UserPagePermissions(FrwUser frwUser)
        {
            List<PagePermission> pagePermissionList = new List<PagePermission>();

            Conjunction conjUserProfile = new Conjunction();
            conjUserProfile.Add(Expression.Eq("objIdCompany", frwUser.objIdCompany));
            conjUserProfile.Add(Expression.Eq("objIdUser", frwUser));
            FrwUserProfile[] entityUserProfiles = FrwUserProfile.FindAll(conjUserProfile);
            if (entityUserProfiles != null)
            {
                foreach (FrwProfile objIdProfile in entityUserProfiles.Select(x => x.objIdProfile))
                {
                    Conjunction conjProfileFilter = new Conjunction();
                    conjProfileFilter.Add(Expression.Eq("objIdCompany", frwUser.objIdCompany));
                    conjProfileFilter.Add(Expression.Eq("objIdProfile", objIdProfile));
                    FrwProfileWindow[] entityProfileWindows = FrwProfileWindow.FindAll(conjProfileFilter);
                    if (entityProfileWindows != null)
                    {
                        foreach (FrwProfileWindow objIdProfileWindow in entityProfileWindows)
                        {
                            PagePermission pagePermission = pagePermissionList.FirstOrDefault(x => x.PageId == objIdProfileWindow.objIdWindow.IdWindow);
                            if (pagePermission != null)
                            {
                                // Validação de permissão pela negativa, caso algum perfil tiver, poderá bloquear o acesso.
                                pagePermission.IsReadOnly = objIdProfileWindow.IsReadOnly.Value ? pagePermission.IsReadOnly : objIdProfileWindow.IsReadOnly.Value;
                                pagePermission.IsPreview = objIdProfileWindow.IsPreview.Value ? pagePermission.IsPreview : objIdProfileWindow.IsPreview.Value;
                            }
                            else
                            {
                                pagePermissionList.Add(new PagePermission(objIdProfileWindow.objIdWindow.IdWindow, 
                                                                        objIdProfileWindow.objIdWindow.WindowEnum,
                                                                        objIdProfileWindow.objIdWindow.WindowTitle)
                                {
                                    IsReadOnly = objIdProfileWindow.IsReadOnly.Value,
                                    IsPreview = objIdProfileWindow.IsPreview.Value
                                });
                            }
                        }
                    }
                }
            }

            return pagePermissionList.ToArray();
        }

        internal static ComponentPermission[] UserComponentPermissions(FrwUser frwUser)
        {
            List<ComponentPermission> componentPermissionList = new List<ComponentPermission>();

            Conjunction conjUserProfile = new Conjunction();
            conjUserProfile.Add(Expression.Eq("objIdCompany", frwUser.objIdCompany));
            conjUserProfile.Add(Expression.Eq("objIdUser", frwUser));
            FrwUserProfile[] entityUserProfiles = FrwUserProfile.FindAll(conjUserProfile);
            if (entityUserProfiles != null)
            {
                foreach (FrwProfile objIdProfile in entityUserProfiles.Select(x => x.objIdProfile))
                {
                    Conjunction conjProfileFilter = new Conjunction();
                    conjProfileFilter.Add(Expression.Eq("objIdCompany", frwUser.objIdCompany));
                    conjProfileFilter.Add(Expression.Eq("objIdProfile", objIdProfile));

                    FrwProfileComponent[] entityProfileComponents = FrwProfileComponent.FindAll(conjProfileFilter);
                    foreach (FrwProfileComponent objIdProfileComponent in entityProfileComponents)
                    {
                        ComponentPermission componentPermission = componentPermissionList.FirstOrDefault(x => x.ComponentId == objIdProfileComponent.objIdComponent.IdComponent);
                        if (componentPermission != null)
                        {
                            // Validação de permissão pela negativa.
                            componentPermission.IsEnabled = objIdProfileComponent.IsEnable.Value ? componentPermission.IsEnabled : objIdProfileComponent.IsEnable.Value;
                            componentPermission.IsVisible = objIdProfileComponent.IsVisible.Value ? componentPermission.IsVisible : objIdProfileComponent.IsVisible.Value;
                        }
                        else
                        {
                            componentPermissionList.Add(new ComponentPermission(objIdProfileComponent.objIdComponent.objIdWindow.IdWindow,
                                                                                objIdProfileComponent.objIdComponent.objIdWindow.WindowEnum,
                                                                                objIdProfileComponent.objIdComponent.IdComponent,
                                                                                objIdProfileComponent.objIdComponent.ComponentNameId)
                            {
                                IsEnabled = objIdProfileComponent.IsEnable.Value,
                                IsVisible = objIdProfileComponent.IsVisible.Value
                            });
                        }
                    }
                }
            }

            return componentPermissionList.ToArray();
        }

        /// <summary>
        /// Redirect Page for the main application.
        /// </summary>
        /// <param name="page">Object Page</param>
        /// <param name="login">User login</param>
        private static void RedirectMain(Page page, string login)
        {
            try
            {
                page.Response.Redirect(FormsAuthentication.GetRedirectUrl(login, true));
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Authentication User and Password
        /// </summary>
        /// <param name="login">User login</param>
        /// <param name="password">User password</param>
        /// <returns>return true when success or false when user/login incorrect</returns>
        private static Authenticate Authenticate(string login, string password)
        {
            Authenticate authenticate = null;
            try
            {
                Conjunction conj = new Conjunction();
                conj.Add(Expression.Eq("UserLogin", login).IgnoreCase());
                conj.Add(Expression.Eq("IsAccessAllowed", true));
                FrwUser frwUser = FrwUser.FindOne(conj);

                if (frwUser == null)
                    return authenticate;

                if (KPCryptography.VerifyHash(String.Concat(login.ToLower(), password), KPAlgorithmEnum.MD5, frwUser.UserPassword))
                {
                    conj = new Conjunction();
                    conj.Add(Expression.Eq("objIdUser", frwUser));
                    FrwUserCompany[] userCompany = FrwUserCompany.FindAll(conj);
                    if (userCompany != null && userCompany.Length > 0)
                    {
                        FrwCompany[] companies = userCompany.Select(x => x.objIdCompany).ToArray();
                        FrwCompany companyDefault = companies[0];
                        FrwUserCompany[] userCompanyDefault = userCompany.Where(x => x.IsDefaultCompany == true).ToArray();
                        if (userCompanyDefault != null && userCompanyDefault.Length > 0)
                            companyDefault  = userCompanyDefault[0].objIdCompany;

                        authenticate = new Authenticate(frwUser, true, companies, companyDefault);
                    }
                }
            }
            catch 
            {
                throw;
            }

            return authenticate;
        }

        /// <summary>
        /// This method Logout the User from the application and open the Login Page.
        /// </summary>
        /// <seealso cref="Logout(bool)"/>
        public static void Logout()
        {
            KPFormsAuthentication.Logout(true);
        }

        /// <summary>
        /// This method is responsible for Logout the User in the application.
        /// </summary>
        /// <param name="redirectToLoginPage">redirect the user to the Login Page if true</param>
        public static void Logout(bool redirectToLoginPage)
        {
            FormsAuthentication.SignOut();
            if (System.Web.HttpContext.Current != null && System.Web.HttpContext.Current.Session != null)
            {
                System.Web.HttpContext.Current.Session.RemoveAll();
                System.Web.HttpContext.Current.Session.Abandon();
            }

            if (redirectToLoginPage)
            {
                FormsAuthentication.RedirectToLoginPage();
            }
        }

        /// <summary>
        /// Get the Security session data, related to the logged user.
        /// </summary>
        /// <seealso cref="KPCore.KPSecurity.KPSecuritySession"/>
        public static KPSecuritySession SecuritySession
        {
            get
            {
                return KPSessionHelper.GetSession<KPSecuritySession>(KPSessionKeyEnum.SESSION_LOGIN);
            }
        }
    }
}
