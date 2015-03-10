﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ATP.AnimationPathAnimator.APAnimatorComponent;

namespace ATP.AnimationPathAnimator.APEventsMessageComponent {

    [RequireComponent(typeof(APAnimator))]
    public sealed class APEventsMessage : MonoBehaviour {

        #region FIELDS
        [SerializeField]
        private GUISkin skin;

        [SerializeField]
        private APAnimator apAnimator;

        [SerializeField]
        private APEventsMessageData apEventsMessageData;

        [SerializeField]
        private APEventsMessageSettings messageSettings;

        [SerializeField]
        private bool advancedSettingsFoldout;
        #endregion

        #region PROPERTIES
        public APAnimator ApAnimator {
            get { return apAnimator; }
            set { apAnimator = value; }
        }

        public GUISkin Skin {
            get { return skin; }
            set { skin = value; }
        }

        public APEventsMessageData ApEventsMessageData {
            get { return apEventsMessageData; }
            set { apEventsMessageData = value; }
        }

        public APEventsMessageSettings MessageSettings {
            get { return messageSettings; }
            set { messageSettings = value; }
        }

        #endregion

        #region UNITY MESSAGES

        private void OnDisable() {
            ApAnimator.NodeReached -= Animator_NodeReached;
        }

        private void OnEnable() {
            if (ApAnimator == null) return; 

            ApAnimator.NodeReached += Animator_NodeReached;
        }

        private void Reset() {
            apAnimator = GetComponent<APAnimator>();
            messageSettings =
                Resources.Load<APEventsMessageSettings>("DefaultEventsMessageSettings");
            skin = Resources.Load("DefaultEventsMessageSkin") as GUISkin;
        }
        #endregion
        #region EVENT HANDLERS
        private void Animator_NodeReached(
                    object sender,
                    NodeReachedEventArgs arg) {

            if (ApEventsMessageData == null) return;

            // Return if no event was specified for current and later nodes.
            if (arg.NodeIndex > ApEventsMessageData.NodeEvents.Count - 1) return;

            // Get NodeEvent for current path node.
            var nodeEvent = ApEventsMessageData.NodeEvents[arg.NodeIndex];

            // Call method that will handle this event.
            gameObject.SendMessage(
                nodeEvent.MethodName,
                nodeEvent.MethodArg,
                SendMessageOptions.DontRequireReceiver);
        }

        #endregion
        #region METHODS
        public List<string> GetMethodNames() {
            var methodNames = new List<string>();

            foreach (var nodeEvent in ApEventsMessageData.NodeEvents) {
                methodNames.Add(nodeEvent.MethodName);
            }

            return methodNames;
        }

        public Vector3[] GetNodePositions() {
            // TODO Move GetGlobalNodePositions() to APAnimator class.
            var nodePositions =
                ApAnimator.PathData.GetGlobalNodePositions(ApAnimator.ThisTransform);

            return nodePositions;
        }

        #endregion
    }

}
