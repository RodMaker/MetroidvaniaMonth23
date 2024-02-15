using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bardent
{
    public class SavePoint : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                GameManager.gm.Save();
            }
        }
    }
}
