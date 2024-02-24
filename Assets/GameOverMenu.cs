using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Bardent
{
    public class GameOverMenu : MonoBehaviour
    {
        public void Restart()
        {
            GameManager.gm.firstTime = false;
            StartCoroutine(RestartRoutine());
        }

        private IEnumerator RestartRoutine()
        {
            var asyncLevelLoad = SceneManager.LoadSceneAsync("Game", LoadSceneMode.Single);
            yield return new WaitUntil(() => asyncLevelLoad.isDone);
            GameManager.gm.playerObj.SetActive(true);
            GameManager.gm.playerObj.GetComponent<PlayerHealth>().StartPlayer();
            Bardent.Camera.Instance.GetComponent<CinemachineVirtualCamera>().m_Follow = GameManager.gm.playerObj.transform;
        }

        public void MainMenu()
        {
            GameManager.gm.firstTime = false;
            SceneManager.LoadScene("Menu");
            GameManager.gm.playerObj.GetComponent<PlayerHealth>().StartPlayer();
        }

        public void Quit()
        {
            Application.Quit();
        }
    }
}
