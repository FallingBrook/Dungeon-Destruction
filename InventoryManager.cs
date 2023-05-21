using System;
using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    private GameObject player;
    public GameObject wp1;
    public GameObject wp2;
    public GameObject AttackButton;
    public Button ChangeWeaponButton;
    private int currentWeaponNum = 1;
    public int coins;
    public int mana;
    private TextMeshProUGUI coinsText;
    private Slider manaBar;
    public LayerMask coinLayer;
    // Start is called before the first frame update
    void Start()
    {
        GameObject.Find("XpBar").GetComponent<Slider>().value = PlayerPrefs.GetFloat("XP");
        manaBar = GameObject.FindWithTag("ManaText").GetComponent<Slider>();
        manaBar.value = mana;
        coinsText = GameObject.FindWithTag("CoinText").GetComponent<TextMeshProUGUI>();
        coinsText.text = coins.ToString();
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        Collider2D consumeHit = Physics2D.OverlapCircle(transform.position, 0.1f, coinLayer);
        if (consumeHit)
        {
            if (consumeHit.CompareTag("Coin"))
            {
                Destroy(consumeHit.gameObject);
                UpdateObject(1, "Coin");
            }
            else
            {
                UpdateObject(1, "Mana");
                Destroy(consumeHit.gameObject);
            }

        }
    }

    public void ChangeWeapons()
    {
        if (currentWeaponNum == 1)
            wp1.SetActive(false);
        else if (currentWeaponNum == 2)
            wp2.SetActive(false);
        currentWeaponNum += 1;
        if (currentWeaponNum > 2)
            currentWeaponNum = 1;
        if (currentWeaponNum == 1)
        {
            player.GetComponent<PlayerScript>().weapon = wp1;
            GameObject.Find("WeaponPicture").GetComponent<WeaponPictureScript>().ChangePicture(wp1);
            wp1.SetActive(true);
            wp2.SetActive(false);
            AttackButton.GetComponent<AttackBScript>().weaponGO = wp1;
            //AttackButton.onClick.RemoveAllListeners();
            //AttackButton.onClick.AddListener(wp1.GetComponent<WeaponParent>().Attack);
        }
        else if (currentWeaponNum == 2)
        {
            player.GetComponent<PlayerScript>().weapon = wp2;
            GameObject.Find("WeaponPicture").GetComponent<WeaponPictureScript>().ChangePicture(wp2);
            wp2.SetActive(true);
            wp1.SetActive(false);
            AttackButton.GetComponent<AttackBScript>().weaponGO = wp2;
            //AttackButton.onClick.RemoveAllListeners();
            //AttackButton.onClick.AddListener(wp2.GetComponent<WeaponParent>().Attack);
        }
    }
    public void PickupWeapon(GameObject weapon, int price)
    {
        Vector2 pos = weapon.transform.position;
        if (currentWeaponNum == 1)
        {
            if(wp1.transform.childCount > 0)
                Enabled(wp1.transform.GetChild(0), false);
            //SwitchWeapon(wp1.transform.GetChild(0).gameObject, weapon);
            wp1.transform.GetChild(0).rotation = Quaternion.Euler(0, 0, 0);
            wp1.transform.GetChild(0).gameObject.layer = 12;
            wp1.transform.GetChild(0).SetParent(null);
            weapon.transform.parent = wp1.transform;
            weapon.transform.position = wp1.transform.position;
            GameObject.Find("WeaponPicture").GetComponent<WeaponPictureScript>().ChangePicture(wp1);
        }
        else if(currentWeaponNum == 2)
        {
            if (wp2.transform.childCount > 0)
                Enabled(wp2.transform.GetChild(0), false);
            //switchWeapon(wp2.transform.GetChild(0).gameObject, weapon);
            wp2.transform.GetChild(0).rotation = Quaternion.Euler(0, 0, 0);
            wp2.transform.GetChild(0).gameObject.layer = 12;
            wp2.transform.GetChild(0).SetParent(null);
            weapon.transform.parent = wp2.transform;
            weapon.transform.position = wp2.transform.position;
            GameObject.Find("WeaponPicture").GetComponent<WeaponPictureScript>().ChangePicture(wp2);
        }
        weapon.transform.localScale = new Vector3(1, 1, 0);
        Enabled(weapon.transform, true);
        weapon.layer = 0;
        if (price > 0)
        {
            coins -= price;
            coinsText.text = coins.ToString();
        }
    }

    private void Enabled(Transform weapon, bool i)
    {
        if (weapon.tag == "MeleeWeapon")
            weapon.GetComponent<SwordWeapon>().enabled = i;
        else if (weapon.tag == "BowWeapon")
        {
            weapon.GetComponent<BowScript>().enabled = i;
            weapon.GetChild(1).gameObject.SetActive(true);
        }
        else if (weapon.tag == "MilitaryRangedWeapon")
            weapon.GetComponent<ModernGunScript>().enabled = i;
        else if (weapon.tag == "MilitaryShotgunWeapon")
            weapon.GetComponent<ShotgunWeaponScript>().enabled = i;
    }
    public void UpdateObject(int value, string type)
    {
        if(type == "Coin")
        {
            coins += value;
            coinsText.text = coins.ToString();
        }
        else
        {
            mana += value;
            manaBar.value = mana;
        }
    }
    public void UpdateMana(int num)
    {
        mana -= num;
        manaBar.value = mana;
        StartCoroutine(DecreaseBar("ManaBar2", mana));
    }   

    public void UpdateManaPos(int num)
    {
        mana += num;
        manaBar.value = mana;
    }
    public void UpdateHealthPos(float num)
    {
        player.GetComponent<PlayerScript>().health += num;
        GameObject.Find("HealthBar").GetComponent<Slider>().value = player.GetComponent<PlayerScript>().health;
    }
    public void UpdateHealth()
    {
        GameObject.FindWithTag("HeartText").GetComponent<Slider>().value = player.GetComponent<PlayerScript>().health;
        StartCoroutine(DecreaseBar("HealthBar2", player.GetComponent<PlayerScript>().health));
    }
    private IEnumerator DecreaseBar(string name, float t)
    {
        yield return new WaitForSeconds(0.5f);
        float currentTime = 0;
        float bar2 = GameObject.Find(name).GetComponent<Slider>().value;
        while (currentTime < 1)
        {
            float amount = Mathf.Lerp(bar2, t, currentTime / 1);
            GameObject.Find(name).GetComponent<Slider>().value = amount;
            currentTime += Time.deltaTime;
            yield return null;
        }
    }
    public void UpdateCoinsNeg(int num)
    {
        coins -= num;
        coinsText.text = coins.ToString();
    }

    public void UpdateXp(float xpGranted)
    {
        PlayerPrefs.SetFloat("XP", PlayerPrefs.GetFloat("XP") + xpGranted);
        PlayerPrefs.Save();
        GameObject.Find("XpBar").GetComponent<Slider>().value = PlayerPrefs.GetFloat("XP");
    }
}
