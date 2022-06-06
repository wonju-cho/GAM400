using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public GameObject impactEffect;
    public float radius = 1;
    public int damageAmount = 15;

    private void OnCollisionEnter(Collision collision)
    {
        //add audio effect
        //FindObjectOfType<AudioManager>().Play("");
        GameObject impact = Instantiate(impactEffect, transform.position, Quaternion.identity);
        Destroy(impact, 2);

        if(collision.collider.tag == "Player")
        {
            Debug.Log("collision detected: player");
        }
        else
        {
            Debug.Log("collision detected: wall");
        }
        //Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

        //foreach(Collider nearbyObject in colliders)
        //{
            
        //    if (nearbyObject.tag == "Player")
        //    {
        //        //playermanager.takedamage();
        //    }
        //}
        Destroy(gameObject);
    }
}