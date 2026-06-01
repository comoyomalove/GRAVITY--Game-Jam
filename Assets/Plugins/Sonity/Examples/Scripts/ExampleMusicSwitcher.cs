// Created by Victor Engström. This free trial is for testing only. It doesn't output sound in builds.
// Copyright 2024 Sonigon AB. Please buy the full version to get sound in builds and to support developement.
// https://assetstore.unity.com/packages/tools/audio/sonity-audio-middleware-229857

#if ENABLE_INPUT_SYSTEM
// The new Input System

using UnityEngine;
using UnityEngine.InputSystem;

namespace ExampleSonity {

    [AddComponentMenu("")]
    public class ExampleMusicSwitcher : MonoBehaviour {

        private void Update() {

            if (Mouse.current.leftButton.wasPressedThisFrame) {
                // Play main menu music
                SonityTemplate.TemplateSoundMusicManager.Instance.PlayMainMenu();
            } else if (Mouse.current.rightButton.wasPressedThisFrame) {
                // Play ingame music
                SonityTemplate.TemplateSoundMusicManager.Instance.PlayIngame();
            }

            // Setting gui text
            GetComponent<ExampleHelperGuiText>().textString = "Press left/right mouse buttons\nto play main menu/ingame music\nCurrent music is " + SonityTemplate.TemplateSoundMusicManager.Instance.GetMusicPlaying();
        }
    }
}

#elif ENABLE_LEGACY_INPUT_MANAGER
// The old Input System

using UnityEngine;

namespace ExampleSonity {

    [AddComponentMenu("")]
    public class ExampleMusicSwitcher : MonoBehaviour {

        private void Update() {

            if (Input.GetKeyDown(KeyCode.Mouse0)) {
                // Play main menu music
                SonityTemplate.TemplateSoundMusicManager.Instance.PlayMainMenu();
            } else if (Input.GetKeyDown(KeyCode.Mouse1)) {
                // Play ingame music
                SonityTemplate.TemplateSoundMusicManager.Instance.PlayIngame();
            }

            // Setting gui text
            GetComponent<ExampleHelperGuiText>().textString = "Press left/right mouse buttons\nto play main menu/ingame music\nCurrent music is " + SonityTemplate.TemplateSoundMusicManager.Instance.GetMusicPlaying();
        }
    }
}
#endif

