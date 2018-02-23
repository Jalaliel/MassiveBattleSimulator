using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Classe qui sert au déplacement des agents
/// </summary>

/*[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]*/
public class Move : MonoBehaviour
{

    NavMeshAgent nevAgent;
    private Vector2 smoothDeltaPosition = Vector2.zero;
    private Vector2 velocity = Vector2.zero;
    Animator anim;
    Agent agent;

    /// <summary>
    /// Constructeur
    /// </summary>
    public Move(NavMeshAgent nav, Animator an, Agent ag)
    {
        nevAgent = nav;
        anim = an;
        agent = ag;
    }

    /// <summary>
    /// Fonction qui sert à déplacer l'agent vers la destination contenue dans le NavMeshAgent
    /// </summary>
    public void LetsMove()
    {
        Vector3 worldDeltaPosition = nevAgent.nextPosition - agent.transform.position;

        // Map 'worldDeltaPosition' to local space
        float dx = Vector3.Dot(agent.transform.right, worldDeltaPosition);
        float dy = Vector3.Dot(agent.transform.forward, worldDeltaPosition);
        Vector2 deltaPosition = new Vector2(dx, dy);

        // Low-pass filter the deltaMove
        float smooth = Mathf.Min(1.0f, Time.deltaTime / 0.15f);
        smoothDeltaPosition = Vector2.Lerp(smoothDeltaPosition, deltaPosition, smooth);

        // Update velocity if time advances
        if (Time.deltaTime > 1e-5f)
            velocity = smoothDeltaPosition / Time.deltaTime;

        bool shouldMove = velocity.magnitude > 1.5f && nevAgent.remainingDistance > nevAgent.radius;

        // Update animation parameters
        agent.anim.SetBool("Moving", shouldMove);
    }

}

