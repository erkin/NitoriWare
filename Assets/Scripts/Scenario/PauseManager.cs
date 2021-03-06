﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PauseManager : MonoBehaviour
{
	public UnityEvent onPause, onUnPause;

	//Whitelisted items won't be affected by pause
	public AudioSource[] audioSourceWhitelist;
	public MonoBehaviour[] scriptWhitelist;

	private bool paused;
	private float timeScale;

	private List<AudioSource> pausedAudioSources;
	private List<MonoBehaviour> disabledScripts;

	void Start ()
	{
		paused = false;
	}
	
	void Update ()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
			if (!paused)
				pause();
			else
				unPause();
	}

	void pause()
	{
		timeScale = Time.timeScale;
		Time.timeScale = 0f;

		AudioSource[] audioSources = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
		pausedAudioSources = new List<AudioSource>();
		List<AudioSource> whitelistedAudioSources = new List<AudioSource>(audioSourceWhitelist);
		foreach (AudioSource source in audioSources)
		{
			if (!whitelistedAudioSources.Remove(source) && source.isPlaying)
			{
				source.Pause();
				pausedAudioSources.Add(source);
			}
		}

		MonoBehaviour[] scripts = FindObjectsOfType(typeof(MonoBehaviour)) as MonoBehaviour[];
		disabledScripts = new List<MonoBehaviour>();
		List<MonoBehaviour> whitelistedScripts = new List<MonoBehaviour>(scriptWhitelist);
		foreach( MonoBehaviour script in scripts)
		{
			if (!whitelistedScripts.Remove(script) && script.enabled && script != this)
			{
				script.enabled = false;
				disabledScripts.Add(script);
			}
		}

		onPause.Invoke();
		if (MicrogameController.instance != null)
			MicrogameController.instance.onPause.Invoke();

		paused = true;
	}

	void unPause()
	{
		Time.timeScale = timeScale;

		foreach (AudioSource source in pausedAudioSources)
		{
			source.UnPause();
		}

		foreach (MonoBehaviour script in disabledScripts)
		{
			script.enabled = true;
		}

		onUnPause.Invoke();
		if (MicrogameController.instance != null)
			MicrogameController.instance.onUnPause.Invoke();

		paused = false;
	}
}
