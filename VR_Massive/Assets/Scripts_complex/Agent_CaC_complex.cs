using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Agent_CaC_complex : Agent_complex {
	/// <summary>
	/// <see cref="Agent_complex.selectTaper()"/>
	/// </summary>
	protected override bool selectTaper()
	{
		List<Agent> ennemis = terrain.EnnemisADisance (this);
		if (ennemis.Count > 0) {
			Agent ennemiATaper = ennemis [0];
			bool isAgentFaible = ennemis [0].GetType ().Name.Contains (fortContre ());
			int size = ennemis.Count;
			for (int i = 0; i < size && !isAgentFaible; i++)
				if (ennemis [i].GetType ().Name.Contains (fortContre ())) {
					ennemiATaper = ennemis [i];
					isAgentFaible = true;
				}
			this.anim.SetTrigger ("Attack");
			this.transform.LookAt (ennemiATaper.transform);
			Wait (3.5f);
			this.terrain.Attaquer (this, ennemiATaper);
			return true;
		} else
			return false;
	}

}
