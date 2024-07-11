using Unity.Mathematics;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

public class FindNearset4 : MonoBehaviour
{
    NativeArray<float3> targetPositions;
    NativeArray<float3> seekerPositions;
    NativeArray<float3> nearestTargetPositions;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Spawner spawner = FindFirstObjectByType<Spawner>();

        // Since we are using a job, we need to allocate the arrays on the native side
        // and we should dispose of them manually when we are done
        targetPositions = new NativeArray<float3>(spawner.numTargets, Allocator.Persistent);
        seekerPositions = new NativeArray<float3>(spawner.numSeekers, Allocator.Persistent);
        nearestTargetPositions = new NativeArray<float3>(spawner.numSeekers, Allocator.Persistent);
    }

    // Update is called once per frame
    void Update()
    {
        // Copy the target and seeker positions from the spawner to the native arrays
        // since the number of targets and seekers are not strictly the same as the length of the arrays
        // we need to copy them seperately
        for (int i = 0; i < targetPositions.Length; i++)
        {
            targetPositions[i] = Spawner.targetTransforms[i].localPosition;
        }
        for (int i = 0; i < seekerPositions.Length; i++)
        {
            seekerPositions[i] = Spawner.seekerTransforms[i].localPosition;
        }

        // Before create the find job, we need to sort the target positions array
        // so that the job can find the nearest target efficiently
        SortJob<float3, AxisXComparer> sortJob = targetPositions.SortJob(new AxisXComparer());
        JobHandle sortHandle = sortJob.Schedule();



        // Create the job 
        FindSmartJob findSmartJob = new(){
            targetPositions = targetPositions,
            seekerPositions = seekerPositions,
            nearestTargetPositions = nearestTargetPositions
        };

        // Schedule the job, we need find job to be dependent on the sort job
        JobHandle findHandle = findSmartJob.Schedule(seekerPositions.Length, 100, sortHandle);

        // Wait for the job to complete
        findHandle.Complete();

        // Draw the lines between the seekers and their nearest targets
        for (int i = 0; i < nearestTargetPositions.Length; i++)
        {
            Debug.DrawLine(seekerPositions[i], nearestTargetPositions[i], Color.white);
        }

        // Test Result: FindSmartJob[Burst] complete in 0.3ms for 10000 seekers and 10000 targets
    }

    void OnDestroy()
    {
        targetPositions.Dispose();
        seekerPositions.Dispose();
        nearestTargetPositions.Dispose();
    }
}
