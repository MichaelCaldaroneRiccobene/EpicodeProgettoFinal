using System.Collections;
using UnityEngine;

public class Pool_Obj : MonoBehaviour
{
    private float lifeTime;
    private ObjTypePoolling objTypePoolling;

    private Rigidbody rb;

    private WaitForSeconds waitLife;
    private Coroutine lifeTimeCoroutine;

    private void OnEnable()
    {
        if (rb == null) rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.velocity = Vector3.zero;
        }

        lifeTimeCoroutine = StartCoroutine(LifeRoutine());
    }

    public void SetUp(float lifeTime, ObjTypePoolling objTypePoolling)
    {
        this.lifeTime = lifeTime;
        this.objTypePoolling = objTypePoolling;

        if (this.lifeTime > 0) waitLife = new WaitForSeconds(this.lifeTime);
        else
        {
            Debug.LogWarning("lifeTime Not Correct ", transform);
            waitLife = new WaitForSeconds(1);
        }
    }

    public void SetUpNewTime(float lifeTime) => this.lifeTime = lifeTime;

    private IEnumerator LifeRoutine()
    {
        yield return new WaitForSeconds(lifeTime);
        ManagerPooling.Instance.ReturnToPool(objTypePoolling, this);
    }

    private void OnDisable()
    {
        if (lifeTimeCoroutine != null)
        {
            StopCoroutine(lifeTimeCoroutine);
            lifeTimeCoroutine = null;
        }

        if (rb == null) rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.isKinematic = true;
        }
    }
}
