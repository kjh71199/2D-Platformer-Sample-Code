using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ǥ���� UI ǥ�� ������Ʈ
public class SignPostPopup : MonoBehaviour
{
    [SerializeField] private GameObject popupUI;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("Player"))
        {
            popupUI.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag.Equals("Player"))
        {
            popupUI.SetActive(false);
        }
    }
}
