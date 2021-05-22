using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //[SerializeField] AudioClip carCrashSound;
    private bool isAlive;

    void Start()
    {
        isAlive=true;
    }

    void Update()
    {
        if( !isAlive)
        {
            return; //takes away player functionality
        }
    }


    private void OnTriggerEnter2D (Collider2D hit)
    {
        if(hit.gameObject.tag == "Enemy")
        {
            Debug.Log("You hit the enemy!");
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Player Dead");
        isAlive = false;

        //call the session manager
        FindObjectOfType<GameSession>().ProcessPlayerDeath();
    }
}
