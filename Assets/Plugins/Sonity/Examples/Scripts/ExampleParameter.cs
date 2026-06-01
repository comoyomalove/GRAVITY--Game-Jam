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
    public class ExampleParameter : MonoBehaviour {

        public SoundEvent soundEvent;
        public SoundParameterPitchSemitone parameterPitch = new SoundParameterPitchSemitone(0f, UpdateMode.Once);

        void Update() {

            if (Mouse.current.leftButton.wasPressedThisFrame) {

                // SoundParameter lower pitch
                parameterPitch.PitchSemitone -= 1f;

                // Play Sound with the parameter
                soundEvent.Play(transform, parameterPitch);

            } else if (Mouse.current.rightButton.wasPressedThisFrame) {

                // SoundParameter increase pitch
                parameterPitch.PitchSemitone += 1f;

                // Play Sound with the parameter
                soundEvent.Play(transform, parameterPitch);
            }

            // Setting gui text
            GetComponent<ExampleHelperGuiText>().textString = "Press mouse left/right to increase/lower pitch\nCurrent pitch is " + parameterPitch.PitchSemitone.ToString("0") + " semitones";
        }
    }
}

#elif ENABLE_LEGACY_INPUT_MANAGER
// The old Input System

using UnityEngine;
using Sonity;

namespace ExampleSonity {

    [AddComponentMenu("")]
    public class ExampleParameter : MonoBehaviour {

        public SoundEvent soundEvent;
        public SoundParameterPitchSemitone parameterPitch = new SoundParameterPitchSemitone(0f, UpdateMode.Continuous);

        void Update() {

            if (Input.GetKeyDown(KeyCode.Mouse0)) {

                // SoundParameter lower pitch
                parameterPitch.PitchSemitone -= 1f;

                // Play Sound with the parameter
                soundEvent.Play(transform, parameterPitch);

            } else if (Input.GetKeyDown(KeyCode.Mouse1)) {

                // SoundParameter increase pitch
                parameterPitch.PitchSemitone += 1f;

                // Play Sound with the parameter
                soundEvent.Play(transform, parameterPitch);
            }

            // Setting gui text
            GetComponent<ExampleHelperGuiText>().textString = "Press mouse left/right to increase/lower pitch\nCurrent pitch is " + parameterPitch.PitchSemitone.ToString("0") + " semitones";
        }
    }
}
#endif