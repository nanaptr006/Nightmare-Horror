using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image _healthBarFill;

    public void SetHealth(float currentHealth, float maxHealth)
    {
        if (_healthBarFill == null || maxHealth <= 0f)
            return;

        _healthBarFill.fillAmount = Mathf.Clamp01(currentHealth / maxHealth);
    }
}
