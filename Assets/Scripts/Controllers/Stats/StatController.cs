using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StatController : MonoBehaviour
{
    [SerializeField] private StatGroupData _assetDeEstadisticas;
    [SerializeField] GameObject statContainer;
    private List<BaseStat> _stats = new();

    [Header("Eventos HealthStat")]
    public UnityEvent OnDeath;

    private readonly Dictionary<StatType, Type> _statTypeToClassMap = new()
    {
        { StatType.Damage, typeof(DamageStat) },
        { StatType.Health, typeof(HealthStat) },
        { StatType.Stamina, typeof(StaminaStat) },
        { StatType.Speed, typeof(SpeedStat) },
        { StatType.Defense, typeof(DefenseStat) },
    };

    private void Awake()
    {
        InitializeStats();
    }

    private void InitializeStats()
    {
        statContainer = new GameObject("Stats");
        statContainer.transform.SetParent(transform);

        foreach (var statData in _assetDeEstadisticas.statsGroup)
        {
            BaseStat stat = null;

            if (_statTypeToClassMap.TryGetValue(statData.statType, out var statClassType))
            {
                stat = statContainer.AddComponent(statClassType) as BaseStat;

                // Inicializar el stat basado en su tipo
                if (stat is DamageStat damageStat)
                {
                    damageStat.Initialize(this, "Damage", statData.initialValue);
                }
                else if (stat is HealthStat healthStat)
                {
                    healthStat.Initialize(this, "Health", statData.initialValue, statData.maxValue);
                    healthStat.OnLooseAllHealth = () => OnDeath?.Invoke();
                }
                else if (stat is StaminaStat staminaStat)
                {
                    staminaStat.Initialize(this, "Stamina", statData.initialValue, statData.maxValue, statData.regenRate);
                }
                else if (stat is SpeedStat speedStat)
                {
                    speedStat.Initialize(this, "Speed", statData.initialValue);
                }
            }

            if (stat != null)
            {
                _stats.Add(stat);
                stat.transform.SetParent(statContainer.transform);
            }
        }
    }

    // Método para consultar una estadística específica
    public T GetStat<T>() where T : BaseStat
    {
        foreach (BaseStat stat in _stats)
        {
            if (stat is T t)
            {
                return t;
            }
        }
        return null;
    }

    // Metodo que convierte del enum al BaseStat
    public BaseStat GetStat(StatType statType)
    {
        // Verifica si el diccionario contiene la clave especificada
        if (_statTypeToClassMap.TryGetValue(statType, out Type statClassType))
        {
            // Recorre la lista de estadísticas buscando la que coincida con el tipo de clase
            foreach (BaseStat stat in _stats)
            {
                if (stat.GetType() == statClassType)
                {
                    return stat;
                }
            }
        }

        // Si no se encontró ninguna coincidencia, devuelve null
        return null;
    }


}
