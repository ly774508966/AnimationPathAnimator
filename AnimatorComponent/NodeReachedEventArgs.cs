using System;

namespace ATP.AnimationPathAnimator.AnimatorComponent {

    public sealed class NodeReachedEventArgs : EventArgs {

        public NodeReachedEventArgs(int nodeIndex, float timestamp) {
            NodeIndex = nodeIndex;
            Timestamp = timestamp;
        }

        public int NodeIndex { get; set; }
        public float Timestamp { get; set; }

    }

}