using Bardent.Weapons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Bardent.Interaction.Interactables
{
    public class GrabUnlocked : MonoBehaviour
    {
        [SerializeField] PlayerInputHandler playerInputHandler;

        public GameObject grabUI;

        private void Start()
        {
            if (playerInputHandler.grabUnlocked)
            {
                Destroy(gameObject);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        { 
            if (other.gameObject.CompareTag("Player"))
            {
                grabUI.SetActive(true);
                playerInputHandler.grabUnlocked = true;
                Destroy(grabUI, 5f);
                Destroy(gameObject);
            }
        } 
    }
}
