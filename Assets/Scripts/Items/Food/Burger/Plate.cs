using System;
using System.Collections;
using UnityEngine;

public class Plate : MonoBehaviour
{
    public Action OnMealCooked;
    public float MealCookTime;
    public Coroutine CookMealTime;

    public void CookMeal(float time)
    {
        CookMealTime = StartCoroutine(CookMealTick(time));
    }

    public void StopCooking()
    {
        StopCoroutine(CookMealTime);
        CookMealTime = null;
    }

    private IEnumerator CookMealTick(float time)
    {
        yield return new WaitForSeconds(MealCookTime);
        OnMealCooked?.Invoke();
        CookMealTime = null;
    }
}