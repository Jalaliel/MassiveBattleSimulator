using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent_complex : Agent {

	private bool animDeath;
	public int distanceAvantSortie = 1;
	public int nbFrameRefresh = 100;
	private int compteurPeur;
	public double viewDistance = 5.0;

	public void Start()
	{
		animDeath = false;
		compteurPeur = Random.Range(0, nbFrameRefresh - 1);
	}

	protected IEnumerator Wait(float time)
	{
		yield return new WaitForSeconds(time);
	}

	protected void mourir(float timeToDie)
	{
		if(!animDeath)
		{
			this.animDeath = true;
			this.anim.SetTrigger("dead");
			this.terrain.Tuer(this);
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
		Destroy(this.gameObject);
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
		if (etat = 0)
			mourir ();
		else if (sorti ())
			disparaitre ();
		else if (peur ())
			fuir ();
		else if (!selectTaper ())
		{
			Vector3 dest = choisirPlusProche ();
			LetsMove (dest);
		}
	}

	//TODO
	protected abstract bool selectTaper();

	//TODO
	protected abstract Vector3 choisirPlusProche();

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