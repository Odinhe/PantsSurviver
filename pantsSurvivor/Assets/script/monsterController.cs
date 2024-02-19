using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.AI;

public class monsterController : MonoBehaviour
{
    [SerializeField] private Transform movePositionTransform;
    //set the nav mesh system
    public NavMeshAgent agent;
    //set the animator
    [SerializeField] Animator anim;
    Transform mainCamera;
    //the monster will not be able to get hit 2 second after get hit
    float restTime = 2f;
    //the knowck back force that determoins how far player move after get hit
    public float knockForce = 4f;
    //the time that player will stay in the save position after get hit
    public float knockDuration = 1f;
    //set the rigidbody
    private Rigidbody monsters;
    private CapsuleCollider Collider;
    //the monster have 2 hp
    public int monsterHp = 2;
    //the monster will not be able to get hit 2 second after get hit 
    private float getTime = 2f;
    private float getTimer = 0f;
    private bool getAttack = false;
    // Start is called before the first frame update
    void Start()
    {
        //get the main camera
        mainCamera = Camera.main.transform;
        //get the necamesh agent
        agent = GetComponent<NavMeshAgent>();
        //set the speed of the monster
        agent.speed = 8;
        //get rigidbody
        monsters = GetComponent<Rigidbody>();
        //get collider
        Collider = GetComponent<CapsuleCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        //how the monster will move tword the player
        agent.destination = movePositionTransform.position;
        //when monster is get hit and stop, it will stop for 2 second before it restart to move
        if(agent.isStopped == true)
        {
            restTime -= Time.deltaTime;
        }
        if(restTime <= 0)
        {
            agent.isStopped = false;
            restTime = 2f;
        }
        //change different animation when monster get hit
        if(monsterHp == 1)
        {
            anim.SetInteger("damage", 1);
        }
        //when there where no hp left, monster will move to death and will not be trigged
        if(monsterHp <= 0)
        {
            anim.SetInteger("damage", 2);
            agent.isStopped = true;
            Collider.isTrigger = true;
        }

        //if monster get hit, it will have 2 second of time to re set before get hit again
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

    }
    private void OnCollisionEnter(Collision collision)
    {
        //what will happen when monster get hit
            if (collision.gameObject.CompareTag("Player"))
            {
                monsterHp -= 1;
                // get where the monster was collided with player and where should it get knock back
                Vector3 knockDirection = transform.position - collision.contacts[0].point;
                knockDirection.Normalize();
                
                // add the knock back force to the monster
                monsters.AddForce(knockDirection * knockForce, ForceMode.Impulse);

                // stop the currently running knock back and start a new rountine
                StopAllCoroutines();
                StartCoroutine(KnockbackRoutine());
                //stop moveing when get hit
                agent.isStopped = true;
                getAttack = true;
                //will not collide with the player
                Collider.isTrigger = true;
                getTimer = 0f;
                //monster will be faster
                agent.speed++;
            }
    }
    IEnumerator KnockbackRoutine()
    {
        // wait for the duration of monster get knock back to process
        yield return new WaitForSeconds(knockDuration);

        // stop the know back
        monsters.velocity = Vector3.zero;
    }
}
