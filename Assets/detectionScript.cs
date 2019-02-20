using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class detectionScript : MonoBehaviour
{
    
    private enemyScript enemy;

    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponentInParent<enemyScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            enemy.playerDetected = true;
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            
            enemy.playerDetected = true;
        }
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            enemy.playerDetected = false;
        }
        
       
    }

}
