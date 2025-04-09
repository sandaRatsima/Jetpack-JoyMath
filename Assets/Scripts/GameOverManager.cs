using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public void ReturnToMenu()
    {
        SceneManager.LoadScene("MainMenu"); // Mets ici le nom exact de ta scène du menu
    }
}
