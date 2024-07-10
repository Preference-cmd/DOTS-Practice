using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using System.Collections.Generic;

[BurstCompile]
public struct FindSmartJob : IJobParallelFor
{
    // Set [ReadOnly] attribute to native array 
    // to safely run jobs concurrently
    [ReadOnly] public NativeArray<float3> targetPositions;
    [ReadOnly] public NativeArray<float3> seekerPositions;
    public NativeArray<float3> nearestTargetPositions;

    public void Execute(int index)
    {
        float3 seekerPosition = seekerPositions[index];
        
        // Binary search for the target position with the smallest x-axis distance
        // The array must be sorted before using binary search
        // Notice: [[NativeArray].BinarySearch()] seems to have been deprecated in Unity6
        //         use [NativeSortExtension.BinarySearch()] instead
        int startIndex = NativeSortExtension.BinarySearch(targetPositions, seekerPosition, new AxisXComparer());

        // If the target position is not found, get the closest target position
        if (startIndex < 0)
        {
            startIndex = ~startIndex;
        }

        if (startIndex > targetPositions.Length)
        {
            startIndex = targetPositions.Length - 1;
        }

        // Get the closest target position coordinate
        float3 nearestTargetPosition = targetPositions[startIndex];
        float nearestDistance = math.distance(seekerPosition, nearestTargetPosition);

        // TODO: Search around the nearest target position to find the closest one


    }

    void Search()
    {

    }


}

// Comparer for sorting the target positions by their x-axis
public class AxisXComparer : IComparer<float3>
{
    public int Compare(float3 x, float3 y)
    {
        return x.x.CompareTo(y.x);
    }
}
