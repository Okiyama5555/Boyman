using UnityEngine;
using System;

namespace Application
{
    public class App : MonoBehaviour
    {
        private const string APPPATH = "App";

        private static App m_instance = null;
        public static App Instance { get { return m_instance; } }
        public int Score { get; private set; }
        public int MaxScore { get; private set; }

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        /// <summary>
        /// 自身のインスタンス作成
        /// </summary>
        /// <returns></returns>
        public static App Create()
        {
            if (m_instance != null) { return m_instance; }
            var Obj = Resources.Load(APPPATH) as GameObject;
            m_instance = Instantiate<GameObject>(Obj).GetComponent<App>();
            return m_instance;
        }

        /// <summary>
        /// 文字列をEnumの型に変換できるように
        /// </summary>
        /// <param name="objectName"></param>
        /// <returns></returns>
        public static GameObjectTags StringToObjectTag(string objectName)
        {
            var exists = Enum.IsDefined(typeof(GameObjectTags), objectName);
            if (exists)
            {
                return (GameObjectTags)Enum.Parse(typeof(GameObjectTags), objectName);
            }
            Debug.LogError("Non-existent tag.");
            return GameObjectTags.NotExists;
        }

        /// <summary>
        /// Appインスタンスのスコアリセット
        /// </summary>
        public void ScoreReset()
        {
            if (MaxScore < Score)
            {
                MaxScore = Score;
            }
            Score = 0;
        }

        /// <summary>
        /// スコア加算
        /// </summary>
        /// <param name="add"></param>
        public void AddScore(int add)
        {
            Score += add;
        }

        /// <summary>
        /// 判定で使用できるタグ一覧
        /// </summary>
        public enum GameObjectTags
        {
            NotExists,
            Object,
            Player,
            Bullet,
            Enemy,
        }
    }
}