using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Problem
{
    public float firstNumber;           // premier nombre dans le problème
    public float secondNumber;          // deuxième nombre dans le problème
    public MathsOperation operation;    // opérateur entre les deux nombres
    public float[] answers;             // tableau de toutes les réponses possibles, y compris la bonne

    [Range(0, 3)]
    public int correctTube;             // indice du tube correct
}

public enum MathsOperation
{
    Addition,        // Addition
    Subtraction,     // Soustraction
    Multiplication,  // Multiplication
    Division         // Division
}
