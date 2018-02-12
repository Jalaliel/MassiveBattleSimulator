using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class Agent : MonoBehaviour {

    private NavMeshAgent agent;
    public Animator anim;
    private Move m; // ça sert pas à grand chose mais c'est obligatoire pour appeler la méthode

    public float portee;
    protected int idAgent=0;
    public bool equipeA;
    private Monde terrain=null;
    private bool moving;
    private int etat = 0;
 

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
	
    // Méthode qui sert a récupérer les paramètres de chaque agent. Elle ne devrait être appelée que par le Monde
    public void Init(Monde monde, bool team,int id)
    {
        equipeA = team;
        terrain = monde;
        idAgent = id;
        etat = 0;
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

    protected void Attaquer(Agent attaque)
    {
        // A completer avec le booleen d'animation 
        this.anim.SetBool("attack", true);
        this.terrain.Attaquer(this, attaque);
    }

	// Set la destination du NavMeshAgent à position et appelle la méthode qui se charge de faire fonctionner le déplacemment et les animations en même temps.
	protected void LetsMove (Vector3 position) {
        agent.destination = position;
        moving = true;// A virer?
        m.LetsMove();
    }

    protected Agent getEnnemisPortee()
    {
        return (terrain.EnnemisADisance(this) );
    }

    // Sert a faire déplacer l'agent
    void OnAnimatorMove()
    {
        // Update position to agent position
        transform.position = agent.nextPosition;
    }


}
