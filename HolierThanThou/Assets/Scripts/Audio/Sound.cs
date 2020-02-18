﻿using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sound
{
	public string name;

	public AudioClip clip;
	
	[Range(0, 1f)]
	public float volume = 1f;
	[Range(0.1f, 3f)]
	public float pitch = 1f;
	[Range(0, 1f)]
	public float spatialBlend = 1f;

	public bool loop;
	public SoundType type;

	[HideInInspector]
	public AudioSource source;

	public enum SoundType
	{
		SFX, Music
	}
}
