﻿using System;
using ATP.ReorderableList;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace ATP.AnimationPathTools {
    /// <summary>
    /// Allows creating and drawing 3d paths using Unity's animation curves.
    /// </summary>
    [ExecuteInEditMode]
    public class AnimationPathBuilder : GameComponent {

        #region CONSTANTS

        /// <summary>
        /// How many points should be drawn for one meter of a gizmo curve.
        /// </summary>
        public const int GizmoCurveSamplingFrequency = 20;

        #endregion Constants

        #region FIELDS

        public event EventHandler PathReset;
        public event EventHandler NodeTimeChanged;
        public event EventHandler NodeAdded;
        public event EventHandler NodeRemoved;
		public event EventHandler NodePositionChanged;

        /// <summary>
        /// Animation curves that make the animation path.
        /// </summary>
        //[SerializeField]
        //private AnimationPath pathData.AnimatedObjectPath;

        #endregion Fields

        #region EDITOR

        /// <summary>
        /// If true, advenced setting in the inspector will be folded out.
        /// </summary>
        [SerializeField]
#pragma warning disable 414
        private bool advancedSettingsFoldout;

#pragma warning restore 414

        /// <summary>
        /// How many transforms should be created for 1 m of gizmo curve when
        /// exporting nodes to transforms.
        /// </summary>
        /// <remarks>Exporting is implemented in <c>Editor</c> class.</remarks>
        [SerializeField]
#pragma warning disable 414
        private int exportSamplingFrequency = 5;

#pragma warning restore 414

        /// <summary>
        /// Color of the gizmo curve.
        /// </summary>
        [SerializeField]
        private Color gizmoCurveColor = Color.yellow;

        /// <summary>
        /// If "Move All" mode is enabled.
        /// </summary>
        //[SerializeField]
        //private bool moveAllMode;

        //[SerializeField]
        //private bool sceneControls = true;

        /// <summary>
        /// Styles for multiple GUI elements.
        /// </summary>
        [SerializeField]
        private GUISkin skin;

#pragma warning disable 0414

        /// <summary>
        /// If enabled, on-scene handles will be use to change node's in/out
        /// tangents.
        /// </summary>
        //[SerializeField]
        //private bool tangentMode;

        [SerializeField] private AnimationPathBuilderHandleMode handleMode =
            AnimationPathBuilderHandleMode.MoveSingle;

        [SerializeField] private AnimationPathBuilderTangentMode tangentMode =
            AnimationPathBuilderTangentMode.Smooth;

        [SerializeField] private PathData pathData;

#pragma warning restore 0414

        #endregion Editor

        #region PUBLIC PROPERTIES

        //public AnimationPath pathData.AnimatedObjectPath {
        //    get { return pathData.AnimatedObjectPath; }
        //}

        /// <summary>
        /// Color of the gizmo curve.
        /// </summary>
        public Color GizmoCurveColor {
            get { return gizmoCurveColor; }
            set { gizmoCurveColor = value; }
        }

        //public bool MoveAllMode {
        //    get { return moveAllMode; }
        //    set { moveAllMode = value; }
        //}

        /// <summary>
        /// Number of keys in an animation curve.
        /// </summary>
        public int NodesNo {
            get { return pathData.AnimatedObjectPath.KeysNo; }
        }

        //public bool SceneControls {
        //    get { return sceneControls; }
        //    set { sceneControls = value; }
        //}

        public GUISkin Skin {
            get { return skin; }
        }

        //public bool TangentMode {
        //    get { return tangentMode; }
        //    set { tangentMode = value; }
        //}

        //public bool IsInitialized {
        //    get { return (pathData.AnimatedObjectPath.KeysNo >= 2); }
        //}

        public AnimationPathBuilderTangentMode TangentMode {
            get { return tangentMode; }
            set { tangentMode = value; }
        }

        /// <summary>
        /// If enabled, on-scene handles will be use to change node's in/out
        /// tangents.
        /// </summary>
//[SerializeField]
//private bool tangentMode;
        public AnimationPathBuilderHandleMode HandleMode {
            get { return handleMode; }
            set { handleMode = value; }
        }

        public PathData PathData {
            get { return pathData; }
            set { pathData = value; }
        }

        #endregion PUBLIC PROPERTIES

        #region UNITY MESSAGES

        [SuppressMessage("ReSharper", "UnusedMember.Local")]
        private void Awake() {
            // Load default skin.
            skin = Resources.Load("GUISkin/default") as GUISkin;
        }

        private void OnDestroy() {
        }
        [SuppressMessage("ReSharper", "UnusedMember.Local")]
        private void OnDrawGizmosSelected() {
            DrawGizmoCurve();
        }

        [SuppressMessage("ReSharper", "UnusedMember.Local")]
        private void OnEnable() {
            //// Instantiate pathData.AnimatedObjectPath.
            //if (pathData.AnimatedObjectPath == null) {
            //    pathData.AnimatedObjectPath =
            //        ScriptableObject.CreateInstance<AnimationPath>();
            //}
            PathReset += this_PathReset;

        }

        private void this_PathReset(object sender, EventArgs eventArgs) {
            // Change handle mode to MoveAll.
            handleMode = AnimationPathBuilderHandleMode.MoveAll;
        }

        #endregion Unity Messages
        #region EVENT INVOCATORS
        protected virtual void OnNodeRemoved() {
            var handler = NodeRemoved;
            if (handler != null) handler(this, EventArgs.Empty);
        }

        protected virtual void OnNodeAdded() {
            var handler = NodeAdded;
            if (handler != null) handler(this, EventArgs.Empty);
        }

        protected virtual void OnNodeTimeChanged() {
            var handler = NodeTimeChanged;
            if (handler != null) handler(this, EventArgs.Empty);
        }

		protected virtual void OnNodePositionChanged() {
			var handler = NodePositionChanged;
			if (handler != null) handler(this, EventArgs.Empty);
		}

        public virtual void this_PathReset() {
            // Change handle mode to MoveAll.
            //handleMode = AnimationPathBuilderHandleMode.MoveSingle;
            // Call handler methods.
            var handler = PathReset;
            if (handler != null) handler(this, EventArgs.Empty);
        }

        #endregion

        #region PUBLIC METHODS

		public void SetWrapMode (WrapMode wrapMode) {
			pathData.AnimatedObjectPath.SetWrapMode(wrapMode);
		}

        public void SetNodeTangents(int index, Vector3 inOutTangent) {
            pathData.AnimatedObjectPath.ChangePointTangents(index, inOutTangent);
        }

        public void ChangeNodeTimestamp(
                            int keyIndex,
                            float newTimestamp) {

            pathData.AnimatedObjectPath.ChangeNodeTimestamp(keyIndex, newTimestamp);
            OnNodeTimeChanged();
        }

        public void CreateNode(float timestamp, Vector3 position) {
            pathData.AnimatedObjectPath.CreateNewNode(timestamp, position);
            OnNodeAdded();
        }

        public void CreateNodeAtTime(float timestamp) {
            pathData.AnimatedObjectPath.AddNodeAtTime(timestamp);
            OnNodeAdded();
        }
        public void DistributeTimestamps() {
            // Calculate path curved length.
            var pathLength = pathData.AnimatedObjectPath.CalculatePathCurvedLength(
                GizmoCurveSamplingFrequency);
            // Calculate time for one meter of curve length.
            var timeForMeter = 1 / pathLength;
            // Helper variable.
            float prevTimestamp = 0;

            // For each node calculate and apply new timestamp.
            for (var i = 1; i < NodesNo - 1; i++) {
                // Calculate section curved length.
                var sectionLength = pathData.AnimatedObjectPath.CalculateSectionCurvedLength(
                    i - 1,
                    i,
                    GizmoCurveSamplingFrequency);
                // Calculate time interval.
                var sectionTimeInterval = sectionLength * timeForMeter;
                // Calculate new timestamp.
                var newTimestamp = prevTimestamp + sectionTimeInterval;
                // Update previous timestamp.
                prevTimestamp = newTimestamp;

                // Update node timestamp.
                ChangeNodeTimestamp(i, newTimestamp);
            }
        }

        public Vector3 GetNodePosition(int nodeIndex) {
            return pathData.AnimatedObjectPath.GetVectorAtKey(nodeIndex);
        }

        public Vector3[] GetNodePositions(bool globalPositions = false) {
            var result = new Vector3[NodesNo];

            for (var i = 0; i < NodesNo; i++) {
                // Get node 3d position.
                result[i] = pathData.AnimatedObjectPath.GetVectorAtKey(i);

                // Convert position to global coordinate.
                if (globalPositions) {
                    result[i] = transform.TransformPoint(result[i]);
                }
            }

            return result;
        }

		public Vector3[] GetNodeGlobalPositions() {
			var nodePositions = GetNodePositions();

			for (var i = 0; i < nodePositions.Length; i++) {
				// Convert each position to global coordinate.
				nodePositions[i] = transform.TransformPoint(nodePositions[i]);
			}

			return nodePositions;
		}

        public float GetNodeTimestamp(int nodeIndex) {
            return pathData.AnimatedObjectPath.GetTimeAtKey(nodeIndex);
        }

        public float[] GetNodeTimestamps() {
            // Output array.
            var result = new float[NodesNo];

            // For each key..
            for (var i = 0; i < NodesNo; i++) {
                // Get key time.
                result[i] = pathData.AnimatedObjectPath.GetTimeAtKey(i);
            }

            return result;
        }

        public Vector3 GetVectorAtTime(float timestamp) {
            return pathData.AnimatedObjectPath.GetVectorAtTime(timestamp);
        }

        public void OffsetNodePositions(Vector3 moveDelta) {
            // For each node..
            for (var i = 0; i < NodesNo; i++) {
                // Old node position.
                var oldPosition = GetNodePosition(i);
                // New node position.
                var newPosition = oldPosition + moveDelta;
                // Update node positions.
                pathData.AnimatedObjectPath.MovePointToPosition(i, newPosition);

				OnNodePositionChanged();
            }
        }

        public void MoveNodeToPosition(int nodeIndex, Vector3 position) {
            pathData.AnimatedObjectPath.MovePointToPosition(nodeIndex, position);
			OnNodePositionChanged();
        }

        public void RemoveNode(int nodeIndex) {
            pathData.AnimatedObjectPath.RemoveNode(nodeIndex);
            OnNodeRemoved();
        }

        public void SetNodesLinear() {
            for (var i = 0; i < 3; i++) {
                Utilities.SetCurveLinear(pathData.AnimatedObjectPath[i]);
            }
        }

        /// <summary>
        /// Smooth tangents in all nodes in all animation curves.
        /// </summary>
        /// <param name="weight">Weight to be applied to the tangents.</param>
        public void SmoothAllNodeTangents(float weight = 0) {
            // For each key..
            for (var j = 0; j < NodesNo; j++) {
                // Smooth in and out tangents.
                pathData.AnimatedObjectPath.SmoothPointTangents(j);
            }
        }

        public void SmoothSingleNodeTangents(int nodeIndex) {
            pathData.AnimatedObjectPath.SmoothPointTangents(nodeIndex);
        }

        #endregion Public Methods

        #region PRIVATE METHODS



        private void DrawGizmoCurve() {
            // Return if path asset is not assigned.
            if (pathData == null) return;

            // Get transform component.
            var transform = GetComponent<Transform>();

            // Get path points.
            var points = pathData.AnimatedObjectPath.SamplePathForPoints(
                GizmoCurveSamplingFrequency);

            // Convert points to global coordinates.
            var globalPoints = new Vector3[points.Count];
            for (int i = 0; i < points.Count; i++) {
                globalPoints[i] = transform.TransformPoint(points[i]);
            }

            // There must be at least 3 points to draw a line.
            if (points.Count < 3) return;

            Gizmos.color = gizmoCurveColor;

            // Draw curve.
            for (var i = 0; i < points.Count - 1; i++) {
				Gizmos.DrawLine(globalPoints[i], globalPoints[i + 1]);
            }
        }

        #endregion PRIVATE METHODS

        public void RemoveAllNodes() {
            var nodesNo = NodesNo;
            for (var i = 0; i < nodesNo; i++) {
                // NOTE After each removal, next node gets index 0.
                RemoveNode(0);
            }
        }
    }
}