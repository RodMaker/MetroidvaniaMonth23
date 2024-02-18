using Bardent.CoreSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Bardent
{
    public class EnemyHealth : MonoBehaviour
    {
        [SerializeField] private int startingHealth = 3;
        //[SerializeField] private GameObject deathVFXPrefab;
        //[SerializeField] private float knockBackThrust = 15f;
        [SerializeField] private int expAmount = 100;

        private int currentHealth;
        //private Knockback knockback;
        //private Flash flash;

        public bool isBoss = false;
        public GameObject floatingText;

        [SerializeField] private GameObject[] deathParticles;

        private ParticleManager ParticleManager =>
            particleManager ? particleManager : Enemy1.FindObjectOfType<Core>().GetCoreComponent(ref particleManager);

        private ParticleManager particleManager;

        /*
        private void Awake()
        {
            flash = GetComponent<Flash>();
            knockback = GetComponent<Knockback>();
        }
        */

        private void Start()
        {
            currentHealth = startingHealth;
        }

        public void TakeDamage(int damage)
        {
            currentHealth -= damage;
            SoundManager.Instance.PlaySound3D("EnemyTakeDamage", transform.position);
            //knockback.GetKnockedBack(PlayerController.Instance.transform, knockBackThrust);
            GameObject text = Instantiate(floatingText, transform.position, Quaternion.identity) as GameObject; // ADDED
            text.transform.GetChild(0).GetComponent<TextMesh>().text = "-" + damage.ToString(); // ADDED
            //StartCoroutine(flash.FlashRoutine());
            //StartCoroutine(CheckDetectDeathRoutine());
            DetectDeath();
        }

        /*
        private IEnumerator CheckDetectDeathRoutine()
        {
            yield return new WaitForSeconds(flash.GetRestoreMatTime());
            DetectDeath();
        }
        */

        public void DetectDeath()
        {
            if (currentHealth <= 0)
            {
                SoundManager.Instance.PlaySound3D("EnemyDeath", transform.position);
                //Instantiate(deathVFXPrefab, transform.position, Quaternion.identity);
                //GetComponent<PickUpSpawner>().DropItems();
                ExperienceManager.Instance.AddExperience(expAmount);

                foreach (var particle in deathParticles)
                {
                    ParticleManager.StartParticles(particle);
                }

                if (isBoss)
                {
                    SceneManager.LoadScene("Credits");
                }

                Destroy(gameObject);
            }
        }
    }
}
