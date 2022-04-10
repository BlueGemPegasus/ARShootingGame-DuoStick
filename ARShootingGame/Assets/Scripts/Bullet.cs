using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Respawn _respawn = collision.gameObject.GetComponent<Respawn>();
        if (_respawn != null)
        {
            
        }
        Destroy(this.gameObject);
    }
}
