using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class WeaponInfoScript : MonoBehaviour
{
    public TextMeshProUGUI damageText;
    public TextMeshProUGUI manaText;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI costText;
    private string currentState;
    public Sprite attackSymb;
    public Sprite heartSymb;
    public void ShowText(GameObject ob)
    {
        int damage = 0;
        int mana = 0;
        if (ob.CompareTag("MilitaryRangedWeapon"))
        {
            damage = ((int)ob.GetComponent<ModernGunScript>().damage);
            mana = ob.GetComponent<ModernGunScript>().manaCost;
        }
        else if (ob.CompareTag("MilitaryShotgunWeapon"))
        {
            damage = ((int)ob.GetComponent<ShotgunWeaponScript>().damage);
            mana = ob.GetComponent<ShotgunWeaponScript>().manaCost;
        }
        else if (ob.CompareTag("BowWeapon"))
        {
            damage = ((int)ob.GetComponent<BowScript>().damage);
            mana = ob.GetComponent<BowScript>().manaCost;
        }
        else if (ob.CompareTag("MeleeWeapon"))
        {
            damage = ((int)ob.GetComponent<SwordWeapon>().damage);
            mana = ob.GetComponent<SwordWeapon>().manaCost;
        }
        damageText.text = damage.ToString();
        manaText.text = mana.ToString();
        nameText.text = ob.name;
        ChangeAnimationState("WeaponInfoShow");
    }
    public void ShowShopText(GameObject ob, int cost)
    {
        int damage = 0;
        int mana = 0;
        transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = attackSymb;
        if (ob.CompareTag("MilitaryRangedWeapon"))
        {
            damage = ((int)ob.GetComponent<ModernGunScript>().damage);
            mana = ob.GetComponent<ModernGunScript>().manaCost;
        }
        else if (ob.CompareTag("MilitaryShotgunWeapon"))
        {
            damage = ((int)ob.GetComponent<ShotgunWeaponScript>().damage);
            mana = ob.GetComponent<ShotgunWeaponScript>().manaCost;
        }
        else if (ob.CompareTag("BowWeapon"))
        {
            damage = ((int)ob.GetComponent<BowScript>().damage);
            mana = ob.GetComponent<BowScript>().manaCost;
        }
        else if (ob.CompareTag("MeleeWeapon"))
        {
            damage = ((int)ob.GetComponent<SwordWeapon>().damage);
            mana = ob.GetComponent<SwordWeapon>().manaCost;
        }
        damageText.text = damage.ToString();
        manaText.text = mana.ToString();
        nameText.text = ob.name;
        costText.text = cost.ToString();
        ChangeAnimationState("WeaponInfoShow");
    }

    public void ShowShopTextPotion(GameObject ob, int cost, int mana, int health)
    {
        transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = heartSymb;
        //mana = ob.GetComponent<ConsumablePotionsScript>().manaInc;
        damageText.text = health.ToString();
        manaText.text = mana.ToString();
        nameText.text = ob.name;
        costText.text = cost.ToString();
        ChangeAnimationState("WeaponInfoShow");
    }
    public void HideText()
    {
        ChangeAnimationState("WeaponInfoHide");
    }
    public void ToIdle()
    {
        ChangeAnimationState("WeaponInfoIdle");
    }
    void ChangeAnimationState(string newState)
    {
        //Debug.Log(direction.x);
        // stop same anim from playing
        if (currentState == newState) return;
        // play new anim
        GetComponent<Animator>().Play(newState);

        // reassign the current state
        currentState = newState;
    }
}
