using UnityEngine;

public class Spawner : MonoBehaviour
{
    public static Transform[] targetTransforms;
    public static Transform[] seekerTransforms;
    public GameObject targetPrefab;
    public GameObject seekerPrefab;
    public int numSeekers;
    public int numTargets;
    public Vector2 bounds;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Random.InitState(123);

        seekerTransforms = new Transform[numSeekers];
        for (int i = 0; i < numSeekers; i++)
        {
            // Instantiate a seeker and get the Seeker script
            GameObject go = GameObject.Instantiate(seekerPrefab);
            Seeker seeker = go.GetComponent<Seeker>();
            // Set the direction of the seeker
            Vector2 dir = Random.insideUnitCircle;
            seeker.direction = new Vector3(dir.x, 0, dir.y);
            // Put the transform in the array
            seekerTransforms[i] = go.transform;
            // Set the position of the seeker
            go.transform.localPosition = new Vector3(
                Random.Range(0, bounds.x), 0, Random.Range(0, bounds.y)
            );
        }

        targetTransforms = new Transform[numTargets];
        for (int i = 0; i < numTargets; i++)
        {
            // Instantiate a target and get the Target script
            GameObject go = GameObject.Instantiate(targetPrefab);
            Target target = go.GetComponent<Target>();
            // Set the position of the target
            Vector2 dir = Random.insideUnitCircle;
            target.direction = new Vector3(dir.x, 0, dir.y);
            // Put the transform in the array
            targetTransforms[i] = go.transform;
            // Set the position of the target
            go.transform.localPosition = new Vector3(
                Random.Range(0, bounds.x), 0, Random.Range(0, bounds.y)
            );
        }
    }
}
