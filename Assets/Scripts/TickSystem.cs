using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class TickSystem : MonoBehaviour
{
    public static TickSystem Instance { get; private set; }

    public event EventHandler OnTickChange;


    float tickSpeed = 1f;

    float minTickspeed = 0.07f;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There's more than one TickSystem! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        ScoreManager.Instance.OnLevelUpChanged+= IncreaseTickSpeedByLevel;
        StartTickSystemCoroutine();
    }


    IEnumerator TickSystemCoroutine()
    {
        while (true)
        {

            OnTickChange?.Invoke(this, EventArgs.Empty);
            if (tickSpeed < minTickspeed)
            {
                tickSpeed = minTickspeed;
            }
            yield return new WaitForSeconds(tickSpeed);

        }
    }

    private void StartTickSystemCoroutine()
    {
        StartCoroutine(TickSystemCoroutine());
    }

    private void StopTickSystemCoroutine()
    {
        StopCoroutine(TickSystemCoroutine());
    }

    public void ChangeTickSpeed(float newTickSpeed)
    {
        tickSpeed = newTickSpeed;
    }

    private void IncreaseTickSpeedByLevel(int levels)
    {
        tickSpeed -= 0.019f *levels;
    }
}
