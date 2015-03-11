﻿using UnityEngine;

namespace ATP.AnimationPathAnimator.APEventsComponent {

    // TODO Rename to APEventsSettings.
    public sealed class APEventsReflectionSettings : ScriptableObject {

        #region FIELDS

        [SerializeField]
        private float defaultNodeLabelHaight = 30;

        [SerializeField]
        private float defaultNodeLabelWidth = 100;

        [SerializeField]
        private int methodNameLabelOffsetX = 30;

        [SerializeField]
        private int methodNameLabelOffsetY = -20;
        
        [SerializeField]
        private bool drawMethodNames = true;
        #endregion

        #region PROPERTIES

        public float DefaultNodeLabelHeight {
            get { return defaultNodeLabelHaight; }
            set { defaultNodeLabelHaight = value; }
        }

        public float DefaultNodeLabelWidth {
            get { return defaultNodeLabelWidth; }
            set { defaultNodeLabelWidth = value; }
        }

        public int MethodNameLabelOffsetX {
            get { return methodNameLabelOffsetX; }
            set { methodNameLabelOffsetX = value; }
        }

        public int MethodNameLabelOffsetY {
            get { return methodNameLabelOffsetY; }
        }

        public bool DrawMethodNames {
            get { return drawMethodNames; }
            set { drawMethodNames = value; }
        }

        #endregion
    }

}