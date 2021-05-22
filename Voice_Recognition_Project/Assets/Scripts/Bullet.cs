using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class Bullet : MonoBehaviour
{
    // used to identify a bullet
    // Destroy the bullet whenever it hits something to reduce the memory
    void OnCollisionEnter2D(Collision2D col)
    {
        Destroy(gameObject);
    }
}
