using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Boost
{
    public float BoostValue;
    public int Cost;
}

public class Boosters : MonoBehaviour
{
    [SerializeField] Shoot _shootManager;
    [SerializeField] Bank _bank;

    [SerializeField] private TMP_Text _attakLevelText;
    [SerializeField] private TMP_Text _fireRateLevelText;
    [SerializeField] private TMP_Text _attakCostText;
    [SerializeField] private TMP_Text _fireRateCostText;
    [SerializeField] private Button _attakBoostBtn;
    [SerializeField] private Button _fireRateBoostBtn;

    int AttakBoostLvl = 0;
    int FireRateBoostLvl = 0;

    public List<Boost> Attak;
    public List<Boost> FireRate;

    private void Awake()
    {
        //GetSaved data
        if (PlayerPrefs.HasKey("AttakBoostLvl"))
            AttakBoostLvl = PlayerPrefs.GetInt("AttakBoostLvl");
        else
            PlayerPrefs.SetInt("AttakBoostLvl", AttakBoostLvl);

        if (PlayerPrefs.HasKey("FireRateBoostLvl"))
            FireRateBoostLvl = PlayerPrefs.GetInt("FireRateBoostLvl");
        else
            PlayerPrefs.GetInt("FireRateBoostLvl", FireRateBoostLvl);

    }

    private void Start()
    {
        _shootManager.DamageBoost = Attak[AttakBoostLvl].BoostValue;
        _shootManager.FireRateBoost = FireRate[FireRateBoostLvl].BoostValue;

        _attakLevelText.SetText((AttakBoostLvl + 2).ToString() + " Lvl");
        _fireRateLevelText.SetText((FireRateBoostLvl + 2).ToString() + " Lvl");

        if (AttakBoostLvl >= Attak.Count - 1)
        {
            _attakBoostBtn.interactable = false;
            _attakLevelText.SetText("Max");
            _attakCostText.SetText("");
        }
        else
        {
            _attakCostText.SetText(Attak[AttakBoostLvl + 1].Cost.ToString());
        }

        if (FireRateBoostLvl >= FireRate.Count - 1)
        {
            _fireRateBoostBtn.interactable = false;
            _fireRateLevelText.SetText("Max");
            _fireRateCostText.SetText("");
        }
        else
        {
            _fireRateCostText.SetText(FireRate[FireRateBoostLvl + 1].Cost.ToString());
        }
    }

    public void UpgradeDamage()
    {
        if (!_bank.SpendCoins(Attak[AttakBoostLvl + 1].Cost))
            return;

        AttakBoostLvl++;
        PlayerPrefs.SetInt("AttakBoostLvl", AttakBoostLvl);
        _shootManager.DamageBoost = Attak[AttakBoostLvl].BoostValue;

        if (AttakBoostLvl >= Attak.Count - 1)
        {
            _attakBoostBtn.interactable = false;
            _attakLevelText.SetText("Max");
            _attakCostText.SetText("");
        }
        else
        {
            _attakLevelText.SetText((AttakBoostLvl + 2).ToString() + " Lvl");
            _attakCostText.SetText(Attak[AttakBoostLvl + 1].Cost.ToString());

        }
    }

    public void FireRateFireRate()
    {
        if (!_bank.SpendCoins(FireRate[FireRateBoostLvl + 1].Cost))
            return;

        FireRateBoostLvl++;
        PlayerPrefs.SetInt("FireRateBoostLvl", FireRateBoostLvl);
        _shootManager.FireRateBoost = FireRate[FireRateBoostLvl].BoostValue;

        if (FireRateBoostLvl >= FireRate.Count - 1)
        {
            _fireRateBoostBtn.interactable = false;
            _fireRateLevelText.SetText("Max");
            _fireRateCostText.SetText("");
        }
        else
        {
            _fireRateLevelText.SetText((FireRateBoostLvl + 2).ToString() + " Lvl");
            _fireRateCostText.SetText(FireRate[FireRateBoostLvl + 1].Cost.ToString());
        }
    }
}
