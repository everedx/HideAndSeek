﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; protected set; }
    protected JsonSaver<GameDataStore> m_DataSaver;
    protected GameDataStore m_DataStore;

    public string masterVolumeParameter;
    public string sfxVolumeParameter;
    public string musicVolumeParameter;

    const string k_SavedGameFile = "save";

    public AudioMixer gameMixer;
    public static bool instanceExists
    {
        get { return instance != null; }
    }

    public LevelList levelList;

    protected virtual void Start()
    {
        SetVolumes(m_DataStore.masterVolume, m_DataStore.sfxVolume, m_DataStore.musicVolume, false);

    }
    protected void Awake()
    {
        instance = this;
        LoadData();
    }

    public void closeGame()
    {
        Debug.Log("Closing");
        Application.Quit();
    }

    public void loadScene1()
    {
        SceneManager.LoadScene("MainScene");
    }

    public virtual void SetVolumes(float master, float sfx, float music, bool save)
    {
        // Early out if no mixer set
        if (gameMixer == null)
        {
            return;
        }

        // Transform 0-1 into logarithmic -80-0
        if (masterVolumeParameter != null)
        {
            gameMixer.SetFloat(masterVolumeParameter, LogarithmicDbTransform(Mathf.Clamp01(master)));
        }
        if (sfxVolumeParameter != null)
        {
            gameMixer.SetFloat(sfxVolumeParameter, LogarithmicDbTransform(Mathf.Clamp01(sfx)));
        }
        if (musicVolumeParameter != null)
        {
            gameMixer.SetFloat(musicVolumeParameter, LogarithmicDbTransform(Mathf.Clamp01(music)));
        }

        if (save)
        {
            Debug.Log(Application.persistentDataPath);
            // Apply to save data too
            m_DataStore.masterVolume = master;
            m_DataStore.sfxVolume = sfx;
            m_DataStore.musicVolume = music;
            SaveData();
        }
    }

    public virtual void GetVolumes(out float master, out float sfx, out float music)
    {
        master = m_DataStore.masterVolume;
        sfx = m_DataStore.sfxVolume;
        music = m_DataStore.musicVolume;
    }


    protected static float LogarithmicDbTransform(float volume)
    {
        volume = (Mathf.Log(89 * volume + 1) / Mathf.Log(90)) * 80;
        return volume - 80;
    }

    protected virtual void SaveData()
    {
        m_DataSaver.Save(m_DataStore);
    }

    protected void LoadData()
    {
        // If it is in Unity Editor use the standard JSON (human readable for debugging) otherwise encrypt it for deployed version
#if UNITY_EDITOR
        m_DataSaver = new JsonSaver<GameDataStore>(k_SavedGameFile);
#else
			m_DataSaver = new EncryptedJsonSaver<GameDataStore>(k_SavedGameFile);
#endif

        try
        {
            if (!m_DataSaver.Load(out m_DataStore))
            {
                m_DataStore = new GameDataStore();
                SaveData();
            }
        }
        catch (Exception)
        {
            Debug.Log("Failed to load data, resetting");
            m_DataStore = new GameDataStore();
            SaveData();
        }
    }

    public bool IsLevelCompleted(string levelId)
    {
        if (!levelList.ContainsKey(levelId))
        {
            Debug.LogWarningFormat("[GAME] Cannot check if level with id = {0} is completed. Not in level list", levelId);
            return false;
        }

        return m_DataStore.IsLevelCompleted(levelId);
    }
    public int GetStarsForLevel(string levelId)
    {
        if (!levelList.ContainsKey(levelId))
        {
            Debug.LogWarningFormat("[GAME] Cannot check if level with id = {0} is completed. Not in level list", levelId);
            return 0;
        }

        return m_DataStore.GetNumberOfStarForLevel(levelId);
    }

    public void CompleteLevel(string levelId, int starsEarned)
    {
        if (!levelList.ContainsKey(levelId))
        {
            Debug.LogWarningFormat("[GAME] Cannot complete level with id = {0}. Not in level list", levelId);
            return;
        }

        m_DataStore.CompleteLevel(levelId, starsEarned);
        SaveData();
    }

    public virtual void restartProgress(float master, float sfx, float music)
    {
        m_DataStore = new GameDataStore();
        SetVolumes(master, sfx, music, true);
        //m_DataStore.
    }
}
