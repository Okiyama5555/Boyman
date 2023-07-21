using Manager;
using Character;

namespace scene
{
    /// <summary>
    /// MainScene制御クラス
    /// </summary>
    public class MainScene : BaseManager
    {
        private void Awake()
        {
            base.AudioReset();
            CharacterManager.Instance.SpawnPlayer();
            CharacterManager.Instance.DisplayHeart(Player.Instance.ReturnHealth());
            CharacterManager.Instance.SpawnEnemy();
            base.SceneIn();
        }
    }
}
