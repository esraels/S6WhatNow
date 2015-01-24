﻿using UnityEngine;
using System.Collections;
using System;

public class NetworkManager : MonoBehaviour
{
	private static NetworkManager m_instance;
	public static NetworkManager Instance
	{
		get
		{
			if ( m_instance == null )
			{
				m_instance = GameObject.FindObjectOfType<NetworkManager>();
			}
			return m_instance;
		}
	}
	
	[SerializeField] private GameObject m_serverPlayerPrefab;
	[SerializeField] private GameObject m_clientPlayerPrefab;
	
	private bool m_isHosting;
	public bool IsHosting
	{
		get { return m_isHosting; }
	}
	private bool m_isConnectedToServer;
	public bool IsConnectedToServer
	{	
		get { return m_isConnectedToServer; }
	}
	
	private Rect m_serverWindow = new Rect(20, 20, 200, 50);
	private Rect m_clientWindow = new Rect(20, 90, 200, 50);
		
	private string m_ipAddress = "";
	
	private void Start ()
	{
		m_isHosting = false;
		m_isConnectedToServer = false;
	}
	
	private void OnGUI ()
	{
		m_serverWindow = GUI.Window(0, m_serverWindow, ServerWindow, "Create");
		m_clientWindow = GUI.Window(1, m_clientWindow, ClientWindow, "Join");
	}
	
	private void ServerWindow (int p_id)
	{
		if (Network.peerType == NetworkPeerType.Disconnected)
		{
			if (GUILayout.Button("Create"))
			{
				InitializeServer();
			}
		}
		else
		{
			if (GUILayout.Button("Disconnect"))
			{
				DisconnectFromServer();
			}
		}
	}
	
	private void ClientWindow (int p_id)
	{
		GUILayout.BeginHorizontal();
		m_ipAddress = GUILayout.TextField(m_ipAddress, 25);
		if (GUILayout.Button("Join"))
		{
			ConnectToHost(m_ipAddress);
		}
		GUILayout.EndHorizontal();
	}
	
	private void InitializeServer ()
	{
		Network.incomingPassword = "";
		Network.InitializeServer(2, 2500, !Network.HavePublicAddress());
	}
	
	private void OnServerInitialized ()
	{	
		m_isHosting = true;
		SpawnServerPlayer();
	}
	
	private void DisconnectFromServer ()
	{
		Network.Disconnect();
	}
	
	private void ConnectToHost (string p_ipAddress)
	{
		Network.Connect(p_ipAddress, 2500, "");
	}
	
	private void OnConnectedToServer ()
	{
		m_isConnectedToServer = true;
		SpawnClientPlayer();
	}
	
	private void OnFailedToConnect ()
	{
	}
	
	private void OnDisconnectedFromServer ()
	{
	}
	
	public bool IsServer ()
	{
		return Network.isServer;
	}
	
	public bool IsClient ()
	{
		return Network.isClient;
	}
	
	private void SpawnServerPlayer ()
	{
		Network.Instantiate(m_serverPlayerPrefab, Vector3.zero, Quaternion.identity, 0);
	}
	
	private void SpawnClientPlayer ()
	{
		Network.Instantiate(m_clientPlayerPrefab, Vector3.zero, Quaternion.identity, 0);
	}
}
