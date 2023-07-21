using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Character;
using Application;

namespace Manager
{
    public class BaseManager : MonoBehaviour
    {
        public static class SceneName
        {
            public const string Title = "Title";
            public const string Main = "Main";
            public const string Result = "Result";
        }

        private const string FADE = "Fade";
        private GameObject m_fade;

        protected void AudioReset()
        {
            SoundManager.Instance.Reset();
        }

        protected void SceneChange(string name)
        {
            SceneOut(() => { SceneManager.LoadScene(name); });
        }

        /// <summary>
        /// 演出としてFadeOutする
        /// </summary>
        /// <param name="action"></param>
        private void SceneOut(UnityAction action)
        {
            var fade = GameObject.Find(FADE).GetComponent<Image>();
            var color = fade.color;
            while (fade.color.a < 1)
            {
                color.a += 0.001f;
                fade.color = color;
            }
            action.Invoke();
        }

        protected void SceneIn()
        {
            var fade = GameObject.Find(FADE).GetComponent<Image>();
            var color = fade.color;
            while (fade.color.a > 0)
            {
                color.a -= 0.001f;
                fade.color = color;
            }
        }
    }
}
