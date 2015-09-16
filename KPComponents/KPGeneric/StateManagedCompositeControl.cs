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
using System.Web.UI.WebControls;
using System.Web.UI;

namespace KPComponents.Generic
{
    /// <summary>
    /// <para>Authors: Juliano Tiago Rinaldi and 
    /// Tiago Antonio Jacobi</para>
    /// </summary>
    public class StateManagedCompositeControl : CompositeControl, IStateManagedItem
    {
        #region Fields

        private bool trackingViewState = false;

        #endregion

        #region IStateManagedItem Members

        public void SetDirty()
        {
            ViewState.SetDirty(true);
        }

        #endregion

        #region IStateManager Members

        public new bool IsTrackingViewState
        {
            get { return trackingViewState; }
        }

        public new void LoadViewState(object state)
        {
            ((IStateManager)this.ViewState).LoadViewState(state);
        }

        public new object SaveViewState()
        {
            return ((IStateManager)this.ViewState).SaveViewState();
        }

        public new void TrackViewState()
        {
            this.trackingViewState = true;
            ((IStateManager)this.ViewState).TrackViewState();
        }

        #endregion

        #region IStateManager Members

        bool IStateManager.IsTrackingViewState
        {
            get { return this.IsTrackingViewState; }
        }

        void IStateManager.LoadViewState(object state)
        {
            this.LoadViewState(state);
        }

        object IStateManager.SaveViewState()
        {
            return this.SaveViewState();
        }

        void IStateManager.TrackViewState()
        {
            this.TrackViewState();
        }

        #endregion
    }
}