using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ConnectionPopUp : MonoBehaviour
{
    public TextMeshProUGUI Message;
    public GameObject Ok_Button;
    [SerializeField] private string[] PopUpMessages;

    [HideInInspector] public uint currentMessageIndex = uint.MaxValue;

    public IEnumerator DelayedCallPopUp(uint messageIndex, float waitTime = 0f)
    {
        yield return new WaitForSeconds(waitTime);
        CallPopUp(messageIndex);
    }

    public void CallPopUp(uint messageIndex)
    {
        //if (currentMessageIndex == messageIndex) return;

        currentMessageIndex = messageIndex;
        gameObject.SetActive(true);
        Message.text = PopUpMessages[messageIndex];
        Ok_Button.SetActive(messageIndex > 1);
    }

    public void HidePopUp()
    {
        if (gameObject.activeSelf)
        {
            currentMessageIndex = uint.MaxValue;
            gameObject.SetActive(false);
        }
    }

    public void Return()
    {
        HidePopUp();
        //EOSSDKComponent.Instance.ReturnToLobby();
    }
}
