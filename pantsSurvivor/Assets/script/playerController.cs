using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class playerController : MonoBehaviour
{
    //player trans form position
    [SerializeField] private Transform movePositionTransform;
    //set up the move speed
    public float moveSpeed = 5f;
    //set up the navmesh
    public NavMeshAgent agent;
    
    private bool isAttacking = false;
    private bool getAttack = false;
    //player will attack for 3 second
    private float attackTime = 3f;
    //where the timer will start
    private float attackTimer = 0f;
    //player will get 2 second of time to repair after get hit
    private float getTime = 2f;
    private float getTimer = 0f;
    //player have 3 hp
    public int playerHP = 10;
    //get the game object
    public GameObject Player;
    //get the animator
    [SerializeField] Animator anim;
    public string PlayerAttack;
    //get the collider
    private CapsuleCollider Collider;
    //when player attack, the collider will be bigger
    public float newR = 2.0f;

    void Start()
    {
        //get the two component that will be used later
        agent = GetComponent<NavMeshAgent>();
        Collider = GetComponent<CapsuleCollider>();
    }
    void Update()
    {
        //get the data that was inputed in wasd controll
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 movementDirection = new Vector3(horizontalInput, 0f, verticalInput).normalized;
        //get player to move based on the input
        transform.Translate(movementDirection * moveSpeed * Time.deltaTime);

        //if space was pressed, player will start playing the attack animation, change the collider size and start 3 second of attacking
        if (Input.GetKeyDown(KeyCode.Space) && !isAttacking)
        {
            playerHP++;
            anim.SetBool("IsAttacking", true);
            Collider.radius = 2f;
            isAttacking = true;
            attackTimer = 0f;
        }
        //when is attacking, the timer will start to going down, if its not bigger than 3, will keep going tell its 3, then it will stop and reset till next time attack was called
        if (isAttacking)
        {
            attackTimer += Time.deltaTime;
            Debug.Log("attack");

            if (attackTimer >= attackTime)
            {
                anim.SetBool("IsAttacking", false);
                Collider.radius = 0.5f;
                isAttacking = false;
                attackTimer = 0f;
            }
        }
        
        //when player get attacked, they will have 2 second of time to reset, which by setting the is trigger to true, so it won't collide with monsters
        if (getAttack)
        {
            getTimer += Time.deltaTime;
            if (getTimer >= getTime)
            {
                getAttack = false;
                Collider.isTrigger = false;
                getTimer = 0f;
                Debug.Log("finished");
            }
        }

        //game will end when player's hp is lower than 0
        if(playerHP <= 0)
        {
            SceneManager.LoadScene(0);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        //when collide with monster, player will lose hp, and set the player get attacked system to triggerd
        if (collision.gameObject.CompareTag("Monster") && !getAttack)
        {
            playerHP -= 1;
            Collider.isTrigger = true;
            getAttack = true;
            getTimer = 0f; 
        }
    }
}
