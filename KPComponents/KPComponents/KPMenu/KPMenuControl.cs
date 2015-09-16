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
using KPEnumerator.KPEntity;
using KPEnumerator.KPSecurity;
using NHibernate.Criterion;
using System;
using System.Linq;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace KPComponents
{
    /// <summary>
    /// <para>Authors: Juliano Tiago Rinaldi and 
    /// Tiago Antonio Jacobi</para>
    /// </summary>
    [ToolboxData(@"<{0}:KPMenuControl runat=server ></{0}:KPMenuControl>")]
    public class KPMenuControl : WebControl
    {
        public string Title { get; set; }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (Page != null)
                EnsureChildControls();
        }

        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            #region MenuControl
            HtmlGenericControl menuControl = new HtmlGenericControl("div");
            menuControl.Attributes.Add("class", "Menu");

            KPSecuritySession session = (KPSecuritySession)KPSessionHelper.GetSession(KPSessionKeyEnum.SESSION_LOGIN);
            if (session == null)
            {
                FormsAuthentication.SignOut();
                FormsAuthentication.RedirectToLoginPage();
                return;
            }

            FrwUser loggedUser = FrwUser.FindOne(Expression.Eq("UserLogin", session.Login));

            // Add Main Menu Bar
            menuControl.Controls.Add(this.MainMenuBar(loggedUser));

            // Add user Action Bar
            menuControl.Controls.Add(this.UserActionBar(session));

            // Add 
            //menuControl.Controls.Add(menuForm);


            #endregion MenuControl
            this.Controls.Add(menuControl);

            #region Script code
            // TODO: WindowFinderInput Foi comentado

            /*
            string userCrypt = KPCryptography.EncryptStringAES(session.Login);
            string wcfAjaxJason = System.Web.VirtualPathUtility.ToAbsolute("~/WCF/WCFAjaxJson.svc/GetWindowFinder");
            string scriptWindowFinder = @"
                    $(function () {
                            $(""#WindowFinderInput"").keypress(function (e) {
                                if ((e.which && e.which == 13) || (e.keyCode && e.keyCode == 13)) {
                                    var params = new Object();
                                    params.windowID = $(""#WindowFinderInput"").val();
                                    params.user = '" + userCrypt + @"';

                                    $.ajax({
                                            url: '" + wcfAjaxJason + @"',
                                            data: JSON.stringify(params),
                                            dataType: 'json',
                                            type: 'POST',
                                            contentType: 'application/json; charset=utf-8'
                                        })
                                        .done(function (data) {
                                            if (!isEmptyOrNull(data.d)) {
                                                eval(data.d);
                                            }
                                            $(""#WindowFinderInput"").val('');
                                        })
                                        .fail(function (jqXHR, textStatus, error) { 
                                            alert(error);
                                        });
                                    return false;
                                }
                                return true;
                            });
                        });";

            this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "WindowFinderScript", scriptWindowFinder, true);
            */

            #endregion Script code
        }

        public HtmlGenericControl MainMenuBar(FrwUser loggedUser)
        {
            HtmlGenericControl mainMenuBar = new HtmlGenericControl("ul");
            mainMenuBar.Attributes.Add("class", "MainMenuBar");

            #region UL MainMenuBar

            ICriterion query = Expression.And(Expression.Eq("IsVisible", StateVisibleEnum.Yes), Expression.IsNull("objIdParent"));
            FrwMenu[] frwMenuRootList = FrwMenu.FindAll(Order.Asc("Sequence"), query);

            #region Menu Item

            HtmlGenericControl menuItemRoot = this.CreateMenuItem(Title);
            HtmlGenericControl menuItemsBox = this.CreateMenuItemItems(frwMenuRootList);

            #endregion Menu Item
            mainMenuBar.Controls.Add(menuItemRoot);

            #region Menu Item WindowFinder

            // TODO: WindowFinderInput foi comentado.
            // HtmlGenericControl menuItemWindowFinder = this.CreateMenuItemWindowFinder();

            #endregion Menu Item WindowFinder
            // mainMenuBar.Controls.Add(menuItemWindowFinder);

            #region Items Sub Menus
            foreach (FrwMenu item in frwMenuRootList)
            {
                HtmlGenericControl subMenu = this.CreateMenuItemItemSubMenu(item);

                if (subMenu != null)
                {
                    menuItemsBox.Controls.Add(this.CreateMenuItemItem(item));
                    mainMenuBar.Controls.Add(subMenu);
                }
            }

            menuItemRoot.Controls.Add(menuItemsBox);
            #endregion Items Sub Menus

            #endregion UL MainMenuBar

            return mainMenuBar;
        }

        /// <summary>
        /// This method creates the user action bar
        /// </summary>
        /// <param name="userSecSess"></param>
        /// <returns></returns>
        private HtmlGenericControl UserActionBar(KPSecuritySession userSecSess)
        {
            FrwUser loggedUser = FrwUser.FindOne(Expression.Eq("UserLogin", userSecSess.Login));

            //StringBuilder sbMenu = new StringBuilder();
            HtmlGenericControl menuUserAction = new HtmlGenericControl("div");
            menuUserAction.Attributes.Add("class", "MenuUserAction");

            #region DIV MenuUserAction

            #region DIV User Info
            HtmlGenericControl userInfo = new HtmlGenericControl("div");
            userInfo.Attributes.Add("class", "UserInfo");

            HtmlGenericControl userName = new HtmlGenericControl("span");
            userName.Attributes.Add("class", "UserName");
            userName.Attributes.Add("title", "Nome do Usuário");
            userName.InnerText = loggedUser.UserFullName;

            userInfo.Controls.Add(userName);
            #endregion DIV User Info
            menuUserAction.Controls.Add(userInfo);

            #region DIV Company
            FrwUserCompany[] listUserCompany = FrwUserCompany.FindAllByProperty("objIdUser", loggedUser);

            DropDownList userCompanyControl = new DropDownList() { AutoPostBack = true };
            userCompanyControl.SelectedIndexChanged += new EventHandler(userCompanyControl_SelectedIndexChanged);
            userCompanyControl.Attributes.Add("class", "UserCompany");
            userCompanyControl.Attributes.Add("title", "Trocar Empresa");

            foreach (FrwUserCompany userCompany in listUserCompany)
            {
                ListItem item = new ListItem();
                item.Value = userCompany.objIdCompany.IdCompany.ToString();
                item.Text = userCompany.objIdCompany.CompanyFantasyName;

                if (userSecSess.FrwCompany == userCompany.objIdCompany.IdCompany)
                {
                    item.Selected = true;
                }

                userCompanyControl.Items.Add(item);
            }

            #endregion DIV Company

            //HtmlForm companyForm = new HtmlForm();
            //companyForm.Controls.Add(userCompanyControl);
            menuUserAction.Controls.Add(userCompanyControl);

            #region Actions

            //HtmlGenericControl btSettings = new HtmlGenericControl("button");
            //btSettings.Attributes.Add("class", "Action Settings");

            HtmlGenericControl btLogout = new HtmlGenericControl("button");
            btLogout.Attributes.Add("class", "Action Logout");
            btLogout.Attributes.Add("title", "Sair do Sistema");
            btLogout.Attributes.Add("onclick", "window.location.replace('Logout.aspx'); return false;");

            #endregion Actions
            // menuUserAction.Controls.Add(btSettings);
            menuUserAction.Controls.Add(btLogout);

            #endregion DIV MenuUserAction

            return menuUserAction;
        }

        private void userCompanyControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            KPSecuritySession userSecSess = (KPSecuritySession)KPSessionHelper.GetSession(KPSessionKeyEnum.SESSION_LOGIN);
            int companyID = userSecSess.FrwCompany;
            userSecSess.FrwCompany = Convert.ToInt32(((DropDownList)sender).SelectedValue);
            LogHelper.Log(String.Format("Usuário [{0}] trocou de empresa de ID [{1}] para ID [{2}]", userSecSess.Login, companyID, userSecSess.FrwCompany));
            KPSessionHelper.SetSession(KPSessionKeyEnum.SESSION_LOGIN, userSecSess);

            //ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "RefrechAll", "parent.__doPostBack('','');", true);

        }

        /// <summary>
        /// This method creates a new Menu Item
        /// </summary>
        /// <param name="menuItemTitle"></param>
        /// <returns></returns>
        private HtmlGenericControl CreateMenuItem(string menuItemTitle)
        {
            HtmlGenericControl menuItem = new HtmlGenericControl("li");
            menuItem.Attributes.Add("class", "MenuItem");

            HtmlGenericControl menuTitle = new HtmlGenericControl("a");
            menuTitle.Attributes.Add("class", "MenuItemTitle");
            menuTitle.InnerText = (String.IsNullOrEmpty(menuItemTitle) ? "" : menuItemTitle);
            //sbMenu.Append(@"<img src=""/Assets/Imgs/Themes/Default/Pages/Menu/dropdown.png""></img>");
            // <img src="https://www.ghostery.com/images/show-menu-icon.png" class="MenuTitleImage">

            HtmlGenericControl menuTitleImg = new HtmlGenericControl("img");
            menuTitleImg.Attributes.Add("class", "MenuTitleImage");
            menuTitleImg.Attributes.Add("src", "/Assets/Imgs/Themes/Default/menu_icon.png");

            menuTitle.Controls.Add(menuTitleImg);

            menuItem.Controls.Add(menuTitle);

            return menuItem;
        }

        /// <summary>
        /// Create the items to the menu Item
        /// </summary>
        /// <param name="frwMenuItemList"></param>
        /// <returns>An HtmlGenericControl with items to be add to MenuItem</returns>
        private HtmlGenericControl CreateMenuItemItems(FrwMenu[] frwMenuItemList)
        {
            HtmlGenericControl menuItem = new HtmlGenericControl("ul");
            menuItem.Attributes.Add("class", "DropdownMenu DropdownMenuHidden");
            /*
            foreach (FrwMenu item in frwMenuItemList)
            {
                HtmlGenericControl idMenuItem = new HtmlGenericControl("li");
                idMenuItem.Attributes.Add("data-submenu-id", ("MenuItem" + item.IdMenu));

                HtmlGenericControl idMenuTitle = new HtmlGenericControl("a");
                idMenuTitle.InnerText = item.WindowTitleMenu;

                idMenuItem.Controls.Add(idMenuTitle);

                menuItem.Controls.Add(idMenuItem);
            }
            */
            return menuItem;
        }

        private HtmlGenericControl CreateMenuItemItem(FrwMenu item)
        {
            HtmlGenericControl idMenuItem = new HtmlGenericControl("li");
            idMenuItem.Attributes.Add("data-submenu-id", ("MenuItem" + item.IdMenu));

            HtmlGenericControl idMenuTitle = new HtmlGenericControl("a");
            idMenuTitle.InnerText = item.WindowTitleMenu;

            idMenuItem.Controls.Add(idMenuTitle);

            return idMenuItem;
        }


        /// <summary>
        /// This method creates the MenuItem related to FindWindows in the system.
        /// </summary>
        /// <returns></returns>
        private HtmlGenericControl CreateMenuItemWindowFinder()
        {
            HtmlGenericControl menuItemWindowFinder = new HtmlGenericControl("li");
            menuItemWindowFinder.Attributes.Add("class", "MenuItem");

            HtmlForm menuForm = new HtmlForm();
            menuForm.Attributes.Add("id", "WindowFinderForm");

            HtmlGenericControl menuInput = new HtmlGenericControl("input");
            menuInput.Attributes.Add("id", "WindowFinderInput");
            menuInput.Attributes.Add("class", "WindowFinder");

            menuForm.Controls.Add(menuInput);

            menuItemWindowFinder.Controls.Add(menuInput);

            return menuItemWindowFinder;
        }

        private HtmlGenericControl CreateMenuItemItemSubMenu(FrwMenu subMenuItem)
        {
            bool hasChildren = false;

            KPSecuritySession securitySession = KPFormsAuthentication.SecuritySession;
            string strOnClickMenu = @"addNewTab('KPTabControl', '{0}', '{1}');";

            HtmlGenericControl subMenuItemControl = new HtmlGenericControl("div");
            subMenuItemControl.Attributes.Add("id", ("MenuItem" + subMenuItem.IdMenu));
            subMenuItemControl.Attributes.Add("class", "PopOverMenu");

            #region Title

            HtmlGenericControl subMenuItemTitle = new HtmlGenericControl("h3");
            subMenuItemTitle.Attributes.Add("class", "PopOverMenuTitle");
            subMenuItemTitle.InnerText = subMenuItem.WindowTitleMenu;

            #endregion
            subMenuItemControl.Controls.Add(subMenuItemTitle);

            HtmlGenericControl menuContent = new HtmlGenericControl("div");
            menuContent.Attributes.Add("class", "PopOverMenuContent");

            #region Menu Content Items

            HtmlGenericControl menuContentItems = new HtmlGenericControl("ul");
            menuContentItems.Attributes.Add("class", "PopOverMenuContentItems");

            PagePermission securityPage = null;
            bool itemNivel2Create = false;
            bool isOnlyMenuItem = false;
            FrwMenu parentNivel1Order = FrwMenu.TryFind(subMenuItem.IdMenu);
            FrwMenu[] frwMenuNivel1List = FrwMenu.FindAll(Order.Asc("Sequence"), Expression.And(Expression.Eq("IsVisible", StateVisibleEnum.Yes), Expression.Eq("objIdParent", parentNivel1Order)));
            foreach (FrwMenu itemNivel1 in frwMenuNivel1List)
            {

                isOnlyMenuItem = itemNivel1.objIdWindow.WindowEnum.Equals("_UNKNOW_");
                securityPage = securitySession.GetPagePermission(itemNivel1.objIdWindow.IdWindow);
                if (isOnlyMenuItem || (securityPage != null && securityPage.ExistPage && securityPage.IsPreview))
                {
                    HtmlGenericControl subMenuItemLi = this.CreateItemLinkToOpeMenu(strOnClickMenu, itemNivel1);

                    FrwMenu parentNivel2Order = FrwMenu.TryFind(itemNivel1.IdMenu);
                    FrwMenu[] frwMenuNivel2List = FrwMenu.FindAll(Order.Asc("Sequence"), Expression.And(Expression.Eq("IsVisible", StateVisibleEnum.Yes), Expression.Eq("objIdParent", parentNivel2Order)));
                    if (frwMenuNivel2List.Count() > 0)
                    {
                        HtmlGenericControl subSubMenuItem = new HtmlGenericControl("ul");
                        subSubMenuItem.Attributes.Add("class", "SubSubMenuItem");

                        itemNivel2Create = false;
                        foreach (FrwMenu itemNivel2 in frwMenuNivel2List)
                        {
                            if (itemNivel2 != null && itemNivel2.objIdWindow != null)
                            {
                                securityPage = securitySession.GetPagePermission(itemNivel2.objIdWindow.IdWindow);
                                if (securityPage != null && securityPage.ExistPage && securityPage.IsPreview)
                                {
                                    itemNivel2Create = true;
                                    HtmlGenericControl subSubMenuItemLi = this.CreateItemLinkToOpeMenu(strOnClickMenu, itemNivel2);
                                    subSubMenuItem.Controls.Add(subSubMenuItemLi);
                                }
                            }
                        }

                        if (itemNivel2Create || !isOnlyMenuItem)
                        {
                            hasChildren = true;
                            subMenuItemLi.Controls.Add(subSubMenuItem);
                        }
                    }

                    menuContentItems.Controls.Add(subMenuItemLi);
                }
            }

            #endregion Menu Content Items
            menuContent.Controls.Add(menuContentItems);

            #region Menu content image

            HtmlGenericControl menuContentImage = new HtmlGenericControl("img");
            menuContentImage.Attributes.Add("alt", subMenuItem.WindowTitleMenu);
            menuContentImage.Attributes.Add("src", subMenuItem.ImageUrl);

            #endregion Menu content image
            menuContent.Controls.Add(menuContentImage);

            subMenuItemControl.Controls.Add(menuContent);

            if (!hasChildren)
            {
                subMenuItemControl = null;
            }


            return subMenuItemControl;
        }

        private HtmlGenericControl CreateItemLinkToOpeMenu(string strOnClickMenu, FrwMenu item)
        {

            string onClickAction = "";
            if (!String.IsNullOrEmpty(item.objIdWindow.WindowPath) && !item.objIdWindow.WindowPath.Equals("#"))
            {
                onClickAction = String.Format(strOnClickMenu, item.objIdWindow.WindowTitle, item.objIdWindow.WindowPath);
            }

            HtmlGenericControl subMenuItemLi = new HtmlGenericControl("li");

            HtmlGenericControl subMenuItemLink = new HtmlGenericControl("a");
            subMenuItemLink.InnerText = item.WindowTitleMenu;
            if (!String.IsNullOrEmpty(onClickAction))
            {
                subMenuItemLink.Attributes.Add("onclick", onClickAction);
            }

            subMenuItemLi.Controls.Add(subMenuItemLink);

            return subMenuItemLi;
        }

    }
}
