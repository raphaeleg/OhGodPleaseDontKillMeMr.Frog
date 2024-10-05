using DG.Tweening;
using UnityEngine;

public class CharacterMoving : MonoBehaviour
{
    [SerializeField] private Transform Lily;
    [SerializeField] private float cyclelength = 2;

    private void Start()
    {
        //transform.DOMove(new Vector3(1250, 560, 0), cyclelength).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);//
        transform.DOMoveX(1300, 5);
        Lily.DORotate(new Vector3(0, 0, 10), cyclelength/3).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);


    }
}
