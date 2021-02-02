using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// 控制页面之间的显示与跳转，按钮的触发方法，在GameManager之后实例化
/// </summary>
public class UIManager : MonoBehaviour {
    public static UIManager Instance { get; private set; }

    // 0.主菜单 1.单机 2.模式选择 3.难度选择 4.单机游戏 5.联网游戏
    public GameObject[] panels;

    // 当前需要改变那个文本UI
    public Text tipUIText;
    // 具体显示的文本
    public Text[] tipUITexts;
    GameManager gameManager;

    private void Start() {
        Instance = this;
        gameManager = GameManager.Instance;
    }

    #region 页面跳转
    /// <summary>
    /// 单机模式
    /// </summary>
    public void StandAloneMode() {
        foreach (var item in panels) {
            item.SetActive(false);
        }
        panels[1].SetActive(true);
        panels[2].SetActive(true);
    }

    /// <summary>
    /// 人机模式
    /// </summary>
    /// <returns></retruns>
    public void PvEMode() {
        gameManager.chessPeople = 1;
        panels[2].SetActive(false);
        panels[3].SetActive(true);
    }

    /// <summary>
    /// 双人模式
    /// </summary>
    /// <returns></retruns>
    public void PvPMode() {
        gameManager.chessPeople = 2;
        tipUIText = tipUITexts[0];

        // 游戏加载
        ResetUI();
    }

    /// <summary>
    /// 联网模式
    /// </summary>
    public void NetWorkMode() {
        gameManager.chessPeople = 3;
        foreach (var item in panels) {
            item.SetActive(false);
        }
        panels[5].SetActive(true);

    }

    /// <summary>
    /// 难度选择
    /// </summary>
    /// <param name="level">难度选择</param>
    /// <returns></retruns>
    public void LevelOption(int level) {
        gameManager.currentLevel = level;
        tipUIText = tipUITexts[0];

        // 游戏加载
        ResetUI();
    }

    /// <summary>
    /// 退出游戏
    /// </summary>
    public void ExitGame() {
        Application.Quit();
    }
    #endregion

    #region 加载游戏
    /// <summary>
    /// 重载游戏
    /// </summary>
    /// <returns></retruns>
    void LoadGame() {
        gameManager.ResetGame();
        ResetUI();
    }

    /// <summary>
    /// 重置界面UI
    /// </summary>
    /// <returns></retruns>
    void ResetUI() {
        foreach (var item in panels) {
            item.SetActive(false);
        }
        panels[0].SetActive(true);
        panels[2].SetActive(true);
        panels[4].SetActive(true);
    }
    #endregion

    #region 游戏中UI操作方法
    /// <summary>
    /// 悔棋
    /// </summary>
    /// <returns></retruns>
    public void UnDo() {

    }

    /// <summary>
    /// 重玩
    /// </summary>
    /// <returns></retruns>
    public void Replay() {

    }

    /// <summary>
    /// 返回
    /// </summary>
    /// <param name="paramName">param description</param>
    /// <returns></retruns>
    public void ReturnToMain() {

    }

    /// <summary>
    /// 下棋信息提示
    /// </summary>
    /// <param name="string">提示信息</param>
    /// <returns></retruns>
    public void ShowTip(string str) {
        // 测试用
        tipUIText = tipUITexts[0];

        tipUIText.text = str;
    }

    /// <summary>
    /// 开始联网
    /// </summary>
    /// <returns></retruns>
    public void StartNetWorkingMode() {

    }

    /// <summary>
    /// 认输
    /// </summary>
    /// <returns></retruns>
    public void GiveUp() {

    }
    #endregion
}
