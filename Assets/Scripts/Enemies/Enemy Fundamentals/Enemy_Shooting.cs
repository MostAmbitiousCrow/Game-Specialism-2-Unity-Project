using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Shooting : MonoBehaviour
{
    //========================================
    // The Shooting/Attacking aspect of the Enemy.
    // Attack Process is triggered by the StartAttacking function, of which is triggered by the completed movement function
    //========================================

    public Enemy_Character_Data data;
    public SO_Standard_Enemy_Attack attackData;
    private Coroutine coroutine;

    // Start is called before the first frame update
    public void StartAttacking()
    {
        if(coroutine == null)
        Debug.Log($"{name} Started Attacking");
        coroutine = StartCoroutine(AttackProcess());  // TODO needs an established attack time.
    }

    public void StopAttacking()
    {
        if (coroutine != null) StopCoroutine(coroutine);
    }

    // Update is called once per frame
    IEnumerator AttackProcess()
    {
        while (true)
        {


            yield return null;
        }
        yield break;
    }
}
