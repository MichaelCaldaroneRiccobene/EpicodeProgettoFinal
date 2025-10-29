using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AnimationUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Setting Animation")]
    [SerializeField] private Vector3 custmMaxScale = new Vector3(1.2f, 1.2f, 1.2f);
    [SerializeField] private float rotationZForce = 16f;
    [SerializeField] private float timeAnimation = 0.5f;

    [Header("Setting Audio")]
    [SerializeField] private AudioList_SO clipSoundSelect;

    private Vector3 originalScale;
    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();

        originalScale = transform.localScale;
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        if (clipSoundSelect) clipSoundSelect.PlaySound(transform);
        transform.DOScale(custmMaxScale, timeAnimation).SetLink(gameObject);

        AnimationUIOnMouse(eventData);
    }

    private void AnimationUIOnMouse(PointerEventData eventData)
    {
        Vector2 mousePos = eventData.position;
        Vector2 buttonCenter = RectTransformUtility.WorldToScreenPoint(eventData.pressEventCamera, rectTransform.position);

        float tot = Mathf.Abs((mousePos.x - buttonCenter.x) / rotationZForce);
        bool fromRight = mousePos.x > buttonCenter.x;

        if (fromRight) tot *= -1f;
        transform.DORotate(new Vector3(0, 0, tot), timeAnimation).SetEase(Ease.OutQuad).SetLink(gameObject); ;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOScale(originalScale, timeAnimation).SetLink(gameObject);
        transform.DORotate(new Vector3(0, 0, 0f), timeAnimation).SetLink(gameObject);
    }

    private void OnDisable()
    {
        transform.DOKill();
    }
    private void OnDestroy()
    {
        DOTween.Kill(this);
    }
}
