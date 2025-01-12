using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    public void Game()
    {
        AudioManager.Instance.PlaySE("decide");
        SceneManager.LoadScene("Demo");
    }
}
