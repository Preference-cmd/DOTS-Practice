using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

// Use Unity BurstCompile
[BurstCompile]
public struct FindNearestParaJob : IJobParallelFor
{
    // Set [ReadOnly] attribute to native array 
    // to safely run jobs concurrently
    [ReadOnly] public NativeArray<float3> targetPositions;
    [ReadOnly] public NativeArray<float3> seekerPositions;

    // for every seeker, store the nearest target position
    public NativeArray<float3> nearestTargetPositions;
    public void Execute(int index)
    {
        // Compute the nearest target position for each seeker
        float3 seekerPosition = seekerPositions[index];
        float maxDistSq = float.MaxValue;

        for (int j = 0; j < targetPositions.Length; j++)
        {
            float3 targetPosition = targetPositions[j];
            float distSq = math.distancesq(seekerPosition, targetPosition);

            if (distSq < maxDistSq)
            {
                maxDistSq = distSq;
                nearestTargetPositions[index] = targetPosition;
            }
        }
    }
}
