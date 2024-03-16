using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Bardent
{
    public class UIManager : MonoBehaviour
    {
        public GameObject gameOverMenu;

        /*
        private void OnEnable()
        {
            PlayerHealth.OnPlayerDeath += EnableGameOverMenu;
        }

        private void OnDisable()
        {
            PlayerHealth.OnPlayerDeath -= EnableGameOverMenu;
        }

        public void EnableGameOverMenu()
        {
            gameOverMenu.SetActive(true);
        }
        */

        public void Restart()
        {
            SceneManager.LoadScene("Game");
        }

        public void MainMenu()
        {
            SceneManager.LoadScene("Menu");
        }

        public void Quit()
        {
            Application.Quit();
        }
    }
}
