// Created by Victor Engström. This free trial is for testing only. It doesn't output sound in builds.
// Copyright 2024 Sonigon AB. Please buy the full version to get sound in builds and to support developement.
// https://assetstore.unity.com/packages/tools/audio/sonity-audio-middleware-229857

using UnityEngine;
using Sonity;

namespace ExampleSonity {

    [AddComponentMenu("")]
    public class ExampleColliderSoundTag : MonoBehaviour {

        public SoundTag soundTagIndoor;
        public SoundTag soundTagOutdoor;

        private AudioListener cachedAudioListener;
        private Collider cachedCollider;

        void Start() {
#if UNITY_6000_0_OR_NEWER
            // FindFirstObjectByType is slower than FindAnyObjectByType but is more consistent
            cachedAudioListener = UnityEngine.Object.FindFirstObjectByType<AudioListener>();
#else
            cachedAudioListener = UnityEngine.Object.FindObjectOfType<AudioListener>();
#endif
            cachedCollider = GetComponent<Collider>();
        }

        void Update() {
            if (cachedCollider.bounds.Contains(cachedAudioListener.GetComponent<Transform>().position)) {
                // Is Indoor
                SoundManager.Instance.SetGlobalSoundTag(soundTagIndoor);
            } else {
                // Is Outdoor
                SoundManager.Instance.SetGlobalSoundTag(soundTagOutdoor);
            }
        }
    }
}
