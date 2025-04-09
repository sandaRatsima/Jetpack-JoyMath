using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Problem[] problems;      // liste de tous les problèmes
    public int curProblem;          // problème actuel que le joueur doit résoudre
    public float timePerProblem;    // temps autorisé pour répondre à chaque problème

    public float remainingTime;     // temps restant pour le problème actuel

    public PlayerController player; // objet joueur

    // instance
    public static GameManager instance;

    void Awake ()
    {
        // définir l'instance sur ce script.
        instance = this;
    }

    void Start ()
    {
        // définir le problème initial
        SetProblem(0);
    }

    void Update ()
    {
        remainingTime -= Time.deltaTime;

        // le temps restant s'est-il écoulé ?
        if(remainingTime <= 0.0f)
        {
            Lose();
        }
    }

    // appelé lorsque le joueur entre dans un tube
    public void OnPlayerEnterTube (int tube)
    {
        // est-il entré dans le tube correct ?
        if (tube == problems[curProblem].correctTube)
            CorrectAnswer();
        else
            IncorrectAnswer();
    }

    // appelé lorsque le joueur entre dans le bon tube
    void CorrectAnswer()
    {
        // est-ce le dernier problème ?
        if(problems.Length - 1 == curProblem)
            Win();
        else
            SetProblem(curProblem + 1);
    }

    // appelé lorsque le joueur entre dans le mauvais tube
    void IncorrectAnswer ()
    {
        player.Stun();
    }

    // définit le problème actuel
    void SetProblem (int problem)
    {
        curProblem = problem;
        UI.instance.SetProblemText(problems[curProblem]);
        remainingTime = timePerProblem;
    }

    // appelé lorsque le joueur répond à tous les problèmes
    void Win ()
    {
        Time.timeScale = 0.0f;
        UI.instance.SetEndText(true);
    }

    // appelé si le temps restant pour un problème atteint 0
    void Lose ()
    {
        Time.timeScale = 0.0f;
        UI.instance.SetEndText(false);
    }
}
