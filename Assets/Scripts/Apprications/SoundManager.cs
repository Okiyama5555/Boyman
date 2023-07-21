using UnityEngine;

namespace Application
{
    public class SoundManager : MonoBehaviour
    {
        private const string SOUNDMANAGER = "SoundManager";
        private const string SE = "SEPlayer";

        public static class SEName
        {
            public const string SHOOT = "Shoot";
            public const string BOMB = "Bomb";
            public const string PUSH = "Push";
            public const string DAMAGE = "Damage";
        }

        private AudioSource m_audio;

        private static SoundManager m_instance;
        public static SoundManager Instance { get { return m_instance; } }

        /// <summary>
        /// SoundManagerインスタンス作成
        /// </summary>
        /// <returns></returns>
        public static SoundManager Create()
        {
            if (m_instance != null) { return m_instance; }
            var obj = Resources.Load(SOUNDMANAGER) as GameObject;
            m_instance = Instantiate<GameObject>(obj).GetComponent<SoundManager>();
            return m_instance;
        }

        private static SEPlayer _seInstance;
        public static SEPlayer SEInstance
        {
            get
            {
                if (_seInstance != null) return _seInstance;
                _seInstance = m_instance.transform.Find(SE).GetComponent<SEPlayer>();
                return _seInstance;
            }
        }

        private void Awake()
        {
            m_audio = this.GetComponent<AudioSource>();
            m_audio.playOnAwake = false;
            m_audio.loop = true;
            m_audio.Play();
            DontDestroyOnLoad(gameObject);
        }

        /// <summary>
        /// BGMリセット
        /// </summary>
        public void Reset()
        {
            m_audio.Stop();
            m_audio.time = 0f;
            m_audio.Play();
        }
    }
}