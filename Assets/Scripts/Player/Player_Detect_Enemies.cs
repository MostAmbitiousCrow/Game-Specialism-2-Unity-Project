using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Detect_Enemies : MonoBehaviour
{
    [SerializeField] float detectRadius = 5;

    [SerializeField] bool enableDebug = true;

    void FixedUpdate()
    {
        
    }


    void OnDrawGizmosSelected()
    {
        if (enableDebug)
        {
            
        }
    }
}
