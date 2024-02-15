using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bardent
{
    public class DashUnlocked : MonoBehaviour
    {
        [SerializeField] PlayerInputHandler playerInputHandler;

        public GameObject dashUI;

        private void Start()
        {
            if (playerInputHandler.dashUnlocked)
            {
                Destroy(gameObject);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                dashUI.SetActive(true);
                playerInputHandler.dashUnlocked = true;
                Destroy(dashUI, 5f);
                Destroy(gameObject);
            }
        }
    }
}
