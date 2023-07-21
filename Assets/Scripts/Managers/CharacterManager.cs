using UnityEngine;
using UnityEngine.Events;
using Manager;
using Application;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace Character
{
    /// <summary>
    /// Character管理クラス
    /// </summary>
    public class CharacterManager : BaseManager
    {

        private const string CHARACTERMANAGERNAME = "CharacterManager";

        [SerializeField] List<Transform> m_enemyFlagObject = new List<Transform>();
        [SerializeField] GameObject m_enemyObject;
        [SerializeField] ParticleSystem m_enemyDeadParticle;
        [SerializeField] Transform m_playerInitPos;
        [SerializeField] ParticleSystem m_playerDeadParticle;
        [SerializeField] GameObject m_healthGroup;
        [SerializeField] GameObject m_healthImage;

        private List<GameObject> m_heartList = new List<GameObject>();
        private const int ADDSCORE = 300;

        private static CharacterManager m_instance = null;
        public static CharacterManager Instance
        {
            get
            {
                if (m_instance != null) { return m_instance; }
                m_instance = GameObject.Find(CHARACTERMANAGERNAME).GetComponent<CharacterManager>();
                return m_instance;
            }
        }

        /// <summary>
        /// リストに登録された座標の情報から敵作成
        /// </summary>
        public void SpawnEnemy()
        {
            foreach (Transform spawnPoint in m_enemyFlagObject)
            {
                var enemy = Instantiate(m_enemyObject, spawnPoint).GetComponent<Enemy>();
                enemy.transform.SetParent(spawnPoint);
            }
        }

        /// <summary>
        /// player作成メソッド
        /// </summary>
        public void SpawnPlayer()
        {
            var player = Character.Player.Create(m_playerInitPos);
            player.transform.SetParent(this.gameObject.transform);
        }

        /// <summary>
        /// HPの数だけハートを表示する
        /// </summary>
        /// <param name="health"></param>
        public void DisplayHeart(int health)
        {
            for (int i = 1; i <= health; i++)
            {
                var obj = Instantiate(m_healthImage, m_healthGroup.transform);
                m_heartList.Add(obj);
            }
        }

        /// <summary>
        /// HPが減った分ハートも減らす
        /// </summary>
        /// <param name="health"></param>
        public void DecreaseHeart(int health)
        {
            var heartCount = m_healthGroup.transform.childCount;
            if (heartCount == health) { return; }
            var decreaseAmount = heartCount - health;
            if (decreaseAmount < 0) { return; }
            for (int i = 0; i <= decreaseAmount - 1; i++)
            {
                Destroy(m_heartList[i]);
                m_heartList.RemoveAt(i);
            }
        }

        /// <summary>
        /// 登録したリストから情報を削除
        /// 全部敵がいなくなるとScene移動
        /// </summary>
        /// <param name="enemy"></param>
        public void DeleteEnemyForList(Transform enemy)
        {
            m_enemyFlagObject.Remove(enemy);
        }

        /// <summary>
        /// enemy or playerが倒れた際の処理
        /// </summary>
        /// <param name="action"></param>
        public void DieCharactor(GameObject obj, bool isPlayer, UnityAction action)
        {
            if (!isPlayer)
            {
                App.Instance.AddScore(ADDSCORE);
                DeleteEnemyForList(obj.transform.parent);
            }
            StartCoroutine(DieEffect(obj, isPlayer, action));
        }

        /// <summary>
        /// particle演出
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="isPlayer"></param>
        /// <returns></returns>
        public IEnumerator DieEffect(GameObject obj, bool isPlayer, UnityAction action)
        {
            var particle = isPlayer ? m_playerDeadParticle : m_enemyDeadParticle;
            var particleObj = Instantiate(particle, obj.transform);
            particleObj.transform.SetParent(m_instance.transform);
            particleObj.Play();
            yield return new WaitForSeconds(particle.main.duration);
            if (m_enemyFlagObject.Any() && !isPlayer)
            {
                yield break;
            }
            action.Invoke();
        }
    }
}

