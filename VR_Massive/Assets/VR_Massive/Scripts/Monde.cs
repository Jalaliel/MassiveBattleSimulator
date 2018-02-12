using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monde : MonoBehaviour {

    List<Agent> teamA;
    List<Agent> teamB;

    public int nbAgentTeam=0;
    public int nbAgentTeamMage = 2;
    public int nbAgentTeamDeuxMains = 2;
    public int nbAgentTeamBouclier = 2;
    public GameObject mage;
    public GameObject deuxMains;
    public GameObject bouclier;
    public Transform agentsA;
    public Transform agentsB;

	// Use this for initialization
	void Start () {
        // Generation des listes avec des Instantiate et Init().

        nbAgentTeam = nbAgentTeamBouclier + nbAgentTeamDeuxMains + nbAgentTeamMage;

        /*TEAM A  */
        for (int i = 0; i < nbAgentTeamMage; i++)
        {
            GameObject NAgent=Instantiate(mage) as GameObject;
            teamA[i]=NAgent.GetComponent<Agent_mage>();// TODO A verifier !!!!
            teamA[i].Init(this, true,i);
        }
        int decalage = teamA.Count;
        for (int i = 0; i < nbAgentTeamDeuxMains; i++)
        {
            GameObject NAgent = Instantiate(deuxMains) as GameObject;
            teamA[i+decalage] = NAgent.GetComponent<Agent_deuxMains>();// TODO A verifier !!!!
            teamA[i+decalage].Init(this, true, i+decalage);
        }
        decalage = teamA.Count;
        for (int i = 0; i < nbAgentTeamBouclier; i++)
        {
            GameObject NAgent = Instantiate(bouclier) as GameObject;
            teamA[i + decalage] = NAgent.GetComponent<Agent_bouclier>();
            teamA[i + decalage].Init(this, true, i + decalage);
        }


        /*TEAM B  */
        for (int i = 0; i < nbAgentTeamMage; i++)
        {
            GameObject mageC = Instantiate(mage) as GameObject;
            teamB[i] = mageC.GetComponent<Agent_mage>();// TODO A verifier !!!!
            teamB[i].Init(this, false, i);
        }
        decalage = teamB.Count;
        for (int i = 0; i < nbAgentTeamDeuxMains; i++)
        {
            GameObject NAgent = Instantiate(deuxMains) as GameObject;
            teamB[i + decalage] = NAgent.GetComponent<Agent_deuxMains>();// TODO A verifier !!!!
            teamB[i + decalage].Init(this, false, i + decalage);
        }
        decalage = teamB.Count;
        for (int i = 0; i < nbAgentTeamBouclier; i++)
        {
            GameObject NAgent = Instantiate(bouclier) as GameObject;
            teamB[i + decalage] = NAgent.GetComponent<Agent_bouclier>();// TODO A verifier !!!!
            teamB[i + decalage].Init(this, false, i + decalage);
        }
    }
	
	// Update is called once per frame
	void Update () {

    }


    public void Attaquer(Agent attaquant, Agent attaque)
    {

    }

    // Ressors un ennemis à portée de l'attaquant si il y en a un (le premier qu'il trouve), null sinon
    public Agent EnnemisADisance(Agent attaquant)
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
