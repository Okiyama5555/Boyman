using UnityEngine;

namespace Application
{
    public class SEPlayer : MonoBehaviour
    {
        private AudioSource m_audio;
        private AudioClip m_audioClip;

        private void Start()
        {
            m_audio = GetComponent<AudioSource>();
        }

        /// <summary>
        /// AudioClipを探して再生する
        /// </summary>
        /// <param name="SEName"></param>
        public void FindClipAndPlay(string SEName)
        {
            m_audioClip = Resources.Load<AudioClip>("Audio/" + SEName);
            m_audio.clip = m_audioClip;
            m_audio.Play();
        }
    }
}
