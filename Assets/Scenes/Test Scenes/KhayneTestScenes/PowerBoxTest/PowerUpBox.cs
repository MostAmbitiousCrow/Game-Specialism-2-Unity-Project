using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpBox : MonoBehaviour // Made by Khayne Lutchmun.
{
    [SerializeField] private List<GameObject> PowerUps; // This is a list of power-ups that can be dropped from the box. The power-ups must be assigned in the inspector.
    [SerializeField] private GameObject SetPowerUp; // This is the power-up that will be assigned to the box. If it is not assigned, then a random power-up will be chosen from the list.
    [SerializeField] private bool Assigned = false; // This is a boolean that determines if the box has a specific power-up assigned to it. If it is true, then the box will drop the assigned power-up.
    [SerializeField] private bool EmptyBox = false; // This is a boolean that determines if the box is empty or not. If it is true, then the box is empty and will not drop anything.

    public void BreakOpen() 
    {
        Destroy(gameObject);
        if (Assigned) // If the box has a specific power-up assigned to it, then drop that power-up.
        {
            Instantiate(SetPowerUp, transform.position, Quaternion.identity); // Instantiate the assigned power-up at the box's position.
        }
        else if(!Assigned) // If the box does not have a specific power-up assigned to it, then drop a random power-up from the list.
        {
            int RandomPowerUp = Random.Range(0, PowerUps.Count); // Get a random index from the PowerUps list.
            Instantiate(PowerUps[RandomPowerUp], transform.position, Quaternion.identity); // Instantiate a random power-up from the list at the box's position.
        }
        else if (EmptyBox) // If the box is empty, then do nothing.
        {
            Debug.Log("Empty Box"); // This box is empty, so nothing happens.
        }
    }
    
}
