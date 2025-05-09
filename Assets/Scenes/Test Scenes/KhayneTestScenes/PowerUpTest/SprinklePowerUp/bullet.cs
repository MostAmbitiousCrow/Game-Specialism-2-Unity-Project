using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SprinklesBullet : MonoBehaviour // Made by Khayne Lutchmun.
{
    [Header("Bullet Data")]
    [SerializeField] float speed = 40f;
    [SerializeField] float lifetime = 4f;
    [SerializeField] int damage = 1;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
        private void OnTriggerEnter(Collider c)
    {
        if (c.CompareTag("EnemyB")) c.GetComponent<Enemy_Character_Data>().Damage(damage); // Damage the target
        else if (c.CompareTag("Box")) c.GetComponent<Character_Health_Script>().Damage(damage); // Damage the target
        Deactivate();
    }
    public void Deactivate() // Destroy the bullet
    {
            Destroy(gameObject);
    }
}
