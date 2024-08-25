public class SpeedStat : BaseStat
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
        // Aumentar o disminuir el valor inicial basado en los puntos positivos y negativos
        float damageIncrease = PositivePoints;
        float damageDecrease = NegativePoints;

        CurrentValue = StatInitialValue + damageIncrease - damageDecrease;

        // Evitar valores negativos si es necesario
        if (CurrentValue < 0) CurrentValue = 0;

        // Resetear los puntos después de aplicar ?
        PositivePoints = 0;
        NegativePoints = 0;
    }
}
