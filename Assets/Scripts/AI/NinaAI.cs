using System.Collections;
using Unity.VisualScripting;
using Photon.Pun;
using UnityEngine;
using Photon.Realtime;
using UnityEngine.AI;

public class NinaAI : MonoBehaviour
{
    public GameObject[] players;
    private GameObject closerPlayer;
    [SerializeField]
    private AudioSource Laugh;

    private bool isSantiActive = false;
    private bool isJoseActive = false;
    private bool jumpscareActive = false;

    public UnityEngine.AI.NavMeshAgent aiAgent;               //  Nav mesh agent component
    static float startWaitTime = 4;                 //  Wait time of every action
    public float timeToRotate = 1;                  //  Wait time when the enemy detect near the player without seeing
    public float walkSpeed = 3;                     //  Walking speed, speed in the nav mesh agent
    public float chaseSpeed = 3;                      //  Running speed

    public float viewRadius = 15;                   //  Radius of the enemy view
    public float viewAngle = 90;                    //  Angle of the enemy view
    public LayerMask playerMask;                    //  To detect the player with the raycast
    public LayerMask obstacleMask;                  //  To detect the obstacules with the raycast
    public float meshResolution = 1.0f;             //  How many rays will cast per degree
    public int edgeIterations = 4;                  //  Number of iterations to get a better performance of the mesh filter when the raycast hit an obstacule
    public float edgeDistance = 0.5f;               //  Max distance to calcule the a minumun and a maximum raycast when hits something
    private int JoseIteration = 1;


    public Transform[] waypoints;
    public Transform[] spawns;
    public Transform spawn;                         //  All the waypoints where the enemy patrols
    int CurrentWaypointIndex;                       //  Current waypoint where the enemy is going to

    Vector3 playerLastPosition = Vector3.zero;      //  Last position of the player when was near the enemy
    Vector3 PlayerPosition;                         //  Last position of the player when the player is seen by the enemy

    public float WaitTime;                          //  Variable of the wait time that makes the delay
    float minWaitTime;
    float maxWiatTime;
    float TimeToRotate;                             //  Variable of the wait time to rotate when the player is near that makes the delay
    public bool playerInRange;                      //  If the player is in range of vision, state of chasing
    //public bool isPlayerNear;                     //  If the player is near, state of hearing
    public bool isPatrol;                           //  If the enemy is patrol, state of patroling
    public bool isPlayerCaught;                     //  if the enemy has caught the player
    public bool isChasing;
    public bool attackingJose;
    public bool attackingSanti;
    public bool isSeen;
    public bool isWarping = false;

    private PhotonView JosePV;
    private PhotonView SantiPV;

    public float catchDistance;

    public float seenCooldownTimer;
    public float stoppedTimer;
    public float defaultCooldownTime = 5f;

    private Animator ninaAnimator;
    private PhotonView PV;

    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
        players = new GameObject[2];

        PlayerPosition = Vector3.zero;
        isPatrol = true;
        isPlayerCaught = false;


        //Testing
        isChasing = false;
        attackingJose = true;
        attackingSanti = false;
        //aiAgent.destination = waypoints[CurrentWaypointIndex].position;
        //randNum = 0;
        minWaitTime = 1f;
        maxWiatTime = 3f;
        catchDistance = 1.5f;

        stoppedTimer = defaultCooldownTime;
        seenCooldownTimer = defaultCooldownTime;
        //Testing

        WaitTime = startWaitTime;                 //  Set the wait time variable that will change
        TimeToRotate = timeToRotate;

        CurrentWaypointIndex = 0;                 //  Set the initial waypoint
        aiAgent = GetComponent<NavMeshAgent>();

        aiAgent.isStopped = false;
        aiAgent.speed = walkSpeed;             //  Set the navemesh speed with the normal speed of the enemy
        aiAgent.SetDestination(waypoints[CurrentWaypointIndex].position);    //  Set the destination to the first waypoint

        ninaAnimator = GetComponent<Animator>();
    }

    [PunRPC]
    public void SyncInRange(bool InRange)
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isJoseActive && isSantiActive)
        {
            EnviromentView();                       //  Check whether or not the player is in the enemy's field of vision

            closerPlayer = GetCloserPlayer(); //relevant player

            if (attackingJose)
            {
                ChaseJose();
                walkSpeed = 0.9f;
            }
            else if (attackingSanti)
            {
                walkSpeed = 3f;
                if (isChasing && !isPlayerCaught)
                {
                    //aiAnimation.ResetTrigger("walk");
                    //aiAnimation.ResetTrigger("idle");
                    //aiAnimation.SetTrigger("sprint");
                    Chasing();
                    Debug.Log("TOPO1");
                }
                else if (isPatrol && !isPlayerCaught)
                {
                    //aiAnimation.ResetTrigger("sprint");
                    //aiAnimation.ResetTrigger("idle");
                    //aiAnimation.SetTrigger("walk");
                    Patroling();
                    Debug.Log("TOPO2");
                }
                else if (isPlayerCaught)
                {
                    Debug.Log("Attacking");
                    Attacking();
                    Debug.Log("TOPO3");
                }
            }
        }
    }

    private void Chasing()
    {
        PV.RPC("UpdateAttackingAnimation", RpcTarget.All);
        //  The enemy is chasing the player
        //  Set false that hte player is near beacause the enemy already sees the player
        playerLastPosition = Vector3.zero;          //  Reset the player near position

        if (!isPlayerCaught)
        {
            Move(chaseSpeed);
            aiAgent.SetDestination(PlayerPosition);          //  set the destination of the enemy to the player location
        }
        if (aiAgent.remainingDistance <= aiAgent.stoppingDistance)    //  Control if the enemy arrive to the player location
        {
            if (WaitTime <= 0 && !isPlayerCaught && Vector3.Distance(transform.position, closerPlayer.transform.position) >= 6f)
            {
                //  Check if the enemy is not near to the player, returns to patrol after the wait time delay
                isPatrol = true;
                isChasing = false;
                Move(walkSpeed);
                TimeToRotate = timeToRotate;
                WaitTime = startWaitTime;
                aiAgent.SetDestination(waypoints[CurrentWaypointIndex].position);
            }
            else
            {
                WaitTime -= Time.deltaTime;
            }
            if (Vector3.Distance(transform.position, closerPlayer.transform.position) < catchDistance && !players[0].GetComponent<Down>().isPlayerDowned)
            {
                CaughtPlayer();
            }
        }
    }

    public void SantiActivation()
    {
        players[0] = GameObject.FindGameObjectWithTag("PlayerSanti");
        isSantiActive = true;
        SantiPV = players[0].GetComponent<PhotonView>();
    }

    public void JoseActivation()
    {
        players[1] = GameObject.FindGameObjectWithTag("PlayerJose");
        isJoseActive = true;
        JosePV = players[1].GetComponent<PhotonView>();
    }

    private void Patroling()
    {
        playerLastPosition = Vector3.zero;
        Move(walkSpeed);
        aiAgent.SetDestination(waypoints[CurrentWaypointIndex].position);    //  Set the enemy destination to the next waypoint
        if (aiAgent.remainingDistance <= aiAgent.stoppingDistance)
        {
            //  If the enemy arrives to the waypoint position then wait for a moment and go to the next
            if (WaitTime <= 0)
            {
                PV.RPC("UpdateMoveAnimation", RpcTarget.All,true);
                NextPoint();
                Move(walkSpeed);
                WaitTime = Random.Range(minWaitTime, maxWiatTime);
            }
            else
            {
                PV.RPC("UpdateMoveAnimation", RpcTarget.All, false);
                //aiAnimation.ResetTrigger("sprint");
                //aiAnimation.ResetTrigger("walk");
                //aiAnimation.SetTrigger("idle");
                WaitTime -= Time.deltaTime;
            }
        }
    }

    private void Attacking()
    {
        if (closerPlayer == players[0])
        {
            isPlayerCaught = false;
            SantiPV.RPC("SyncDowned", RpcTarget.All);
            
            
        }
        else if (closerPlayer == players[1])
        {
            isPlayerCaught = false;
            JosePV.RPC("SyncDowned", RpcTarget.All);

        }

        isPatrol = true;

        Patroling();
    }

    private void OnAnimatorMove()
    {

    }
    [PunRPC]
    public void InvertPlayers()
    {
        if (isWarping)
        {
            return; // If warping is already in progress, exit the method
        }

        if(JoseIteration < 5)
        {
            Laugh.Play();
            attackingJose = !attackingJose;
            attackingSanti = !attackingSanti;

            if (attackingJose)
            {
                StartCoroutine(WarpWithDelay(spawns[JoseIteration].localPosition, 0.5f));
                JoseIteration++;
            }
            else if (attackingSanti)
            {
                StartCoroutine(WarpWithDelay(spawn.position, 9.0f));
            }
        }
        else
        {
            this.gameObject.SetActive(false);
        }
    }

    private IEnumerator WarpWithDelay(Vector3 position, float delay)
    {
        isWarping = true; // Mark warping in progress
        yield return new WaitForSeconds(delay); // Wait for the specified delay

        // Perform the warp after the delay
        this.GetComponent<NavMeshAgent>().Warp(position);

        isWarping = false; // Reset the flag when warping is done
    }

    public void NextPoint()
    {
        CurrentWaypointIndex = Random.Range(0, waypoints.Length);
        aiAgent.SetDestination(waypoints[CurrentWaypointIndex].position);
    }

    public void ChaseJose()
    {
        PV.RPC("UpdateAttackingAnimation", RpcTarget.All);
        aiAgent.SetDestination(players[1].transform.position);
        aiAgent.speed = 0.8f;
        if (Vector3.Distance(transform.position, players[1].transform.position) < catchDistance && !jumpscareActive)
        {
            //JosePV.RPC("SyncDowned", RpcTarget.All);
            //SantiPV.RPC("SyncDowned", RpcTarget.All);
            isPlayerCaught = false;
            
            StartJoseJumpscare();
        }
    }

    void Move(float speed)
    {
        aiAgent.isStopped = false;
        aiAgent.speed = speed;
    }

    void CaughtPlayer()
    {
        isPlayerCaught = true;
        if (closerPlayer == players[0])
        {
            //SantiPV.RPC("SyncDowned", RpcTarget.All);
            isPlayerCaught = false;

            StartSantiJumpscare();
        }
        else if (closerPlayer == players[1])
        {
            JosePV.RPC("SyncDowned", RpcTarget.All);
            isPlayerCaught = false;

            //GameManager.Instance.NinaJumpscare.Play();
            //JosePV.RPC("UpdateNinaJumpScareAnimationJose", RpcTarget.All);
            //ninaAnimator.SetBool("IsMoving", false);
            //ninaAnimator.SetTrigger("IsAttacking");
            //StartCoroutine(EndJoseJumpscare());
        }
        Patroling();
    }

    void LookingPlayer(Vector3 player)
    {
        aiAgent.SetDestination(player);
        if (Vector3.Distance(transform.position, player) <= 0.3)
        {
            if (WaitTime <= 0)
            {

                Move(walkSpeed);
                aiAgent.SetDestination(waypoints[CurrentWaypointIndex].position);
                WaitTime = startWaitTime;
                TimeToRotate = timeToRotate;
            }
            else
            {
                WaitTime -= Time.deltaTime;
            }
        }
    }

    void EnviromentView()
    {
        Collider[] playerInRange = Physics.OverlapSphere(transform.position, viewRadius, playerMask);   //  Make an overlap sphere around the enemy to detect the playermask in the view radius

        for (int i = 0; i < playerInRange.Length; i++)
        {
            Transform player = playerInRange[i].transform;
            if (player.GetComponent<Injection>().isPlayerInjected || player.GetComponent<Down>().isPlayerDowned)
            {
                break;
            }
            Vector3 dirToPlayer = (player.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToPlayer) < viewAngle / 2)
            {
                float dstToPlayer = Vector3.Distance(transform.position, player.position);          //  Distance of the enmy and the player
                if (!Physics.Raycast(transform.position, dirToPlayer, dstToPlayer, obstacleMask) && !isPlayerCaught)
                {
                    this.playerInRange = true;             //  The player has been seen by the enemy and then the enemy chases the player
                    isChasing = true;                 //  Change the state to chasing the player
                    isPatrol = false;

                }
                else
                {
                    /*
                     *  If the player is behind a obstacle the player position will not be registered
                     * */
                    this.playerInRange = false;
                }
            }
            if (Vector3.Distance(transform.position, player.position) > viewRadius)
            {
                /*
                 *  If the player is further than the view radius, then the enemy will no longer keep the player's current position.
                 *  Or the enemy is a safe zone, the enemy will no chase
                 * */
                this.playerInRange = false;                //  Change the sate of chasing
            }
            if (this.playerInRange)
            {
                /*
                 *  If the enemy no longer sees the player, then the enemy will go to the last position that has been registered
                 * */
                PlayerPosition = player.transform.position;       //  Save the player's current position if the player is in range of vision
            }
        }
    }

    private GameObject GetCloserPlayer()
    {
        GameObject close;
        float distanceToSanti;
        float distanceToJose;
        if (players[0] != null && players[1] != null)
        {
            distanceToSanti = Vector3.Distance(transform.position, players[0].transform.position);
            distanceToJose = Vector3.Distance(transform.position, players[1].transform.position);
            close = (distanceToSanti < distanceToJose) ? players[0] : players[1];
        }
        else if (players[1] != null)
        {
            close = players[1];
        }
        else if (players[0] != null)
        {
            close = players[0];
        }
        else
        {
            close = null;
        }

        return close;
    }

    void StartSantiJumpscare()
    {
        GameManager.Instance.NinaJumpscare.Play();
        SantiPV.RPC("UpdateNinaJumpScareAnimationSanti", RpcTarget.All);
        ninaAnimator.SetBool("IsMoving", false);
        ninaAnimator.SetTrigger("IsAttacking");
        StartCoroutine(EndSantiJumpscare());
    }

    void StartJoseJumpscare()
    {
        jumpscareActive = true;
        GameManager.Instance.NinaJumpscare.Play();
        JosePV.RPC("UpdateNinaJumpScareAnimationJose", RpcTarget.All);
        ninaAnimator.SetBool("IsMoving", false);
        ninaAnimator.SetTrigger("IsAttacking");
        StartCoroutine(EndJoseJumpscare());
    }
    IEnumerator EndSantiJumpscare()
    {

        yield return new WaitForSeconds(3.3f);
        SantiPV.RPC("SyncDowned", RpcTarget.All);
        ninaAnimator.ResetTrigger("IsAttacking");
        ninaAnimator.SetBool("IsMoving", true);

        //StartJoseJumpscare();
        //SantiPV.RPC("ResetNinaJumpScareAnimationSanti", RpcTarget.All);
        if (SantiPV.IsMine)
        {
            SantiPV.RPC("ResetNinaJumpScareAnimationSanti", RpcTarget.All);
        }
        if (attackingSanti)
        {
            StartJoseJumpscare();
        }
    }

    IEnumerator EndJoseJumpscare()
    {

        yield return new WaitForSeconds(3.3f);
        JosePV.RPC("SyncDowned", RpcTarget.All);
        ninaAnimator.ResetTrigger("IsAttacking");
        ninaAnimator.SetBool("IsMoving", true);
        //jumpscareActive = false;
        
        
        Debug.Log("Jump a ver");
        if (JosePV.IsMine)
        {
            JosePV.RPC("ResetNinaJumpScareAnimationJose", RpcTarget.All);    
        }
        if (attackingJose)
        {
            StartSantiJumpscare();
        }
    }

    [PunRPC]
    void UpdateMoveAnimation(bool isMoving)
    {
        if (ninaAnimator != null)
        {
            ninaAnimator.SetBool("IsMoving", isMoving);
            ninaAnimator.ResetTrigger("IsAttacking");
        }
    }

    [PunRPC]
    void UpdateAttackingAnimation()
    {
        if (ninaAnimator != null)
        {
        ninaAnimator.SetTrigger("IsAttacking");
        }
    }
}
