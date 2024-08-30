using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.UI;

namespace StarterPack.Audio
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance;

        private const string ALLSOUNDS = "AllSounds";
        private const string MUSIC = "Music";
        private const string SFX = "Sfx";
        private const string ASFX = "AmbientSfx";
        private const string MUSICSOURCENAME = "MusicSource";
        private const string EFFECTSSOURCENAME = "EffectsSource";

        [SerializeField] private AudioSource _musicSource;
        [SerializeField] private AudioSource _effectsSource;
        [SerializeField] private AudioMixer _mixer;
        [SerializeField] private Slider _allSoundsVolumeSlider;
        [SerializeField] private Slider _musicVolumeSlider;
        [SerializeField] private Slider _effectsVolumeSlider;

        [SerializeField] private AudioClip _getMoneySfx;

        public UnityEvent<bool> OnMuteEvent { get; private set; } = new();

        protected void Awake()
        {
            Instance = this;
            EnsureAudioSources();
        }

        protected void Start()
        {
            LoadSettings();
        }

        private void EnsureAudioSources()
        {
            if(_musicSource == null)
            {
                GameObject musicSourceObject = new(MUSICSOURCENAME);
                musicSourceObject.transform.SetParent(transform);
                _musicSource = musicSourceObject.AddComponent<AudioSource>();
            }

            if(_effectsSource == null)
            {
                GameObject effectsSourceObject = new(EFFECTSSOURCENAME);
                effectsSourceObject.transform.SetParent(transform);
                _effectsSource = effectsSourceObject.AddComponent<AudioSource>();
            }
        }

        public void PlayMusic(AudioClip clip)
        {
            if(_musicSource != null)
            {
                _musicSource.clip = clip;
                _musicSource.Play();
            }
        }

        public void PlayEffect(AudioClip clip)
        {
            if(_effectsSource != null)
            {
                _effectsSource.PlayOneShot(clip);
            }
        }


        public void ControlAllSouns(float value)
        {
            PlayerPrefs.SetFloat(ALLSOUNDS, value);
            _mixer.SetFloat(ALLSOUNDS, value);

            OnMuteEvent.Invoke(false);
        }


        public void ControlMusic(float value)
        {
            PlayerPrefs.SetFloat(MUSIC, value);
            _mixer.SetFloat(MUSIC, value);

            OnMuteEvent.Invoke(false);
        }

        public void ControlSfx(float value)
        {
            PlayerPrefs.SetFloat(SFX, value);
            _mixer.SetFloat(SFX, value);
            _effectsSource.Play();

            OnMuteEvent.Invoke(false);
        }

        public void ControlAmbientSfx(float value)
        {
            PlayerPrefs.SetFloat(ASFX, value);
            _mixer.SetFloat(ASFX, value);

            OnMuteEvent.Invoke(false);
        }

        public void LoadSettings()
        {
            var allSoundsVal = PlayerPrefs.GetFloat(ALLSOUNDS);
            _allSoundsVolumeSlider.value = allSoundsVal;

            var musicVal = PlayerPrefs.GetFloat(MUSIC);
            _musicVolumeSlider.value = musicVal;

            var sfxVal = PlayerPrefs.GetFloat(SFX);
            _effectsVolumeSlider.value = sfxVal;

        }


        public void Mute(bool isOffMusic)
        {
            var maxVal = PlayerPrefs.GetFloat(ALLSOUNDS, 0);
            var value = isOffMusic ? _allSoundsVolumeSlider.minValue : maxVal;
            _mixer.SetFloat(ALLSOUNDS, value);

        }

        internal void GetMoneySfx()
        {
            PlayEffect(_getMoneySfx);
        }
    }
}
