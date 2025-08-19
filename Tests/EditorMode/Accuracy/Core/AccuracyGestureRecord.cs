using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public sealed class AccuracyGestureRecord {
    [field: SerializeField] public List<Vector2> RecordedPath { get; private set; }
    [field: SerializeField] public AccuracyTestGesturePattern Gesture { get; private set; }

    public AccuracyGestureRecord(AccuracyTestGesturePattern gesture, List<Vector2> recordedPath) {
        Gesture = gesture;
        RecordedPath = recordedPath;
    }
}