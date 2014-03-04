﻿/*
 * @author Valentin Simonov / http://va.lent.in/
 */

using System.Collections.Generic;
using TouchScript.Utils.Editor.Attributes;
using UnityEngine;

namespace TouchScript.Gestures
{
    /// <summary>
    /// Recognizes when last touch is released from target.
    /// Works with any gesture unless a Delegate is set.
    /// </summary>
    [AddComponentMenu("TouchScript/Gestures/Release Gesture")]
    public class ReleaseGesture : Gesture
    {

        #region Constants

        public const string RELEASED_MESSAGE = "OnReleased";

        #endregion

        #region Public properties
        
        public bool IgnoreChildren
        {
            get { return ignoreChildren; }
            set { ignoreChildren = value; }
        }
        
        #endregion
        
        #region Private variables
        
        [SerializeField]
        [ToggleLeft]
        private bool ignoreChildren = false;
        
        #endregion
        
        #region Gesture callbacks

        public override bool ShouldReceiveTouch(ITouch touch)
        {
            if (!IgnoreChildren) return base.ShouldReceiveTouch(touch);
            if (!base.ShouldReceiveTouch(touch)) return false;

            if (touch.Target != transform) return false;
            return true;
        }

        /// <inheritdoc />
        public override bool CanPreventGesture(Gesture gesture)
        {
            if (Delegate == null) return false;
            return Delegate.ShouldRecognizeSimultaneously(this, gesture);
        }
        
        /// <inheritdoc />
        public override bool CanBePreventedByGesture(Gesture gesture)
        {
            if (Delegate == null) return false;
            return !Delegate.ShouldRecognizeSimultaneously(this, gesture);
        }

        /// <inheritdoc />
        protected override void touchesEnded(IList<ITouch> touches)
        {
            base.touchesEnded(touches);

            if (activeTouches.Count == 0) setState(GestureState.Recognized);
        }

        /// <inheritdoc />
        protected override void onRecognized()
        {
            base.onRecognized();
            if (UseSendMessage) SendMessageTarget.SendMessage(RELEASED_MESSAGE, this, SendMessageOptions.DontRequireReceiver);
        }

        #endregion

    }
}