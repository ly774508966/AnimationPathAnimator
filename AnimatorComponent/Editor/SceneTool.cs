using UnityEditor;

namespace ATP.AnimationPathAnimator.AnimatorComponent {

    public static class SceneTool {

        public static Tool LastTool = Tool.None;

        public static void RememberCurrentTool() {
            // Remember active scene tool.
            if (Tools.current != Tool.None) {
                LastTool = Tools.current;
                Tools.current = Tool.None;
            }
        }

        public static void RestoreTool() {
            Tools.current = LastTool;
        }

    }

}