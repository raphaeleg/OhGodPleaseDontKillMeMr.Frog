using DG.Tweening;
using UnityEngine;

public class CharacterMoving : MonoBehaviour
{
    [SerializeField] private Transform Lily;
    [SerializeField] private float cyclelength = 2;

    private void Start()
    {
        //transform.DOMove(new Vector3(1250, 560, 0), cyclelength).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);//
        transform.DOMoveX(1425, 5);
    }
}
