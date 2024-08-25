using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

[RequireComponent(typeof(FantasyArmorController))]
public class WeaponSpawnerController : MonoBehaviour
{
    [Header("Set In Inspector")]
    public Transform ManoDerechaPoint;
    public Transform ManoIzquierdaPoint;
    [Header("Read Only")]
    public GameObject SlotManoDerecha;
    public GameObject SlotManoIzquierda;
    public bool hasSlotArmadura;

    public void TakeWeaponRigth(GameObject weapon)
    {
        if (SlotManoDerecha == null)
        {
            SlotManoDerecha = Instantiate(weapon);
            SlotManoDerecha.transform.position = ManoDerechaPoint.position;
            SlotManoDerecha.transform.rotation = ManoDerechaPoint.rotation;
            SlotManoDerecha.transform.SetParent(ManoDerechaPoint, true);
            SlotManoDerecha.transform.localScale = new Vector3(50, 50, 50);
        }
    }
    public void TakeWeaponLeft(GameObject weapon)
    {
        if (SlotManoIzquierda == null)
        {
            SlotManoIzquierda = Instantiate(weapon);
            SlotManoIzquierda.transform.position = ManoIzquierdaPoint.position;
            SlotManoIzquierda.transform.rotation = ManoIzquierdaPoint.rotation;
            SlotManoIzquierda.transform.SetParent(ManoIzquierdaPoint, true);
            SlotManoIzquierda.transform.localScale = new Vector3(50, 50, 50);

        }
    }
    public void TakeArmor(ArmorMeshData armorData)
    {
        if (hasSlotArmadura == false)
        {
            GetComponent<FantasyArmorController>().EquipEspecificArmor(armorData);
            hasSlotArmadura = true;
        }
    }
}
