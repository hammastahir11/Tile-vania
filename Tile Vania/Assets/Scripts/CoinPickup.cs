using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    [SerializeField] AudioClip coinPickupSFX;
    [SerializeField] int pointsForCoin = 10;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        FindObjectOfType<GameSession>().AddToScore(pointsForCoin);
        AudioSource.PlayClipAtPoint(coinPickupSFX,Camera.main.transform.position);
        Destroy(gameObject);
    }

}
