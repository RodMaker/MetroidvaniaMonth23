using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bardent
{
    public class ProjectileDamage : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.name == "Player")
            {
                PlayerHealth.Instance.TakeDamage(1);
            }
        }
    }
}
