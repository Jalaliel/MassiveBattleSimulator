using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Agent_mage_complex : Agent_complex {
	public override void Start()
	{
		this.portee = 10;
		base.StartA ();
	}

	protected override bool selectTaper()
	{
		List<Agent> ennemis = terrain.EnnemisADisance (this);
		if (ennemis.Count > 0) {
			Debug.Log ("Attaque");
			Agent ennemiATaper = ennemis [0];
			bool isAgentFaible = ennemis [0].GetType ().Name.Contains (fortContre ());
			int size = ennemis.Count;
			for (int i = 0; i < size && !isAgentFaible; i++)
				if (ennemis [i].GetType ().Name.Contains (fortContre ())) {
					ennemiATaper = ennemis [i];
					isAgentFaible = true;
				}
			this.anim.SetTrigger ("attack");
			this.transform.LookAt (ennemiATaper.transform);
			StartCoroutine (Wait (3.5f));
			this.terrain.Attaquer (this, ennemiATaper);
			return true;
		} else
			return false;
	}

	protected override string fortContre()
	{
		return "Agent_deuxMains";
	}
}
