using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AccuracyTestDataSet", menuName = "Create Test Data Set")]
public sealed class AccuracyTestDataSet : ScriptableObject {
    [field: SerializeField] public List<AccuracyTestGesturePattern> Patterns { get; private set; }
    [field: SerializeField] public List<AccuracyGestureRecord> Records { get; private set; }
}
