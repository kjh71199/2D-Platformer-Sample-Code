using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 물 출렁임 효과 컴포넌트
public class WaterMoveEffect : MonoBehaviour
{
    private WaitForSeconds WaitForSeconds;

    private void Awake()
    {
        WaitForSeconds = new WaitForSeconds(1f);
    }

    private void Start()
    {
        StartCoroutine(MoveEffect());
    }

    private IEnumerator MoveEffect()
    {
        while (true) 
        {
            transform.position -= new Vector3(1f, 0f, 0f);

            yield return WaitForSeconds;

            transform.position += new Vector3(1f, 0f, 0f);

            yield return WaitForSeconds;
        }
    }
}
