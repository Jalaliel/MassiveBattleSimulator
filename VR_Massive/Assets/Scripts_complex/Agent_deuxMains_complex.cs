using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent_deuxMains_complex : Agent_CaC_complex {
	public override void Start()
	{
		this.portee = 2;
		base.StartA ();
	}
	protected override string fortContre()
	{
		return "Agent_bouclier";
	}
}
