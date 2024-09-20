using UnityEngine;
using UnityEngine.Events;

public class StaminaStat : BaseStat
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
    public float RegenRate;

    public UnityEvent<float> OnCurrentValueChange = new();

     // Método de inicialización
    public void Initialize(StatController statController, string name, float initialValue, float maxValue, float regenRate)
    {
        base.Initialize(statController, name, initialValue);
        StatType = StatType.Stamina;
        CurrentValue = initialValue;
        MaxValue = maxValue;
        RegenRate = regenRate;
    }

    protected override void UpdateStat()
    {
        float staminaIncrease = PositivePoints;
        float staminaDecrease = NegativePoints;

        MaxValue = StatInitialValue + staminaIncrease - staminaDecrease;

        // Limitar el valor de CurrentValue entre 0 y MaxValue
        if (CurrentValue > MaxValue) CurrentValue = MaxValue;
        if (CurrentValue < 0) CurrentValue = 0;
        
    }

    private void Update() {
        if(CurrentValue < MaxValue)
        {
            CurrentValue += RegenRate * Time.deltaTime;
            if (CurrentValue > MaxValue) CurrentValue = MaxValue;
        }
    }
}
