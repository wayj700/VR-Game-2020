using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBarrel : MonoBehaviour
{
    [SerializeField] private float health = 100.0f;

    public float minimumDamage = 5.0f;
    public float impactDamageMultiplier = 0.25f;
    public float blastRadius = 10.0f;
    public float blastForce = 700.0f;

    public GameObject explosionEffect;
    public GameObject nonExplodedBarrel;
    public GameObject explodedBarrel;

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            explode();
        }
    }

    private void explode()
    {
        Instantiate(explosionEffect, transform.position, transform.rotation);

        Destroy(nonExplodedBarrel);

        Instantiate(explodedBarrel, transform.position, transform.rotation);

        Collider[] colliders = Physics.OverlapSphere(transform.position, blastRadius);

        foreach (Collider nearbyObject in colliders)
        {
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(blastForce, transform.position, blastRadius);
            }
        }

        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.relativeVelocity.magnitude >= minimumDamage)
        {
            health -= collision.relativeVelocity.magnitude / impactDamageMultiplier;
        }
    }
}