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
using System.Web.UI.WebControls;
using System.Web.UI;
using KPExtension;
using KPGlobalization;

namespace KPComponents.KPAlert
{
    /// <summary>
    /// TODO: It's not finished, also not used.
    /// <para>Authors: Juliano Tiago Rinaldi and 
    /// Tiago Antonio Jacobi</para>
    /// </summary>
    public class KPAlert
    {
        public bool ShowYesButton = true;
        public bool ShowNoButton = true;

        public string AlertTitle;

        private KPAlertTypeEnum Type;
        private string Title;
        private string Message;

        private List<Button> buttons;

        public KPAlert(KPAlertTypeEnum type, string title, string message)
        {
            this.buttons = new List<Button>();
            this.Type = type;
            this.Title = title;
            this.Message = message;

            this.AlertTitle = KPGlobalizationLanguageEnum.GetString(this.Type.GetTranslatableDescription());
        }

        private void AddButton(Button button)
        {
            buttons.Add(button);
        }

        private void GenerateMessage()
        {
            
            //<div class="DecisionAlert MessageInfo" tabindex="-1">
            //    <div class="AlertHeader">
            //        <span class="TitleHeader">Info</span>
            //        <span class="CloseButton HeaderButton"></span>
            //        <span class="HelpButton HeaderButton"></span>
            //    </div>
            //    <div class="LocalHeader">
            //        <span class="LocalTitle">Local Title</span>
            //    </div>
            //    <div class="LabelMessage">Message to the User</div>
            //    <div class="ButtonBar">
            //        <button class="ButtonBarButton">Ok</button>
            //        <button class="ButtonBarButton">Cancel</button>
            //    </div>
            //</div>


            throw new NotImplementedException();
        }

        public void Show() {
            throw new NotImplementedException();
        }
    }
}
