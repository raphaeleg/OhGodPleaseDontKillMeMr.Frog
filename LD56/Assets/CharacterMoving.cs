using DG.Tweening;
using UnityEngine;

public class CharacterMoving : MonoBehaviour
{
    [SerializeField] private Transform Lily;
    [SerializeField] private float cyclelength = 2;

    private void Start()
    {
        transform.DOMove(new Vector3(700, 700, 0), cyclelength).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
    }
}
