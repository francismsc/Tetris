using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ParticlesController : MonoBehaviour
{
    [SerializeField]
    ParticleSystem BigBurstParticle1;

    [SerializeField]
    ParticleSystem CircleBurstLeft;

    [SerializeField]
    ParticleSystem CircleBurstMiddle;

    [SerializeField]
    ParticleSystem CircleBurstRight;

    [SerializeField]
    ParticleSystem[] bubblesParticleArray;

    private int lastProcessedThreshold = 0;
    private const int threshold = 10;
    void Start()
    {
        LineClearingManager.OnLinesCleared += PlayBigBurstParticle;
        ScoreManager.Instance.OnLinesChanged += PlayEffectsCircles;
        StartTickSystemCoroutine();
        StartTickSystemCoroutine();
    }



    // Update is called once per frame
    private void OnDestroy()
    {
        LineClearingManager.OnLinesCleared -= PlayBigBurstParticle;
        ScoreManager.Instance.OnLinesChanged -= PlayEffectsCircles;
    }

    IEnumerator TickSystemCoroutine()
    {
        while (true)
        {
            bubblesParticleArray[UnityEngine.Random.Range(0, bubblesParticleArray.Length)].Play();
            float randomTimer = UnityEngine.Random.Range(2f, 4f);
            yield return new WaitForSeconds(randomTimer);

        }
    }

    private void StartTickSystemCoroutine()
    {
        StartCoroutine(TickSystemCoroutine());
    }

    private void PlayBigBurstParticle(int linesCleared)
    {
        BigBurstParticle1.Play();
    }

    private void PlayEffectsCircles(int linesClearedTotal)
    {
        int currentThreshold = (linesClearedTotal / 5) * 5;


        if (currentThreshold > lastProcessedThreshold)
        {
            CircleBurstLeft.Play();
            CircleBurstMiddle.Play();
            CircleBurstRight.Play();
            lastProcessedThreshold = currentThreshold;
        }
    }
}
