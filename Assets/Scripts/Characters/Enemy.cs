using UnityEngine;
using Application;
using Manager;

namespace Character
{
    public class Enemy : BaseManager
    {
        [SerializeField] int m_health;
        [SerializeField] int m_DamageAmount;

        public int GetDamageAmount() { return m_DamageAmount; }

        /// <summary>
        /// 敵のHP管理用メソッド
        /// </summary>
        private void DecreaseHealth(int damage)
        {
            m_health -= damage;
            //TODO: 敵を赤く点灯させる
            if (m_health == 0)
            {
                var obj = this.gameObject;
                this.gameObject.DestoryObj(() =>
                {
                    SoundManager.SEInstance.FindClipAndPlay(SoundManager.SEName.BOMB);
                    var isPlayer = false; //enemy or playerのフラグ作成
                    CharacterManager.Instance.DieCharactor(this.gameObject, isPlayer, () =>
                    {
                        base.SceneChange(SceneName.Result);
                    });
                });
            }
        }

        /// <summary>
        /// 接触したコリジョンのタグから判別
        /// </summary>
        /// <param name="tagName"></param>
        private void SetCollisionAction(App.GameObjectTags tagName)
        {
            var damage = GetPlayerGunDamege();
            switch (tagName)
            {
                case App.GameObjectTags.Player:
                    break;
                case App.GameObjectTags.Bullet:
                    DecreaseHealth(damage);
                    break;
                case App.GameObjectTags.Object:
                    break;
                case App.GameObjectTags.Enemy:
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// :TODO playerのモードを切り替えるようにしてDamage量を変化させる
        /// まだ実装できていないので返り値1で固定
        /// </summary>
        /// <returns></returns>
        private int GetPlayerGunDamege()
        {
            var normalGunDamage = 1;
            return normalGunDamage;
        }

        /// <summary>
        /// 接触時にSetCollisionActionにタグの情報を渡す
        /// </summary>
        /// <param name="collision"></param>
        private void OnCollisionEnter2D(Collision2D collision)
        {
            var tagName = App.StringToObjectTag(collision.gameObject.tag);
            SetCollisionAction(tagName);
        }
    }
}
