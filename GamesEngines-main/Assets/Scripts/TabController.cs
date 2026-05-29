using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class TabController : MonoBehaviour
{
    public Image[] tabImages;
    public GameObject[] pages;

    void Start()
    {
        ActivateTab(0);
    }

    void Update()
    {
        // New Input System way to check for a mouse click safely
        if (UnityEngine.InputSystem.Mouse.current != null &&
            UnityEngine.InputSystem.Mouse.current.leftButton.wasPressedThisFrame)
        {
            PointerEventData pointerData = new PointerEventData(EventSystem.current);
            pointerData.position = UnityEngine.InputSystem.Mouse.current.position.ReadValue();

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);

            if (results.Count > 0)
            {
                // Prints the exact name of the physical layer your mouse is touching
                Debug.LogWarning("PHYSICAL CLICK HIT OBJECT: " + results[0].gameObject.name);
            }
        }
    }

    public void ActivateTab(int tabNum)
    {
        if (tabNum < 0 || tabNum >= pages.Length) return;

        Debug.Log("ActivateTab was successfully called for index: " + tabNum);

        for (int i = 0; i < pages.Length; i++)
        {
            if (pages[i] != null) pages[i].SetActive(i == tabNum);
            if (i < tabImages.Length && tabImages[i] != null) tabImages[i].enabled = (i == tabNum);
        }
    }
}
