using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monde : MonoBehaviour {
    // Liste des agents dans chaque équipe encore en vie
    public List<Agent> teamA=null;
    public List<Agent> teamB=null;

    // Booleen
	public bool complexIATeamA = false; // Vrai si on veut avoir une equipe avec les IA complexes
	public bool complexIATeamB = false;
    public bool spawnSemiAlea; // Vrai si le spawn est semi aléatoire, faux si le point de spawn est fixé pour chaque agent
	
    public int nbAgentTeam=0;
    public int nbAgentTeamMage;
    public int nbAgentTeamDeuxMains;
    public int nbAgentTeamBouclier;

    public GameObject mage;
    public GameObject deuxMains;
    public GameObject bouclier;

    public Transform agentsA1;
    public Transform agentsA2;
    public Transform agentsB1;
    public Transform agentsB2;
    public Transform fuite; // Utile seulement pour les agents pas complexes

    public Vector3 centreDeGraviteA { get; private set; }
    public Vector3 centreDeGraviteB { get; private set; }



    // Use this for initialization
    void Start()
    {
        // Generation des listes avec des Instantiate et Init().

        nbAgentTeam = nbAgentTeamBouclier + nbAgentTeamDeuxMains + nbAgentTeamMage;
        teamA = new List<Agent>();
        teamB = new List<Agent>();
        float profondeur=this.agentsA1.transform.position.x-this.agentsA2.transform.position.x;
        float longueur= this.agentsA1.transform.position.z - this.agentsA2.transform.position.z;
        float dp = profondeur / 3;
        int max = Mathf.Max(nbAgentTeamBouclier,Mathf.Max(nbAgentTeamDeuxMains,nbAgentTeamMage));
        float dl = longueur / (spawnSemiAlea ? 1 :max);

        /* -----------*/
        /*   TEAM A   */
        /* -----------*/

        for (int i = 0; i < nbAgentTeamMage; i++)
        {
			if(complexIATeamA)
			{
				GameObject NAgent = Instantiate(mage_complex, agentsA.position, agentsA.rotation) as GameObject;
				teamA.Add(NAgent.GetComponent<Agent_mage_complex>());
			}
			else
			{
				GameObject NAgent = Instantiate(mage, agentsA.position, agentsA.rotation) as GameObject;
				teamA.Add(NAgent.GetComponent<Agent_mage>());
			}
            teamA[i].Init(this, true, i);
        }

        int decalage = teamA.Count;
        if (decalage == nbAgentTeamMage)
            Debug.Log("Pas de soucis avec les mages A");
        for (int i = 0; i < nbAgentTeamDeuxMains; i++)
        {
			if(complexIATeamA)
			{
				GameObject NAgent = Instantiate(deuxMains_complex, agentsA.position, agentsA.rotation) as GameObject;
				teamA.Add(NAgent.GetComponent<Agent_deuxMains_complex>());
			}
			else
			{
				GameObject NAgent = Instantiate(deuxMains, agentsA.position, agentsA.rotation) as GameObject;
				teamA.Add(NAgent.GetComponent<Agent_deuxMains_complex>());
			}
            teamA[i + decalage].Init(this, true, i + decalage);
        }
        decalage = teamA.Count;
        if (decalage == nbAgentTeamMage + nbAgentTeamDeuxMains)
            Debug.Log("Pas de soucis avec les deuxMains A");
        for (int i = 0; i < nbAgentTeamBouclier; i++)
        {
			if(complexIATeamA)
			{
				GameObject NAgent = Instantiate(bouclier_complex, agentsA.position, agentsA.rotation) as GameObject;
				teamA.Add(NAgent.GetComponent<Agent_bouclier_complex>());
			}
			else
			{
				GameObject NAgent = Instantiate(bouclier, agentsA.position, agentsA.rotation) as GameObject;
				teamA.Add(NAgent.GetComponent<Agent_bouclier>());				
			}
            teamA[i + decalage].Init(this, true, i + decalage);
        }
        decalage = teamA.Count;
        if (decalage == nbAgentTeam)
            Debug.Log("Pas de soucis avec les boubou A");


        /*TEAM B  */
        for (int i = 0; i < nbAgentTeamMage; i++)
        {
			if(complexIATeamB)
			{
				GameObject NAgent = Instantiate(mage_complex, agentsB.position, agentsB.rotation) as GameObject;
				teamB.Add(NAgent.GetComponent<Agent_mage_complex>());
			}
			else
			{
				GameObject NAgent = Instantiate(mage, agentsB.position, agentsB.rotation) as GameObject;
				teamB.Add(NAgent.GetComponent<Agent_mage>());				
			}
            teamB[i].Init(this, false, i+nbAgentTeam);
        }

        decalage = teamB.Count;
        if (decalage == nbAgentTeamMage)
            Debug.Log("Pas de soucis avec les mages B");

        for (int i = 0; i < nbAgentTeamDeuxMains; i++)
        {
			if(complexIATeamB)
			{
				GameObject NAgent = Instantiate(deuxMains_complex, agentsB.position, agentsB.rotation) as GameObject;
				teamB.Add(NAgent.GetComponent<Agent_deuxMains_complex>());
			}
			else
			{
				GameObject NAgent = Instantiate(deuxMains, agentsB.position, agentsB.rotation) as GameObject;
				teamB.Add(NAgent.GetComponent<Agent_deuxMains>());				
			}
            teamB[i + decalage].Init(this, false, i + decalage + nbAgentTeam);
        }
        decalage = teamB.Count;
        if (decalage == nbAgentTeamMage + nbAgentTeamDeuxMains)
            Debug.Log("Pas de soucis avec les deuxMains B");
        for (int i = 0; i < nbAgentTeamBouclier; i++)
        {
			if(complexIATeamB)
			{
				GameObject NAgent = Instantiate(bouclier_complex, agentsB.position, agentsB.rotation) as GameObject;
				teamB.Add(NAgent.GetComponent<Agent_bouclier_complex>());
			}
			else
			{
				GameObject NAgent = Instantiate(bouclier, agentsB.position, agentsB.rotation) as GameObject;
				teamB.Add(NAgent.GetComponent<Agent_bouclier>());
			}
            teamB[i + decalage].Init(this, false, i + decalage + nbAgentTeam);
        }
        decalage = teamB.Count;
        if (decalage == nbAgentTeam)
            Debug.Log("Pas de soucis avec les boubou B");
    }

        // Update is called once per frame
    void Update ()
    {
        // Update du centre de gravité de la team A
        Vector3 temp = new Vector3();
        for (int i = 0; i < teamA.Count; i++)
            temp += teamA[i].transform.position;
        this.centreDeGraviteA = temp / teamA.Count;
        // Update du centre de gravité de la team B
        temp = new Vector3();
        for (int i = 0; i < teamB.Count; i++)
            temp += teamB[i].transform.position;
        this.centreDeGraviteB = temp / teamB.Count;
    }

    public int nbAPortee(bool teamVoulueA, Agent demandeur,double portee)
    {
        int nb = 0;
        if (teamVoulueA)
        {
            for (int i = 0; i < teamA.Count; i++)
                if (Vector3.Distance(demandeur.transform.position, teamA[i].transform.position) < portee)
                    nb++;
        }
        else
        {
            for (int i = 0; i < teamB.Count; i++)
                if (Vector3.Distance(demandeur.transform.position, teamB[i].transform.position) < portee)
                    nb++;
        }
        return nb;
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
    private Agent EnnemiProcheA(Agent sujet)
    {
        Agent a = teamB[0];
        List<Agent> temp = EnnemisADisance(sujet);
        if (temp.Count > 0)
        {
            a = temp[0];
        }
        return a;
    }
    private Agent EnnemiProcheB(Agent sujet)
    {
        Agent a = teamA[0];
        List<Agent> temp = EnnemisADisance(sujet);
        if (temp.Count > 0)
        {
            a = temp[0];
        }
        return a;
    }
    public Agent EnnemisProche(Agent sujet)
    {
        if (sujet.equipeA)
        {
            return EnnemiProcheA(sujet);
        }
        else
        {
            return EnnemiProcheB(sujet);
        }
    }
}
