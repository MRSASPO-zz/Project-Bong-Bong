﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour {
    public AudioClip[] clips;
    public static Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>();
    public GameObject audioSourcePrefab;

    private static AudioManager _instance;

    public static AudioManager Instance { get { return _instance; } }

    void Awake() {
        if(_instance != null && _instance != this) {
            Destroy(gameObject);
        } else {
            _instance = this;
        }

        
        DontDestroyOnLoad(gameObject);
        foreach(AudioClip clip in clips) {
            audioClips[clip.name] = clip;
        }
        bool createpool = ObjectPoolManager.Instance.createPool(audioSourcePrefab, 10, 100);
        if (!createpool) {
            ObjectPoolManager.Instance.resetPool(audioSourcePrefab.name);
        }
    }
}
