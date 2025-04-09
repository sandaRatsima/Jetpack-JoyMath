using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public Text problemText;                // texte qui affiche le problème mathématique
    public Text[] answersTexts;             // tableau des 4 textes de réponses
    public Image remainingTimeDial;         // image de temps restant avec remplissage radial
    private float remainingTimeDialRate;    // 1.0 / temps par problème
    public Text endText;                    // texte affiché à la fin du jeu (victoire ou game over)

    // instance
    public static UI instance;

    void Awake ()
    {
        // définir l'instance sur ce script
        instance = this;
    }

    void Start ()
    {
        // définir le taux de remplissage du cadran de temps restant
        // utilisé pour convertir le temps par problème (8 secondes par exemple)
        // et convertit cela pour être utilisé sur une échelle de 0.0 à 1.0
        remainingTimeDialRate = 1.0f / GameManager.instance.timePerProblem;
    }

    void Update ()
    {
        // mettre à jour la quantité de remplissage du cadran de temps restant
        remainingTimeDial.fillAmount = remainingTimeDialRate * GameManager.instance.remainingTime;
    }

    // définit l'interface utilisateur du vaisseau pour afficher le nouveau problème
    public void SetProblemText (Problem problem)
    {
        string operatorText = "";

        // convertir l'opérateur du problème d'une énumération en un symbole de texte réel
        switch(problem.operation)
        {
            case MathsOperation.Addition: operatorText = " + "; break;
            case MathsOperation.Subtraction: operatorText = " - "; break;
            case MathsOperation.Multiplication: operatorText = " × "; break;
            case MathsOperation.Division: operatorText = " ÷ "; break;
        }

        // définir le texte du problème pour afficher le problème
        problemText.text = problem.firstNumber + operatorText + problem.secondNumber;

        // définir les textes des réponses pour afficher les réponses correctes et incorrectes
        for(int index = 0; index < answersTexts.Length; ++index)
        {
            answersTexts[index].text = problem.answers[index].ToString();
        }
    }

    // définit le texte de fin pour afficher si le joueur a gagné ou perdu
    public void SetEndText (bool win)
    {
        // activer l'objet de texte de fin
        endText.gameObject.SetActive(true);

        // le joueur a-t-il gagné ?
        if (win)
        {
            endText.text = "Vous avez gagné !";
            endText.color = Color.green;
        }
        // le joueur a-t-il perdu ?
        else
        {
            endText.text = "Game Over !";
            endText.color = Color.red;
        }
    }
}
