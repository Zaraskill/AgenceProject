using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextLocaliserUI : MonoBehaviour
{

    Text textField;

    public LocalisedString localisedString;


    void Start()
    {
        textField = GetComponent<Text>();
        textField.text = localisedString.value;
    }

}
