using Application;
using UnityEngine;
using UnityEngine.UI;
using Manager;

namespace Scene
{
    /// <summary>
    /// ResultScene制御クラス
    /// </summary>
    public class ResultScene : BaseManager
    {
        [SerializeField] Button m_toTitleSceneButton;
        [SerializeField] Text m_scoreText;
        [SerializeField] Text m_maxScoreText;

        private void Awake()
        {
            base.AudioReset();
            m_scoreText.text = App.Instance.Score.ToString();
            m_maxScoreText.text = App.Instance.MaxScore.ToString();
            m_toTitleSceneButton.SetListener(() =>
            {
                base.SceneChange(SceneName.Title);
                SoundManager.SEInstance.FindClipAndPlay(SoundManager.SEName.PUSH);
            });
            base.SceneIn();
        }
    }
}