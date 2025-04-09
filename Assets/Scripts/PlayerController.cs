using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PlayerState curState;            // état actuel du joueur

    // valeurs
    public float moveSpeed;                 // force appliquée horizontalement lors du déplacement
    public float flyingSpeed;               // force appliquée vers le haut lors du vol
    public bool grounded;                   // le joueur est-il actuellement sur le sol ?
    public float stunDuration;              // durée d'une étourdissement
    private float stunStartTime;            // moment où le joueur a été étourdi

    // composants
    public Rigidbody2D rig;                 // composant Rigidbody2D
    public Animator anim;                   // composant Animator
    public ParticleSystem jetpackParticle;  // ParticleSystem du jetpack

    void FixedUpdate ()
    {
        grounded = IsGrounded();
        CheckInputs();

        // le joueur est-il étourdi ?
        if(curState == PlayerState.Stunned)
        {
            // le joueur a-t-il été étourdi pendant la durée spécifiée ?
            if(Time.time - stunStartTime >= stunDuration)
            {
                curState = PlayerState.Idle;
            }
        }
    }

    // vérifie les entrées utilisateur pour contrôler le joueur
    void CheckInputs ()
    {
        if (curState != PlayerState.Stunned)
        {
            // mouvement
            Move();

            // vol
            if (Input.GetKey(KeyCode.UpArrow))
                Fly();
            else
                jetpackParticle.Stop();
        }

        // mettre à jour notre état actuel
        SetState();
    }

    // définit l'état du joueur
    void SetState ()
    {
        // ne pas changer d'état si le joueur est étourdi
        if (curState != PlayerState.Stunned)
        {
            // immobile
            if (rig.linearVelocity.magnitude == 0 && grounded)
                curState = PlayerState.Idle;
            // marche
            if (rig.linearVelocity.x != 0 && grounded)
                curState = PlayerState.Walking;
            // vol
            if (rig.linearVelocity.magnitude != 0 && !grounded)
                curState = PlayerState.Flying;
        }

        // informer l'animateur que nous avons changé d'état
        anim.SetInteger("State", (int)curState);
    }

    // déplace le joueur horizontalement
    void Move ()
    {
        // obtenir l'axe horizontal (A & D, flèche gauche & flèche droite)
        float dir = Input.GetAxis("Horizontal");

        // retourner le joueur pour qu'il regarde dans la direction où il se déplace
        if (dir > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (dir < 0)
            transform.localScale = new Vector3(-1, 1, 1);

        // définir la vélocité horizontale du Rigidbody
        rig.linearVelocity = new Vector2(dir * moveSpeed, rig.linearVelocity.y);
    }

    // ajoute une force vers le haut au joueur
    void Fly ()
    {
        // ajouter une force vers le haut
        rig.AddForce(Vector2.up * flyingSpeed, ForceMode2D.Impulse);

        // jouer l'effet de particules du jetpack
        if (!jetpackParticle.isPlaying)
            jetpackParticle.Play();
    }

    // appelé lorsque le joueur est étourdi
    public void Stun ()
    {
        curState = PlayerState.Stunned;
        rig.linearVelocity = Vector2.down * 3;
        stunStartTime = Time.time;
        jetpackParticle.Stop();
    }

    // retourne true si le joueur est sur le sol, false sinon
    bool IsGrounded ()
    {
        // lancer un rayon vers le bas sous le joueur
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - 0.85f), Vector2.down, 0.3f);

        // avons-nous touché quelque chose ?
        if(hit.collider != null)
        {
            // était-ce le sol ?
            if(hit.collider.CompareTag("Floor"))
            {
                return true;
            }
        }

        return false;
    }

    // appelé lorsque le joueur entre en collision avec le collider d'un autre objet
    //void OnTriggerEnter2D (Collider2D col)
    //{
    //    // si le joueur n'est pas déjà étourdi, étourdissez-le si l'objet était un obstacle
    //    if(curState != PlayerState.Stunned)
    //    {
    //        if(col.CompareTag("Obstacle"))
    //        {
    //            Stun();
    //        }
    //    }
    //}
}

public enum PlayerState
{
    Idle,       // 0
    Walking,    // 1
    Flying,     // 2
    Stunned     // 3
}
