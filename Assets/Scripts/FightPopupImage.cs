using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class FightPopupImage : MonoBehaviour
{
    private static float damagePopupDuration = 1.0f;

    //Instantiate a popup
    //Set its different values
    //LeanTween to move them into position
    //Destroy them after duration or a set amount of time for damages

    //----------------------------------------Damage functions
    public static void AttackDamagePopup(Vector3 positon, float damage)
    {
        GameObject inst = Instantiate(Resources.Load("Prefabs/DamagePopUp") as GameObject, new Vector3(positon.x,positon.y + 2, positon.z), Quaternion.identity);
        inst.transform.LookAt(2 * inst.transform.position - Camera.main.transform.position);
        inst.transform.FindDeepChild("DamageText").GetComponent<TextMeshProUGUI>().text = damage.ToString();
        inst.name = "DamagePopUpTest";
        AddLeanTweenDamage(inst);
        Destroy(inst, damagePopupDuration);
    }

    public static void CritAttackDamagePopup(Vector3 positon, float damage)
    {

    }

    public static void AbilityDamagePopup(Vector3 positon, float damage)
    {

    }
    public static void CritAbilityDamagePopup(Vector3 positon, float damage)
    {

    }

    //-------------------------------------Other functions
    public static void BlockedAttackPopup(Vector3 positon)
    {

    }

    public static void StunnedPopup(Vector3 positon, float duration)
    {

    }

    //----------------------------------------Reducing functions

    public static void ArmorReducedPopup(Vector3 positon, float duration)
    {

    }

    public static void MagicResistReducedPopup(Vector3 positon, float duration)
    {

    }

    public static void AttackSpeedReducedPopup(Vector3 positon, float duration)
    {

    }

    public static void AttackDamageReducedPopup(Vector3 positon, float duration)
    {

    }

    public static void MagicDamageReducedPopup(Vector3 positon, float duration)
    {

    }


    private static void AddLeanTweenDamage(GameObject inst)
    {
        Vector3 bezierPathPosition = inst.transform.position;
        LTBezierPath ltPath = new LTBezierPath(new Vector3[]
        {
            new Vector3(bezierPathPosition.x, bezierPathPosition.y + 0.2f, bezierPathPosition.z-0.4f),
            new Vector3(bezierPathPosition.x+0.8f, bezierPathPosition.y+0.2f, bezierPathPosition.z-0.4f),
            new Vector3(bezierPathPosition.x+0.8f, bezierPathPosition.y+2.2f, bezierPathPosition.z-0.4f),
            new Vector3(bezierPathPosition.x+0.8f, bezierPathPosition.y, bezierPathPosition.z-0.4f)
        });
        LeanTween.move(inst, ltPath, damagePopupDuration).setEase(LeanTweenType.easeInOutQuad);
        LeanTween.scale(inst, Vector3.zero, damagePopupDuration);

    }
}
