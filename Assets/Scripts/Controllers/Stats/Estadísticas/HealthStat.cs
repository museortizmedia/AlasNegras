using UnityEngine;

public class HealthStat : BaseStat
{
    public float CurrentValue;
    public float MaxValue;

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
    }

    #endregion
}
