using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public PlayerController owner;

    public float BulletSpeed = 20f;
    public Rigidbody rb;

    private void Start()
    {
        rb.velocity = transform.forward * BulletSpeed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Respawn _respawn = collision.gameObject.GetComponent<Respawn>();
        if (_respawn != null)
        {
            
        }
        Destroy(this.gameObject);
    }
}
