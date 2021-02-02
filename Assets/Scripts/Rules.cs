using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 棋子规则处理
/// </summary>
public class Rules {

    // 下一步棋盘棋子状态
    public int[,] nextChessBoard;

    /// <summary>
    /// 棋子移动是否合法
    /// </summary>
    /// <param name="position">当前棋盘状态</param>
    /// <param name="fromX">当前棋子来自x索引</param>
    /// <param name="fromY">当前棋子来自y索引</param>
    /// <param name="toX">当前棋子去x索引</param>
    /// <param name="toY">当前棋子去y索引</param>
    /// <returns></retruns>
    public bool IsValidMove(int[,] position, int fromX, int fromY, int toX, int toY) {
        int moveChessId, targetId;
        moveChessId = position[fromX, fromY];
        targetId = position[toX, toY];
        // 判断两次是否为同一方,两次同方不合法
        if (IsSameSide(moveChessId, targetId)) {
            return false;
        }

        // 判断走法是否合法并返回结果
        return IsVaild(moveChessId, position, fromX, fromY, toX, toY);
    }

    /// <summary>
    /// 判断棋子的走法是否合法
    /// </summary>
    /// <param name="moveChessId">当前棋子ID</param>
    /// <param name="position">当前棋盘状态</param>
    /// <param name="fromX">当前棋子来自x索引</param>
    /// <param name="fromY">当前棋子来自y索引</param>
    /// <param name="toX">当前棋子去x索引</param>
    /// <param name="toY">当前棋子去y索引</param>
    /// <returns></retruns>
    public bool IsVaild(int moveChessId, int[,] position, int fromX, int fromY, int toX, int toY) {
        // 判断原地移动
        if (fromX == toX && fromY == toX) {
            return false;
        }

        // 如果棋子后棋盘状态
        nextChessBoard = (int[,])position.Clone();
        nextChessBoard[toX, toY] = nextChessBoard[fromX, fromY];
        nextChessBoard[fromX, fromY] = 0;
        // 打印走后棋盘状态
        // for (int i = 0; i < 10; i++) {
        //     string tmp = "";
        //     for (int j = 0; j < 9; j++) {
        //         tmp += "," + nextChessBoard[i, j].ToString();
        //     }
        //     Debug.Log("x,y:" + tmp);
        // }
        // 判断将帅是否对面
        if (!KingKill(position, fromX, fromY, toX, toY)) {
            Debug.Log("将帅对面了");
            return false;
        }

        switch (moveChessId) {
            case 1: // 黑将
                if (toX > 2 || toY < 3 || toY > 5) {
                    Debug.Log("黑将出圈");
                    return false;
                }
                break;

            case 3:
                break;
            case 5: //　黑士
                //　黑士出圈判断
                if (toX > 2 || toY < 3 || toY > 5) {
                    Debug.Log("黑士出圈");
                    return false;
                }
                // 黑士的走法判断
                if ((Mathf.Abs(fromX - toX) != 1) || (Mathf.Abs(fromY - toY)) != 1) {
                    Debug.Log("黑士走法不合规");
                    return false;
                }
                break;
            case 6: // 黑象
                // 象不能过河
                if (toX > 4) {
                    Debug.Log("黑象出圈");
                    return false;
                }
                // 走法判断,走田
                if ((Mathf.Abs(fromX - toX)) != 2 || (Mathf.Abs(fromY - toY)) != 2) {
                    Debug.Log("黑象走法不合规");
                    return false;
                }
                // 象眼判断
                if (position[((fromX + toX) / 2), ((fromY + toY) / 2)] != 0) {
                    Debug.Log("象眼被塞,走法不合规");
                    return false;
                }
                break;
            case 7: // 黑卒
                if (toX < fromX) {
                    Debug.Log("黑卒不能回走");
                    return false;
                }
                if (toX < 5 && fromX == toX) {
                    Debug.Log("黑卒走法不合规");
                    return false;
                }
                if ((toX - fromX) + Mathf.Abs(toY - fromY) > 1) {
                    Debug.Log("黑卒走法不合规");
                    return false;
                }
                break;
            case 8: // 红帅
                if (toX < 7 || toY < 3 || toY > 5) {
                    Debug.Log("红帅出圈");
                    return false;
                }
                break;
            case 12: // 红士
                //　红士出圈判断
                if (toX < 7 || toY < 3 || toY > 5) {
                    Debug.Log("红士出圈");
                    return false;
                }
                // 红士的走法判断
                if ((Mathf.Abs(fromX - toX) != 1) || (Mathf.Abs(fromY - toY)) != 1) {
                    Debug.Log("红士走法不合规");
                    return false;
                }
                break;
            case 13: // 红象
                // 象不能过河
                if (toX < 5) {
                    Debug.Log("红象出圈");
                    return false;
                }
                // 走法判断,走田
                if ((Mathf.Abs(fromX - toX)) != 2 || (Mathf.Abs(fromY - toY)) != 2) {
                    Debug.Log("红象走法不合规");
                    return false;
                }
                // 象眼判断
                if (position[((fromX + toX) / 2), ((fromY + toY) / 2)] != 0) {
                    Debug.Log("象眼被塞,走法不合规");
                    return false;
                }
                break;
            case 14: // 红兵
                if (toX > fromX) {
                    Debug.Log("红兵不能回走");
                    return false;
                }
                if (toX > 4 && fromX == toX) {
                    Debug.Log("红兵走法不合规");
                    return false;
                }
                if ((fromX - toX) + Mathf.Abs(toY - fromY) > 1) {
                    Debug.Log("红兵走法不合规");
                    return false;
                }
                break;
            // 不区分执棋方
            case 2: // 黑车
            case 9: // 红车
                Debug.Log("黑车");
                // 先判断是否在一条直线上,二是两点之间不能有棋子
                int[] startPos = { fromX, fromY };
                int[] targetPos = { toX, toY };
                if (fromX == toX) {
                    int cnt = IsSameLineAndChessNum(startPos, targetPos, 0);
                    if (cnt != 0) {
                        return false;
                    }
                } else if (fromY == toY) {
                    int cnt = IsSameLineAndChessNum(startPos, targetPos, 1);
                    if (cnt != 0) {
                        return false;
                    }
                } else {
                    return false;
                }
                break;
            default:
                break;
        }
        return true;
    }

    /// <summary>
    /// 将帅不能对面判断
    /// </summary>
    /// <param name="position">当前棋盘状态</param>
    /// <param name="fromX">当前棋子来自x索引</param>
    /// <param name="fromY">当前棋子来自y索引</param>
    /// <param name="toX">当前棋子去x索引</param>
    /// <param name="toY">当前棋子去y索引</param>
    /// <returns></retruns>
    public bool KingKill(int[,] position, int fromX, int fromY, int toX, int toY) {
        int[] blackLeader = new int[2];
        int[] redLeader = new int[2];
        int count = 0;

        // 黑将坐标
        for (int i = 0; i < 3; i++) {
            for (int j = 3; j < 6; j++) {
                if (nextChessBoard[i, j] == 1) {
                    blackLeader[0] = i;
                    blackLeader[1] = j;
                }
            }
        }

        // 红帅坐标
        for (int i = 7; i < 10; i++) {
            for (int j = 3; j < 6; j++) {
                if (nextChessBoard[i, j] == 8) {
                    redLeader[0] = i;
                    redLeader[1] = j;
                }
            }
        }

        // Debug.Log("blackLeader:[" + blackLeader[0] + ',' + blackLeader[1] + ']');
        // Debug.Log("redLeader:[" + redLeader[0] + ',' + redLeader[1] + ']');
        // 判断是否在一条直线上,如果在,中间是否有棋子
        count = IsSameLineAndChessNum(blackLeader, redLeader, 1);
        // Debug.Log("count:" + count);
        if (count == 0) {
            return false;
        }

        return true;
    }

    /// <summary>
    /// 判断两个棋子是否在一条直线上,并返回中间是否有棋子
    /// </summary>
    /// <param name="startPos">起始点</param>
    /// <param name="targetPos">目标点</param>
    /// <param name="xOrY">是判断x轴,还是y轴, x=0,y=1</param>
    /// <returns>-1:不在一条直线上, 0:在一条直线上并且中间没有棋子 >0:有多颗棋子</retruns>
    public int IsSameLineAndChessNum(int[] startPos, int[] targetPos, int xOrY) {
        int chessNum = 0;
        // 判断是否在一条线上
        if (startPos[xOrY] == targetPos[xOrY]) {
            int compare = xOrY == 0 ? 1 : 0;
            // Debug.Log("same line:" + compare);
            bool direction = ((startPos[compare] - targetPos[compare]) > 0) ? false : true;
            int index = startPos[compare];
            for (int i = startPos[compare] + 1; i < targetPos[compare]; i++) {
                // 判断起始点的方向
                if (direction) {
                    index++;
                } else {
                    index--;
                }
                // Debug.Log("index:" + index);
                // 判断是否有棋子
                int isChess = (compare == 0) ? (nextChessBoard[index, startPos[1]]) : nextChessBoard[startPos[0], index];
                // Debug.Log("isChess:" + isChess);
                if (isChess != 0) {
                    chessNum++;
                }
            }
            return chessNum;
        }

        return -1;
    }

    /// <summary>
    /// 判断选中的两个游戏物体是否同为空格,或者红黑棋
    /// </summary>
    /// <param name="x">param description</param>
    /// <returns></retruns>
    public bool IsSameSide(int x, int y) {
        if ((IsBlack(x) && IsBlack(y)) || (IsRed(x) && IsRed(y))) {
            return true;
        }
        return false;
    }

    /// <summary>
    /// 判断是否为黑方
    /// </summary>
    /// <param name="x">游戏对象ID</param>
    /// <returns></retruns>
    public bool IsBlack(int x) {
        if (x > 0 && x < 8) {
            return true;
        }
        return false;
    }

    /// <summary>
    /// 判断是否为红方
    /// </summary>
    /// <param name="x">游戏对象ID</param>
    /// <returns></retruns>
    public bool IsRed(int x) {
        if (x > 7 && x < 15) {
            return true;
        }
        return false;
    }
}
