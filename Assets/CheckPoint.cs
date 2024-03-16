using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bardent
{
    public class CheckPoint : MonoBehaviour
    {
        PlayerHealth playerHealth;
        public Transform respawnPoint;

        private void Awake()
        {
            playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                playerHealth.UpdateCheckpoint(respawnPoint.position);
            }
        }
    }
}
