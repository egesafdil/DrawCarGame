using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] int coinPoint = 2;

    CoinCounter coinCounter;
    
    void Start()
    {
        coinCounter = FindObjectOfType<CoinCounter>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Car")
        {
            coinCounter.IncreaseCoin(coinPoint);
            Destroy(gameObject);
        }
    }

}
