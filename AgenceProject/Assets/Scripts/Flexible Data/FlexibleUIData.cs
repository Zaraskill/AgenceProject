using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( menuName = "Flexible UI Results")]
public class FlexibleUIData : ScriptableObject
{
    [Header("Display Results")]
    public Sprite VictoryOneStar;
    public Sprite VictoryTwoStar;
    public Sprite VictoryThreeStar;
    public Sprite DefeatZeroStar;

    [Header("Stars Selection")]
    public Sprite oneStar;
    public Sprite twoStar;
    public Sprite threeStar;
    public Sprite zeroStar;

    [Header("Button level")]
    public Sprite UnlockedLevelNoStar;
    public Sprite UnlockedLevelOneStar;
    public Sprite UnlockedLevelTwoStar;
    public Sprite UnlockedLevelThreeStar;

    [Header("Next Page")]
    public Sprite lockedPage;
    public Sprite unlockedPage;
    public Sprite unlockeablePage;

    [Header("Tutorial")]
    public List<Sprite> firstTuto;

    [Header("UI Button")]
    public Sprite ActivatedMusic;
    public Sprite DeactivatedMusic;
    public Sprite ActivatedSound;
    public Sprite DeactivatedSound;

    [Header("Pause")]
    public Sprite pauseNoStar;
    public Sprite pauseZeroStar;
    public Sprite pauseOneStar;
    public Sprite pauseTwoStar;
    public Sprite pauseThreeStar;
}
