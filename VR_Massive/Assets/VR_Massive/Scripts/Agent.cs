using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class Agent : MonoBehaviour {

    protected NavMeshAgent agent;
    public Animator anim;
    private Move m; 

    public double portee;
    public int idAgent=0;
    public bool equipeA;
    protected Monde terrain=null;
    public bool moving;
    public int etat = 3; // 3 full life, 2 légèrement blessé, 1 gravement blessé, 0 mort
    public bool enFuite;
 

    // Use this for initialization
    protected void StartA ()
    {
    }
    /// <summary>
    /// Méthode qui sert a récupérer les paramètres de chaque agent. Elle ne devrait être appelée que par le Monde
    /// </summary>
    public void Init(Monde monde, bool team,int id)
    {
        agent = GetComponent<NavMeshAgent>();// Component qui va nous servir à se déplacer correctement
        // Don’t update position automatically
        agent.updatePosition = false;
        anim = GetComponent<Animator>();
        m = new Move(agent, anim, this);
        equipeA = team;
        terrain = monde;
        idAgent = id;
        etat = 3; //Au début, tous les agents sont au maximum de leur vie
        enFuite = false;
    }
    /// <summary>
    /// Set l'etat de l'agent
    /// </summary>
    public void SetEtat(int e)
    {
        this.etat = e;
    }
    /// <summary>
    /// Retourne l'état de l'agent
    /// </summary>
    public int GetEtat()
    {
        return this.etat;
    }

    /// <summary>
	/// Set la destination du NavMeshAgent à position et appelle la méthode qui se charge de faire fonctionner le déplacemment et les animations en même temps.
    /// </summary>
	protected virtual void LetsMove (Vector3 position) {
        
        agent.destination = position;
        moving = true;// A virer?
        m.LetsMove();
    }
    /// <summary>
    /// Retourne les ennemis à portée
    /// </summary>
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

    /// <summary>
    /// Appelé quand l'agent entre dans le trigger de la lance du joueur
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            if (Random.Range(1, 100) > 75)
                etat--;
    }
}
