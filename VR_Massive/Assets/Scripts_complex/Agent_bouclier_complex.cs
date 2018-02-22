using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent_bouclier_complex : Agent_CaC_complex {
	public override void Start()
	{
		this.portee = 1;
		base.StartA ();
	}
	protected override string fortContre()
	{
		return "Agent_mage";
	}
}
