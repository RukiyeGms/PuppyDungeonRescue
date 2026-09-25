using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeToBlack : MonoBehaviour
{

    float transparency = 0f;
    float toBlackRate = 0.1f;
    float toBlackSpeed = 0.05f;
    [SerializeField] Image fadeBlackPanel;



    void Start()
    {
        
        gameObject.SetActive(true);
        StartCoroutine(FadeBackfromBlack());
        
    }


    // Update is called once per frame
    public IEnumerator FadetoBlack()
    {
        gameObject.SetActive(true);
        transparency = 0;
        ChangeAlpha(transparency);
        
        while (transparency < 1f)
        {
            transparency += toBlackRate;
            ChangeAlpha(transparency);
            yield return new WaitForSeconds(toBlackSpeed);

        }
        

    }

    public IEnumerator FadeBackfromBlack()
    {
        
        transparency = 1;
        ChangeAlpha(transparency);
        while (transparency > 0f)
        {
            transparency -= toBlackRate;
            ChangeAlpha(transparency);
            yield return new WaitForSeconds(toBlackSpeed);
        }
        gameObject.SetActive(false);

    }

    void ChangeAlpha(float a)
    {
        var tempColor = fadeBlackPanel.color;
        tempColor.a = a;
        fadeBlackPanel.color = tempColor;
    }
}
