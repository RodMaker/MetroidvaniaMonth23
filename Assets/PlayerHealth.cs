using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Bardent
{
    public class PlayerHealth : MonoBehaviour
    {
        [SerializeField] private int maxHealth = 3;
        //[SerializeField] private float knockBackThrustAmount = 10f;
        [SerializeField] private float damageRecoveryTime = 1f;
        [SerializeField] private int currentExperience, maxExperience, currentLevel;

        private Slider healthSlider;
        private int currentHealth;
        private bool canTakeDamage = true;
        //private Knockback knockback;
        //private Flash flash;
        private TMP_Text levelText;

        const string HEALTH_SLIDER_TEXT = "HealthSlider";
        const string LEVEL_AMOUNT_TEXT = "LevelAmountText";

        public GameObject floatingText;

        private void Awake()
        {
            //flash = GetComponent<Flash>();
            //knockback = GetComponent<Knockback>();

            StartPlayer();
        }

        public void StartPlayer()
        {
            ExperienceManager.Instance.OnExperienceChange += HandleExperienceChange;

            maxHealth = 3;

            currentHealth = maxHealth;

            UpdateHealthSlider();

            //flash.StartPlayer();

            canTakeDamage = true;

            currentExperience = 0;

            maxExperience = 300;

            currentLevel = 1;

            UpdateCurrentLevel();
        }

        private void OnCollisionStay2D(Collision2D other)
        {
            Debug.Log(other.transform.parent.gameObject);

            if (other.transform.parent.gameObject.CompareTag("Enemy"))
            {
                Debug.Log("Took Damage");
                TakeDamage(1);
            }
        }

        public void HealPlayer()
        {
            if (currentHealth < maxHealth)
            {
                currentHealth += 1;
                UpdateHealthSlider();
            }
        }

        public void TakeDamage(int damageAmount)
        {
            if (!canTakeDamage)
            {
                return;
            }

            Debug.Log("MERDA");

            SoundManager.Instance.PlaySound3D("PlayerTakeDamage", transform.position);
            ScreenShakeManager.Instance.ShakeScreen();
            //knockback.GetKnockedBack(hitTransform, knockBackThrustAmount);
            //StartCoroutine(flash.FlashRoutine());
            canTakeDamage = false;
            currentHealth -= damageAmount;
            StartCoroutine(DamageRecoveryRoutine());
            UpdateHealthSlider();
            CheckIfPlayerDeath();
        }

        private void CheckIfPlayerDeath()
        {
            if (currentHealth <= 0)
            {
                currentHealth = 0;
                Debug.Log("Player Death");

                //BGM.Instance.PauseMusic();
                SoundManager.Instance.PlaySound3D("PlayerDeath", transform.position);
                //GameManager.Instance.GameOver();
                //flash.StopAllCoroutines();
                //knockback.StopAllCoroutines();
                //knockback.SetKnockBack(false);
                gameObject.SetActive(false);
            }
        }

        private IEnumerator DamageRecoveryRoutine()
        {
            yield return new WaitForSeconds(damageRecoveryTime);
            canTakeDamage = true;
        }

        private void UpdateHealthSlider()
        {
            if (healthSlider == null)
            {
                healthSlider = GameObject.Find(HEALTH_SLIDER_TEXT).GetComponent<Slider>();
            }

            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }

        // ADDED LEVEL SYSTEM
        private void OnEnable()
        {

        }

        private void OnDisable()
        {
            ExperienceManager.Instance.OnExperienceChange -= HandleExperienceChange;
        }

        private void HandleExperienceChange(int newExperience)
        {
            currentExperience += newExperience;
            if (currentExperience >= maxExperience)
            {
                LevelUp();
            }
        }

        private void LevelUp()
        {
            maxHealth += 1;
            currentHealth = maxHealth;
            UpdateHealthSlider();
            currentLevel++;
            UpdateCurrentLevel();
            GameObject text = Instantiate(floatingText, transform.position, Quaternion.identity) as GameObject; // ADDED
            text.transform.GetChild(0).GetComponent<TextMesh>().text = "Level Up"; // ADDED
            currentExperience -= maxExperience;
            maxExperience += 100;
            //SkillTree.Instance.SkillPoints += 1;
        }

        public void UpdateCurrentLevel()
        {
            if (levelText == null)
            {
                levelText = GameObject.Find(LEVEL_AMOUNT_TEXT).GetComponent<TMP_Text>();
            }

            levelText.text = currentLevel.ToString();
        }
    }
}
