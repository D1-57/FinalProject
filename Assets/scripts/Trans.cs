using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class Trans : MonoBehaviour
{
    [SerializeField]
    private float duration;

    public void LookAt(Transform target)
    {
        transform.DOLookAt(target.position, duration);
    }
}