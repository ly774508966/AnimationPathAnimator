﻿using System.Collections.Generic;
using ATP.AnimationPathAnimator.APAnimatorComponent;
using UnityEngine;

namespace ATP.AnimationPathAnimator.APEventsReflectionComponent {

    [RequireComponent(typeof(APAnimator))]
    public class APEventsReflection : MonoBehaviour {

        #region FIELDS
        [SerializeField]
        private GUISkin skin;

        [SerializeField]
        private APAnimator apAnimator;

        [SerializeField]
        private APEventsReflectionSettings settings;

        [SerializeField]
        private bool advancedSettingsFoldout;

        [SerializeField]
        private List<NodeEvent> nodeEvents;
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

        public APEventsReflectionSettings Settings {
            get { return settings; }
            set { settings = value; }
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
            settings =
                Resources.Load<APEventsReflectionSettings>("DefaultAPEventsReflectionSettings");
            skin = Resources.Load("DefaultAPEventsReflectionSkin") as GUISkin;
        }
        #endregion
        #region EVENT HANDLERS
        private void Animator_NodeReached(
                    object sender,
                    NodeReachedEventArgs arg) {

            // Return if there's no event slot created for current path node.
            if (arg.NodeIndex > nodeEvents.Count - 1) return;
            // Get event slot.
            var nodeEvent = nodeEvents[arg.NodeIndex];
            // Return if source GO was not specified in the event slot.
            if (nodeEvent.SourceGO == null) return;
            // Get method metadata.
            var methodInfo = nodeEvent.SourceCo.GetType()
                    .GetMethod(nodeEvent.SourceMethodName);
            // Get method parameters.
            var methodParams = methodInfo.GetParameters();
            // Method has no parameters.
            if (methodParams.Length == 0) {
                // Invoke method.
                methodInfo.Invoke(nodeEvent.SourceCo, null);
            }
            // Method has one parameter.
            else if (methodParams.Length == 1) {
                // Return if the parameter is not a string.
                if (methodParams[0].ParameterType.Name != "String") return;
                // Create string parameter argument.
                var stringParam = new object[] {nodeEvent.MethodArg};
                // Invoke method with string parameter.
                methodInfo.Invoke(nodeEvent.SourceCo, stringParam);
            }
        }

        #endregion
        #region METHODS

        public Vector3[] GetNodePositions() {
            // TODO Move GetGlobalNodePositions() to APAnimator class.
            var nodePositions =
                ApAnimator.PathData.GetGlobalNodePositions(ApAnimator.ThisTransform);

            return nodePositions;
        }

        #endregion   

    }

}