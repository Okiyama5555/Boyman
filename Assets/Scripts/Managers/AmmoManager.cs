using UnityEngine;
using Character;
using Manager;
using Application;
using UniRx.Triggers;
using UniRx;

namespace Ammo
{
    /// <summary>
    /// 弾薬管理クラス
    /// </summary>
    public class AmmoManager : BaseManager
    {
        private const string AMMOMANAGER = "AMMOMANAGER";

        [SerializeField] float m_shootInterval;
        [SerializeField] GameObject m_bullet;
        private float m_lastShootTime;

        private void Start()
        {
            this.UpdateAsObservable()
                .Where(_ => Input.GetKeyDown(KeyCode.Z))
                .Where(_ => CanShoot())
                .Subscribe(_ =>
                {
                    Shoot();
                });
        }

        private static AmmoManager m_instance = null;
        public static AmmoManager Instance
        {
            get
            {
                if (m_instance != null) { return m_instance; }
                m_instance = GameObject.Find(AMMOMANAGER).GetComponent<AmmoManager>();
                return m_instance;
            }
        }

        /// <summary>
        /// 弾射出
        /// </summary>
        private void Shoot()
        {
            SoundManager.SEInstance.FindClipAndPlay(SoundManager.SEName.SHOOT);
            var obj = Instantiate(m_bullet, Player.Instance.transform);
            obj.transform.SetParent(this.gameObject.transform);
            m_lastShootTime = Time.time;
        }

        /// <summary>
        /// インターバルの秒数まで射撃できないように
        /// </summary>
        /// <returns></returns>
        private bool CanShoot()
        {
            if (Player.Instance == null) { return false; }
            return Time.time - m_lastShootTime >= m_shootInterval;
        }
    }
}