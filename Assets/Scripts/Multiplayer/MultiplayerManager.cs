using Colyseus;
using System;
using UnityEngine;

public class MultiplayerManager : ColyseusManager<MultiplayerManager>
{
    [SerializeField] private GameObject _player; 
    [SerializeField] private GameObject _enemy; 

    private ColyseusRoom<State> _room;

    protected override void Awake()
    {
        base.Awake();

        Instance.InitializeClient();
        Connect();
    }

    public async void Connect()
    {
        _room = await Instance.client.JoinOrCreate<State>("state_handler");

        _room.OnStateChange += OnChange;
    }

    private void OnChange(State state, bool isFirstState)
    {
        if (!isFirstState) return;

        // создание на сцене игрока
        var player = state.players[_room.SessionId];
        InstantiatePlayer(_player, player);

        // создание на сцене противников
        state.players.ForEach(ForEachEnemy);
    }

    private void ForEachEnemy(string key, Player player)
    {
        if (key == _room.SessionId) return;

        InstantiatePlayer(_enemy, player);
    }

    private void InstantiatePlayer(GameObject prefab, Player player)
    {
        Vector3 position = new Vector3(player.x - 200, 0, player.y - 200) / 8;
        Instantiate(prefab, position, Quaternion.identity);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        _room.Leave();
    }
}
