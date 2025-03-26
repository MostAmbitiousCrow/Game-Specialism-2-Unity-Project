using UnityEngine;

public class Environment_Camera_Move : MonoBehaviour
{
    [SerializeField] Transform environment;
    [SerializeField] private Vector3 startPosition, endPosition;
    private float t = 0;
    [SerializeField] float travelTime;
    [SerializeField] bool pause;
    // Update is called once per frame
    void Update()
    {
        if (pause) return; 
        t += Time.deltaTime / travelTime;
        environment.position = Vector3.Lerp(startPosition, endPosition, t);

        if (t > 1) t = 0;
    }
}
