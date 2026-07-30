using UnityEngine;
using R3;

namespace KoiAI.Health
{
    using KoiAI.Utilities;

    public abstract class HealthBar : FollowableUI
    {
        private float _maxHealth = -1f;

        public void Init(Health health, HealthBarData healthBarData, float curHealth, float maxHealth)
        {
            health.CurrentHealthReactive.Subscribe(currentHealth =>
            {
                HealthBarManager.Instance.RemoveHealthBar(health);
                ChangeHealthBar(currentHealth);
            });
            _maxHealth = maxHealth;
            float normHealth = Mathf.Clamp01(curHealth / maxHealth);
            SetHealthBarValue(normHealth);
            Init(healthBarData);
        }


        private void ChangeHealthBar(float currentHealth)
        {
            if (_maxHealth <= 0)
            {
                return;
            }
            float normHealth = Mathf.Clamp01(currentHealth / _maxHealth);
            SetHealthBarValue(normHealth);
        }

        protected abstract void Init(HealthBarData healthBarData);
        protected abstract void SetHealthBarValue(float normHealth);
    }
}
