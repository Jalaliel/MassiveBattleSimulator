using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monde : MonoBehaviour {

    List<Agent> teamA=null;
    List<Agent> teamB=null;

    public int nbAgentTeam=0;
    public int nbAgentTeamMage = 2;
    public int nbAgentTeamDeuxMains = 2;
    public int nbAgentTeamBouclier = 2;
    public GameObject mage;
    public GameObject deuxMains;
    public GameObject bouclier;
    public Transform agentsA;
    public Transform agentsB;

    // Proba de toucher
    private float proba_fort = 0.40f; // Proba de toucher un mec plus fort que soit
    private float proba_egal = 0.70f;
    private float proba_faible = 0.90f;

    // Use this for initialization
    void Start()
    {
        // Generation des listes avec des Instantiate et Init().

        nbAgentTeam = nbAgentTeamBouclier + nbAgentTeamDeuxMains + nbAgentTeamMage;
        teamA = new List<Agent>();
        teamB = new List<Agent>();

        /*TEAM A  */
        for (int i = 0; i < nbAgentTeamMage; i++)
        {
            GameObject NAgent = Instantiate(mage, agentsA.position, agentsA.rotation) as GameObject;
            teamA.Add(NAgent.GetComponent<Agent_mage>());// TODO A verifier !!!!
            teamA[i].Init(this, true, i);
        }

        int decalage = teamA.Count;
        if (decalage == nbAgentTeamMage)
            Debug.Log("Pas de soucis avec les mages A");
        for (int i = 0; i < nbAgentTeamDeuxMains; i++)
        {
            GameObject NAgent = Instantiate(deuxMains, agentsA.position, agentsA.rotation) as GameObject;
            teamA.Add(NAgent.GetComponent<Agent_deuxMains>());// TODO A verifier !!!!
            teamA[i + decalage].Init(this, true, i + decalage);
        }
        decalage = teamA.Count;
        if (decalage == nbAgentTeamMage + nbAgentTeamDeuxMains)
            Debug.Log("Pas de soucis avec les deuxMains A");
        for (int i = 0; i < nbAgentTeamBouclier; i++)
        {
            GameObject NAgent = Instantiate(bouclier, agentsA.position, agentsA.rotation) as GameObject;
            teamA.Add(NAgent.GetComponent<Agent_bouclier>());
            teamA[i + decalage].Init(this, true, i + decalage);
        }
        decalage = teamA.Count;
        if (decalage == nbAgentTeam)
            Debug.Log("Pas de soucis avec les boubou A");


        /*TEAM B  */
        for (int i = 0; i < nbAgentTeamMage; i++)
        {
            GameObject NAgent = Instantiate(mage, agentsB.position, agentsB.rotation) as GameObject;
            teamB.Add(NAgent.GetComponent<Agent_mage>());// TODO A verifier !!!!
            teamB[i].Init(this, true, i);
        }

        decalage = teamB.Count;
        if (decalage == nbAgentTeamMage)
            Debug.Log("Pas de soucis avec les mages B");

        for (int i = 0; i < nbAgentTeamDeuxMains; i++)
        {
            GameObject NAgent = Instantiate(deuxMains, agentsB.position, agentsB.rotation) as GameObject;
            teamB.Add(NAgent.GetComponent<Agent_deuxMains>());// TODO A verifier !!!!
            teamB[i + decalage].Init(this, true, i + decalage);
        }
        decalage = teamB.Count;
        if (decalage == nbAgentTeamMage + nbAgentTeamDeuxMains)
            Debug.Log("Pas de soucis avec les deuxMains B");
        for (int i = 0; i < nbAgentTeamBouclier; i++)
        {
            GameObject NAgent = Instantiate(bouclier, agentsB.position, agentsB.rotation) as GameObject;
            teamB.Add(NAgent.GetComponent<Agent_bouclier>());
            teamB[i + decalage].Init(this, true, i + decalage);
        }
        decalage = teamB.Count;
        if (decalage == nbAgentTeam)
            Debug.Log("Pas de soucis avec les boubou B");
    }

        // Update is called once per frame
        void Update ()
        {

        }


    public void Attaquer(Agent attaquant, Agent attaque)
    {
        int indice=0;
        if (attaquant.GetType().Name == attaque.GetType().Name)
            indice = 2;
        else
        {
            if ((attaquant.GetType().Name == "Agent_mage" && attaque.GetType().Name == "Agent_bouclier") || (attaquant.GetType().Name == "Agent_bouclier" && attaque.GetType().Name == "Agent_deuxMains") || (attaquant.GetType().Name == "Agent_deuxMains" && attaque.GetType().Name == "Agent_mage"))
                indice = 1;
            else
                indice = 3;
        }
        bool reussi = false;
        switch (indice)
        {
            case 1:
                reussi = (Random.Range(1, 10) > 6);
                break;

            case 2:
                reussi = (Random.Range(1, 10) > 3);
                break;

            case 3:
                reussi = (Random.Range(1, 10) > 1);
                break;
            default:
                Debug.Log("Il y a une couille dans le paté");
                break;
        }
        if (reussi)
        {
            attaque.SetEtat(attaque.GetEtat() - 1);
            if (attaque.GetEtat() == 0)
                this.Tuer(attaque,attaque.equipeA);
        }
    }



    public void Tuer(Agent mort, bool team)
    {
        if (team)
        {
            teamA.Remove(mort);
            // TODO animation mort
            Destroy(mort.gameObject, 5);
        }
        else
        {
            teamB.Remove(mort);
            // TODO: Animation morts
            Destroy(mort.gameObject,5);
        }
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
        while ((ennemi==null) & (i < teamA.Count))
        { 
            if (Vector3.Distance(this.transform.position, teamA[i].transform.position) < attaquant.portee)
                ennemi = teamA[i];
            i++;
        }
        return ennemi;
    }
    private Agent EnnemisADistanceB(Agent attaquant)
    {
        Agent ennemi = null;
        int i = 0;
        while ((ennemi == null) & (i < teamB.Count))
        {
            if (Vector3.Distance(this.transform.position, teamB[i].transform.position) < attaquant.portee)
                ennemi = teamA[i];
            i++;
        }
        return ennemi;
    }
}
