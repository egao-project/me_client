using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoadAnimationSwitcher : MonoBehaviour {
    [SerializeField] private InitController LoadingObj;

    public void Shift()
    {
        LoadingObj.AutoLogin();
    }

}
