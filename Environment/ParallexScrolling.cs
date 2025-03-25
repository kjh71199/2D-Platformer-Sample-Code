using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 배경 패럴랙스 효과 컴포넌트
public class ParallexScrolling : MonoBehaviour
{
    [SerializeField] private float parallaxEffect;  // 패럴랙스 효과 설정

    private Transform cameraTransform;              // 카메라 위치
    private Vector3 lastCameraPosition;             // 이전 카메라 위치

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

    // 카메라 위치 이동에 따라 배경 이동
    private void CameraUpdate(Camera cam)
    {
        Vector3 cameraDelta = cameraTransform.position - lastCameraPosition;

        transform.position += new Vector3(cameraDelta.x * parallaxEffect, 0, 0);

        lastCameraPosition = cameraTransform.position;
    }
}
