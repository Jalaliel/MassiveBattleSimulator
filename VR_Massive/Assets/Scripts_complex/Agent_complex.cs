using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Agent_complex : Agent {

	private bool animDeath; //Vrai si le processus pour tuer l'agent a été lancé (pour éviter de le lancer plusieurs fois)

	/// <summary>
	/// Nombre d'ennemis dans l'échantillon dans lequel on choisi une cible dans choisirPlusProche
	/// </summary>
	public int nbEchantillon = 10;  

	/// <summary>
	/// La distance maximale à laquelle un adversaire doit être du bord du terrain pour être considéré comme sorti
	/// </summary>
	public int distanceAvantSortie = 1;

	/// <summary>
	/// Le nombre de frames avant que la valeur de peur ne soit à nouveau calculée
	/// </summary>
	public int nbFrameRefresh = 100;
	private int compteurPeur; //Le nombre de frames depuis lesquelles la peur a été calculée

	/// <summary>
	/// Le rayon de la zone autour de l'agent dans laquelle il vérifie si son équipe est en supériorité numérique, pour le calcul de la peur
	/// </summary>
	public double viewDistance = 5.0;

	/// <summary>
	/// Variable de débuguage servant à connaitre l'action en cours de l'agent
	/// </summary>
	public string reactEnCours = "";

	/// <summary>
	/// Initialisation des agents
	/// </summary>
	public virtual void Start()
	{
		animDeath = false;
		compteurPeur = Random.Range(0, nbFrameRefresh - 1);
	}

	/// <summary>
	/// Appelée à chaque frame, cette fonction entraine la prise de décision de l'agent
	/// </summary>
	void Update()
	{
		this.moving = false;
		this.anim.SetBool ("Moving", false);
		priseDeDecision ();
	}

	/// <summary>
	/// Méthode interne servant à mettre en pause l'agent
	/// </summary>
	/// <param name="time">Le temps en secondes durant lequel le mettre en pause</param>
	protected IEnumerator Wait(float time)
	{
		yield return new WaitForSeconds(time);
	}

	/// <summary>
	/// Méthode jouant l'animation de mort puis détruisant l'agent
	/// </summary>
	/// <param name="timeToDie">Le temps durant lequel l'agent reste mort avant de disparaitre</param>
	protected void mourir(float timeToDie)
	{
		if(!animDeath)
		{
			this.terrain.Tuer(this);
			this.animDeath = true;
			this.anim.SetTrigger("dead");
			Destroy(this.gameObject, timeToDie);
			Wait (timeToDie);
		}
	}

	/// <summary>
	/// Vérifie si l'agent est trop proche du bord du terrain, auquel cas il doit être supprimé
	/// </summary>
	protected bool sorti()
	{
		return (this.transform.position.x > 100 - distanceAvantSortie) || (this.transform.position.x < -(100 - distanceAvantSortie)) || (this.transform.position.z > (100 - distanceAvantSortie)) || (this.transform.position.z < -(100 - distanceAvantSortie));
	}

	/// <summary>
	/// Détruit immédiatement l'agent (sans animation de mort)
	/// </summary>
	protected void disparaitre()
	{
		base.terrain.Tuer(this);
		Destroy(this.gameObject);
	}

	/// <summary>
	/// Provoque la fuite de l'agent, à l'opposé du centre de gravité des forces adverses
	/// </summary>
	protected void fuir()
	{
		Vector3 center;
		if (equipeA)
			center = terrain.centreDeGraviteB;
		else
			center = terrain.centreDeGraviteA;
		LetsMove (this.transform.position * 2 - center);
	}

	/// <summary>
	/// Vérifie si l'agent décide de fuir ou pas
	/// </summary>
	protected bool peur()
	{
		compteurPeur++;
		if (compteurPeur >= nbFrameRefresh) {
			compteurPeur = 0;
			int chanceFuite = 0;
			switch (etat) {
			case 1:
				chanceFuite += 3;
				break;
			case 2:
				chanceFuite++;
				break;
			}
			if (enFuite)
				chanceFuite += 5;
			int nbAllies = terrain.nbAPortee (this.equipeA, this, viewDistance);
			int nbEnnemis = terrain.nbAPortee (!(this.equipeA), this, viewDistance);
			if (nbAllies < nbEnnemis)
				chanceFuite++;
			if (nbAllies < nbEnnemis * 2)
				chanceFuite++;
			enFuite = (Random.Range (0, 10) < chanceFuite);
		}
		return enFuite;
	}

	/// <summary>
	/// Effectue la prise de décision et la réalisation de l'action associée
	/// </summary>
	protected void priseDeDecision()
	{
		reactEnCours = "Prise de décision";
		if (etat <= 0) 
		{
			reactEnCours = "mourir";
			mourir (3.5f);
		} 
		else if (sorti ()) 
		{
			reactEnCours = "disparaitre";
			disparaitre ();
		} 
		else if (peur ()) 
		{
			reactEnCours = "fuir";
			fuir ();
		}
		else if (!selectTaper ()) 
		{
			reactEnCours = "move";
			Vector3 dest = choisirPlusProche ();
			LetsMove (dest);
		} 
		else
			reactEnCours = "taper";
	}

	/// <summary>
	/// Retourne le type d'agents contre lequel l'agent est fort sous forme de string
	/// </summary>
	/// <returns>Le type d'agents contre lequel l'agent est fort</returns>
	protected abstract string fortContre ();

	/// <summary>
	/// Selectionne un agent à taper et le tape.
	/// </summary>
	/// <returns><c>true</c>, si un agent a été tapé, <c>false</c> sinon (si aucun adversaire n'est à portée).</returns>
	protected abstract bool selectTaper();

	/// <summary>
	/// Choisi la destination vers laquelle l'agent doit aller
	/// </summary>
	protected Vector3 choisirPlusProche()
	{
		List<Agent> ennemis;
		if (equipeA)
			ennemis = terrain.getTeamB ();
		else
			ennemis = terrain.getTeamA ();

		int size = ennemis.Count;
		if (size == 0)//Si il n'y a pas d'ennemis, l'agent ne bouge pas
			return this.transform.position;
		else//Sinon il tire un échantillon d'ennemis au hasard dans la liste de ceux-ci, et il va vers celui contre lequel il est le plus fort d'abord, le plus proche ensuite
		{
			if (size > nbEchantillon)
				ennemis = tirerNDans (nbEchantillon, ennemis);

			bool agentFaible = ennemis [0].GetType ().Name.Contains (fortContre ());
			Agent aViser = ennemis[0];
			float dist = Vector3.Distance(this.transform.position, ennemis[0].transform.position);
			size = ennemis.Count;
			for (int i = 1; i < size; i++)
			{
				bool cetAgentEstFaible = ennemis [i].GetType ().Name.Contains (fortContre ());
				if (!agentFaible && cetAgentEstFaible) 
				{
					agentFaible = true;
					aViser = ennemis [i];
					dist = Vector3.Distance(this.transform.position, ennemis[i].transform.position);
				}
				else if(!(agentFaible && !cetAgentEstFaible))
				{
					float distCetAgent = dist = Vector3.Distance (this.transform.position, ennemis [i].transform.position);
					if (distCetAgent < dist) 
					{
						dist = distCetAgent;
						aViser = ennemis [i];
					}
				}
			}
			return aViser.transform.position;
		}
	}

	/// <summary>
	/// Méthode interne, tire nb agents aléatoirement dans la liste
	/// </summary>
	/// <returns>Les agents tirés aléatoirement</returns>
	/// <param name="nb">Le nombre d'agents à tirer</param>
	/// <param name="list">La liste dans laquelle tirer</param>
	private List<Agent> tirerNDans(int nb, List<Agent> list)
	{
		List<Agent> listCopiee = new List<Agent> (list);
		int size = list.Count;
		List<Agent> retour = new List<Agent>();
		for (int i = 0; i < nb; i++)
		{
			int indice = Random.Range (0, size - 1);
			retour.Add (listCopiee [indice]);
			listCopiee.Remove(listCopiee[indice]);
			size--;
		}
		return retour;
	}

	/// <summary>
	/// Provoque le déplacement de l'agent vers la position
	/// </summary>
	/// <param name="position">Le point vers lequel doit se déplacer l'agent</param>
	protected override void LetsMove (Vector3 position)
	{
		this.moving = true;
		this.anim.SetBool ("Moving", true);
		base.LetsMove (position);
	}

	/// <summary>
	/// Méthode interne nécessaire à l'utilisation du navmesh
	/// </summary>
	void Hit()
	{

	}

	/// <summary>
	/// Méthode interne nécessaire à l'utilisation du navmesh
	/// </summary>
	void FootR()
	{
	}

	/// <summary>
	/// Méthode interne nécessaire à l'utilisation du navmesh
	/// </summary>
	void FootL()
	{
	}
}