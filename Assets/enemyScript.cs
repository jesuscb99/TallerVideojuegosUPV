using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * */
 
public class enemyScript : MonoBehaviour
{
    public EnemyState currentState;
    public Transform path;
    public float walkSpeed;
    public GameObject player;
    public bool playerDetected;

    private List<Vector3> pathPositions;
    private int currentPosition = 0;
    private int nextPosition;
    private float margen = 0.5f;
    private bool forwardMovement = true;

    // Start is called before the first frame update
    void Start()
    {
        currentState = EnemyState.Patrol;
        
        pathPositions = new List<Vector3>();

        for(int i = 0; i < path.childCount; i++)
        {
            pathPositions.Add(path.GetChild(i).position);
        }
        if (pathPositions.Count > 1) nextPosition = currentPosition + 1;
    }
    public enum EnemyState
    {
        Patrol,
        Chase
    };

   

    // Update is called once per frame
    void Update()
    {
      

        switch (currentState)
        {
            case EnemyState.Patrol:
                Patrol();
                break;
            case EnemyState.Chase:
                Chase();
                break;
            default:
                break;
        }
    }
    private void Patrol()
    {
        int side = transform.position.x < pathPositions[nextPosition].x ? 1 : -1;

        transform.Translate(new Vector3(side* walkSpeed * Time.deltaTime, 0, 0));
        UpdateCurrentPosition();

        if(playerDetected)
        {
            currentState = EnemyState.Chase;
        }
    }

    private void UpdateCurrentPosition()
    {
        if (Vector3.Distance(transform.position, pathPositions[nextPosition]) < margen )
        {
            CalculateNextPosition();
        }
    }
    private void CalculateNextPosition()
    {
        currentPosition = nextPosition;
        if (forwardMovement)
        {
            if (currentPosition == pathPositions.Count - 1)
            {
                forwardMovement = false;
                nextPosition--;
            }
            else nextPosition++;
        }
        else
        {
            if(currentPosition == 0)
            {
                forwardMovement = true;
                nextPosition++;
            }
        }
    }

    private void Chase()
    {
        int side = transform.position.x > player.transform.position.x ? -1 : 1;
        transform.Translate(new Vector3(walkSpeed * side * Time.deltaTime, 0, 0));

        if (!playerDetected)
        {
            currentState = EnemyState.Patrol;
        }
    }
}

