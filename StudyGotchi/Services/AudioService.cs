using System;
using System.IO;
using System.Media;
using System.Windows.Media;

namespace StudyGotchi.Services
{
    /// <summary>
    /// Handles sound effects (WAV) and background music (MP3/any MediaPlayer format).
    /// Place audio files in Assets/Audio/:
    ///   - sfx_start.wav, sfx_complete.wav, sfx_levelup.wav, sfx_reminder.wav, sfx_end.wav
    ///   - bgm.mp3  (background music — drop your preferred BGM file here)
    /// Falls back to SystemSounds if WAV files are missing.
    /// </summary>
    public class AudioService
    {
        private readonly MediaPlayer _bgmPlayer = new MediaPlayer();
        private bool _bgmLoaded = false;
        private bool _isMuted = false;

        public bool IsMuted
        {
            get => _isMuted;
            set
            {
                _isMuted = value;
                _bgmPlayer.IsMuted = value;
            }
        }

        private string AudioDir => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "Audio");

        public AudioService()
        {
            _bgmPlayer.MediaEnded += (s, e) =>
            {
                // Loop BGM
                _bgmPlayer.Position = TimeSpan.Zero;
                _bgmPlayer.Play();
            };
        }

        // Using MediaPlayer for SFX allows overlapping sounds and avoids SoundPlayer disposal bugs
        private readonly Dictionary<string, MediaPlayer> _sfxPlayers = new Dictionary<string, MediaPlayer>();

        /// <summary>
        /// Play a named SFX file (without extension) from Assets/Audio/.
        /// Falls back to a system beep if the file does not exist.
        /// </summary>
        public void PlaySfx(string name)
        {
            if (_isMuted) return;

            string path = Path.Combine(AudioDir, name + ".wav");
            if (File.Exists(path))
            {
                try
                {
                    if (!_sfxPlayers.TryGetValue(name, out var player))
                    {
                        player = new MediaPlayer();
                        player.Open(new Uri(path, UriKind.Absolute));
                        player.Volume = 0.8;
                        _sfxPlayers[name] = player;
                    }
                    player.Position = TimeSpan.Zero;
                    player.Play();
                }
                catch { /* Audio failure should never crash the app */ }
            }
            else
            {
                // Graceful fallback: system beep for distinctive events
                switch (name)
                {
                    case "sfx_complete":
                    case "sfx_levelup":
                        SystemSounds.Exclamation.Play();
                        break;
                    case "sfx_reminder":
                        SystemSounds.Asterisk.Play();
                        break;
                    default:
                        SystemSounds.Beep.Play();
                        break;
                }
            }
        }

        /// <summary>Start looping background music.</summary>
        public void PlayBgm()
        {
            if (_isMuted) return;

            string path = Path.Combine(AudioDir, "bgm.mp3");
            if (!File.Exists(path)) return;

            try
            {
                if (!_bgmLoaded)
                {
                    _bgmPlayer.Open(new Uri(path, UriKind.Absolute));
                    _bgmPlayer.Volume = 0.4; // Subtle background level
                    _bgmLoaded = true;
                }
                _bgmPlayer.Position = TimeSpan.Zero;
                _bgmPlayer.Play();
            }
            catch { }
        }

        /// <summary>Stop background music.</summary>
        public void StopBgm()
        {
            try { _bgmPlayer.Stop(); } catch { }
        }

        /// <summary>Pause background music (preserves position).</summary>
        public void PauseBgm()
        {
            try { _bgmPlayer.Pause(); } catch { }
        }

        /// <summary>Resume background music from paused position.</summary>
        public void ResumeBgm()
        {
            if (_isMuted) return;
            try { _bgmPlayer.Play(); } catch { }
        }
    }
}
