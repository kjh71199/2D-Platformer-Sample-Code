using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ī�޶� �̵� ������Ʈ
public class CameraMovement : MonoBehaviour
{
    [SerializeField] float lookDistance;        // ī�޶� �̵� �Ÿ�
    [SerializeField] Transform lookTransform;   // ī�޶� �̵� ��ġ

    private IEnumerator camerMovement;          // ī�޶� �̵� �ڷ�ƾ ����

    private void Start()
    {
        camerMovement = CameraMovementCoroutine();
        StartCoroutine(camerMovement);
    }

    // ī�޶� �̵� �ڷ�ƾ
    private IEnumerator CameraMovementCoroutine()
    {
        float h = 0f;
        float v = 0f;

        while (true)
        {
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
                h = Input.GetAxis("Horizontal");
            else
                h = 0f;
            
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
                v = Input.GetAxis("Vertical");
            else
                v = 0f;

            float xPos = Mathf.Clamp(lookDistance * h, -lookDistance, lookDistance);
            float yPos = Mathf.Clamp(lookDistance * v, -lookDistance, lookDistance);

            lookTransform.position = new Vector2(transform.position.x + xPos, transform.position.y + yPos);
            yield return null;
        }
    }
}
