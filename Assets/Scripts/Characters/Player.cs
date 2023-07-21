using UnityEngine;
using Application;
using Manager;
using UniRx;
using UniRx.Triggers;

namespace Character
{
    public class Player : BaseManager
    {
        private const string PLAYERPATH = "Player";
        private const string CAMERA = "Camera";

        [SerializeField] Transform m_playerRayCastStartPos;
        [SerializeField] int m_health;
        [SerializeField] float m_playerSpeed;
        [SerializeField] float m_playerJampForce;
        [SerializeField] float m_playerJumpTime;
        [SerializeField] float m_raycastDistance;

        private int m_maxHealth;
        private float m_maxJumpTime;
        private float m_moveInput;
        private GameObject m_playerCamera;
        private Rigidbody2D m_playerRB;
        private SpriteRenderer m_playerSprite;

        private static Player m_instance = null;
        public static Player Instance { get { return m_instance; } }

        public int ReturnHealth() { return m_health; }

        private void Awake()
        {
            m_instance = this;
            m_playerRB = GetComponent<Rigidbody2D>();
            m_playerSprite = GetComponent<SpriteRenderer>();
            m_maxHealth = m_health;
            m_maxJumpTime = m_playerJumpTime;
            m_playerCamera = GameObject.Find(CAMERA);
        }

        /// <summary>
        /// インスタンス作成
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public static Player Create(Transform pos)
        {
            if (m_instance != null) { return m_instance; }
            var playerGameObj = Resources.Load(PLAYERPATH) as GameObject;
            m_instance = Instantiate<GameObject>(playerGameObj, pos).GetComponent<Player>();
            return m_instance;
        }

        private void Start()
        {
            this.UpdateAsObservable()
                .Select(_ => Input.GetAxis("Horizontal"))
                .Subscribe(value =>
                {
                    FlipImage(value);
                    Move(value);
                });

            this.UpdateAsObservable()
                .Where(_ => Input.GetKey(KeyCode.Space))
                .Where(_ => !Jumping() && RemainJumpTime())
                .Subscribe(_ =>
                {
                    Jump();
                });
        }

        private void Update()
        {
            TrackingPlayer();
        }

        /// <summary>
        /// PlayerのHP変更、HPが0になったらResultにScemeChangeする
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="type"></param>
        private void ChangeHealth(int amount, AmountType type)
        {
            switch (type)
            {
                case AmountType.Damage:
                    m_health -= amount;
                    SoundManager.SEInstance.FindClipAndPlay(SoundManager.SEName.DAMAGE);
                    CharacterManager.Instance.DecreaseHeart(m_health);
                    break;
                //case AmountType.Heal:
                //    m_health += amount;
                //    break;
                default:
                    break;
            }

            if (m_health <= 0)
            {
                this.gameObject.DestoryObj(() =>
                {
                    var isPlayer = true; //enemy or playerのフラグ作成
                    CharacterManager.Instance.DieCharactor(this.gameObject, isPlayer, () =>
                    {
                        base.SceneChange(SceneName.Result);
                    });
                });
            }
        }

        /// <summary>
        /// Playerの移動管理
        /// </summary>
        private void Move(float value)
        {
            float moveAmount = value * m_playerSpeed * Time.deltaTime;
            transform.Translate(moveAmount, 0f, 0f);
        }

        /// <summary>
        /// Playerが左右どっちを見ているか返す
        /// </summary>
        /// <returns></returns>
        public bool IsLookingLeftSide()
        {
            return m_instance.m_playerSprite.flipX;
        }

        /// <summary>
        /// 進行している方向に画像の向きを合わせる
        /// </summary>
        /// <param name="Power"></param>
        private void FlipImage(float Power)
        {
            if (Power == 0) { return; }
            if (Power > 0)
            {
                m_instance.m_playerSprite.flipX = false;
            }
            else
            {
                m_instance.m_playerSprite.flipX = true;
            }
        }

        /// <summary>
        /// Space押下時にジャンプする
        /// </summary>
        private void Jump()
        {
            m_playerRB.velocity = new Vector2(m_playerRB.velocity.x, m_playerJampForce);
        }

        /// <summary>
        /// カメラ追従
        /// </summary>
        private void TrackingPlayer()
        {
            var pos = new Vector3(m_instance.transform.position.x, m_playerCamera.transform.position.y, m_playerCamera.transform.position.z);
            m_playerCamera.transform.position = pos;
        }

        /// <summary>
        /// コリジョンタグをenumに変換して体力を変化させる
        /// </summary>
        /// <param name="collision"></param>
        private void OnCollisionEnter2D(Collision2D collision)
        {
            var tag = App.StringToObjectTag(collision.gameObject.tag);
            switch (tag)
            {
                case App.GameObjectTags.Enemy:
                    var type = AmountType.Damage;
                    var damageAmount = collision.gameObject.GetComponent<Enemy>().GetDamageAmount();
                    KnockBack();
                    ChangeHealth(damageAmount, type);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 敵に当たった時にノックバックする
        /// </summary>
        private void KnockBack()
        {
            var knockBackAmount = 50f;
            if (m_moveInput >= 0)
            {
                var vec = new Vector2(knockBackAmount * -1, 0f); //マイナスx軸にノックバック
                m_playerRB.AddForce(vec, ForceMode2D.Impulse);
            }
            else
            {
                var vec = new Vector2(knockBackAmount, 0f); //x軸にノックバック
                m_playerRB.AddForce(vec, ForceMode2D.Impulse);
            }
        }

        /// <summary>
        /// ジャンプ入力可能時間
        /// </summary>
        /// <returns></returns>
        private bool RemainJumpTime()
        {
            if (m_playerJumpTime > 0)
            {
                m_playerJumpTime -= Time.deltaTime;
                return true;
            }
            return false;
        }

        /// <summary>
        /// 現在jump中か
        /// </summary>
        /// <returns></returns>
        private bool Jumping()
        {
            Vector2 player = m_playerRayCastStartPos.position;
            var hit = Physics2D.Raycast(player, Vector2.down, m_raycastDistance);
            if (!hit) { return true; }
            var hitObjectTag = App.StringToObjectTag(hit.collider.tag);
            if (hitObjectTag == App.GameObjectTags.Object)
            {
                m_playerJumpTime = m_maxJumpTime;
                return false;
            }
            return true;
        }

        /// <summary>
        /// 変化量に対してのタイプ分け
        /// まだHealは想定できていない
        /// </summary>
        public enum AmountType
        {
            Damage,
            Heal
        }
    }
}
