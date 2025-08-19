using System.Collections;
using NUnit.Framework;
using UnistrokeGestureRecognition;
using Unity.PerformanceTesting;
using UnityEngine.TestTools;

public sealed class AccuracyTests
{
    [Test, Performance] public void RotationAndDirectionSensitive_AccuracyTest_512p() => Run(AccuracyTestHelper.LoadTestStorage().RotationAndDirectionSensitiveSet, 512);
    [Test, Performance] public void RotationAndDirectionSensitive_AccuracyTest_256p() => Run(AccuracyTestHelper.LoadTestStorage().RotationAndDirectionSensitiveSet, 256);
    [Test, Performance] public void RotationAndDirectionSensitive_AccuracyTest_128p() => Run(AccuracyTestHelper.LoadTestStorage().RotationAndDirectionSensitiveSet, 128);

    [Test, Performance] public void RotationSensitiveAndDirectionInvariant_AccuracyTest_512p() => Run(AccuracyTestHelper.LoadTestStorage().RotationSensitiveAndDirectionInvariantSet, 512);
    [Test, Performance] public void RotationSensitiveAndDirectionInvariant_AccuracyTest_256p() => Run(AccuracyTestHelper.LoadTestStorage().RotationSensitiveAndDirectionInvariantSet, 256);
    [Test, Performance] public void RotationSensitiveAndDirectionInvariant_AccuracyTest_128p() => Run(AccuracyTestHelper.LoadTestStorage().RotationSensitiveAndDirectionInvariantSet, 128);

    [Test, Performance] public void RotationInvariantAndDirectionSensitive_AccuracyTest_512p() => Run(AccuracyTestHelper.LoadTestStorage().RotationInvariantAndDirectionSensitiveSet, 512);
    [Test, Performance] public void RotationInvariantAndDirectionSensitive_AccuracyTest_256p() => Run(AccuracyTestHelper.LoadTestStorage().RotationInvariantAndDirectionSensitiveSet, 256);
    [Test, Performance] public void RotationInvariantAndDirectionSensitive_AccuracyTest_128p() => Run(AccuracyTestHelper.LoadTestStorage().RotationInvariantAndDirectionSensitiveSet, 128);

    [Test, Performance] public void RotationAndDirectionInvariant_AccuracyTest_512p() => Run(AccuracyTestHelper.LoadTestStorage().RotationAndDirectionInvariantSet, 512);
    [Test, Performance] public void RotationAndDirectionInvariant_AccuracyTest_256p() => Run(AccuracyTestHelper.LoadTestStorage().RotationAndDirectionInvariantSet, 256);
    [Test, Performance] public void RotationAndDirectionInvariant_AccuracyTest_128p() => Run(AccuracyTestHelper.LoadTestStorage().RotationAndDirectionInvariantSet, 128);

    public void Run(AccuracyTestDataSet dataSet, int resamplePoints)
    {
        var accuracyGroupe = new SampleGroup("Accuracy Score", SampleUnit.Undefined, increaseIsBetter: true);
        var presentsGroupe = new SampleGroup("Accuracy %", SampleUnit.Undefined, increaseIsBetter: true);

        var (score, presents) = CalculateScore(dataSet, resamplePoints);

        Measure.Custom(accuracyGroupe, score);
        Measure.Custom(presentsGroupe, presents);
    }

    private (double total, double percents) CalculateScore(AccuracyTestDataSet dataSet, int resamplePoints)
    {
        var recognizer = new GestureRecognizer<AccuracyTestGesturePattern>(dataSet.Patterns, resamplePoints);

        double score = 0;
        foreach (var item in dataSet.Records)
        {
            var path = AccuracyTestHelper.PathToNative(item.RecordedPath);
            var result = recognizer.Recognize(path);

            if (result.Pattern == item.Gesture) score += result.Score;

            path.Dispose();
        }

        recognizer.Dispose();

        return (score, score / dataSet.Records.Count);
    }

}
