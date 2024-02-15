using System;
using Bardent.CoreSystem.StatsSystem;
using UnityEngine;

namespace Bardent.CoreSystem
{
    public class Stats : CoreComponent
    {
       [field: SerializeField] public Stat Health { get; private set; }
       [field: SerializeField] public Stat Poise { get; private set; }

       [SerializeField] private float poiseRecoveryRate;

        // ADDED
        private GameManager gm;
        
        protected override void Awake()
        {
            base.Awake();
            
            Health.Init();
            Poise.Init();
        }

        // ADDED
        private void Start()
        {
            SetPlayer();
        }

        private void Update()
        {
            if (Poise.CurrentValue.Equals(Poise.MaxValue))
                return;
            
            Poise.Increase(poiseRecoveryRate * Time.deltaTime);
        }

        public void SetPlayer()
        {
            Health.MaxValue = gm.health;
        }
    }
}
