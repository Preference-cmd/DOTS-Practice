using UnityEngine;

public class Seeker : MonoBehaviour
{

    public Vector3 direction;

    // Update is called once per frame
    void Update()
    {
        // change the position of the object
        transform.localPosition += direction * Time.deltaTime;
    }
}
