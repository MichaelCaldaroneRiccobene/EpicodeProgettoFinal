using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class GenerateInEditor : MonoBehaviour
{
    [Header("General Setting")]
    [SerializeField] private GameObject[] objs;
    [SerializeField] private Vector3[] definiteRotation;

    [SerializeField] private int xNumber;
    [SerializeField] private int zNumber;
    [SerializeField] private int yNumber;

    [SerializeField] private float space = 1;

    [Header("Random Rotation Setting")]
    [SerializeField] private bool isRandomRotationAll;
    [SerializeField] private bool isRandomRotationX;
    [SerializeField] private bool isRandomRotationY;
    [SerializeField] private bool isRandomRotationZ;

    [Header("Random Value")]
    [SerializeField] private float maxRotationX = 360;
    [SerializeField] private float minRotationX = 0;

    [SerializeField] private float maxRotationY = 360;
    [SerializeField] private float minRotationY = 0;

    [SerializeField] private float maxRotationZ = 360;
    [SerializeField] private float minRotationZ = 0;

    private Vector3 startPos;
    private Vector3 upDatePos;

    private float maxRot = 360;
    private float minRot = 0;

    [ContextMenu("GenerateObjs")]
    public void GenerateObjs()
    {
        startPos = transform.position;
        upDatePos = transform.position;


        for (int i = transform.childCount - 1; i >= 0; i--) DestroyImmediate(transform.GetChild(i).gameObject);

        for (int i = 0; i < zNumber; i++)
        {
            for (int j = 0; j < yNumber; j++)
            {
                for (float k = 0; k < xNumber; k++)
                {
                    SpawnObj();
                }
                upDatePos.x = startPos.x;
                upDatePos.y += space;
            }
            upDatePos.y = startPos.y;
            upDatePos.z += space;
        }
    }

    private void SpawnObj()
    {
        int randomObj = Random.Range(0, objs.Length);
        GameObject obj = Instantiate(objs[randomObj], upDatePos, Quaternion.identity, transform);

        ObjRotation(obj);
        upDatePos.x += space;
    }

    private void ObjRotation(GameObject obj)
    {
        if(definiteRotation.Length > 0)
        {
            int selectRotation = Random.Range(0, definiteRotation.Length);
            obj.transform.localRotation = Quaternion.Euler(definiteRotation[selectRotation]);
            return;
        }


        if (isRandomRotationAll)
        {
            obj.transform.rotation = Quaternion.Euler(Random.Range(minRot, maxRot), Random.Range(minRot, maxRot), Random.Range(minRot, maxRot));
            return;
        }
        Vector3 finalRotation = obj.transform.eulerAngles;

        if (isRandomRotationX) finalRotation.x = Random.Range(minRotationX, maxRotationX);
        if (isRandomRotationY) finalRotation.y = Random.Range(minRotationY, maxRotationY);
        if (isRandomRotationZ) finalRotation.z = Random.Range(minRotationZ, maxRotationZ);

        obj.transform.rotation = Quaternion.Euler(finalRotation) * transform.rotation;
    }
}
