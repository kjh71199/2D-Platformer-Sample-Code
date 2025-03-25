using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ��� �з����� ȿ�� ������Ʈ
public class ParallexScrolling : MonoBehaviour
{
    [SerializeField] private float parallaxEffect;  // �з����� ȿ�� ����

    private Transform cameraTransform;              // ī�޶� ��ġ
    private Vector3 lastCameraPosition;             // ���� ī�޶� ��ġ

    void Start()
    {
        cameraTransform = Camera.main.transform;
        lastCameraPosition = cameraTransform.position;

        Camera.onPreRender += CameraUpdate;
    }

    private void OnDisable()
    {
        Camera.onPreRender -= CameraUpdate;
    }

    // ī�޶� ��ġ �̵��� ���� ��� �̵�
    private void CameraUpdate(Camera cam)
    {
        Vector3 cameraDelta = cameraTransform.position - lastCameraPosition;

        transform.position += new Vector3(cameraDelta.x * parallaxEffect, 0, 0);

        lastCameraPosition = cameraTransform.position;
    }
}
