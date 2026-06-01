// Created by Victor Engström. This free trial is for testing only. It doesn't output sound in builds.
// Copyright 2024 Sonigon AB. Please buy the full version to get sound in builds and to support developement.
// https://assetstore.unity.com/packages/tools/audio/sonity-audio-middleware-229857

using UnityEngine;

namespace ExampleSonity {

    [AddComponentMenu("")]
    public class ExampleFollowCamera : MonoBehaviour {

        // Follows and looks at the target
        public Transform target;
        public float lerpSpeed = 10f;
        public Vector3 locationOffset = new Vector3(0f, 8f, -15f);

        private Vector3 previousPosition;

        private void Awake() {
            transform.position = target.position + locationOffset;
            previousPosition = transform.position;
        }

        void LateUpdate() {
            transform.position = Vector3.Lerp(previousPosition, target.position + locationOffset, lerpSpeed * Time.deltaTime);
            previousPosition = transform.position;
            transform.LookAt(target);
        }
    }
}