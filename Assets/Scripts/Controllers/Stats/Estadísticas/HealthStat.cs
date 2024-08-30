using System;
using UnityEngine;
using UnityEngine.Events;

public class HealthStat : BaseStat
{
    [SerializeField] float _currentValue;
    public float CurrentValue
    {
        get => _currentValue;
        set
        {
            _currentValue = value;
            OnCurrentValueChange?.Invoke(value);
        }
    }
    public float MaxValue;

    public Action OnLooseAllHealth;
    public UnityEvent<float> OnCurrentValueChange = new();

    // Método de inicialización
    public void Initialize(StatController statController, string name, float initialValue, float maxValue)
    {
        base.Initialize(statController, name, initialValue);
        StatType = StatType.Health;
        CurrentValue = initialValue;
        MaxValue = maxValue;
    }
    
    protected override void UpdateStat()
    {
        float healthIncrease = PositivePoints;
        float healthDecrease = NegativePoints;

        CurrentValue = StatInitialValue + healthIncrease - healthDecrease;
    }

    #region Logica de Estadistica
    public void MakeDamage(float amount)
    {
        CurrentValue -= amount;
        if (CurrentValue <= 0) { OnLooseAllHealth?.Invoke(); }
    }

    #endregion
}
