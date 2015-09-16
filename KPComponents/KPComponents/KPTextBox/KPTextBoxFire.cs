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
using System.Web.UI;
using System.Drawing;

namespace KPComponents
{
    /// <summary>
    /// <para>Authors: Juliano Tiago Rinaldi and 
    /// Tiago Antonio Jacobi</para>
    /// </summary>
    [ToolboxBitmap(typeof(KPTextBoxFire), "KPComponents.Asset.Img.KPComponent.bmp")]
    [ToolboxData("<{0}:KPTextBoxFire runat=server />")]
    public class KPTextBoxFire : System.Web.UI.WebControls.TextBox
    {
        public KPTextBoxFire()
        {
        }

        protected override void CreateChildControls()
        {
            RegisterScripts();
            SetAttributes();
            base.CreateChildControls();
        }

        private void RegisterScripts()
        {
            StringBuilder stringScript = new StringBuilder();
            stringScript.Append(@"var delay = (function(){
                                  var timer = 0;
                                  return function(callback, ms){
                                    clearTimeout (timer);
                                    timer = setTimeout(callback, ms);
                                  };})();");

            stringScript.AppendFormat(@"function runPostback() 
                                        {{
                                            __doPostBack('{0}', 'TextChanged'); 
                                        }}", this.ClientID);

            if (!Page.ClientScript.IsClientScriptBlockRegistered("ScriptKPTextBox"))
                Page.ClientScript.RegisterClientScriptBlock(typeof(KPTextBoxFire), "ScriptKPTextBox", stringScript.ToString(), true);
        }

        private void SetAttributes()
        {
            this.Attributes.Add("onkeyup", "delay(function(){ runPostback(); }, 1000)");
        }
    }
}
