using UnityEngine;

public class AccuracyTestDataSetsStorage : ScriptableObject {
    [field: SerializeField] public AccuracyTestDataSet RotationAndDirectionSensitiveSet { get; private set; }
}