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

    public void ApplyArticle(GameObject jugador)
    {
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

        // Buscar el punto del jugador del articlePose e instanciar prefabArticle ahí

    }
}
