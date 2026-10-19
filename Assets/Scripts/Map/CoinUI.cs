using TMPro;
using UnityEngine;

public class CoinUI : MonoBehaviour
{

    TextMeshProUGUI meshPro;
    string coinText;
    int coin;

    private void Awake()
    {
        meshPro = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        UpdateCoinText();
    }

    public void UpdateCoinText()
    {
        coin = GameController.getCoin();
        coinText = $"x {coin}";
        meshPro.text = coinText;
    }
}
