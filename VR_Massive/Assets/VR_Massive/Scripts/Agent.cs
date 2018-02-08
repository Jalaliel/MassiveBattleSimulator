using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class Agent : MonoBehaviour {

    NavMeshAgent agent;
    private bool moving=false;
    private Vector2 smoothDeltaPosition = Vector2.zero;
    private Vector2 velocity = Vector2.zero;
    Animator anim;

    float portee;
    public int idAgent=0;
    public bool equipe=false;
    private Monde terrain=null;

    // Use this for initialization
    void Start ()
    {
        agent = GetComponent<NavMeshAgent>();
        agent = GetComponent<NavMeshAgent>();
        // Don’t update position automatically
        agent.updatePosition = false;
        anim = GetComponent<Animator>();
    }
	
    void init(Monde m, bool team,int id)
    {
        equipe = team;
        terrain = m;
        idAgent = id;
    }


	// Update is called once per frame
	void Update () {
        if (moving){
            Vector3 worldDeltaPosition = agent.nextPosition - transform.position;

            // Map 'worldDeltaPosition' to local space
            float dx = Vector3.Dot(transform.right, worldDeltaPosition);
            float dy = Vector3.Dot(transform.forward, worldDeltaPosition);
            Vector2 deltaPosition = new Vector2(dx, dy);

            // Low-pass filter the deltaMove
            float smooth = Mathf.Min(1.0f, Time.deltaTime / 0.15f);
            smoothDeltaPosition = Vector2.Lerp(smoothDeltaPosition, deltaPosition, smooth);

            // Update velocity if time advances
            if (Time.deltaTime > 1e-5f)
                velocity = smoothDeltaPosition / Time.deltaTime;

            bool shouldMove = velocity.magnitude > 0.5f && agent.remainingDistance > agent.radius;

            // Update animation parameters
            anim.SetBool("Moving", shouldMove);
            anim.SetFloat("velx", velocity.x);
            anim.SetFloat("vely", velocity.y);
        }
        else
        {
            priseDecision();
        }

    }

    void priseDecision()
    {
        

    }

    bool Move(Vector3 position)
    {
        moving = true;
        agent.destination = position;
        return true;
    }
}
