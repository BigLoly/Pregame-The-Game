using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

public class LiquidScript : MonoBehaviour
{
    [SerializeField] RectTransform wave; //For liquid animation
    [SerializeField] RectTransform liquidFullTarget;
    [SerializeField] RectTransform liquidEmptyTarget;
    [SerializeField] float amplitude = 8f;
    [SerializeField] float frequency = 4f;
    Vector2 startPos;

    [SerializeField] float maxLiquid = 100f; //Alcohol level
    [SerializeField] float liquidAmount = 100f;

    public bool onScreen = false; //For onscreen transitions
    [SerializeField] GameObject bottle;
    [SerializeField] RectTransform bottleActivePOS;
    [SerializeField] RectTransform bottleInactivePOS;
    [SerializeField] float easeTime;

    void Start()
    {
        startPos = wave.anchoredPosition;
    }

    void BottleTransitioner() //transitions bottle from offscreen to onscreen and vice versa (TRANS?!?!?!)
    {
        Vector3 activePOS = bottleActivePOS.transform.localPosition;
        Vector3 inactivePOS = bottleInactivePOS.transform.localPosition;
        transform.DOKill();
        if (onScreen && liquidAmount > 0)
        {
            bottle.transform.DOLocalMove(activePOS, easeTime).SetEase(Ease.OutCirc);
        }
        else
        {
            bottle.transform.DOLocalMove(inactivePOS, easeTime).SetEase(Ease.OutCirc);
        }
    }

    public void ReduceLiquid(float amount) //Public function to reduce liquid
    {
        liquidAmount -= amount;
        liquidAmount = Mathf.Clamp(liquidAmount, 0, maxLiquid);
    }

    public void AddLiquid(float amount) //Public function to add liquid
    {
        liquidAmount += amount;
        liquidAmount = Mathf.Clamp(liquidAmount, 0, maxLiquid);
    }

    void AnimatedLiquid() //Handles Animated Liquid and amount
    {
        float liquidPercent = liquidAmount / maxLiquid;
        float baseY = Mathf.Lerp(
            liquidEmptyTarget.anchoredPosition.y,
            liquidFullTarget.anchoredPosition.y,
            liquidPercent
        );

        float waveOffSet = Mathf.Sin(Time.time * frequency) * amplitude;

        wave.anchoredPosition = new Vector2(
            0f, 
            baseY + waveOffSet
        );
    }

    void Update() 
    {
        AnimatedLiquid();
        BottleTransitioner();
    }
}
