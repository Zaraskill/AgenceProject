using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAntiSlidingPlayer //frozen for now, i'm on it later, Ulysse
{
    float countDown { get; }
    bool isSliding { get; }
    bool isCheckingSliding { get; }

    void AntiSlide();
}
