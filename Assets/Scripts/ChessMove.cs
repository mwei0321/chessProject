using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessMove {
    GameManager gameManagerObj;

    /// <summary>
    /// 棋子移动方法
    /// </summary>
    /// <param name="chessGeObj">移动的棋子对象</param>
    /// <param name="chessGeObj">移动到目标位置的格子对象</param>
    /// <param name="fromX">当前棋子来自x索引</param>
    /// <param name="fromY">当前棋子来自y索引</param>
    /// <param name="toX">当前棋子去x索引</param>
    /// <param name="toY">当前棋子去y索引</param>
    /// <returns></retruns>
    public void IsMove(GameObject chessGeObj, GameObject targetGrid, int fromX, int fromY, int toX, int toY) {
        // 显示移动前UI标签特效
        gameManagerObj.ShowLastPositionUI(chessGeObj.transform.position);
        chessGeObj.transform.SetParent(targetGrid.transform);
        chessGeObj.transform.localPosition = Vector3.zero;
        gameManagerObj.chessBoard[toX, toY] = gameManagerObj.chessBoard[fromX, fromY];
        gameManagerObj.chessBoard[fromX, fromY] = 0;
    }

    /// <summary>
    /// 棋子吃子处理
    /// </summary>
    /// <param name="toEatChess">吃棋子对象</param>
    /// <param name="byEatChess">被吃棋子对象</param>
    /// <param name="fromX">当前棋子来自x索引</param>
    /// <param name="fromY">当前棋子来自y索引</param>
    /// <param name="toX">当前棋子去x索引</param>
    /// <param name="toY">当前棋子去y索引</param>
    /// <returns></retruns>
    public void IsEat(GameObject toEatChess, GameObject byEatChess, int fromX, int fromY, int toX, int toY) {
        // 显示移动前UI标签特效
        gameManagerObj.ShowLastPositionUI(toEatChess.transform.position);
        // 被吃棋子对象的父对象
        GameObject byEatChessGrid = byEatChess.transform.parent.gameObject;
        toEatChess.transform.SetParent(byEatChessGrid.transform);
        toEatChess.transform.localPosition = Vector3.zero;
        // 更新棋盘状态
        gameManagerObj.chessBoard[toX, toY] = gameManagerObj.chessBoard[fromX, fromY];
        gameManagerObj.chessBoard[fromX, fromY] = 0;
        // 被吃棋子存放
        gameManagerObj.BeEatChess(byEatChess);
    }

    /// <summary>
    /// 构造方法
    /// </summary>
    /// <param name="mGameManager">自己对象</param>
    /// <returns></retruns>
    public ChessMove(GameManager mGameManager) {
        gameManagerObj = mGameManager;
    }
}
