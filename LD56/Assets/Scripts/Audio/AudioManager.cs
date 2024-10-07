using FMODUnity;
using FMOD.Studio;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum Audio_MusicArea { DAY = 0, NIGHT = 1, MINIGAME_FISHING = 2, MINIGAME = 3 }

/// <summary>
/// A signleton class that directly manages the audio.
/// 1. Creates and holds all running event instances
/// 2. Volume Control
/// </summary>
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Event Instances")]
    private EventInstance eventInstance_Music;
    private List<EventInstance> eventInstances = new();

    [Header("Volume Control")]
    private List<VolumeController> VC = new();
    public struct VolumeController
    {
        public VolumeType type;
        public float volume;
        public Bus bus;

        public VolumeController(VolumeType _type, float _value, Bus _bus)
        {
            type = _type;
            volume = _value;
            bus = _bus;
        }
    };

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        VC.Add(new(VolumeType.MUSIC, 1, RuntimeManager.GetBus("bus:/Music")));
        VC.Add(new(VolumeType.SFX, 1, RuntimeManager.GetBus("bus:/SFX")));

        InitializeMusic();
    }

    public void SetVolume(VolumeType type, float value)
    {
        int index = VC.FindIndex(item => item.type == type);
        if (index >= VC.Count) { return; }

        VolumeController vc = VC[index];
        vc.volume = value;
        vc.bus.setVolume(value);
        VC[index] = vc;
    }
    public float GetVolume(VolumeType type)
    {
        VolumeController vc = VC.First((item) => { return item.type == type; });
        return vc.volume;
    }

    public void PlayOneShot(EventReference sound) { RuntimeManager.PlayOneShot(sound, Vector3.zero); }

    private void InitializeMusic()
    {
        eventInstance_Music = CreateEventInstance(FMODEvents.Instance.BG);
        eventInstance_Music.start();
    }
    public void SetMusicArea(Audio_MusicArea area)
    {
        eventInstance_Music.setParameterByName("Music", (float)area);
    }
    public EventInstance CreateEventInstance(EventReference eventRef, bool isGameEvent = false)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(eventRef);
        eventInstances.Add(eventInstance); 
        RuntimeManager.AttachInstanceToGameObject(eventInstance, GetComponent<Transform>(), GetComponent<Rigidbody>());
        return eventInstance;
    }
    private void OnDestroy()
    {
        foreach (EventInstance ei in eventInstances)
        {
            ei.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            ei.release();
        }
    }
}