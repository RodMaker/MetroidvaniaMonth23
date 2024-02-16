using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bardent
{
    public class ExperienceManager : Singleton<ExperienceManager>
    {
        public delegate void ExperienceChangeHandler(int amount);
        public event ExperienceChangeHandler OnExperienceChange;

        protected override void Awake()
        {
            base.Awake();
        }

        public void AddExperience(int amount)
        {
            OnExperienceChange?.Invoke(amount);
        }
    }
}
