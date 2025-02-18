using UnityEngine;

public class Environment_Camera_Move : MonoBehaviour
{
    [SerializeField] private Vector3 startPosition, endPosition;
    private float t = 0;
    [SerializeField] float travelTime;
    // Update is called once per frame
    void Update()
    {
        t += Time.deltaTime / travelTime;
        transform.position = Vector3.Lerp(startPosition, endPosition, t);

        if (t > 1)
        {
            t = 0;
        }
    }
}
