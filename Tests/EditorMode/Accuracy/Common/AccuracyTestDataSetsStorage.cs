using UnityEngine;

public class AccuracyTestDataSetsStorage : ScriptableObject {
    [field: SerializeField] public AccuracyTestDataSet RotationAndDirectionSensitiveSet { get; private set; }
    [field: SerializeField] public AccuracyTestDataSet RotationSensitiveAndDirectionInvariantSet { get; private set; }
    [field: SerializeField] public AccuracyTestDataSet RotationInvariantAndDirectionSensitiveSet { get; private set; }
    [field: SerializeField] public AccuracyTestDataSet RotationAndDirectionInvariantSet { get; private set; }
}