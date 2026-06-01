// Created by Victor Engström. This free trial is for testing only. It doesn't output sound in builds.
// Copyright 2024 Sonigon AB. Please buy the full version to get sound in builds and to support developement.
// https://assetstore.unity.com/packages/tools/audio/sonity-audio-middleware-229857

#if ENABLE_INPUT_SYSTEM
// The new Input System

using UnityEngine;
using UnityEngine.InputSystem;
using Sonity;

namespace ExampleSonity {

    [AddComponentMenu("")]
    public class ExampleShot : MonoBehaviour {

        public SoundEvent soundEventShot;

        void Update() {
            if (Mouse.current.leftButton.wasPressedThisFrame) {
                soundEventShot.Play(transform);
            }
        }
    }
}

#elif ENABLE_LEGACY_INPUT_MANAGER
// The old Input System

using UnityEngine;
using Sonity;

namespace ExampleSonity {

    [AddComponentMenu("")]
    public class ExampleShot : MonoBehaviour {

        public SoundEvent soundEventShot;

        void Update() {
            if (Input.GetKeyDown(KeyCode.Mouse0)) {
                soundEventShot.Play(transform);
            }
        }
    }
}
#endif