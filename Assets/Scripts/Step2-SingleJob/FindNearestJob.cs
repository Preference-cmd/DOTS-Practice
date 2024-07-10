using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;


// Use Unity BurstCompile
[BurstCompile]
public struct FindNearestJob : IJob
{
    // Set [ReadOnly] attribute to native array 
    // to safely run jobs concurrently
    [ReadOnly] public NativeArray<float3> targetPositions;
    [ReadOnly] public NativeArray<float3> seekerPositions;

    // for every seeker, store the nearest target position
    public NativeArray<float3> nearestTargetPositions;
    public void Execute()
    {
        // Compute the nearest target position for each seeker
        for (int i = 0; i < seekerPositions.Length; i++)
        {
            float3 seekerPosition = seekerPositions[i];
            float maxDistSq = float.MaxValue;

            for (int j = 0; j < targetPositions.Length; j++)
            {
                float3 targetPosition = targetPositions[j];
                float distSq = math.distancesq(seekerPosition, targetPosition);

                if (distSq < maxDistSq)
                {
                    maxDistSq = distSq;
                    nearestTargetPositions[i] = targetPosition;
                }
            }
        }
    }
}
