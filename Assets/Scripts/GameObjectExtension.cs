using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameObjectExtension {
    public static void SetActiveChilds(this GameObject gameObject, bool active)
    {
        int count = gameObject.transform.childCount;
        for (int i = 0; i != count; i++) {
            gameObject.transform.GetChild(0).gameObject.SetActive(active);
        }
    }

}
