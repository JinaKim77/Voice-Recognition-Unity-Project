using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Points : MonoBehaviour
{

    public GameObject oneUp;
    [SerializeField] int coinValue = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            Debug.Log("Collected pickup item");
            FindObjectOfType<GameSession>().addScore(coinValue);

          
            Instantiate(oneUp,
                new Vector3 (transform.position.x,
                             transform.position.y + 0.7f,
                             transform.position.z),
                             Quaternion.identity);
            
            Destroy(gameObject); //This destroy things            
        }
    }
}
