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
        rb.velocity = transform.forward * BulletSpeed * 0.00001f;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Respawn _respawn = collision.gameObject.GetComponent<Respawn>();
        if (_respawn != null)
        {
            Debug.Log("HIT PLAYER!");
            Destroy(this.gameObject);
        }
        if (collision.gameObject.CompareTag("Arena"))
        {
            Debug.Log("HIT ARENA!");
            Destroy(this.gameObject);
        }
    }
}
