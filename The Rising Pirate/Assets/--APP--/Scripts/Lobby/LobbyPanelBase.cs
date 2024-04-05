using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyPanelBase : MonoBehaviour
{
    [field: SerializeField,  Header("LobbyPanelBase")]
    public LobbyPanelType panelType { get; private set; }
    protected LobbyUIManager lobbyUIManager;
    public enum LobbyPanelType
    {
        None,
        CreateNickNamePanel,
        MiddleSectionPanel
    }
    public virtual void InitPanel(LobbyUIManager UIManager)
    {
        lobbyUIManager = UIManager;
    }

    public void ShowPanel()
    {
        this.gameObject.SetActive(true);
       // const string POP_IN_CLIP_NAME = "In";   
    }

    protected void ClosePanel()
    {
        this.gameObject.SetActive(false);
    }
}
