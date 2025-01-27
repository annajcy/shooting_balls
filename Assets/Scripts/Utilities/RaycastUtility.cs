using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public static class RaycastUtility
{
    public static GameObject GetCameraRaySelect(Vector3 mousePosition)
    {
        if (Camera.main != null)
        {
            Ray ray = Camera.main.ScreenPointToRay(mousePosition);
            if (Physics.Raycast(ray, out var rayCastHit)) return rayCastHit.collider.gameObject;
        }
        return null;
    }

    public static bool GetCameraRaySelectPoint(Vector3 mousePosition, out Vector3 point, List<string> inclusionTags = null)
    {
        point = Vector3.zero;

        if (IsPointerOverUI(mousePosition))
        {
            Debug.Log("On UI, return false");
            return false;
        }

        if (Camera.main != null)
        {
            Ray ray = Camera.main.ScreenPointToRay(mousePosition);

            if (Physics.Raycast(ray, out var rayCastHit, Mathf.Infinity))
            {
                GameObject hitGameObject = rayCastHit.collider.gameObject;
                Debug.Log(hitGameObject);

                if (hitGameObject == null) return false;
                else
                {
                    if (inclusionTags != null)
                    {
                        if (inclusionTags.Contains(hitGameObject.tag))
                        {
                            point = rayCastHit.point;
                            return true;
                        }
                        else return false;
                    }
                    else
                    {
                        point = rayCastHit.point;
                        return true;
                    }
                }
            }
        }

        return false;
    }

    private static bool IsPointerOverUI(Vector3 mousePosition)
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current) { position = mousePosition };
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);
        return results.Count > 0;
    }

}