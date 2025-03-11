using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpBoxScuffedEdition : MonoBehaviour // Made by Khayne. This script is for the power up box that the player can destroy to get a power up.
{[SerializeField] private List<GameObject> PowerUps;[SerializeField] private GameObject SetPowerUp;[SerializeField] private bool Assigned = false;void BreakOpenBox(){Destroy(gameObject);if (Assigned){Instantiate(SetPowerUp, transform.position, Quaternion.identity);}else{int RandomPowerUp = Random.Range(0, PowerUps.Count);Instantiate(PowerUps[RandomPowerUp], transform.position, Quaternion.identity);}}}
