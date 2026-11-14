using UnityEngine;

public class AudienceReactions : MonoBehaviour
{
    Animator[] animators;

    void Awake() { animators = GetComponentsInChildren<Animator>(); }

    public void Wrong() { foreach (var a in animators) a.SetTrigger("Wrong"); }
    public void Right() { foreach (var a in animators) a.SetTrigger("Right"); }
}