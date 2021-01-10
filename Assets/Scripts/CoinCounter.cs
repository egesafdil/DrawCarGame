using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinCounter : MonoBehaviour
{
    private int coinPoint = 0;
    private Text coinText;

    // Start is called before the first frame update
    void Start()
    {
        coinText = GetComponent<Text>();
    }

    private void FixedUpdate()
    {
        coinText.text = coinPoint.ToString();
    }

    public void IncreaseCoin(int coinPoint)
    {
        this.coinPoint += coinPoint;
    }

}
