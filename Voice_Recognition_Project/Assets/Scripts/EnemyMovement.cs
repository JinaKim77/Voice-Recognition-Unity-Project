using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] float moveSpeed = 2.0f;
    public GameObject oneUp;

    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(IsFacingRight())
        {
            rb.velocity = new Vector2(moveSpeed,0f);
        }
        else 
        {
            rb.velocity = new Vector2(-moveSpeed,0f);
        }
    }

    private bool IsFacingRight()
    {
        return transform.localScale.x > 0;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {

        transform.localScale = new Vector2(-(Mathf.Sign(rb.velocity.x)),1f);

        //Debug.Log("Hit Something");

        if(collision.tag ==  "Player")
        {
            Debug.Log("It was the player");
        }


        if(collision.tag == "Bullet")
        {
            Debug.Log("Got shot");

            Instantiate(oneUp,
                new Vector3 (transform.position.x,
                             transform.position.y + 0.7f,
                             transform.position.z),
                             Quaternion.identity);
            
            Destroy(gameObject); //This destroy things            
        }
    }

}

