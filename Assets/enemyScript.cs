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
    public Rigidbody2D rb;
    public float lives = 1f;

    private List<Vector3> pathPositions;
    private int currentPosition = 0;
    private int nextPosition;
    private float margen = 0.5f;
    private bool forwardMovement = true;

    [Header("Jumping")]
    public float jumpForce = 5f;
    public bool isGrounded = true;

    //A distancia
    

    [Header("Comportaments")]
    public bool jumping;
    public bool shooting;
    public GameObject bullet;
    public float spawnTime = 1f;

    [Header("RayCast")]
    private Vector3 rightOrigin;
    private Vector3 leftOrigin;
    public float width;
    public float heigth;
    LayerMask Ground;
    public float RayLength = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        currentState = EnemyState.Patrol;
        rb = transform.GetComponent<Rigidbody2D>();
        pathPositions = new List<Vector3>();

        for (int i = 0; i < path.childCount; i++)
        {
            pathPositions.Add(path.GetChild(i).position);
        }
        if (pathPositions.Count > 1) nextPosition = currentPosition + 1;

        Ground = 1 << LayerMask.NameToLayer("Ground");

        if (lives >= 1 && shooting)
        {
            InvokeRepeating("Shoot", spawnTime, spawnTime);
        }
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
        if(lives >= 1 &&  jumping)
        {
            Jump();
        }
        IsGrounded();

        
    }
    private void Patrol()
    {
        int side = transform.position.x < pathPositions[nextPosition].x ? 1 : -1;

        transform.Translate(new Vector3(side * walkSpeed * Time.deltaTime, 0, 0));
        UpdateCurrentPosition();

        if (playerDetected)
        {
            currentState = EnemyState.Chase;
        }
    }

    private void UpdateCurrentPosition()
    {
        if (Vector3.Distance(transform.position, pathPositions[nextPosition]) < margen)
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
            if (currentPosition == 0)
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

    void IsGrounded()
    {
        rightOrigin = transform.position + new Vector3(width, -heigth / 2, 0);
        leftOrigin = transform.position + new Vector3(-width, -heigth / 2, 0);

        RaycastHit2D rightRay = Physics2D.Raycast(rightOrigin, -Vector3.up, RayLength, Ground);
        RaycastHit2D leftRay = Physics2D.Raycast(leftOrigin, -Vector3.up, RayLength, Ground);

        Debug.DrawLine(rightOrigin, rightOrigin + -Vector3.up * RayLength, Color.red);
        Debug.DrawLine(leftOrigin, leftOrigin + -Vector3.up * RayLength, Color.red);


        isGrounded = rightRay.collider != null || leftRay.collider != null;
        
    }

    private void Jump()
    {
        if (isGrounded)

        {
            
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isGrounded = false;
        }
    }
    private void Shoot()
    {
        
            Instantiate(bullet, transform.position, transform.rotation);
            
        
    }
}

