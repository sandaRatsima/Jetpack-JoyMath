using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int totalProblems = 10;       // Total number of problems to generate
    public float timePerProblem = 100000.0f;   // remettre à 8
    public float remainingTime;          // Time remaining for current problem
    public PlayerController player;       // Reference to the player

    public Problem[] problems;            // Array of all problems (now auto-generated)
    public int curProblem;                // Current problem index

    // Instance for singleton pattern
    public static GameManager instance;

    void Awake()
    {
        // Set the instance to this script
        instance = this;
    }

    void Start()
    {
        // Generate random problems
        GenerateAllProblems();

        // Set the initial problem
        SetProblem(0);
    }

    void Update()
    {
        remainingTime -= Time.deltaTime;

        // Has time run out?
        if (remainingTime <= 0.0f)
        {
            Lose();
        }
    }

    // Generates all problems at the start of the game
    void GenerateAllProblems()
    {
        problems = new Problem[totalProblems];
        for (int i = 0; i < totalProblems; i++)
        {
            problems[i] = GenerateRandomProblem();
        }
    }

    // Creates a single random math problem
    Problem GenerateRandomProblem()
    {
        Problem problem = new Problem();

        problem.firstNumber = Random.Range(1, 10);
        problem.secondNumber = Random.Range(1, 10);

        problem.operation = (MathsOperation)Random.Range(0, 4);

        float correctAnswer = CalculateAnswer(problem.firstNumber, problem.secondNumber, problem.operation);

        problem.answers = GenerateRandomAnswers(correctAnswer, problem.operation);
        for (int i = 0; i < problem.answers.Length; i++)
        {
            if (problem.answers[i] == correctAnswer)
            {
                problem.correctTube = i;
            }
        }
        Debug.Log(problem.correctTube);

        return problem;
    }

    // Calculates the correct answer for a problem
    float CalculateAnswer(float a, float b, MathsOperation op)
    {
        switch (op)
        {
            case MathsOperation.Addition: return a + b;
            case MathsOperation.Subtraction: return a - b;
            case MathsOperation.Multiplication: return a * b;
            case MathsOperation.Division: return a / b;
            default: return 0;
        }
    }

    // Generates 4 answers (1 correct, 3 incorrect)
    float[] GenerateRandomAnswers(float correctAnswer, MathsOperation op)
    {
        float[] answers = new float[4];
        int correctIndex = Random.Range(0, 3);

        // Fill array with wrong answers first
        for (int i = 0; i < 4; i++)
        {
            if (i == correctIndex)
            {
                answers[i] = correctAnswer;
            }
            else
            {
                // Generate plausible wrong answers
                float offset = Random.Range(1, 4);
                if (Random.value > 0.5f) offset *= -1;

                // For division, keep answers as multiples
                if (op == MathsOperation.Division)
                {
                    answers[i] = correctAnswer + (int)offset;
                }
                else
                {
                    answers[i] = correctAnswer + offset;
                }
            }
        }

        return answers;
    }

    // Called when player enters a tube
    public void OnPlayerEnterTube(int tube)
    {
        if (tube == problems[curProblem].correctTube)
            CorrectAnswer();
        else
            IncorrectAnswer();
    }

    void CorrectAnswer()
    {
        // Is this the last problem?
        if (curProblem == problems.Length - 1)
            Win();
        else
            SetProblem(curProblem + 1);
    }

    void IncorrectAnswer()
    {
        player.Stun();
    }

    void SetProblem(int problemIndex)
    {
        curProblem = problemIndex;
        UI.instance.SetProblemText(problems[curProblem]);
        remainingTime = timePerProblem;
    }

    void Win()
    {
        Time.timeScale = 0.0f;
        UI.instance.SetEndText(true);
    }

    void Lose()
    {
        Time.timeScale = 0.0f;
        UI.instance.SetEndText(false);
    }
}