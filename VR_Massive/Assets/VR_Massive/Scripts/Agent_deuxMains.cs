using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent_deuxMains : Agent {
    private bool animDeath;
    public Transform desti; // A virer dès qu'on pourra dire à nos agent l'endroit où se déplacer, c'est juste pour les test
    private int nbAttaques = 2;
    
    // A mettre obligatoirement au début (appelé lors du Instantiate ou au lancement) du coup obligatoire!!! Sert de constructeur
    void Start()
    {
        // Le "base" sert a apppeler une méthode/attribut de la classe mère (protected ou public seulement)
        base.StartA();// Obligatoire aussi (initialisation de la classe mère)
        portee = 2; // A changer pour mettre votre portée: j'ai aucune idée de l'unite utilisée donc il faudra faire des test mais j'aurais tendance à dire que on peut dire que c'est des mètres
    }

    // Update is called once per frame=> du coup obligatoire aussi: c'est là dedans qu'il faut la prise de décision....
    // Update is called once per frame=> du coup obligatoire aussi: c'est là dedans qu'il faut la prise de décision....
    void Update()
    {
        if (base.etat > 0)
        {

            if (base.etat != 10)
            {
                int nbEquipe = 0;
                int nbEquipeEn = 0;
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
                if (Random.Range(1, 1000) < 10 * nbEquipeEn / nbEquipe)
                {
                    this.etat = 10;
                    this.LetsMove(this.terrain.fuite.position);
                }
                else
                {
                    List<Agent> ennemis = GetEnnemisPortee();
                    if (ennemis.Count > 0)
                    {
                        this.Attaquer(ennemis);
                    }
                    else
                    {
                        base.LetsMove(desti.position);// Exemple pour le déplacement. Il suffit d'un Vector3.
                    }
                }
            }
            else
            {
                // TODO: implémentation de la fuite à faire (en gros mettre une destination en dehors de la map)
                
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
            // TODO: Animation morts
            if (!animDeath)
            {
                this.anim.SetTrigger("dead");
                this.terrain.Tuer(this);
                Destroy(this.gameObject, 3.5f);
                this.animDeath = true;
            }
        }
    }

    protected void Attaquer (List<Agent> attaque)
    {
        // A completer avec le booleen d'animation 
        int attaqueNumber = Random.Range(1, nbAttaques);
        string anima = "attack" + attaqueNumber;
        this.anim.SetTrigger(anima);
        StartCoroutine(Wait());
        this.terrain.Attaquer(this, attaque[0]);
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
