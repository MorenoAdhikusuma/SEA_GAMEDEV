using UnityEngine;
using UnityEngine.SceneManagement;

public class Newgame : MonoBehaviour
{
    public void Playgame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
