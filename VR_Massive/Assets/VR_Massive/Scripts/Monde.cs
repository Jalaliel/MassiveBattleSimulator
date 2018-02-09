using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monde : MonoBehaviour {

    List<Agent> teamA;
    List<Agent> teamB;

	// Use this for initialization
	void Start () {
        // Generation des listes avec des Instantiate et Init().
		
	}
	
	// Update is called once per frame
	void Update () {

    }


    // Ressors un ennemis à portée de l'attaquant si il y en a un (le premier qu'il trouve), null sinon
    protected Agent EnnemisADisance(Agent attaquant)
    {
        if (attaquant.equipeA)
            return EnnemisADistanceB(attaquant);
        else
            return EnnemisADistanceA(attaquant);
    }

    private Agent EnnemisADistanceA(Agent attaquant)
    {
        Agent ennemi = null;
        int i = 0;
        while ((ennemi==null) & (i<teamA.Count))
            if (Vector3.Distance(this.transform.position, teamA[i].transform.position) < attaquant.portee)
                ennemi = teamA[i];
            i++;
        return ennemi;
    }
    private Agent EnnemisADistanceB(Agent attaquant)
    {
        Agent ennemi = null;
        int i = 0;
        while ((ennemi == null) & (i < teamB.Count))
            if (Vector3.Distance(this.transform.position, teamB[i].transform.position) < attaquant.portee)
                ennemi = teamA[i];
        i++;
        return ennemi;
    }
}
