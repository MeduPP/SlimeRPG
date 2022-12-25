using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BankUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _coins;

    private void UpdateText(ulong value)
    {
        _coins.SetText(value.ToString());
    }

    private void OnEnable()
    {
        Bank.OnCoinsValueChanged += UpdateText;
    }

    private void OnDisable()
    {
        Bank.OnCoinsValueChanged -= UpdateText;
    }
}
