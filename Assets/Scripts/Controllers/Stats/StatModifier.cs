using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public struct StatModification
{
    public StatType StatType;
    public float PositivePoints;
    public float NegativePoints;
}
public enum ArticlePose { ManoDerecha, ManoIzquierda, Armadura }
public class StatModifier : MonoBehaviour
{
    public StatModification[] statModifications;
    public ArticlePose articlePose;
    public GameObject prefabArticle;
    public ArmorMeshData armorMeshData;


    private void Start()
    {
        if( (articlePose == ArticlePose.ManoDerecha || articlePose == ArticlePose.ManoIzquierda) && prefabArticle==null)
        {
            Debug.LogWarning($"Se estableció un Artefacto Sagrado que va en las manos ¡Pero no hay modelos! Se desactivará el Script", transform);
            enabled = false;
            return;
        }
        if( articlePose == ArticlePose.Armadura && armorMeshData==null)
        {
            Debug.LogWarning($"Se estableció una Armadura Sagrada ¡Pero no hay datos de armadura! Se desactivará el Script", transform);
            enabled = false;
            return;
        }
    }

    public void ApplyArticle(GameObject jugador)
    {
        if(!enabled) return;
        //Debug.Log($"aplicando objeto para {jugador.name}");
        if (jugador.TryGetComponent<StatController>(out StatController playerStatController))
        {
            //Debug.Log($"El controlador de estadisticas de {jugador.name} es {playerStatController}", playerStatController.transform);

            foreach (var modificacion in statModifications)
            {
                //Debug.Log($"Se aplicará la modificación: {modificacion.StatType} {modificacion.PositivePoints} {modificacion.NegativePoints}", playerStatController.transform);
                StatType estadistica = modificacion.StatType;

                BaseStat currentBaseStat = playerStatController.GetStat(estadistica);
                currentBaseStat?.ApplyPoints(modificacion.PositivePoints, modificacion.NegativePoints);
            }
        }

        WeaponSpawnerController weaponSpawnerController = jugador.GetComponentInChildren<WeaponSpawnerController>();
        // Buscar el punto del jugador del articlePose e instanciar prefabArticle ahí
        if(weaponSpawnerController!=null)
        {
            switch (articlePose)
            {
                case ArticlePose.ManoDerecha:
                //Debug.Log($"Se añadirá arma a la derecha del jugador", transform);
                weaponSpawnerController.TakeWeaponRigth(prefabArticle);
                break;
                case ArticlePose.ManoIzquierda:
                //Debug.Log($"Se añadirá arma a la izquierda del jugador", transform);
                weaponSpawnerController.TakeWeaponLeft(prefabArticle);
                break;
                case ArticlePose.Armadura:
                //Debug.Log($"Se añadirá ropa al jugador", transform);
                weaponSpawnerController.TakeArmor(armorMeshData);
                break;
                default:
                break;
            }
        }

    }
}
