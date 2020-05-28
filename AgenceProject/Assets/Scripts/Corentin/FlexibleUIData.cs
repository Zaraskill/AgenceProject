using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( menuName = "Flexible UI Results")]
public class FlexibleUIData : ScriptableObject
{
    [Header("Flexible Images")]
    public Sprite VictoryOneStar;
    public Sprite VictoryTwoStar;
    public Sprite VictoryThreeStar;
    public Sprite DefeatZeroStar;

    public Sprite VictoryText;
    public Sprite DefeatText;

    [Header("Flexible Buttons")]
    public Sprite LockedLevel;
    public Sprite UnlockedLevelNoStar;
    public Sprite UnlockedLevelOneStar;
    public Sprite UnlockedLevelTwoStar;
    public Sprite UnlockedLevelThreeStar;

    [Header("Tutorial")]
    public List<Sprite> firstTuto;

    [Header("UI Button")]
    public Sprite ActivatedMusic;
    public Sprite DeactivatedMusic;
    public Sprite ActivatedSound;
    public Sprite DeactivatedSound;

}
