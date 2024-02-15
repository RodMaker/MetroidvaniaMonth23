using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Bardent
{
    public class PauseMenuManager : MonoBehaviour
    {
        bool isPaused;
        public GameObject PauseMenu;

        // Start is called before the first frame update
        void Start()
        {
            isPaused = false;
        }

        public void OnTogglePause(InputAction.CallbackContext ctxt)
        {
            if (ctxt.performed)
            {
                isPaused = !isPaused;
                if (isPaused )
                {
                    Time.timeScale = 0;
                    PauseMenu.SetActive(true);
                }
                else
                {
                    Time.timeScale = 1;
                    PauseMenu.SetActive(false);
                }
            }
        }

        public void OnOK()
        {
            isPaused = false;
            Time.timeScale = 1;
            PauseMenu.SetActive(false);
        }
    }
}
