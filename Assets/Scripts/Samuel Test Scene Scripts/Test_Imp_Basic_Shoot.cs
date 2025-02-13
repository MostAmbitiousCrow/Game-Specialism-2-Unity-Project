using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Test_Imp_Basic_Shoot : MonoBehaviour // By Samuel White
{
    [SerializeField] Test_Enemy_Shoot_Controller controller; // The controller that holds the projectile pool

    private Coroutine attackRoutine; // The coroutine that runs the attack

    [SerializeField] private int attackID; // The ID of the attack
    [SerializeField] Basic_Projectile_Scriptable_Object scriptableObject; // The scriptable object that holds the projectile prefab
    [SerializeField] Test_Imp_Atk_Beh_SO attackData; // The Imps attack data

    public void Begin() // Start the attack
    {
        attackRoutine = StartCoroutine(AttackRoutine());
    }

    private void OnDisable()
    {
        if (attackRoutine != null)
        StopCoroutine(attackRoutine);
    }

    IEnumerator AttackRoutine()
    {
        while (true)
        {
            Test_Enemy_Projectile projectile = controller.storedProjectiles[attackID].availableProjectiles[0];
            projectile.gameObject.SetActive(true);

            Vector2 direction = controller.player.position - transform.position; // Calculate the direction to the target
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // Calculate the angle
            Quaternion targetRotation = Quaternion.Euler(0, 0, angle); // Create the rotation

            projectile.transform.SetPositionAndRotation(transform.position, targetRotation); // Set the position and rotation of the projectile

            projectile.gameObject.SetActive(true); // Activate the projectile
            controller.storedProjectiles[attackID].activeProjectiles.Add(projectile); // Add the projectile to the active projectile list

            yield return new WaitForSeconds(attackData.attackTime);
        }
    }

    private void Awake()
    {
        GameObject prefab = scriptableObject.projectilePrefab;
        for (int j = 0; j < controller.storedProjectiles[attackID].poolingSpawnCount; j++) // Spawn the projectiles
        {
            controller.storedProjectiles[attackID].availableProjectiles.Add
             (Instantiate(prefab).GetComponent<Test_Enemy_Projectile>()); // Instantiate and add the projectile to the active list
        }
        // enabled = false; // Disable the script
    }

    void OnDrawGizmosSelected()
    {
        attackID = GetComponentIndex() - 2;
    }
}
