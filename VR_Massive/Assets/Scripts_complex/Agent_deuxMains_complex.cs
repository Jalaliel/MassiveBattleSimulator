using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent_deuxMains_complex : Agent_CaC_complex {
	/// <summary>
	/// <see cref="Agent_complexe.Start()"/>
	/// </summary>
	public override void Start()
	{
		this.portee = 2;
		base.StartA ();
	}

	/// <summary>
	/// <see cref="Agent_complex.fortContre()"/>
	/// </summary>
	protected override string fortContre()
	{
		return "Agent_bouclier";
	}
}
