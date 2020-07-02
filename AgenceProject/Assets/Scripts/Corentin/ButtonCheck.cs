using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonCheck : MonoBehaviour
{
    private void OnMouseDown()
    {
        GameManager.gameManager.gameState = GameManager.STATE_PLAY.inMenu;
        if (PlayerController.playerState == PlayerState.charging)
            PlayerController.playerState = PlayerState.idle;
        //PlayerController.throwAllowed = true;
        Debug.Log("TA = true !!!");
    }
}
