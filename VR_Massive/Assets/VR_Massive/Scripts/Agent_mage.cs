﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Agent_mage : Agent {

    public Transform desti; // A virer dès qu'on pourra dire à nos agent l'endroit où se déplacer, c'est juste pour les test
    // A mettre obligatoirement au début (appelé lors du Instantiate ou au lancement) du coup obligatoire!!! Sert de constructeur
    void Start() {
        // Le "base" sert a apppeler une méthode/attribut de la classe mère (protected ou public seulement)
        base.StartA();// Obligatoire aussi (initialisation de la classe mère)
        portee = 30; // A changer pour mettre votre portée: j'ai aucune idée de l'unite utilisée donc il faudra faire des test mais j'aurais tendance à dire que on peut dire que c'est des mètres
	}

    // Update is called once per frame=> du coup obligatoire aussi: c'est là dedans qu'il faut la prise de décision....
    void Update ()
    {
        if (base.etat != 10)
        { 
            Agent ennemi = GetEnnemisPortee();
            if (ennemi != null)
            {
                this.Attaquer(ennemi);
            }
            else
            {
                base.LetsMove(desti.position);// Exemple pour le déplacement. Il suffit d'un Vector3.
            }
        }
        else
        {
            // TODO: implémentation de la fuite à faire (en gros mettre une destination en dehors de la map)

            if ((this.transform.position.x > 100) || (this.transform.position.x < -100) || (this.transform.position.z > 100) || (this.transform.position.z < -100))
                base.terrain.Tuer(this, this.equipeA);
            // TODO: checker la sortie de map et appeler Monde.Tuer
        }
	}
}
