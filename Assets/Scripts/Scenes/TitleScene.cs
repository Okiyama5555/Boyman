using UnityEngine;
using UnityEngine.UI;
using Manager;
using Application;

namespace Scene
{
    /// <summary>
    /// TitleScene制御クラス
    /// </summary>
    public class TitleScene : BaseManager
    {
        [SerializeField] Button m_ToMainSceneButton;

        private void Awake()
        {
            App.Create();
            App.Instance.ScoreReset();
            SoundManager.Create();
            base.AudioReset();
            m_ToMainSceneButton.SetListener(() =>
            {
                base.SceneChange(SceneName.Main);
                SoundManager.SEInstance.FindClipAndPlay(SoundManager.SEName.PUSH);
            });
            base.SceneIn();
        }
    }
}