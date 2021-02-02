using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessOrGrid : MonoBehaviour {

    // 格子索引
    public int xIndex, yIndex;

    // 是否是格子
    public bool isGrid;

    // 对阵方
    public bool isRed;

    // 当前对象引用
    GameManager gameManager;

    // 格子对象,－－移动的时候需要设置父对象，如果是棋子，需要得到的是它的父对象
    GameObject gridGeObj;

    // Start is called before the first frame update
    void Start() {
        gameManager = GameManager.Instance;
        gridGeObj = this.gameObject;
    }

    /// <summary>
    /// 点击检测方法
    /// </summary>
    /// <param name="paramName">param description</param>
    /// <returns></retruns>
    public void ClickCheck() {
        if (gameManager.gameOver) {
            return;
        }
        int itemColorId; // 定义颜色ID， 0：为格子 1.黑色 2.红色
        if (isGrid) {
            itemColorId = 0;
        } else {
            // itemColorId
            // 得到父对象
            gridGeObj = transform.parent.gameObject;
            // 提取当前棋子所在格子索引
            ChessOrGrid chessOrGrid = gridGeObj.GetComponent<ChessOrGrid>();
            xIndex = chessOrGrid.xIndex;
            yIndex = chessOrGrid.yIndex;
            if (isRed) {
                itemColorId = 2;
            } else {
                itemColorId = 1;
            }
        }
        Debug.Log("itemColorId:" + itemColorId + "--xIndex:" + xIndex + "==yIndex:" + yIndex);
        // 行为处理
        GridOrChessBehavior(itemColorId, xIndex, yIndex);
    }

    /// <summary>
    /// 格子与棋子的行为逻辑处理
    /// </summary>
    /// <param name="itemColorId">对象颜色</param>
    /// <param name="x">当前格子或棋子的对象索引x值</param>
    /// <param name="y">当前格子或棋子的对象索引y值</param>
    /// <returns></retruns>
    void GridOrChessBehavior(int itemColorId, int x, int y) {
        // 对象索引
        int fromX, fromY, toX, toY;
        // 是否能移动棋子
        bool canMove = false;
        switch (itemColorId) {
            case 0: // 空格子
                toX = x;
                toY = y;
                // 第一次点到空格子
                if (gameManager.lastChessOrGrid == null) {
                    gameManager.lastChessOrGrid = this;
                    return;
                }
                // 上一次点击是否为格子
                if (gameManager.lastChessOrGrid.isGrid) return;
                // 提示文字
                string tipStr = "";
                bool playerParty = false;
                // 判断移动对象是那方
                if (gameManager.playerParty) { // 本次执棋方为红方
                    // 判断上次操作是否为黑方
                    if (!gameManager.lastChessOrGrid.isRed) {
                        gameManager.lastChessOrGrid = null;
                        return;
                    }
                    // 执棋成功后切换成黑方
                    playerParty = false;
                    tipStr = "黑方走";

                } else { // 本次执棋方为黑方
                    // 判断上次操作是否为红方
                    if (gameManager.lastChessOrGrid.isRed) {
                        gameManager.lastChessOrGrid = null;
                        return;
                    }
                    // 执棋成功后切换成红方
                    playerParty = true;
                    tipStr = "红方走";
                }
                // 上次操作索引
                fromX = gameManager.lastChessOrGrid.xIndex;
                fromY = gameManager.lastChessOrGrid.yIndex;
                // 判断移动是否合法
                canMove = gameManager.rulesCls.IsValidMove(gameManager.chessBoard, fromX, fromY, toX, toY);
                if (!canMove) return;
                // 棋子移动
                gameManager.chessMoveCls.IsMove(gameManager.lastChessOrGrid.gameObject, gridGeObj, fromX, fromY, toX, toY);
                // 提取对方走
                UIManager.Instance.ShowTip(tipStr);
                // 判断是否将军
                gameManager.checkMateCls.JudgeIsCheckMate();
                // 切换棋手
                gameManager.playerParty = playerParty;
                // 设置本次操作为上次操作
                gameManager.lastChessOrGrid = this;
                // 隐藏选择棋子UI特效
                gameManager.HideClickUI();
                break;
            case 1: // 黑色
                if (!gameManager.playerParty) { // 黑方执棋
                    fromX = x;
                    fromY = y;
                    // 显示所有可以移动的路径
                    gameManager.lastChessOrGrid = this;
                    // 显示选择棋子UI特效
                    gameManager.ShowClickUI(transform);
                } else { // 红方执棋
                    // 上次选择为空直接返回
                    if (gameManager.lastChessOrGrid == null) return;
                    // 两次选择都为黑子
                    if (!gameManager.lastChessOrGrid.isRed) {
                        gameManager.lastChessOrGrid = this;
                        return;
                    }
                    // 索引
                    fromX = gameManager.lastChessOrGrid.xIndex;
                    fromY = gameManager.lastChessOrGrid.yIndex;
                    toX = x;
                    toY = y;
                    // 判断移动是否合法
                    canMove = gameManager.rulesCls.IsValidMove(gameManager.chessBoard, fromX, fromY, toX, toY);
                    if (!canMove) return;
                    // 红方吃黑方棋子
                    gameManager.chessMoveCls.IsEat(gameManager.lastChessOrGrid.gameObject, gameObject, fromX, fromY, toX, toY);
                    // 切换棋手
                    gameManager.playerParty = false;
                    // 提取对方走
                    UIManager.Instance.ShowTip("黑方走");
                    // 设置上次操作为空
                    gameManager.lastChessOrGrid = null;
                    // 判断是否将军
                    gameManager.checkMateCls.JudgeIsCheckMate();
                    // 隐藏选择棋子UI特效
                    gameManager.HideClickUI();
                }
                break;
            case 2: // 红色
                // 两次选择都为红子
                if (gameManager.playerParty) { // 红方执棋
                    fromX = x;
                    fromY = y;
                    gameManager.lastChessOrGrid = this;
                    // 显示选择棋子UI特效
                    gameManager.ShowClickUI(transform);
                } else { // 黑方执棋
                    // 上次选择为空直接返回
                    if (gameManager.lastChessOrGrid == null) return;
                    // 黑方执棋，上次选择为红，直接返回
                    if (gameManager.lastChessOrGrid.isRed) {
                        gameManager.lastChessOrGrid = this;
                        return;
                    }
                    // 索引
                    fromX = gameManager.lastChessOrGrid.xIndex;
                    fromY = gameManager.lastChessOrGrid.yIndex;
                    toX = x;
                    toY = y;
                    // 判断移动是否合法
                    canMove = gameManager.rulesCls.IsValidMove(gameManager.chessBoard, fromX, fromY, toX, toY);
                    if (!canMove) return;
                    // 黑方吃红方棋子
                    gameManager.chessMoveCls.IsEat(gameManager.lastChessOrGrid.gameObject, gameObject, fromX, fromY, toX, toY);
                    // 切换棋手
                    gameManager.playerParty = false;
                    // 提取对方走
                    UIManager.Instance.ShowTip("红方走");
                    // 设置上次操作为空
                    gameManager.lastChessOrGrid = null;
                    // 判断是否将军
                    gameManager.checkMateCls.JudgeIsCheckMate();
                    // 隐藏选择棋子UI特效
                    gameManager.HideClickUI();
                }

                break;
            default:
                break;
        }
    }
}
