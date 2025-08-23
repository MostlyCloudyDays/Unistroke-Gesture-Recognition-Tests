using System;
using NUnit.Framework;
using Unity.Collections;
using Unity.Mathematics;
using Unity.PerformanceTesting;

using MostlyCloudy.LineGestures;

public class RecognizerPerformanceTest {
    const int TEST_RUNS = 1000;
    const int WARMUPS_COUNT = 20;
    const int ITERATIONS_PER_RUN = 1;

    public void Run(IGestureRecognizer<TestGesturePattern> recognizer, TestGesturePattern testGesture, string groupeName) {
        var buffer = TestHelpers.PathToNative(testGesture.Path);

        Measure.Method(
            () => recognizer.Recognize(buffer)
        ).WarmupCount(WARMUPS_COUNT)
         .MeasurementCount(TEST_RUNS)
         .IterationsPerMeasurement(ITERATIONS_PER_RUN)
         .SampleGroup(groupeName)
         .Run();

        buffer.Dispose();
    }

    public void Run(IGestureRecognizer<TestGesturePattern> recognizer, NativeArray<float2> path, string groupeName) {
        Measure.Method(
            () => recognizer.Recognize(path)
        ).WarmupCount(WARMUPS_COUNT)
         .MeasurementCount(TEST_RUNS)
         .IterationsPerMeasurement(ITERATIONS_PER_RUN)
         .SampleGroup(groupeName)
         .Run();

        path.Dispose();
    }

    public void RunUltimateRecognizerUniformOnlyTest(int patternsNumber, int pointsNumber) {
        var uniformGesture = TestHelpers.LoadGesture(TestHelpers.UNIFORM);
        var gestureList = TestHelpers.PopulateAsset(uniformGesture, patternsNumber);

        var recognizer = new GestureRecognizer<TestGesturePattern>(gestureList, pointsNumber);

        Run(
            recognizer,
            gestureList[0],
            $"UniformOnlyTest"
        );

        recognizer.Dispose();
    }

    public void RunUltimateRecognizerUniformAndUnUniformTest(int patternsNumber, int pointsNumber) {
        var uniformGesture = TestHelpers.LoadGesture(TestHelpers.UNIFORM);
        var unUniformGesture = TestHelpers.LoadGesture(TestHelpers.UN_UNIFORM);

        var gestureList = TestHelpers.PopulateAsset(uniformGesture, patternsNumber / 2);
        gestureList.AddRange(TestHelpers.PopulateAsset(unUniformGesture, patternsNumber / 2));

        var recognizer = new GestureRecognizer<TestGesturePattern>(gestureList, pointsNumber);

        Run(
            recognizer,
            gestureList[0],
            $"UniformAndUnUniformTest"
        );

        recognizer.Dispose();
    }

    public void RunUltimateRecognizerUniformDirectionInvariantOnlyTest(int patternsNumber, int pointsNumber) {
        var uniformGesture = TestHelpers.LoadGesture(TestHelpers.UNIFORM_DIRECTION_INVARIANT);
        var gestureList = TestHelpers.PopulateAsset(uniformGesture, patternsNumber);

        var recognizer = new GestureRecognizer<TestGesturePattern>(gestureList, pointsNumber);

        Run(
            recognizer,
            gestureList[0],
            $"UniformDirectionInvariantOnlyTest"
        );

        recognizer.Dispose();
    }

    public void RunUltimateRecognizerUniformRotationInvariantOnlyTest(int patternsNumber, int pointsNumber) {
        var uniformGesture = TestHelpers.LoadGesture(TestHelpers.UNIFORM_ROTATION_INVARIANT);
        var gestureList = TestHelpers.PopulateAsset(uniformGesture, patternsNumber);

        var recognizer = new GestureRecognizer<TestGesturePattern>(gestureList, pointsNumber);

        var path = gestureList[0].Path.ToArray();
        Array.Reverse(path);

        Run(
            recognizer,
            TestHelpers.PathToNative(path),
            $"UniformRotationInvariantOnlyTest"
        );

        recognizer.Dispose();
    }

    public void RunUltimateRecognizerUniformRotationInvariantDirectionInvariantOnlyTest(int patternsNumber, int pointsNumber) {
        var uniformGesture = TestHelpers.LoadGesture(TestHelpers.UNIFORM_ROTATION_INVARIANT_DIRECTION_INVARIANT);
        var gestureList = TestHelpers.PopulateAsset(uniformGesture, patternsNumber);

        var recognizer = new GestureRecognizer<TestGesturePattern>(gestureList, pointsNumber);

        var path = gestureList[0].Path.ToArray();
        Array.Reverse(path);

        Run(
            recognizer,
            TestHelpers.PathToNative(path),
            $"UniformRotationInvariantDirectionInvariantOnlyTest"
        );

        recognizer.Dispose();
    }

    public void RunUltimateRecognizerUniformRotationInvariantDirectionInvariantOnlyWithRotatedGestureTest(int patternsNumber, int pointsNumber) {
        var uniformGesture = TestHelpers.LoadGesture(TestHelpers.UNIFORM_ROTATION_INVARIANT_DIRECTION_INVARIANT);
        var gestureList = TestHelpers.PopulateAsset(uniformGesture, patternsNumber);

        var recognizer = new GestureRecognizer<TestGesturePattern>(gestureList, pointsNumber);

        var rotatedGesture = TestHelpers.LoadGesture(TestHelpers.ROTATED);
        var path = rotatedGesture.Path.ToArray();
        Array.Reverse(path);

        Run(
            recognizer,
            TestHelpers.PathToNative(path),
            $"UniformRotationInvariantDirectionInvariantOnlyWithRotatedGesture"
        );

        recognizer.Dispose();
    }

    [Test, Performance, Version("1")]
    public void UltimateRecognizerPerformanceTest_10_Patterns_128_Points() {
        const int patternsNumber = 10;
        const int pointsNumber = 128;

        RunUltimateRecognizerUniformOnlyTest(patternsNumber, pointsNumber);
        RunUltimateRecognizerUniformAndUnUniformTest(patternsNumber, pointsNumber);
        RunUltimateRecognizerUniformDirectionInvariantOnlyTest(patternsNumber, pointsNumber);

        RunUltimateRecognizerUniformRotationInvariantOnlyTest(patternsNumber, pointsNumber);
        RunUltimateRecognizerUniformRotationInvariantDirectionInvariantOnlyTest(patternsNumber, pointsNumber);
        RunUltimateRecognizerUniformRotationInvariantDirectionInvariantOnlyWithRotatedGestureTest(patternsNumber, pointsNumber);
    }


    [Test, Performance, Version("1")]
    public void UltimateRecognizerPerformanceTest_20_Patterns_128_Points() {
        const int patternsNumber = 20;
        const int pointsNumber = 128;

        RunUltimateRecognizerUniformOnlyTest(patternsNumber, pointsNumber);
        RunUltimateRecognizerUniformAndUnUniformTest(patternsNumber, pointsNumber);

        RunUltimateRecognizerUniformDirectionInvariantOnlyTest(patternsNumber, pointsNumber);

        RunUltimateRecognizerUniformRotationInvariantOnlyTest(patternsNumber, pointsNumber);
        RunUltimateRecognizerUniformRotationInvariantDirectionInvariantOnlyTest(patternsNumber, pointsNumber);
        RunUltimateRecognizerUniformRotationInvariantDirectionInvariantOnlyWithRotatedGestureTest(patternsNumber, pointsNumber);
    }

    [Test, Performance, Version("1")]
    public void UltimateRecognizerPerformanceTest_10_Patterns_256_Points() {
        const int patternsNumber = 10;
        const int pointsNumber = 256;

        RunUltimateRecognizerUniformOnlyTest(patternsNumber, pointsNumber);
        RunUltimateRecognizerUniformAndUnUniformTest(patternsNumber, pointsNumber);
        RunUltimateRecognizerUniformDirectionInvariantOnlyTest(patternsNumber, pointsNumber);

        RunUltimateRecognizerUniformRotationInvariantOnlyTest(patternsNumber, pointsNumber);
        RunUltimateRecognizerUniformRotationInvariantDirectionInvariantOnlyTest(patternsNumber, pointsNumber);
        RunUltimateRecognizerUniformRotationInvariantDirectionInvariantOnlyWithRotatedGestureTest(patternsNumber, pointsNumber);
    }

    [Test, Performance, Version("1")]
    public void UltimateRecognizerPerformanceTest_20_Patterns_256_Points() {
        const int patternsNumber = 20;
        const int pointsNumber = 256;

        RunUltimateRecognizerUniformOnlyTest(patternsNumber, pointsNumber);
        RunUltimateRecognizerUniformAndUnUniformTest(patternsNumber, pointsNumber);
        RunUltimateRecognizerUniformDirectionInvariantOnlyTest(patternsNumber, pointsNumber);

        RunUltimateRecognizerUniformRotationInvariantOnlyTest(patternsNumber, pointsNumber);
        RunUltimateRecognizerUniformRotationInvariantDirectionInvariantOnlyTest(patternsNumber, pointsNumber);
        RunUltimateRecognizerUniformRotationInvariantDirectionInvariantOnlyWithRotatedGestureTest(patternsNumber, pointsNumber);
    }
}
