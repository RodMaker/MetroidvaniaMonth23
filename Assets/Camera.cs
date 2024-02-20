using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bardent
{
    public class Camera : MonoBehaviour
    {
        public static Camera Instance;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }

            DontDestroyOnLoad(gameObject);
        }
    }
}
