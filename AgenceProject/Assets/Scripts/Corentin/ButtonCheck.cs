using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonCheck : MonoBehaviour
{
    private void OnMouseOver()
    {
        //PlayerController.throwAllowed = false;
    }

    private void OnMouseExit()
    {
        //PlayerController.throwAllowed = true;
        //Debug.Log("TA = true !!!");
    }

    private void OnMouseDown()
    {
        //PlayerController.throwAllowed = true;
        //Debug.Log("TA = true !!!");
    }
}
