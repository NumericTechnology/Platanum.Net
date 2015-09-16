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
using System.Text;
using System.Web.UI;
using System.ComponentModel;

namespace KPComponents.Generic
{
    /// <summary>
    /// <para>Authors: Juliano Tiago Rinaldi and 
    /// Tiago Antonio Jacobi</para>
    /// </summary>
    [Serializable]
    public abstract class StateManagedItem : IStateManagedItem
    {
        #region Fields

        private bool trackingViewState = false;
        private StateBag viewState;

        #endregion

        #region Constructor

        public StateManagedItem()
        {
            viewState = new StateBag();
        }

        #endregion

        #region Properties

        public virtual string ID
        {
            get
            {
                object o = ViewState["ID"];
                return o == null ? Guid.NewGuid().ToString() : (string)o;
            }
            set { ViewState["ID"] = value; }
        }

        protected StateBag ViewState
        {
            get { return this.viewState; }
        }

        #endregion

        #region IStateManager Members

        [Browsable(false)]
        public bool IsTrackingViewState
        {
            get { return trackingViewState; }
        }

        public void LoadViewState(object state)
        {
            ((IStateManager)this.ViewState).LoadViewState(state);
        }

        public object SaveViewState()
        {
            return ((IStateManager)this.ViewState).SaveViewState();
        }

        public void TrackViewState()
        {
            this.trackingViewState = true;
            ((IStateManager)this.ViewState).TrackViewState();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Sets the state of the <see cref="System.Web.UI.StateBag">System.Web.UI.StateBag</see> object as well as the <see cref="System.Web.SessionState.ISessionStateItemCollection.Dirty">System.Web.SessionState.ISessionStateItemCollection.Dirty</see> property of each of the <see cref="System.Web.UI.StateItem">System.Web.UI.StateItem</see> objects contained by it.
        /// Mark the state of the collection and its items as modified
        /// </summary>
        public void SetDirty()
        {
            viewState.SetDirty(true);
        }

        #endregion
    }
}