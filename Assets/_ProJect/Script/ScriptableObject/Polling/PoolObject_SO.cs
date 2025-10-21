using UnityEngine;

[CreateAssetMenu(fileName = "PoolObject", menuName = "Pooling/Pool Object")]
public class PoolObject_SO : ScriptableObject
{
    [Header("Settings")]

    public Pool_Obj PreFab;
    public ObjTypePoolling ObjTypePoolling;
    public float LifeTime = 5;

    public int StartSize = 10;
    public int MaxSizePool = 20;
}