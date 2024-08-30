using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD_StatFinder : MonoBehaviour
{
    public StatType SatToFind;
    public Slider HUDBar;
    BaseStat baseStat;
    [SerializeField] float _currentStatValue;

    public void Start()
    {
        Transform parent = transform.parent;
        switch (SatToFind)
        {
            default:
            case StatType.Health:
                HealthStat healthStat = parent.GetComponentInChildren<HealthStat>();
                if (healthStat!=null)
                {
                    // Almacenar la base
                    baseStat = healthStat;

                    // Iniciar el slider
                    HUDBar.maxValue = healthStat.MaxValue;
                    HUDBar.minValue = 0;
                    HUDBar.value = healthStat.CurrentValue;

                    // Sincronizar el _currentStatValue con el evento
                    healthStat.OnCurrentValueChange.AddListener( (float value) => { _currentStatValue = value;  HUDBar.value = _currentStatValue; });
                }
            break;
            case StatType.Damage:
                DamageStat damageStat = parent.GetComponentInChildren<DamageStat>();
                if (damageStat!=null)
                {
                    // Almacenar la base
                    baseStat = damageStat;

                    // Iniciar el slider
                    //HUDBar.maxValue = damageStat.MaxValue;
                    //HUDBar.minValue = 0;
                    //HUDBar.value = damageStat.CurrentValue;

                    // Sincronizar el _currentStatValue con el evento
                    //damageStat.OnCurrentValueChange.AddListener( (float value) => { _currentStatValue = value;  HUDBar.value = _currentStatValue; });
                }
            break;
            case StatType.Defense:
                DefenseStat defenseStat = parent.GetComponentInChildren<DefenseStat>();
                if (defenseStat!=null)
                {
                    // Almacenar la base
                    baseStat = defenseStat;

                    // Iniciar el slider
                    //HUDBar.maxValue = defenseStat.MaxValue;
                    //HUDBar.minValue = 0;
                    //HUDBar.value = defenseStat.CurrentValue;

                    // Sincronizar el _currentStatValue con el evento
                    //defenseStat.OnCurrentValueChange.AddListener( (float value) => { _currentStatValue = value;  HUDBar.value = _currentStatValue; });
                }
            break;
            case StatType.Speed:
                SpeedStat speedStat = parent.GetComponentInChildren<SpeedStat>();
                if (speedStat!=null)
                {
                    // Almacenar la base
                    baseStat = speedStat;

                    // Iniciar el slider
                    //HUDBar.maxValue = speedStat.MaxValue;
                    //HUDBar.minValue = 0;
                    //HUDBar.value = speedStat.CurrentValue;

                    // Sincronizar el _currentStatValue con el evento
                    //speedStat.OnCurrentValueChange.AddListener( (float value) => { _currentStatValue = value;  HUDBar.value = _currentStatValue; });
                }
            break;
            case StatType.Stamina:
                StaminaStat staminahStat = parent.GetComponentInChildren<StaminaStat>();
                if (staminahStat!=null)
                {
                    // Almacenar la base
                    baseStat = staminahStat;

                    // Iniciar el slider
                    HUDBar.maxValue = staminahStat.MaxValue;
                    HUDBar.minValue = 0;
                    HUDBar.value = staminahStat.CurrentValue;

                    // Sincronizar el _currentStatValue con el evento
                    staminahStat.OnCurrentValueChange.AddListener( (float value) => { _currentStatValue = value;  HUDBar.value = _currentStatValue; });
                }
            break;
        }
    }
}
