using Services;
using System.Collections.Generic;
using UnityEngine;

public class ShadowDirUIController : MonoBehaviour
{
    private ObjectManager objectManager;
    private Dictionary<Shadow, ShadowDirUI> shadowDirUIDicts;

    private void Start()
    {
        objectManager = ServiceLocator.Get<ObjectManager>();
        shadowDirUIDicts = new Dictionary<Shadow, ShadowDirUI>();

        ShadowManager shadowManager = ServiceLocator.Get<ShadowManager>();
        shadowManager.OnSetShadow += GenerateShadowDirUI;
        shadowManager.OnCloneShadow += GenerateShadowDirUI;
        shadowManager.AfterRecallShadow += DestroyShadowDirUI;
    }

    private void GenerateShadowDirUI(Shadow shadow)
    {
        Transform shadowDirUITransform = objectManager.Activate(EObject.ShadowDirUI,
            Vector3.zero, transform).Transform;
        shadowDirUITransform.localScale = Vector3.one;
        ShadowDirUI shadowDirUI = shadowDirUITransform.GetComponent<ShadowDirUI>();
        shadowDirUI.Initialize(shadow);
        shadowDirUIDicts.Add(shadow, shadowDirUI);
    }

    private void DestroyShadowDirUI(Shadow shadow)
    {
        if (shadowDirUIDicts.ContainsKey(shadow))
        {
            shadowDirUIDicts[shadow].DestroySelf();
            shadowDirUIDicts.Remove(shadow);
        }
    }

    public void ResetUI()
    {
        foreach (var shadowDirUI in shadowDirUIDicts.Values)
        {
            shadowDirUI.DestroySelf();
        }
        shadowDirUIDicts.Clear();
    }
}
