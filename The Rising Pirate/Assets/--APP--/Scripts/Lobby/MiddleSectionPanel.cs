using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Fusion;
public class MiddleSectionPanel : LobbyPanelBase
{
    [Header("MiddleSectionPanel")]
    [SerializeField] private Button joinRandomRoomBtn;
    [SerializeField] private Button joinRoomByArgBtn;
    [SerializeField] private Button createRoomBtn;

    [SerializeField] private TMP_InputField joinRoomByArgInputField;
    [SerializeField] private TMP_InputField createRoomInputField;

    private NetworkRunnerController networkRunnerController;
    private const int MAX_CHAR_FOR_ROOM = 2;

    public override void InitPanel(LobbyUIManager UIManager)
    {
        base.InitPanel(UIManager);
        networkRunnerController = GlobalManagers.Instance.NetworkRunnerController;
        joinRandomRoomBtn.onClick.AddListener(JoinRandomRoom);
        joinRoomByArgBtn.onClick.AddListener(() => CreateRoom(GameMode.Client, joinRoomByArgInputField.text));
        createRoomBtn.onClick.AddListener(() => CreateRoom(GameMode.Host, createRoomInputField.text));
    }

    private void CreateRoom(GameMode mode, string field)
    {
        if (field.Length >= MAX_CHAR_FOR_ROOM)
        {
            Debug.Log($"----------------{mode}---------------");

            networkRunnerController.StartGame(mode, field);
        }
    }

    private void JoinRandomRoom()
    {
        Debug.Log($"----------------JoinRandomRoom!---------------");
        networkRunnerController.StartGame(GameMode.AutoHostOrClient, string.Empty);
    }
}
