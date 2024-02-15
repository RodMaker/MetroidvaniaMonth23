using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bardent
{
    public class JumpUnlocked : MonoBehaviour
    {
        [SerializeField] PlayerInputHandler playerInputHandler;

        public GameObject jumpUI;

        private void Start()
        {
            if (playerInputHandler.jumpUnlocked)
            {
                Destroy(gameObject);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                jumpUI.SetActive(true);
                playerInputHandler.jumpUnlocked = true;
                Destroy(jumpUI, 5f);
                Destroy(gameObject);
            }
        }
    }
}
