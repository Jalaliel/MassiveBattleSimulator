using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monde : MonoBehaviour {

    public List<Agent> teamA=null;
    public List<Agent> teamB=null;

    public int nbAgentTeam=0;
    public int nbAgentTeamMage = 2;
    public int nbAgentTeamDeuxMains = 2;
    public int nbAgentTeamBouclier = 2;
    public GameObject mage;
    public GameObject deuxMains;
    public GameObject bouclier;
    public Transform agentsA;
    public Transform agentsB;
    public Transform fuite;
    

    // Use this for initialization
    void Start()
    {
        // Generation des listes avec des Instantiate et Init().

        nbAgentTeam = nbAgentTeamBouclier + nbAgentTeamDeuxMains + nbAgentTeamMage;
        teamA = new List<Agent>();
        teamB = new List<Agent>();

        /*TEAM A  */

        /* Pour Antoine: Si tu veux mofifier la position de spawn il faut modifier agentsA.position, tu peux
         * le remplacer par n'importe quel Vector3 */
        for (int i = 0; i < nbAgentTeamMage; i++)
        {
            GameObject NAgent = Instantiate(mage, agentsA.position, agentsA.rotation) as GameObject;
            teamA.Add(NAgent.GetComponent<Agent_mage>());
            teamA[i].Init(this, true, i);
        }

        int decalage = teamA.Count;
        if (decalage == nbAgentTeamMage)
            Debug.Log("Pas de soucis avec les mages A");
        for (int i = 0; i < nbAgentTeamDeuxMains; i++)
        {
            GameObject NAgent = Instantiate(deuxMains, agentsA.position, agentsA.rotation) as GameObject;
            teamA.Add(NAgent.GetComponent<Agent_deuxMains>());
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
            teamB.Add(NAgent.GetComponent<Agent_mage>());
            teamB[i].Init(this, false, i+nbAgentTeam);
        }

        decalage = teamB.Count;
        if (decalage == nbAgentTeamMage)
            Debug.Log("Pas de soucis avec les mages B");

        for (int i = 0; i < nbAgentTeamDeuxMains; i++)
        {
            GameObject NAgent = Instantiate(deuxMains, agentsB.position, agentsB.rotation) as GameObject;
            teamB.Add(NAgent.GetComponent<Agent_deuxMains>());
            teamB[i + decalage].Init(this, false, i + decalage + nbAgentTeam);
        }
        decalage = teamB.Count;
        if (decalage == nbAgentTeamMage + nbAgentTeamDeuxMains)
            Debug.Log("Pas de soucis avec les deuxMains B");
        for (int i = 0; i < nbAgentTeamBouclier; i++)
        {
            GameObject NAgent = Instantiate(bouclier, agentsB.position, agentsB.rotation) as GameObject;
            teamB.Add(NAgent.GetComponent<Agent_bouclier>());
            teamB[i + decalage].Init(this, false, i + decalage + nbAgentTeam);
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
                reussi = (Random.Range(1, 100) < 40);
                break;

            case 2:
                reussi = (Random.Range(1, 100) < 70);
                break;

            case 3:
                reussi = (Random.Range(1, 100) < 90);
                break;
            default:
                Debug.Log("Probleme pour l'attaque");
                break;
        }
        if (reussi)
        {
            Debug.Log("attaque reussie");
            attaque.SetEtat(attaque.GetEtat() - 1);
            //if (attaque.GetEtat() <= 0)
            //{
            //    this.Tuer(attaque);
            //}
        }
    }

    public int GetNbTeamA()
    {
        return this.teamA.Count;
    }

    public int GetNbTeamB()
    {
        return this.teamB.Count;
    }

    public void Tuer(Agent mort)
    {
        if (mort.equipeA)
        {
            int ind = teamA.FindIndex(x=> (x.idAgent==mort.idAgent));
            teamA.RemoveAt(ind);
        }
        else
        {
            int ind = teamA.FindIndex(x => (x.idAgent == mort.idAgent));
            teamB.Remove(mort);
        }
    }


    // Ressors la liste des ennemis à portée
    public List<Agent> EnnemisADisance(Agent attaquant)
    {
        if (attaquant.equipeA)
            return EnnemisADistanceB(attaquant);
        else
            return EnnemisADistanceA(attaquant);
    }


    private List<Agent> EnnemisADistanceA(Agent attaquant)
    {
        List<Agent> ennemi = new List<Agent>();
        for (int i = 0; i < teamA.Count; i++)
        {
            if (Vector3.Distance(this.transform.position, teamA[i].transform.position) < attaquant.portee)
                ennemi.Add(teamA[i]);
        }
        return ennemi;
    }
    private List<Agent> EnnemisADistanceB(Agent attaquant)
    {
        List<Agent> ennemi = new List<Agent>();
        for (int i = 0; i < teamB.Count; i++)
        {
            if (Vector3.Distance(this.transform.position, teamB[i].transform.position) < attaquant.portee)
                ennemi.Add(teamB[i]);
        }
        return ennemi;
    }
}
