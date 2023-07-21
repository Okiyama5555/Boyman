using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public static class ComponentExtensions
{
    /// <summary>
    /// ボタン押下時のアクション設定
    /// </summary>
    /// <param name="button"></param>
    /// <param name="action"></param>
    public static void SetListener(this Button button, UnityAction action)
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(action);
    }

    /// <summary>
    /// GameObject削除後に実行
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="action"></param>
    public static void DestoryObj(this GameObject obj, UnityAction action)
    {
        GameObject.Destroy(obj);
        action.Invoke();
    }
}
