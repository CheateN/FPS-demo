using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private GameObject impactEffect;
    private Player player;

    private void Start()
    {
        player = FindObjectOfType<Player>();
    }

    private void OnCollisionEnter(Collision other)
    {
        AudioManager.instance.Play("Explosion"); // 播放射击音效

        GameObject impact = Instantiate(impactEffect, transform.position, Quaternion.identity);
        Destroy(impact, 2f);

        Collider[] colliders = Physics.OverlapSphere(transform.position, 3f);
        foreach (Collider c in colliders)
        {
            if (c.CompareTag("Player")) // 炸到了Player
            {
                StartCoroutine(player.TakeDamage(10));
            }
        }
        Destroy(gameObject, 2f);

    }
}
