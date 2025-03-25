using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �� �ⷷ�� ȿ�� ������Ʈ
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
