using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Bardent
{
    public class GameOverMenu : MonoBehaviour
    {
        public static GameOverMenu Instance;

        private void Awake()
        {
            Debug.Log("IM HERE");
            Instance = this;
        }
    }
}
