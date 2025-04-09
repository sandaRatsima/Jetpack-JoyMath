using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void ExitButton() {
        Application.Quit();
        Debug.Log("Jeu Fermé !");
    }

    public void StartGame() {
        SceneManager.LoadScene("Game");
    }
}
