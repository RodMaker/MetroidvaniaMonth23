using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bardent
{
    public class CrouchUnlocked : MonoBehaviour
    {
        [SerializeField] Player player;

        public GameObject crouchUI;

        private void Start()
        {
            if (player.crouchUnlocked)
            {
                Destroy(gameObject);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                crouchUI.SetActive(true);
                player.crouchUnlocked = true;
                Destroy(crouchUI, 5f);
                Destroy(gameObject);
            }
        }
    }
}
