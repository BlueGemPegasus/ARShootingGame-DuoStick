using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Components;

[RequireComponent(typeof(NetworkObject))]
[RequireComponent(typeof(NetworkTransform))]
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
            owner.score += 1;
            _respawn.Life -= 1;
            _respawn.Respawning();
            Destroy(this.gameObject);
        }
        if (collision.gameObject.CompareTag("Arena"))
        {
            Debug.Log("HIT ARENA!");
            Destroy(this.gameObject);
        }
    }
}
