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

using KPComponents.KPSecurity;
using KPComponents.KPSession;
using KPCore.KPSecurity;
using KPEntity.ORM;
using KPEnumerator.KPSecurity;
using NHibernate.Criterion;
using System;
using System.Web.UI;

namespace KPComponents
{
    /// <summary>
    /// Page base KPFramework
    /// <para>Authors: Juliano Tiago Rinaldi and 
    /// Tiago Antonio Jacobi</para>
    /// </summary>
    /// <seealso cref="System.Web.UI.Page"/>
    public abstract class KPPageBase : System.Web.UI.Page
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        protected KPPageBase()
            : base()
        {
            KPFrwSecurity.SecurityFramework();
        }

        /// <summary>
        /// Default Constructor
        /// </summary>
        protected KPPageBase(Enum pageEnum)
            : this()
        {
            PageEnum = pageEnum;
        }

        /// <summary>
        /// Get Enum Page Form Key
        /// </summary>
        public Enum PageEnum
        {
            get
            {
                object o = ViewState["PageEnum"];
                return o == null ? null : (Enum)o;
            }
            private set { ViewState["PageEnum"] = value; }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!IsPostBack)
            {
                var pagePermission = SecuritySession.GetPagePermission(PageEnum);
                if (pagePermission != null && pagePermission.ExistPage)
                {
                    LogHelper.Log(String.Format("Usuário [{0}] abriu a página [{1}]", SecuritySession.Login, pagePermission.PageTitle), SecuritySession.Login, pagePermission.PageId);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "WindowTitle", String.Format("setWindowTitle('{0}');", pagePermission.PageTitle), true);
                }
            }

            if (Request != null && Request.QueryString != null && String.IsNullOrWhiteSpace(ParentSessionPageID))
            {
                ParentSessionPageID = this.Request.QueryString.Get("parentID");
            }
            if (String.IsNullOrWhiteSpace(SessionPageID))
                SessionPageID = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Get object User Security Session
        /// </summary>
        /// <seealso cref="KPCore.KPSecurity.KPSecuritySession"/>
        public KPSecuritySession SecuritySession
        {
            get
            {
                return KPSessionHelper.GetSession<KPSecuritySession>(KPSessionKeyEnum.SESSION_LOGIN);
            }
        }

        public string ParentSessionPageID
        {
            get
            {
                if (ViewState["ParentSessionPageID"] != null)
                {
                    return ViewState["ParentSessionPageID"].ToString();
                }
                return null;
            }
            private set
            {
                ViewState["ParentSessionPageID"] = value;
            }
        }

        public string SessionPageID
        {
            get
            {
                if (ViewState["SessionPageID"] != null)
                {
                    return ViewState["SessionPageID"].ToString();
                }
                return null;
            }
            private set
            {
                ViewState["SessionPageID"] = value;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            try
            {
                base.OnInit(e);
                if (SecuritySession == null)
                {
                    if (User.Identity.IsAuthenticated)
                    {
                        FrwUser loggedUser = FrwUser.FindOne(Expression.Eq("UserLogin", User.Identity.Name));

                        Conjunction conj = new Conjunction();
                        conj.Add(Expression.Eq("IsDefaultCompany", true));
                        conj.Add(Expression.Eq("objIdUser", loggedUser));

                        FrwUserCompany userCompany = FrwUserCompany.FindFirst(conj);
                        string sessionID = null;
                        if (System.Web.HttpContext.Current != null && System.Web.HttpContext.Current.Session != null)
                            sessionID = System.Web.HttpContext.Current.Session.SessionID;

                        KPSessionHelper.SetSession(KPSessionKeyEnum.SESSION_LOGIN,
                                    new KPSecuritySession(sessionID, User.Identity.Name,
                                            loggedUser.objIdCompany.IdCompany, loggedUser.IdUser,
                                            KPFormsAuthentication.UserPagePermissions(loggedUser),
                                            KPFormsAuthentication.UserComponentPermissions(loggedUser)));
                    }
                    else
                        KPFormsAuthentication.Logout();
                }

            }
            catch (Exception)
            {
                throw;
            }

        }

        /// <summary>
        /// Open Popup Window with Javascript
        /// </summary>
        /// <param name="url">Url page</param>
        /// <param name="isModal">If true is open Modal</param>
        internal void KPWindow(string url, bool isModal)
        {
            KPWindow(url, isModal, -1, -1);
        }

        /// <summary>
        /// Open Popup Window with Javascript
        /// </summary>
        /// <param name="url">Url page</param>
        /// <param name="isModal">If true is open Modal</param>
        /// <param name="width">Page With</param>
        /// <param name="height">Page Height</param>
        internal void KPWindow(string url, bool isModal, int width, int height)
        {
            KPWindow("", url, isModal, -1, -1);
        }


        /// <summary>
        /// Open Popup Window with Javascript
        /// </summary>
        /// <param name="title">Page Title</param>
        /// <param name="url">Url page</param>
        /// <param name="isModal">If true is open Modal</param>
        /// <param name="width">Page With</param>
        /// <param name="height">Page Height</param>
        public void KPWindow(string title, string url, bool isModal, int width, int height)
        {
            // Tratamento URL
            if (url.Contains("/"))
            {
                string absolutePath = this.Request.Url.AbsolutePath;
                absolutePath = absolutePath.Substring(0, absolutePath.LastIndexOf("/"));
                if (!url.StartsWith("/"))
                {
                    string urlRelative = this.ResolveUrl(String.Format(@"~/{0}", url));
                    string urlRelativePath = urlRelative.Substring(0, urlRelative.LastIndexOf("/"));

                    if (absolutePath.Equals(urlRelativePath))
                    {
                        url = urlRelative.Substring(urlRelative.LastIndexOf("/") + 1);
                    }
                }
            }

            string strWidthHeight = String.Empty;
            if (width > 0 && height > 0)
                strWidthHeight = String.Format(", {0}, {1}", width, height);

            if (!String.IsNullOrWhiteSpace(url))
            {
                if (!url.Contains("parentID") && !String.IsNullOrWhiteSpace(this.SessionPageID))
                    url = String.Format("{0}?parentID={1}", url, this.SessionPageID);
            }

            string script = String.Format(@"KPWindow('{0}', '{1}', {2}{3});", title, url, isModal.ToString().ToLower(), strWidthHeight);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "KPWindow", script, true);
        }

        /// <summary>
        /// Open Popup Window with Javascript
        /// </summary>
        /// <param name="frwWindow">Object Entity FrwWindow</param>
        /// <param name="isModal">If true is open Modal</param>
        /// <param name="width">Page With</param>
        /// <param name="height">Page Height</param>
        /// <seealso cref="KPEntity.ORM.FrwWindow"/>
        public void KPWindow(FrwWindow frwWindow, bool isModal, int width, int height)
        {
            if (frwWindow != null)
            {
                this.KPWindow(frwWindow.WindowTitle, frwWindow.WindowPath, isModal, width, height);
            }
        }

        /// <summary>
        /// Open Popup Window with Javascript
        /// </summary>
        /// <param name="windowId"></param>
        /// <param name="isModal">If true is open Modal</param>
        /// <param name="width">Page With</param>
        /// <param name="height">Page Height</param>
        public void KPWindow(int windowId, bool isModal, int width, int height)
        {
            if (windowId > 0)
            {
                FrwWindow frwWindow = FrwWindow.Find(windowId);

                this.KPWindow(frwWindow, isModal, width, height);
            }
        }

        /// <summary>
        /// Create box message Javascript
        /// </summary>
        /// <param name="message">Message into box</param>
        public void ShowMessage(string message)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertBoxMessage", String.Concat("alertBox('", message, "');"), true);
        }

        /// <summary>
        /// Set activate Loading Control
        /// </summary>
        /// <param name="activate">If true, Loading control bring to front page. False hide control</param>
        public void SetLoading(bool activate)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "setLoadingKPPageBase", this.GetLoadingScript(activate), true);
        }

        /// <summary>
        /// Get Javascript Loading Control
        /// </summary>
        /// <param name="activate">True activate, False deactivate</param>
        /// <returns>Script Javascript</returns>
        private string GetLoadingScript(bool activate)
        {
            return @"setLoading(" + activate.ToString().ToLower() + @");";
        }

    }
}
