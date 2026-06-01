// Created by Victor Engström. This free trial is for testing only. It doesn't output sound in builds.
// Copyright 2024 Sonigon AB. Please buy the full version to get sound in builds and to support developement.
// https://assetstore.unity.com/packages/tools/audio/sonity-audio-middleware-229857

using UnityEngine;
using System;
using Sonity.Internal;

namespace Sonity {

    /// <summary>
    /// <see cref="SoundPhysicsConditionBase">SoundPhysicsCondition</see> objects are used in the <see cref="SoundPhysicsBase">SoundPhysics</see> component to decide if a physics interaction should play a <see cref="SoundEventBase">SoundEvent</see> or not.
    /// They make it easy to manage a large amount of physics objects and linking conditions together with the child nesting feature.
    /// You can also use them for playing different sounds with a single <see cref="SoundEventBase">SoundEvent</see> by using <see cref="SoundTagBase">SoundTag</see>s.
    /// All <see cref="SoundPhysicsConditionBase">SoundPhysicsCondition</see> objects are multi-object editable.
    /// </summary>
    [Serializable]
    [CreateAssetMenu(fileName = "SPC_", menuName = "Sonity 🔊/SoundPhysicsCondition", order = 200)] // Having a big gap in indexes creates dividers
    public class SoundPhysicsCondition : SoundPhysicsConditionBase {

    }
}