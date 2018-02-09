using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class Agent : MonoBehaviour {

    private NavMeshAgent agent;
    public Animator anim;

    public float portee;
    protected int idAgent=0;
    public bool equipeA;
    private Monde terrain=null;
    private bool moving;
    private Move m; // ça sert pas à grand chose mais c'est obligatoire pour appeler la méthode
 

    // Use this for initialization
    protected void StartA ()
    {
        Debug.Log("start");
        agent = GetComponent<NavMeshAgent>();
        // Don’t update position automatically
        agent.updatePosition = false;
        anim = GetComponent<Animator>();
        m = new Move(agent, anim,this);
    }
	
    void Init(Monde monde, bool team,int id)
    {
        equipe = team;
        terrain = monde;
        idAgent = id;
    }


	// Set la destination du NavMeshAgent à position et appelle la méthode qui se charge de faire fonctionner le déplacemment et les animations en même temps.
	protected void LetsMove (Vector3 position) {
        agent.destination = position;
        moving = true;// A virer?
        m.LetsMove();
    }

    // Sert a faire déplacer l'agent
    void OnAnimatorMove()
    {
        // Update position to agent position
        transform.position = agent.nextPosition;
    }


}
