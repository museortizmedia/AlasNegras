using UnityEngine;

public class DamageStat : BaseStat
{
    public float CurrentValue;

    // Método de inicialización
    public new void Initialize(StatController statController, string name, float initialValue)
    {
        base.Initialize(statController, name, initialValue);
        StatType = StatType.Damage;
        CurrentValue = initialValue;
    }

    protected override void UpdateStat()
    {
        // Esta estadistica aplica una actualización en su CurrentValue usando método heredado
        CurrentValue = UpdateAndRecoverStat(CurrentValue);
    }
}
