// Created by Victor Engström. This free trial is for testing only. It doesn't output sound in builds.
// Copyright 2024 Sonigon AB. Please buy the full version to get sound in builds and to support developement.
// https://assetstore.unity.com/packages/tools/audio/sonity-audio-middleware-229857

using UnityEngine;

namespace ExampleSonity {

    [AddComponentMenu("")]
    public class ExampleHelperGuiText : MonoBehaviour {

        [Header("Use /n for newline")]
        public string textString = "";
        private int fontsize = 50;
        private GUIStyle guiStyle;
        public Color textColor = Color.white;

        private void Start() {
            guiStyle = new GUIStyle();
            guiStyle.fontSize = fontsize;
            guiStyle.alignment = TextAnchor.LowerCenter;
            guiStyle.normal.textColor = textColor;
            // So newline works
            textString = textString.Replace("/n", "\n");
        }

        void OnGUI() {
            GUI.Label(new Rect(Screen.width / 2, Screen.height, 0, 0), textString, guiStyle);
        }
    }
}