using UnityEngine;
using UnityEngine.UI;

public class HealthView : MonoBehaviour
{
    [SerializeField] private Image[] heartImages;

    public void ShowHearts(int hp)
    {
        for (int i = heartImages.Length - 1; i >= 0; i--)
        {
            heartImages[i].gameObject.SetActive(i < hp);
        }
    }
}
