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

        if (startIndex >= targetPositions.Length)
        {
            startIndex = targetPositions.Length - 1;
        }

        // Set the nearest target position and distance as the initial value
        float3 nearestTargetPosition = targetPositions[startIndex];
        float nearestDistance = math.distancesq(seekerPosition, nearestTargetPosition);

        // Search from the current target to the end of the array
        Search(seekerPosition, startIndex + 1, targetPositions.Length, 1, ref nearestTargetPosition, ref nearestDistance);

        // Search from the current target to the beginning of the array
        Search(seekerPosition, startIndex - 1, -1, -1, ref nearestTargetPosition, ref nearestDistance);

        // Store the nearest target position
        nearestTargetPositions[index] = nearestTargetPosition;
    }

    
    /// <summary>
    /// Search for the target position with the smallest x-axis distance
    /// </summary>
    /// <param name="seekerPos">The position of the seeker</param>
    void Search(float3 seekerPos, int startIdx, int endIdx,int step, ref float3 nearestTargetPos, ref float nearestDistSq)
    {
        for (int i = startIdx; i != endIdx; i += step)
        {
            float3 targetPos = targetPositions[i];
            float xoffset = math.square(seekerPos.x - targetPos.x);
            
            // if the x-axis distance is larger than the nearest distance, stop the search
            if (xoffset > nearestDistSq)
            {
                break;
            }

            // Since we've already computed the x-axis distance, we only need to compute the z-axis distance
            float dist = xoffset + math.square(seekerPos.z - targetPos.z);
            // if current distance is smaller than the nearest distance, update the nearest target position and distance
            if (dist < nearestDistSq)
            {
                nearestTargetPos = targetPos;
                nearestDistSq = dist;
            }
        }
    }


}

// Comparer for sorting the target positions by their x-axis
public struct AxisXComparer : IComparer<float3>
{
    public int Compare(float3 a, float3 b)
    {
        return a.x.CompareTo(b.x);
    }
}
