using UnityEngine;

public abstract class BaseStat : MonoBehaviour
{
    protected string StatName { get; set; }
    protected StatType StatType { get; set; }
    protected float StatInitialValue { get; set; }
    protected float PositivePoints { get; set; }
    protected float NegativePoints { get; set; }
    protected StatController statController;

    public void Initialize(StatController statController, string name, float initialValue)
    {
        this.statController = statController;

        StatName = name;
        StatInitialValue = initialValue;
        PositivePoints = 0;
        NegativePoints = 0;
    }

    // Método para aplicar puntos positivos y negativos
    public void ApplyPositivePoints(float points)
    {
        PositivePoints += points;
        UpdateStat();
    }
    public void ApplyNegativePoints(float points)
    {
        NegativePoints += points;
        UpdateStat();
    }
    public void ApplyPoints(float positivePoints = 0, float negativePoints = 0)
    {
        if(positivePoints!=0){PositivePoints += positivePoints;}
        if(negativePoints!=0){NegativePoints += negativePoints;}
        UpdateStat();
    }

    // Método abstracto que debe ser implementado por las clases derivadas
    protected abstract void UpdateStat();

    protected float UpdateAndRecoverStat(float CurrentValue)
    {
        float damageIncrease = PositivePoints;
        float damageDecrease = NegativePoints;
        Debug.Log($"Actualizando daño de {CurrentValue} a {StatInitialValue + damageIncrease - damageDecrease}");
        return (StatInitialValue + damageIncrease - damageDecrease) < 0 ? 0 : (StatInitialValue + damageIncrease - damageDecrease);
    }

    /*protected void ActualizarYNoRecuperar(float CurrentValue)
    {
        float damageIncrease = PositivePoints;
        float damageDecrease = NegativePoints;
        Debug.Log($"Actualizando daño de {CurrentValue} a {StatInitialValue + damageIncrease - damageDecrease}");
        CurrentValue = (CurrentValue + damageIncrease - damageDecrease) < 0 ? 0 : (CurrentValue + damageIncrease - damageDecrease);
    }*/
}
