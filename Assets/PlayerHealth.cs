using System;
using Bardent.CoreSystem;
using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Bardent
{
    public class PlayerHealth : Singleton<PlayerHealth>
    {
        //public static event Action OnPlayerDeath;

        public int maxHealth = 3;
        //[SerializeField] private float knockBackThrustAmount = 10f;
        [SerializeField] private float damageRecoveryTime = 1f;
        [SerializeField] private int currentExperience, maxExperience, currentLevel;

        private Slider healthSlider;
        public int currentHealth;
        private bool canTakeDamage = true;
        //private Knockback knockback;
        //private Flash flash;
        private TMP_Text levelText;

        const string HEALTH_SLIDER_TEXT = "HealthSlider";
        const string LEVEL_AMOUNT_TEXT = "LevelAmountText";

        public GameObject floatingText;

        [SerializeField] private GameObject[] deathParticles;

        private ParticleManager ParticleManager =>
            particleManager ? particleManager : Player.FindObjectOfType<Core>().GetCoreComponent(ref particleManager);

        private ParticleManager particleManager;

        Vector2 checkpointPos;
        Rigidbody2D playerRB;

        protected override void Awake()
        {
            base.Awake();

            //flash = GetComponent<Flash>();
            //knockback = GetComponent<Knockback>();
            playerRB = GetComponent<Rigidbody2D>();
            checkpointPos = new Vector2(-17.5f, 21.3f);

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

            Bardent.Camera.Instance.GetComponent<CinemachineVirtualCamera>().m_Follow = gameObject.transform;
        }

        private void OnCollisionStay2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Enemy"))
            {
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

                foreach (var particle in deathParticles)
                {
                    ParticleManager.StartParticles(particle);
                }

                //AudioManager.Instance.PauseMusic();
                SoundManager.Instance.PlaySound3D("PlayerDeath", transform.position);
                StartCoroutine(Respawn(0.5f));
                //OnPlayerDeath?.Invoke();
                //GameManager.gm.GameOver();
                //flash.StopAllCoroutines();
                //knockback.StopAllCoroutines();
                //knockback.SetKnockBack(false);
                //gameObject.SetActive(false);
            }
        }

        IEnumerator Respawn(float duration)
        {
            playerRB.simulated = false;
            playerRB.velocity = new Vector2(0, 0);
            transform.localScale = new Vector3(0, 0, 0);
            yield return new WaitForSeconds(duration);
            transform.position = checkpointPos;
            transform.localScale = new Vector3(1, 1, 1);
            playerRB.simulated = true;
            StartPlayer();
        }

        public void UpdateCheckpoint(Vector2 pos)
        {
            checkpointPos = pos;
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
