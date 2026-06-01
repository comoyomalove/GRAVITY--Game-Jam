// Created by Victor Engström. This free trial is for testing only. It doesn't output sound in builds.
// Copyright 2024 Sonigon AB. Please buy the full version to get sound in builds and to support developement.
// https://assetstore.unity.com/packages/tools/audio/sonity-audio-middleware-229857

using UnityEngine;
using Sonity;

namespace SonityTemplate {

    /// <summary>
    /// Template of a singleton used to play e.g. UI sounds.
    /// Useful when you want to play <see cref="SoundEventBase">SoundEvents</see> in a lot of places through code.
    /// Add to a GameObject in the scene and use like this:
    /// SonityTemplate.TemplateSoundPlayUI.Instance.PlayButtonClick();
    /// </summary>
    [AddComponentMenu("Sonity/Template - Sound Play UI")]
    public class TemplateSoundPlayUI : MonoBehaviour {

        // Expand with more sounds to your hearts content
        public SoundEvent soundButtonHover;
        public SoundEvent soundButtonClick;

        public void PlayButtonHover() {
            SoundManager.Instance.Play(soundButtonHover, transform);
        }

        public void PlayButtonClick() {
            SoundManager.Instance.Play(soundButtonClick, transform);
        }

        // Singleton Instance
        public bool useDontDestroyOnLoad = true;
        private bool isGoingToDelete;
#if UNITY_EDITOR
        private bool debugInstanceDestroyed = true;
#endif

        public static TemplateSoundPlayUI Instance {
            get;
            private set;
        }

        private void Awake() {
            InstanceCheck();
            if (useDontDestroyOnLoad && !isGoingToDelete) {
                // DontDestroyOnLoad only works for root GameObjects
                gameObject.transform.parent = null;
                DontDestroyOnLoad(gameObject);
            }
        }

        // Checks if there are multiple instances this script, if so it destroys one of them
        private void InstanceCheck() {
            if (Instance == null) {
                Instance = this;
            } else if (Instance != this) {
#if UNITY_EDITOR
                if (debugInstanceDestroyed) {
                    Debug.LogWarning($"There can only be one {nameof(TemplateSoundPlayUI)} instance per scene.", this);
                }
#endif
                // So that it does not run the rest of the Awake and Update code
                isGoingToDelete = true;
                if (Application.isPlaying) {
                    Destroy(gameObject);
                }
            }
        }
    }
}