using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    private void Start()
    {
        AudioManager.Instance.PlayBGM("title");
    }

    public void Game()
    {
        AudioManager.Instance.PlaySE("decide");
        SceneManager.LoadScene("Demo");
    }
}
