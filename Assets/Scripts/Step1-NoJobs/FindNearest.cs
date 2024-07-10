using UnityEngine;

public class FindNearest : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        // Compare the squared distance of each seeker to the target
        foreach (var seekerTransform in Spawner.seekerTransforms)
        {
            Vector3 seekerPosition = seekerTransform.localPosition;
            Vector3 nearestPosition = Vector3.zero;
            float maxDistSq = float.MaxValue;

            foreach (var targetTransform in Spawner.targetTransforms)
            {
                // Get the squared distance between the seeker and the target
                Vector3 offset = targetTransform.localPosition - seekerPosition;
                float distSq = offset.sqrMagnitude;

                // If the squared distance is less than the current maximum distance 
                // update the maximum distance and the nearest position
                if (distSq < maxDistSq)
                {
                    maxDistSq = distSq;
                    nearestPosition = targetTransform.localPosition;
                }
            }

            // Draw a line from the seeker to the nearest target
            Debug.DrawLine(seekerPosition, nearestPosition, Color.white);
        }
    }
}
