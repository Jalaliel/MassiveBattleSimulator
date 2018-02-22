using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Agent_complex : Agent {

	private bool animDeath;
	public int nbEchantillon = 10;
	public int distanceAvantSortie = 1;
	public int nbFrameRefresh = 100;
	private int compteurPeur;
	private int compteurCible;
	public double viewDistance = 5.0;
	public string reactEnCours = "";
	private Vector3 cible;

	public virtual void Start()
	{
		animDeath = false;
		cible = this.transform.position;
		compteurPeur = Random.Range(0, nbFrameRefresh - 1);
		compteurCible = Random.Range(0, nbFrameRefresh - 1);
	}

	void Update()
	{
		this.moving = false;
		this.anim.SetBool ("Moving", false);
		priseDeDecision ();
	}

	protected IEnumerator Wait(float time)
	{
		yield return new WaitForSeconds(time);
	}

	protected void mourir(float timeToDie)
	{
		if(!animDeath)
		{
			this.terrain.Tuer(this);
			this.animDeath = true;
			this.anim.SetTrigger("dead");
			Destroy(this.gameObject, timeToDie);
			StartCoroutine (Wait (timeToDie));
		}
	}
		
	protected bool sorti()
	{
		return (this.transform.position.x > 100 - distanceAvantSortie) || (this.transform.position.x < -(100 - distanceAvantSortie)) || (this.transform.position.z > (100 - distanceAvantSortie)) || (this.transform.position.z < -(100 - distanceAvantSortie));
	}
		
	protected void disparaitre()
	{
		base.terrain.Tuer(this);
		Destroy(this.gameObject, 3.0f);
	}

	protected void fuir()
	{
		Vector3 center;
		if (equipeA)
			center = terrain.centreDeGraviteB;
		else
			center = terrain.centreDeGraviteA;
		LetsMove (this.transform.position * 2 - center);
	}
		
	protected bool peur()
	{
		compteurPeur++;
		if (compteurPeur >= nbFrameRefresh)
		{
			compteurPeur = 0;
			int chanceFuite = 0;
			switch (etat) {
				case 1:
					chanceFuite += 3;
					break;
				case 2:
					chanceFuite++;
					break;
			}
			if (enFuite)
				chanceFuite += 5;
			int nbAllies = terrain.nbAPortee (this.equipeA, this, viewDistance);
			int nbEnnemis = terrain.nbAPortee (!(this.equipeA), this, viewDistance);
			if (nbAllies < nbEnnemis)
				chanceFuite++;
			if (nbAllies < nbEnnemis * 2)
				chanceFuite++;
			return (Random.Range (0, 10) < chanceFuite);
		} 
		else
			return enFuite;
	}
		
	protected void priseDeDecision()
	{
		reactEnCours = "Prise de décision";
		if (etat <= 0) {
			reactEnCours = "mourir";
			mourir (3.5f);
		} else if (sorti ()) {
			reactEnCours = "disparaitre";
			disparaitre ();
		} else if (peur ()) {
			reactEnCours = "fuir";
			fuir ();
		} else if (!selectTaper ()) {
			reactEnCours = "move";
			Vector3 dest = choisirPlusProche ();
			LetsMove (dest);
		} else
			reactEnCours = "taper";
	}

	//TODO
	protected abstract string fortContre ();

	//TODO
	protected abstract bool selectTaper();

	protected Vector3 choisirPlusProche()
	{
		List<Agent> ennemis;
		if (equipeA)
			ennemis = terrain.getTeamB ();
		else
			ennemis = terrain.getTeamA ();
		int size = ennemis.Count;
		if (size == 0)
			return this.transform.position;
		else
		{
			if (size > nbEchantillon)
				ennemis = tirerNDans (nbEchantillon, ennemis);

			bool agentFaible = ennemis [0].GetType ().Name.Contains (fortContre ());
			Agent aViser = ennemis[0];
			float dist = Vector3.Distance(this.transform.position, ennemis[0].transform.position);
			size = ennemis.Count;
			for (int i = 1; i < size; i++)
			{
				bool cetAgentEstFaible = ennemis [i].GetType ().Name.Contains (fortContre ());
				if (!agentFaible && cetAgentEstFaible) 
				{
					agentFaible = true;
					aViser = ennemis [i];
					dist = Vector3.Distance(this.transform.position, ennemis[i].transform.position);
				}
				else if(!(agentFaible && !cetAgentEstFaible))
				{
					float distCetAgent = dist = Vector3.Distance (this.transform.position, ennemis [i].transform.position);
					if (distCetAgent < dist) 
					{
						dist = distCetAgent;
						aViser = ennemis [i];
					}
				}
			}
			return aViser.transform.position;
		}
	}

	private List<Agent> tirerNDans(int nb, List<Agent> list)
	{
		List<Agent> listCopiee = new List<Agent> (list);
		int size = list.Count;
		List<Agent> retour = new List<Agent>();
		for (int i = 0; i < nb; i++)
		{
			int indice = Random.Range (0, size - 1);
			retour.Add (listCopiee [indice]);
			listCopiee.Remove(listCopiee[indice]);
			size--;
		}
		return retour;
	}

	protected override void LetsMove (Vector3 position)
	{
		this.moving = true;
		this.anim.SetBool ("Moving", true);
		base.LetsMove (position);
	}

	void Hit()
	{

	}



	void FootR()
	{
	}

	void FootL()
	{
	}
}