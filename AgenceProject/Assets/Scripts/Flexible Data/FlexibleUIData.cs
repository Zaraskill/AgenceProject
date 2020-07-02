using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( menuName = "Flexible UI Results")]
public class FlexibleUIData : ScriptableObject
{
    [Header("Display Results")]
    public Sprite victory;
    public Sprite defeat;
    public Sprite star;
    public Sprite noStar;

    [Header("Stars Selection")]
    public Sprite oneStar;
    public Sprite twoStar;
    public Sprite threeStar;
    public Sprite zeroStar;

    [Header("Button level")]
    public Sprite unlockedLevelNoStar;
    public Sprite unlockedLevelOneStar;
    public Sprite unlockedLevelTwoStar;
    public Sprite unlockedLevelThreeStar;
    public Sprite unlockeableFinal;
    public Sprite lockedLevel;

    [Header("Next Page")]
    public Sprite lockedPage;
    public Sprite unlockedPage;
    public Sprite unlockeablePage;

    [Header("Tutorial")]
    public List<Sprite> firstTuto;

    [Header("UI Button")]
    public Sprite activatedMusic;
    public Sprite deactivatedMusic;
    public Sprite activatedSound;
    public Sprite deactivatedSound;

    [Header("Pause")]
    public Sprite pauseNoStar;
    public Sprite pauseZeroStar;
    public Sprite pauseOneStar;
    public Sprite pauseTwoStar;
    public Sprite pauseThreeStar;
}
