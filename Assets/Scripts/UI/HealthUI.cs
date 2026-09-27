using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    [SerializeField] Image heathPrefab;
    [SerializeField] Sprite fullHeart;
    [SerializeField] Sprite emptyHeart;

    [SerializeField] Image portrait;
    [SerializeField] Sprite healthyPortrait;
    [SerializeField] Sprite woundedPortrait;

    List<Image> hearts = new List<Image>();

    public void SetMaxHearts(int maxHearts)
    {
        portrait.sprite = healthyPortrait;

        foreach (Image heart in hearts)
        {
            Destroy(heart.gameObject);
        }

        hearts.Clear();

        for (int i = 0; i < maxHearts; i++)
        {
            //!!!!!!!transform sets the parent of this heartprefab as heart container
            Image newHeart = Instantiate(heathPrefab, transform);
            newHeart.sprite = fullHeart;
            hearts.Add(newHeart);
        }
    }

    public void UpdateHearts(int currenthealth)
    {
        if (currenthealth <= 1) { portrait.sprite = woundedPortrait; }
        else { portrait.sprite = healthyPortrait; }

            for (int i = 0; i < hearts.Count; i++)
            {

                if (hearts[i] != null)
                {
                    if (i < currenthealth)
                    {
                        hearts[i].sprite = fullHeart;
                    }
                    else
                    {
                        hearts[i].sprite = emptyHeart;
                    }
                }
            }
    }
}
