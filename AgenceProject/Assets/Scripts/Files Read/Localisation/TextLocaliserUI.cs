using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextLocaliserUI : MonoBehaviour
{

    Text textField;

    public LocalisedString localisedString;


    void Awake()
    {
        textField = GetComponent<Text>();
    }

    public void UpdateText()
    {
        textField.text = localisedString.value;
    }

    private void OnEnable()
    {
        UpdateText();
    }
}
