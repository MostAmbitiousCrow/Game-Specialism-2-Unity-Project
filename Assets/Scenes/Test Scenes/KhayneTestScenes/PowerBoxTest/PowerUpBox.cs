using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpBox : MonoBehaviour // Made by Khayne. This script is for the power up box that the player can destroy to get a power up.
{
    [SerializeField] private GameObject prefab;
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerProjectile"))
        {
            BreakOpen();
        }
    }

    [SerializeField] void BreakOpen()
    {
        Instantiate(prefab, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
