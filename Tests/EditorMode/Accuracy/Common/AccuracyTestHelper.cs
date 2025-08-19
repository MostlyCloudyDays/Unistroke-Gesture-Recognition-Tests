using System.Collections.Generic;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

public static class AccuracyTestHelper
{
    private const string STORAGE_NAME = "AccuracyTestDataSetsStorage";

    public static AccuracyTestDataSetsStorage LoadTestStorage()
    {
        var storage = Resources.Load<AccuracyTestDataSetsStorage>(STORAGE_NAME);
        return storage;
    }

    public static NativeArray<float2> PathToNative(List<Vector2> path)
    {
        var buffer = new NativeArray<float2>(path.Count, Allocator.Temp);

        for (int o = 0; o < path.Count; o++)
        {
            buffer[o] = path[o];
        }

        return buffer;
    }
}
