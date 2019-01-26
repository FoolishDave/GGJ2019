using Cinemachine;
using Rewired;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    public int NumPlayers
    {
        get
        {
            return players.Where(p => p != null).Count();
        }
    }

    public Player[] Players
    {
        get
        {
            return players.ToArray();
        }
    }

    public GameObject[] PlayerObjects
    {
        get
        {
            GameObject[] objects = new GameObject[players.Count];
            for (int i = 0; i < players.Count; i++) {
                objects[i] = playerObjects[players[i]];
            }
            return objects;
        }
    }

    public GameObject playerObj;
    public List<Color> playerColors = new List<Color>();
    public CinemachineTargetGroup targetGroup;

    private List<Player> players = new List<Player>();
    private Dictionary<Player, GameObject> playerObjects = new Dictionary<Player, GameObject>();

    private void OnEnable()
    {
        if (Instance != null) Destroy(this);
        else Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        foreach (Player player in ReInput.players.AllPlayers)
        {
            if (player.GetAnyButton() && !player.GetButton("Leave") && !player.isPlaying)
            {
                SpawnPlayer(player.id);
            }
        }
    }

    public void SpawnPlayer(int playerId)
    {
        SpawnPlayer(ReInput.players.GetPlayer(playerId));
    }

    public void SpawnPlayer(Player player)
    {
        player.isPlaying = true;

        PlayerJoinedArgs args = new PlayerJoinedArgs();
        args.player = player;
        args.playerId = player.id;
        OnPlayerJoined(args);

        GameObject newPlayer = Instantiate(playerObj);

        if (players.Any(p => p == null)) {
            int index = players.FindIndex(p => p == null);
            args.playerNum = index;
            Debug.Log("Player spawned as player: " + index);
            players[index] = player;
            newPlayer.GetComponent<Renderer>().material.SetColor("_BaseColor", playerColors[index]);
        } else {
            Debug.Log("Spawned as player: " + players.Count);
            newPlayer.GetComponent<Renderer>().material.SetColor("_BaseColor", playerColors[players.Count]);
            args.playerNum = players.Count;
            players.Add(player);

        }

        FriendController controller = newPlayer.GetComponent<FriendController>();
        controller.playerId = player.id;
        controller.playerNum = args.playerNum;
        controller.Respawn();

        playerObjects.Add(player, newPlayer);
        targetGroup.AddMember(newPlayer.transform, 1f, 0f);
    }

    public void DespawnPlayer(int playerId)
    {
        DespawnPlayer(ReInput.players.GetPlayer(playerId));
    }

    public void DespawnPlayer(Player player)
    {
        PlayerLeftArgs args = new PlayerLeftArgs();
        args.player = player;
        args.playerId = player.id;
        args.playerNum = players.IndexOf(player);
        

        player.isPlaying = false;
        GameObject playerObject = playerObjects[player];
        players[args.playerNum] = null;
        playerObjects.Remove(player);
        Destroy(playerObject);
        OnPlayerLeft(args);
    }

    public event EventHandler<PlayerJoinedArgs> PlayerJoined;
    public event EventHandler<PlayerLeftArgs> PlayerLeft;

    protected virtual void OnPlayerJoined(PlayerJoinedArgs e)
    {
        PlayerJoined?.Invoke(this, e);
    }

    protected virtual void OnPlayerLeft(PlayerLeftArgs e)
    {
        PlayerLeft?.Invoke(this, e);
    }
}

public class PlayerJoinedArgs : EventArgs
{
    public int playerId;
    public Player player;
    public int playerNum;
}

public class PlayerLeftArgs : EventArgs
{
    public int playerId;
    public Player player;
    public int playerNum;
}

