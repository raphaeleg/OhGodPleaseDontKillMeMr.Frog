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
        Lily.DORotate(new Vector3(0, 0, 7), cyclelength * 0.35f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);

        //Asking for an animal [Event Dialogue / popup]

        //Wait until the the player does an action that provides them with the animal

        //Give Money based on if they liked it / Adjust Suspicion meter

        //Leave

        //Repeat 2 more times

        //After this go to Night Time

    }
}
