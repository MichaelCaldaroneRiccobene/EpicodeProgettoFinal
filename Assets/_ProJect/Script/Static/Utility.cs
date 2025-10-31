using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public static class Utility
{
    #region ChooseRandomPoint
    public static Vector3 RandomPointForAngent(NavMeshAgent agent, Vector3 startPosition, float range)
    {
        int numberOfTentativ = 5;

        for (int i = 0; i < numberOfTentativ; i++)
        {
            Vector3 randomPosition = UnityEngine.Random.insideUnitSphere * range + startPosition;
            randomPosition.y = agent.transform.position.y;

            if (NavMesh.SamplePosition(randomPosition, out NavMeshHit hit, 1, NavMesh.AllAreas)) return hit.position;
        }

        Debug.LogError("Nessuna posiziona Trovata");

        return agent.transform.position;
    }
    #endregion


    #region ShockEffect
    private static bool isOnShockEffect;
    public static void ShockEffect(MonoBehaviour owner, float start, float end, float frames) => owner.StartCoroutine(ShockEffectRoutine(start,end,frames));
    private static IEnumerator ShockEffectRoutine(float start,float end,float frames)
    {
        if(isOnShockEffect) yield break;

        isOnShockEffect = true;
        for (int i = 0; i < frames; i++)
        {
            Time.timeScale = Mathf.Lerp(start, end, (float)i / frames);
            yield return null;
        }
        Time.timeScale = end;
        for (int i = 0; i < frames; i++)
        {
            Time.timeScale = Mathf.Lerp(end, start, (float)i / frames);
            yield return null;
        }
        isOnShockEffect = false;
        Time.timeScale = start;
    }
    #endregion


    public static void DelayAction(MonoBehaviour owner, float delay, Action action) => owner.StartCoroutine(DelayActionRoutine(delay, action));

    private static IEnumerator DelayActionRoutine(float delay, Action action)
    {
        yield return new WaitForSeconds(delay);
        action?.Invoke();
    }

    public static void FadeBlackOrOutBlack(CanvasGroup canvasGroup,float value,float time,Ease ease,GameObject gameObject, Action action)
    {
        canvasGroup.DOFade(value, time).SetEase(ease).SetLink(gameObject).OnComplete(() => { action?.Invoke(); });
    }
}
