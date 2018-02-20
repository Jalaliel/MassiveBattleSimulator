using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class Agent : MonoBehaviour {

    protected NavMeshAgent agent;
    public Animator anim;
    private Move m; // ça sert pas à grand chose mais c'est obligatoire pour appeler la méthode

    public double portee;
    public int idAgent=0;
    public bool equipeA;
    protected Monde terrain=null;
    public bool moving;
    public int etat = 3; // 3 full life, 2 légèrement bléssé, 1 gravement blessé, 0 mort
    public bool enFuite;
 

    // Use this for initialization
    protected void StartA ()
    {
        ////Debug.Log("start");
        //agent = GetComponent<NavMeshAgent>();
        //// Don’t update position automatically
        //agent.updatePosition = false;
        //anim = GetComponent<Animator>();
        //m = new Move(agent, anim,this);
        //etat = 3;
    }
	
    // Méthode qui sert a récupérer les paramètres de chaque agent. Elle ne devrait être appelée que par le Monde
    public void Init(Monde monde, bool team,int id)
    {
        agent = GetComponent<NavMeshAgent>();
        // Don’t update position automatically
        agent.updatePosition = false;
        anim = GetComponent<Animator>();
        m = new Move(agent, anim, this);
        equipeA = team;
        terrain = monde;
        idAgent = id;
        etat = 3;
        enFuite = false;
    }

    // Set l'etat de l'agent
    public void SetEtat(int e)
    {
        this.etat = e;
    }

    public int GetEtat()
    {
        return this.etat;
    }


	// Set la destination du NavMeshAgent à position et appelle la méthode qui se charge de faire fonctionner le déplacemment et les animations en même temps.
	protected void LetsMove (Vector3 position) {
        
        agent.destination = position;
        moving = true;// A virer?
        m.LetsMove();
    }

    protected List<Agent> GetEnnemisPortee()
    {
        return (terrain.EnnemisADisance(this) );
    }

    // Sert a faire déplacer l'agent
    void OnAnimatorMove()
    {
        if (moving)
        // Update position to agent position
            transform.position = agent.nextPosition;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            if (Random.Range(1, 100) > 75)
                etat--;
    }
}
