using Unity.Mathematics;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

public class FindNearset3 : MonoBehaviour
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

        // Create the job 
        FindNearestParaJob findJob = new(){
            targetPositions = targetPositions,
            seekerPositions = seekerPositions,
            nearestTargetPositions = nearestTargetPositions
        };

        // Schedule the job
        JobHandle findHandle = findJob.Schedule(seekerPositions.Length, 100);

        // Wait for the job to complete
        findHandle.Complete();

        // Draw the lines between the seekers and their nearest targets
        for (int i = 0; i < nearestTargetPositions.Length; i++)
        {
            Debug.DrawLine(seekerPositions[i], nearestTargetPositions[i], Color.white);
        }
    }

    void OnDestroy()
    {
        targetPositions.Dispose();
        seekerPositions.Dispose();
        nearestTargetPositions.Dispose();
    }
}

