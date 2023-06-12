using UnityEngine;
using Services;
using UnityEngine.EventSystems;

public class UIAudioController : MonoBehaviour, IPointerEnterHandler,IPointerClickHandler
{
    private AudioManager audioManager;

    private void Start()
    {
        audioManager = ServiceLocator.Get<AudioManager>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        audioManager.PlaySound("PointerOnUI",AudioPlayMode.Plenty,0f);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        audioManager.PlaySound("PointerClickUI",AudioPlayMode.Plenty,0f);
    }
}
