using MyTimer;
using Services;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 用于选择生成
/// </summary>
public enum EShowTextType
{
    Stable, //仅生成不动的文字
    Up, //逐渐上升
    Down, //逐渐下降
    XGauss, //Y轴坐标不动,在X轴上符合正态分布
    YGauss, //X轴坐标不动,在Y轴上符合正态分布
    XYGauss, //在X、Y轴上符合正态分布
}

public class ShowText : MonoBehaviour
{
    private Text text;
    private MyObject myObject;
    private TimerOnly recycleTimer;

    private void Awake()
    {
        text = GetComponent<Text>();
        myObject = GetComponent<MyObject>();

        recycleTimer = new TimerOnly();
        recycleTimer.Initialize(0.5f, false);
        recycleTimer.OnComplete += myObject.Recycle;

        myObject.OnRecycle += OnRecycle;
    }

    private void OnRecycle()
    {
        StopAllCoroutines();
        text.text = string.Empty;
        recycleTimer.Paused = true;
    }

    private IEnumerator Move(float vy)
    {
        while (!recycleTimer.Paused)
        {
            transform.position = new Vector2(
                transform.position.x,
                transform.position.y + vy * Time.deltaTime
            );
            float r = text.color.r;
            float g = text.color.g;
            float b = text.color.b;
            float a = 1f - recycleTimer.Percent;
            text.color = new Color(r, g, b, a);
            yield return new WaitForFixedUpdate();
        }
    }

    /// <summary>
    /// 使用string的初始化方法,可改变颜色
    /// </summary>
    /// <param name="str">显示的文本</param>
    /// <param name="str">显示的文本</param>
    /// /// <param name="type">该浮动文字的类型</param>
    public void Init(string str, Color color, EShowTextType type)
    {
        recycleTimer.Restart();
        text.text = str;
        text.color = color;
        float gaussX = Gauss.Get(0) / 2f;
        float gaussY = Gauss.Get(0) / 2f;
        switch (type)
        {
            case EShowTextType.XGauss:
                transform.position += new Vector3(gaussX, 0);
                break;
            case EShowTextType.YGauss:
                transform.position += new Vector3(0, gaussY);
                break;
            case EShowTextType.XYGauss:
                transform.position += new Vector3(gaussX, gaussY);
                break;
            case EShowTextType.Up:
                StartCoroutine(Move(1.5f));
                break;
            case EShowTextType.Down:
                StartCoroutine(Move(-1.5f));
                break;
            case EShowTextType.Stable:
                break;
            default:
                break;
        }
    }

    public void Init(string str) => Init(str, Color.white, EShowTextType.Up);
}
