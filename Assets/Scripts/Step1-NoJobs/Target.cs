using UnityEngine;

public class Target : MonoBehaviour
{

    public Vector3 direction;

    // Update is called once per frame
    void Update()
    {
        // change the position of the target
        transform.localPosition += direction * Time.deltaTime;
    }
}
