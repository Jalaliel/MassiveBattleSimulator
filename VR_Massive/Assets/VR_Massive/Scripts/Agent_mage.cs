using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Agent_mage : Agent {

    private bool animDeath;
    public Transform desti;
    // A mettre obligatoirement au début (appelé lors du Instantiate ou au lancement)
    void Start() {
        // Le "base" sert a apppeler une méthode/attribut de la classe mère (protected ou public seulement)
        base.StartA();// Initialisation de la classe mère
        portee = 10; 
        animDeath = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (base.etat > 0) // Si l'agent est encore vivant
        {
            if (!this.enFuite) // Si l'agent n'est pas en fuite
            {
                int nbEquipe = 0;
                int nbEquipeEn = 0;
                // Calcul des nombres d'agent de chaque équipe 
                if (equipeA)
                {
                    nbEquipe = this.terrain.GetNbTeamA();
                    nbEquipeEn = this.terrain.GetNbTeamB();
                }
                else
                {
                    nbEquipe = this.terrain.GetNbTeamB();
                    nbEquipeEn = this.terrain.GetNbTeamA();
                }
                if ((Random.Range(1, 10000) < 10 * nbEquipeEn / nbEquipe) && (nbEquipeEn >= 2 * nbEquipe)) //Calcul de la peur et fuite si l'agent a peur
                {
                    this.enFuite = true; ;
                    this.LetsMove(this.terrain.fuite.position);
                }
                else // Sinon, si on au moins un ennemi à portée, on attaque sinon on se déplace
                {
                    List<Agent> ennemis = GetEnnemisPortee();
                    if (ennemis.Count > 0)
                    {
                        this.Attaquer(ennemis);
                    }
                    else
                    {
                        base.LetsMove(terrain.EnnemisProche(this).transform.position);
                    }
                }
            }
            else // Dans le cas où l'agent est en fuite, on vérifie si il arrive au bord du terrain et dans ce cas, on le détruit
            {
                if ((this.transform.position.x > 99) || (this.transform.position.x < -99) || (this.transform.position.z > 99) || (this.transform.position.z < -99))
                {
                    base.terrain.Tuer(this);
                    Destroy(this.gameObject);
                }
            }
        }
        else
        {
            this.moving = false;
            this.anim.SetBool("Moving", false);
            //Animation de la mort et destruction de l'agent
            if (!animDeath)
            {
                this.anim.SetTrigger("dead");
                this.terrain.Tuer(this);
                Destroy(this.gameObject, 3.5f);
                this.animDeath = true;
            }
        }
    }

    /// <summary>
    /// Fonction qui sert normalement à décider quel ennemi, parmi la liste, attaquer
    /// </summary>
    protected void Attaquer(List<Agent> attaque)
    {
        // A completer avec le booleen d'animation 
        this.moving = false;
        this.anim.SetBool("Moving", false);
        this.anim.SetTrigger("attack");
        StartCoroutine(Wait());
        if (Random.Range(1, 10) <= 1) { this.terrain.Attaquer(this, attaque[0]); }
        
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(3.5f);
    }

    void Hit()
    {

    }

    void FootR()
    {
    }

    void FootL()
    {
    }

}
