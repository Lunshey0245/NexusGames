using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnerController : NetworkBehaviour , IPlayerJoined , IPlayerLeft
{
    [SerializeField] private NetworkPrefabRef playerNetworkPrefab = NetworkPrefabRef.Empty;
    [SerializeField] private Transform[] spawnPoints;

    public override void Spawned()
    {
        if(Runner.IsServer)
        {
            foreach(var item in Runner.ActivePlayers)
            {
                SpawnPlayer(item);
            }
        }
    }

    private void SpawnPlayer(PlayerRef playerRef)
    {
        if (Runner.IsServer)
        {
            var index = playerRef % spawnPoints.Length;
            var spawnPoint = spawnPoints[index].transform.position; ;
            var playerObject = Runner.Spawn(playerNetworkPrefab, spawnPoint, Quaternion.identity , playerRef);

            Runner.SetPlayerObject(playerRef, playerObject);
        }
    }

    private void DespawnedPlayer(PlayerRef playerRef)
    {
        if (Runner.IsServer)
        {
            if (Runner.TryGetPlayerObject(playerRef , out var playerNetworkObject))
            {
                Runner.Despawn(playerNetworkObject);
            }
            Runner.SetPlayerObject(playerRef,null);
        }
    }
    public void PlayerJoined(PlayerRef player)
    {
       SpawnPlayer(player);
    }

    public void PlayerLeft(PlayerRef player)
    {
        DespawnedPlayer(player);
    }
}
