using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shiedl : MonoBehaviour
{
    [SerializeField] private AnimatorOverrideController blockIdle;

    [SerializeField] private AudioList_SO soundBlock;
    [SerializeField] private GameObject vfxReact;

    private float timeLifeVFXReact = 0.5f;

    public AnimatorOverrideController BlockIdle => blockIdle;

    private void Awake()
    {
        if (vfxReact != null) vfxReact.SetActive(false);
    }

    public void OnBlockReact(){ if (vfxReact != null) StartCoroutine(OnBlockReactRoutine()); } 

    private IEnumerator OnBlockReactRoutine()
    {
        vfxReact.SetActive(false);
        yield return null;
        vfxReact.SetActive(true);
        soundBlock.PlaySound(transform);

        yield return new WaitForSeconds(timeLifeVFXReact);

        vfxReact.SetActive(false);
    }
}
