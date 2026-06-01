// Created by Victor Engström. This free trial is for testing only. It doesn't output sound in builds.
// Copyright 2024 Sonigon AB. Please buy the full version to get sound in builds and to support developement.
// https://assetstore.unity.com/packages/tools/audio/sonity-audio-middleware-229857

using UnityEngine;
using System;
using Sonity.Internal;

namespace Sonity {

    /// <summary>
    /// <see cref="SoundContainerBase">SoundContainers</see> are the building blocks of Sonity.
    /// They contain <see cref="AudioClip"/>s and options of how the sound should be played.
    /// All <see cref="SoundContainerBase">SoundContainers</see> are multi-object editable.
    /// </summary
    [Serializable]
    [CreateAssetMenu(fileName = "_SC", menuName = "Sonity 🔊/SoundContainer", order = 100)] // Having a big gap in indexes creates dividers
    public class SoundContainer : SoundContainerBase {

    }
}