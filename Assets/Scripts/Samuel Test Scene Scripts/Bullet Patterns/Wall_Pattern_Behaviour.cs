using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall_Pattern_Behaviour : MonoBehaviour // By Samuel White //TODO
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private int length = 1;
    [SerializeField] private float spacing = .5f;
    [SerializeField] private float tiltPerBullet = 15;
    [SerializeField] private bool mirror = true;
    [SerializeField] private bool curve = true;

    [SerializeField] private List<Test_Enemy_Projectile> projectiles;

    public void Shoot()
    {
        if (mirror)
        {
            GameObject igo = Instantiate(prefab, transform.position, Quaternion.identity);
            Destroy(igo, 5);
            for (int i = 0; i < length; i++)
            {
                GameObject go = Instantiate(prefab, transform.position, Quaternion.identity);
                // projectiles.Add(go.GetComponent<Test_Enemy_Projectile>());
                if (curve)
                {
                    go.transform.Rotate(0, 0, i+1 * tiltPerBullet + i * tiltPerBullet);
                }
                else
                {
                    go.transform.position += new Vector3(0, i+1 * spacing, 0);
                    go.transform.Rotate(0, 0, i+1 * tiltPerBullet);
                }
                Destroy(go, 5);
            }
            for (int i = 0; i < length; i++)
            {
                GameObject go = Instantiate(prefab, transform.position, Quaternion.identity);
                // projectiles.Add(go.GetComponent<Test_Enemy_Projectile>());
                if (curve)
                {
                    go.transform.Rotate(0, 0, -i-1 * tiltPerBullet + -i * tiltPerBullet);
                }
                else
                {
                    go.transform.position += new Vector3(0, -i-1 * spacing, 0);
                    go.transform.Rotate(0, 0, -i-1 * tiltPerBullet); 
                }
                Destroy(go, 5);
            }
        }
        else
        {
            for (int i = 0; i < length; i++)
            {
                GameObject go = Instantiate(prefab, transform.position, Quaternion.identity);
                go.transform.position += new Vector3(0, i+1 * spacing, 0);
                go.transform.Rotate(0, 0, i * tiltPerBullet);
                // projectiles.Add(go.GetComponent<Test_Enemy_Projectile>());
                Destroy(go, 5);
            }
        }
    }
}
