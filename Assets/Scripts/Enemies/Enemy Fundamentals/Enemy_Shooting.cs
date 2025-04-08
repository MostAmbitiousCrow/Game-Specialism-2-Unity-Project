using System.Collections;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;

public class Enemy_Shooting : MonoBehaviour // By Samuel White
{
    //========================================
    // The Shooting/Attacking aspect of the Enemy.
    // Attack Process is triggered by the StartAttacking function, of which is triggered by the completed movement function
    //========================================

    public Enemy_Character_Data data;
    public SO_Standard_Enemy_Attack attackData;
    public SO_Proj_Eni_Bas projectileData;
    private Coroutine coroutine;
    [SerializeField] UnityEvent shootEvent;

    // Start is called before the first frame update
    public void StartAttacking()
    {
        if(coroutine == null)
        Debug.Log($"{name} Started Attacking");
        coroutine = StartCoroutine(AttackProcess());
    }

    public void StopAttacking()
    {
        if (coroutine != null) StopCoroutine(coroutine);
    }

    // Update is called once per frame
    IEnumerator AttackProcess()
    {
        yield return new WaitForSeconds(attackData.initialDelay);
        while (true)
        {

            ShootFunction();
            yield return new WaitForSeconds(attackData.fireInterval);
        }
    }

    private void ShootFunction()
    {
        Transform player = GameManager.instance.player;

        (GameObject GO, FieldInfo SO) = Bullet_Pool_System.instance.GetBullet(1); // Get Enemy Bullet
        if (GO != null && SO != null)
        {
            //Vector3 dir = player.position;
            Quaternion rot = Quaternion.LookRotation(player.position);
            GO.transform.SetPositionAndRotation(transform.position, rot);
            SO.SetValue(GO.GetComponent(SO.DeclaringType), projectileData); //TODO Might need to update this!
            GO.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Failed to get bullet from pool.");
        }
    }
}
