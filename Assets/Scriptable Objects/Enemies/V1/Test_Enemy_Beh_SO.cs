using UnityEngine;

[CreateAssetMenu(fileName = "Test Imp Behaviour", menuName = "ScriptableObjects/Enemies/Test/BasicBehaviour", order = 0)]
public class Test_Enemy_Beh_SO : ScriptableObject
{
    public float attackTime = .75f;

    public float verticalSpeed = 1;
    public float verticalDistance = 1;
    public float horizontalSpeed = 1;
    public float horizontalDistance = 1;

    public float transitionTime = 1;
}