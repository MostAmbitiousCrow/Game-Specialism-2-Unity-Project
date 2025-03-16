using UnityEngine;

[CreateAssetMenu(fileName = "Standard Enemy Movement Type", menuName = "ScriptableObjects/Enemies/Movement/Standard Types", order = 0)]
public class SO_Standard_Enemy_Movement : ScriptableObject
{
    public enum MovementType { Static, Hover, Circle }

    [Header("Movement Type")]
    public MovementType movementType;

    [Header("Base")]
    [Tooltip("The speed at which the enemy will move.")]
    public float speed = 1;

    [Header("Hover")]
    public float speedHeightMultiplier = 1;
    public float speedWidthMultiplier = 1;
    public float hoverHeightMin = 1, hoverHeightMax = 1;
    public float hoverWidthMin = 1, hoverWidthMax = 1;

    [Tooltip("The animation for the vertical hover.")]
    public AnimationCurve horizontalHoverCurve = new();
    [Tooltip("The animation for the vertical hover.")]
    public AnimationCurve verticalHoverCurve = new();
    
    [Header("Circle")]
    [Tooltip("The radius of the circle the enemy will circle.")]
    public float circleRadius = 1;
    public bool clockwise = true;

    [Header("Cycles")]
    [Tooltip("The amount of cycles the enemy will perform before leaving. Leave at 0 for infinite cycles.")]
    public int cycles = 0;

}