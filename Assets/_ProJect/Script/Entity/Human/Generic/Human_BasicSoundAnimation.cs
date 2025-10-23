using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human_BasicSoundAnimation : MonoBehaviour
{
    [SerializeField] private AudioList_SO footSoundWalk;
    [SerializeField] private AudioList_SO footSoundRun;

    public void PlayFootStepsWalk() => footSoundWalk.PlaySound(transform);
    public void PlayFootStepsRun() => footSoundRun.PlaySound(transform);
}
