using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monde : MonoBehaviour {
    // Liste des agents dans chaque équipe encore en vie
    private List<Agent> teamA=null;
    private List<Agent> teamB=null;

    // Booleen
	public bool complexIATeamA = false; // Vrai si on veut avoir l'equipe A avec les IA complexes
	public bool complexIATeamB = false;
    public bool spawnSemiAlea; // Vrai si le spawn est semi aléatoire, faux si le point de spawn est fixé pour chaque agent (expliqué dans le rapport
	
    public int nbAgentTeam=0; // Nombre d'agents total par équipe
    public int nbAgentTeamMage; // Nombre d'agents mages dans chaque équipe. Pour des raisons de facilités, les deux équipes ont le même nombre d'agents de chaque type
    public int nbAgentTeamDeuxMains;
    public int nbAgentTeamBouclier;

    public GameObject mage;  // Préfab qui nous servent à instancier les GameObject
    public GameObject deuxMains;
    public GameObject bouclier;

    public Transform agentsA1; // Positions utiles pour les spawn
    public Transform agentsA2;
    public Transform agentsB1;
    public Transform agentsB2;
    public Transform fuite; // Utile seulement pour les agents pas complexes, point vers lequel ils vont fuir

    public Vector3 centreDeGraviteA { get; private set; } // Centre de gravité de l'équipe A
    public Vector3 centreDeGraviteB { get; private set; }

	public Texture mageA;
	public Texture knightB;
	public Texture deuxMainsB;


	/// <Summary>
    /// Cette méthode est appelée automatiquement au début du programme car ce script est lié à un objet de la scene
    /// </Summary>
    void Start()
    {
        nbAgentTeam = nbAgentTeamBouclier + nbAgentTeamDeuxMains + nbAgentTeamMage;
        teamA = new List<Agent>();
        teamB = new List<Agent>();
        /* -----------*/
        /*   TEAM A   */
        /* -----------*/
        float longueur=this.agentsA2.transform.position.x-this.agentsA1.transform.position.x;
        float profondeur= this.agentsA2.transform.position.z - this.agentsA1.transform.position.z;
        float dp = profondeur / 3;
        float dl = longueur;

        float xA = agentsA1.transform.position.x;
        float zA = agentsA1.transform.position.z;

        // Création des mages de la team A
        for (int i = 0; i < nbAgentTeamMage; i++)
        {
            Vector3 posi;
            if (spawnSemiAlea)
                posi = new Vector3(xA + Random.Range(0, dl), 0.5f, zA + Random.Range(0, dp));
            else
                posi = new Vector3(xA + i * dl / nbAgentTeamMage, 0.5f, zA);
            GameObject NAgent = Instantiate(mage, posi, agentsA1.rotation) as GameObject;// On instantie un GameObject à l'aide du préfab donné, à la position posi et avec une rotation précise
            if (complexIATeamA) // En fonction du type d'agent (complex ou normal), on récupére l'objet lié à la bonne classe
                teamA.Add(NAgent.GetComponent<Agent_mage_complex>());
            else
                teamA.Add(NAgent.GetComponent<Agent_mage>());
            teamA[i].Init(this, true, i);
            teamA[i].gameObject.GetComponent<Agent_mage>().enabled=!complexIATeamA;// De même que précédemmeent, on active le bon script et désactive le mauvais
            teamA[i].gameObject.GetComponent<Agent_mage_complex>().enabled = complexIATeamA;
            teamA[i].gameObject.SetActive(false);// On désactive le gameObject afin d'éviter que les premiers agents qui soient crés soient avantagés. On les réactivera à la fin de la création 

			teamA[i].gameObject.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>().material.SetTexture("_MainTex", mageA);
			teamA[i].gameObject.transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().material.SetTexture("_MainTex", mageA);

		}
        // Vérification qu'on a bien créé le bon nombre de mages
        int decalage = teamA.Count;
        if (decalage == nbAgentTeamMage)
            Debug.Log("Pas de soucis avec les mages A");

        // Création des combattants à deux mains    
        for (int i = 0; i < nbAgentTeamDeuxMains; i++)
        {
            Vector3 posi;
            if (spawnSemiAlea)
                posi = new Vector3(xA + Random.Range(0, dl), 0.5f, zA + dp+ Random.Range(0, dp));
            else
                posi = new Vector3(xA + i * dl / nbAgentTeamDeuxMains, 0.5f, zA+dp);
            GameObject NAgent = Instantiate(deuxMains, posi, agentsA1.rotation) as GameObject;
            if (complexIATeamA)
                teamA.Add(NAgent.GetComponent<Agent_deuxMains_complex>());
            else
                teamA.Add(NAgent.GetComponent<Agent_deuxMains>());
            teamA[i + decalage].Init(this, true, i+decalage);
            teamA[i + decalage].gameObject.GetComponent<Agent_deuxMains>().enabled = !complexIATeamA;
            teamA[i + decalage].gameObject.GetComponent<Agent_deuxMains_complex>().enabled = complexIATeamA;
            teamA[i + decalage].gameObject.SetActive(false);
        }
        // Verification du bon dérouelement de la création des combattants à deux mains de la team A
        decalage = teamA.Count;
        if (decalage == nbAgentTeamMage + nbAgentTeamDeuxMains)
            Debug.Log("Pas de soucis avec les deuxMains A");


        // Création des combattants avec un bouclier    
        for (int i = 0; i < nbAgentTeamBouclier; i++)
        {
            Vector3 posi;
            if (spawnSemiAlea)
                posi = new Vector3(xA + Random.Range(0, dl), 0.5f, zA + 2*dp + Random.Range(0, dp));
            else
                posi = new Vector3(xA + i * dl / nbAgentTeamBouclier, 0.5f, zA + 2*dp);
            GameObject NAgent = Instantiate(bouclier, posi, agentsA1.rotation) as GameObject;
            if (complexIATeamA)
                teamA.Add(NAgent.GetComponent<Agent_bouclier_complex>());
            else
                teamA.Add(NAgent.GetComponent<Agent_bouclier>());
            teamA[i + decalage].Init(this, true, i + decalage);
            teamA[i + decalage].gameObject.GetComponent<Agent_bouclier>().enabled = !complexIATeamA;
            teamA[i + decalage].gameObject.GetComponent<Agent_bouclier_complex>().enabled = complexIATeamA;
            teamA[i + decalage].gameObject.SetActive(false);
        }
        // Verification du bon dérouelement de la création des combattants avec un bouclier de la team A
        decalage = teamA.Count;
        if (decalage == nbAgentTeam)
            Debug.Log("Pas de soucis avec les boubou A");


        /* -----------*/
        /*   TEAM B   */
        /* -----------*/

		deuxMains.gameObject.transform.GetChild(1).GetChild(0).GetChild(2).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(5).GetComponent<MeshRenderer>().material.SetTexture("_MainTex", deuxMainsB);

        longueur=this.agentsB2.transform.position.x-this.agentsB1.transform.position.x;
        profondeur= this.agentsB2.transform.position.z - this.agentsB1.transform.position.z;
        dp = profondeur / 3;
        dl = longueur;
        float xB = agentsB1.transform.position.x;
        float zB = agentsB1.transform.position.z;

        // Création des mages de la team B
        for (int i = 0; i < nbAgentTeamMage; i++)
        {
            Vector3 posi;
            if (spawnSemiAlea)
				posi = new Vector3(xB + Random.Range(0, dl), 0.5f, zB+2*dp+ Random.Range(0, dp));
            else
				posi = new Vector3(xB + i * dl / nbAgentTeamMage, 0.5f, zB+2*dp);
            GameObject NAgent = Instantiate(mage, posi, agentsB1.rotation) as GameObject;
            if (complexIATeamB)
                teamB.Add(NAgent.GetComponent<Agent_mage_complex>());
            else
                teamB.Add(NAgent.GetComponent<Agent_mage>());
            teamB[i].Init(this, false, i);
            teamB[i].gameObject.GetComponent<Agent_mage>().enabled = !complexIATeamB;
            teamB[i].gameObject.GetComponent<Agent_mage_complex>().enabled = complexIATeamB;
            teamB[i].gameObject.SetActive(false);
        }
        // Vérification qu'on a bien créé le bon nombre de mages
        decalage = teamB.Count;
        if (decalage == nbAgentTeamMage)
            Debug.Log("Pas de soucis avec les mages B");

        // Création des combattants à deux mains    
        for (int i = 0; i < nbAgentTeamDeuxMains; i++)
        {
            Vector3 posi;
            if (spawnSemiAlea)
                posi = new Vector3(xB + Random.Range(0, dl), 0.5f, zB + dp + Random.Range(0, dp));
            else
                posi = new Vector3(xB + i * dl / nbAgentTeamDeuxMains, 0.5f, zB + dp);
            GameObject NAgent = Instantiate(deuxMains, posi, agentsB1.rotation) as GameObject;
            if (complexIATeamB)
                teamB.Add(NAgent.GetComponent<Agent_deuxMains_complex>());
            else
                teamB.Add(NAgent.GetComponent<Agent_deuxMains>());
            teamB[i + decalage].Init(this, false, i + decalage);
            teamB[i + decalage].gameObject.GetComponent<Agent_deuxMains>().enabled = !complexIATeamB;
            teamB[i + decalage].gameObject.GetComponent<Agent_deuxMains_complex>().enabled = complexIATeamB;
            teamB[i + decalage].gameObject.SetActive(false);

        }
        // Verification du bon dérouelement de la création des combattants à deux mains de la team B
        decalage = teamB.Count;
        if (decalage == nbAgentTeamMage + nbAgentTeamDeuxMains)
            Debug.Log("Pas de soucis avec les deuxMains B");


        // Création des combattants avec un bouclier    
        for (int i = 0; i < nbAgentTeamBouclier; i++)
        {
            Vector3 posi;
            if (spawnSemiAlea)
				posi = new Vector3(xB + Random.Range(0, dl), 0.5f, zB + Random.Range(0, dp));
            else
                posi = new Vector3(xB + i * dl / nbAgentTeamBouclier, 0.5f, zB );
            GameObject NAgent = Instantiate(bouclier, posi, agentsB1.rotation) as GameObject;
            if (complexIATeamB)
                teamB.Add(NAgent.GetComponent<Agent_bouclier_complex>());
            else
                teamB.Add(NAgent.GetComponent<Agent_bouclier>());
            teamB[i + decalage].Init(this, false, i + decalage);
            teamB[i + decalage].gameObject.GetComponent<Agent_bouclier>().enabled = !complexIATeamB;
            teamB[i + decalage].gameObject.GetComponent<Agent_bouclier_complex>().enabled = complexIATeamB;
            teamB[i + decalage].gameObject.SetActive(false);

			teamB[i + decalage].gameObject.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>().material.SetTexture("_MainTex", knightB);
			teamB[i + decalage].gameObject.transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().material.SetTexture("_MainTex", knightB);
			teamB[i + decalage].gameObject.transform.GetChild(2).GetComponent<SkinnedMeshRenderer>().material.SetTexture("_MainTex", knightB);
        }
        // Verification du bon déroulement de la création des combattants avec un bouclier de la team B
        decalage = teamB.Count;
        if (decalage == nbAgentTeam)
            Debug.Log("Pas de soucis avec les boubou B");

        // Maintenant on active les agents
        for (int i=0;i<teamA.Count;i++)
            teamA[i].gameObject.SetActive(true);
        for (int i = 0; i < teamB.Count; i++)
            teamB[i].gameObject.SetActive(true);


    }

	/// <Summary>
    /// Cette fonction est appélée automatiquement une fois par frame
    /// </Summary>
    void Update ()
    {
        // Update du centre de gravité de la team A
        Vector3 temp = new Vector3();
        for (int i = 0; i < teamA.Count; i++)
			if(teamA[i] != null)
            	temp += teamA[i].transform.position;
        this.centreDeGraviteA = temp / teamA.Count;
        // Update du centre de gravité de la team B
        temp = new Vector3();
        for (int i = 0; i < teamB.Count; i++)
			if(teamB[i] != null)
            	temp += teamB[i].transform.position;
        this.centreDeGraviteB = temp / teamB.Count;
    }
	
	/// <Summary>
	/// Cette fonction retourne une copie de la liste des agents de l'équipe A 
    /// </Summary>
    public List<Agent> getTeamA()
    {
        return new List<Agent>(this.teamA);
    }	
    /// <Summary>
	/// Cette fonction retourne une copie de la liste des agents de l'équipe B
    /// </Summary>
    public List<Agent> getTeamB()
    {
        return new List<Agent>(this.teamB);
    }
	
	/// <Summary>
	/// Cette fonction retourne le nombre d'agents de l'équipe A si teamVoulueA est vrai ou de l'équipe B sinon qui sont à une portée de "portee" de l'agent "deamndeur"
    /// </Summary>
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

	/// <Summary>
    /// Cette fonction est appelée par l'agent attaquant losqu'il souhaite attaquer l'agent attaque. 
    /// Le mmonde résout l'attaque avec une probabilité de toucher en fonction du type des deux agents
    /// et change la vie de l'agent attaque, si l'attaque est réussie
    /// </Summary>
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
				Debug.LogError("Probleme pour l'attaque");
                break;
        }
        if (reussi)
        {
            Debug.Log("attaque reussie");
            attaque.SetEtat(attaque.GetEtat() - 1);
        }
    }
	
	/// <Summary>
    /// Cette fonction retourne le nombre d'agents de l'équipe A
    /// </Summary>
    public int GetNbTeamA()
    {
        return this.teamA.Count;
    }
	/// <Summary>
    /// Cette fonction retourne le nombre d'agents de l'équipe B
    /// </Summary>
    public int GetNbTeamB()
    {
        return this.teamB.Count;
    }
	/// <Summary>
    /// Cette fonction sert à retirer un agent mort de la liste de son équipe
    /// Elle est appelée par l'agent concerné
    /// </Summary>
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

	/// <Summary>
    /// Ressors la liste des ennemis à portée
    /// </Summary>
    public List<Agent> EnnemisADisance(Agent attaquant)
    {
        if (attaquant.equipeA)
            return EnnemisADistanceB(attaquant);
        else
            return EnnemisADistanceA(attaquant);
    }
 	/// <Summary>
    /// Cette fonction ressort la liste des agents de l'équipe A à portée de l'agent "attaquant".
    /// </Summary>   
    private List<Agent> EnnemisADistanceA(Agent attaquant)
    {
        List<Agent> ennemi = new List<Agent>();
        for (int i = 0; i < teamA.Count; i++)
        {
			if (Vector3.Distance(attaquant.transform.position, teamA[i].transform.position) < attaquant.portee)
                ennemi.Add(teamA[i]);
        }
        return ennemi;
    }
    /// <Summary>
    /// Cette fonction ressort la liste des agents de l'équipe B à portée de l'agent "attaquant".
    /// </Summary>
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
    /// <Summary>
    /// Cette fonction ressors un agent de l'équipe B à portée
    /// ou le premier de la liste si il n'y a pas d'agent à portée
    /// </Summary>
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
    /// <Summary>
    /// Cette fonction ressors un agent de l'équipe A à portée
    /// </Summary>
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
    
    /// <Summary>
    /// Cette fonction retourne un ennemi à portée de l'agent "sujet"
    /// </Summary>
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
