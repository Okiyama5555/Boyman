using UnityEngine;
using Application;
using Manager;
using Character;

namespace Ammo
{
    /// <summary>
    /// 弾薬本体のスクリプト
    /// </summary>
    public class Bullet : BaseManager
    {
        [SerializeField] float m_bulletForce;
        private float m_existTime = 1f;
        private float m_bulletAngleForce;

        private void Awake()
        {
            var obj = this.gameObject.GetComponent<Rigidbody2D>();
            m_bulletAngleForce = ShootingAngle();
            Destroy(this.gameObject, m_existTime);
        }

        private void Update()
        {
            var x = this.transform.position.x + m_bulletAngleForce;
            this.transform.position = new Vector2(x, this.transform.position.y);
        }

        /// <summary>
        /// Playerの向いているほうに弾薬を飛ばす
        /// </summary>
        /// <returns></returns>
        private float ShootingAngle()
        {
            if (Player.Instance.IsLookingLeftSide())
            {
                var leftForce = m_bulletForce * -1;
                return leftForce;
            }
            return m_bulletForce;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            var tag = App.StringToObjectTag(collision.gameObject.tag);
            switch (tag)
            {
                case App.GameObjectTags.Enemy:
                    Destroy(this.gameObject);
                    break;
                case App.GameObjectTags.Object:
                    Destroy(this.gameObject);
                    break;
                default:
                    break;
            }
        }
    }
}

