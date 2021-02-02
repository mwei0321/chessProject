using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 存贮游戏数据，游戏引用，游戏资源，模式切换与控制
/// 棋子ID编号
/// 1.黑将  2.黑车  3.黑马   4.黑炮  5.黑士  6.黑象  7.黑卒
/// 8.红帅  9.红车  10.红马  11.红炮  12.红仕 13.红相 14.红兵
/// </summary>
public class GameManager : MonoBehaviour {
    public static GameManager Instance { get; private set; }
    public int chessPeople; // 当前对战人数 1：PVE 2：PVP 3：联网
    public int currentLevel; // 当前难度 1：简单 2：一般 3：困难

    /// <summary>
    /// 数据
    /// </summary>
    public int[,] chessBoard; // 当前棋盘状态
    public GameObject[,] boardGrid; // 棋盘上的所有格子
    // 棋子尺寸
    private const float gridWidth = 75.15f;
    private const float gridHight = 76.9f;
    private const int gridTotalNum = 90;
    public bool playerParty; // 执棋子方 红：true 黑：false
    // 游戏结束状态
    public bool gameOver;

    /// <summary>
    /// 资源定义
    /// </summary>

    public GameObject gridGo; // 宫格对象
    public Sprite[] sprites; // 所有棋子的标识
    public GameObject chessGeObj; // 棋子
    public GameObject canMovePosUIGeObj; // 棋子可以移动到的位置UI对象


    /// <summary>
    /// 引用定义
    /// </summary>
    // 棋盘
    [HideInInspector]
    public GameObject boardGo;
    public GameObject[] boardGos; // 棋盘对象，单机，联网的棋盘 0.单机 1.联机
    [HideInInspector]
    public ChessOrGrid lastChessOrGrid; // 上次操作的对象
    public Rules rulesCls; // 规则处理
    public ChessMove chessMoveCls; // 棋子移动对象
    public GameObject eatChessPool; // 存在被吃掉棋子
    public CheckMate checkMateCls; // 是否将军
    public GameObject clickChessUIGeObj; // 点击棋子UI对象
    public GameObject lastPosUIGeObj; // 棋子移动前的位置UI显示
    public GameObject canEatPosUIGeObj; // 可以吃掉该棋子的UI显示
    public Stack<GameObject> canMoveUIStack; // 移动位置UI显示对象池
    public Stack<GameObject> currentCanMoveUIStack;// 移动位置UI显示地址对象引用池


    /// <summary>
    /// 初始化棋盘宫格
    /// </summary>
    /// <returns></retruns>
    public void InitGrid() {
        float posX = 0, posY = 0;
        for (int i = 0; i < 10; i++) {
            for (int j = 0; j < 9; j++) {
                // 实例一个宫格
                GameObject itemGo = Instantiate(gridGo);
                // 把它设置在棋盘的子级下
                itemGo.transform.SetParent(boardGo.transform);
                // 重置对象名字为当前宫格坐标
                itemGo.name = "Item[" + i.ToString() + "," + j.ToString() + "]";
                // 配置棋盘位置，基于本地坐标
                itemGo.transform.localPosition = new Vector3(posX, posY, 0);
                // 循环加宽度，定位坐标
                posX += gridWidth;
                // 棋盘横向宫格为9，所以到9后要换行
                if (posX >= gridWidth * 9) {
                    posY -= gridHight;
                    posX = 0;
                }
                // 记录在棋盘上的坐标位置
                itemGo.GetComponent<ChessOrGrid>().xIndex = i;
                itemGo.GetComponent<ChessOrGrid>().yIndex = j;
                // 存入棋盘
                boardGrid[i, j] = itemGo;
            }
        }
    }


    /// <summary>
    /// 生成棋子
    /// </summary>
    /// <returns></retruns>
    void InitChess() {
        for (int i = 0; i < 10; i++) {
            for (int j = 0; j < 9; j++) {
                GameObject item = boardGrid[i, j];
                switch (chessBoard[i, j]) {
                    case 1:
                        CreateChess(item, "b_jiang", sprites[0], false);
                        break;
                    case 2:
                        CreateChess(item, "b_ju", sprites[1], false);
                        break;
                    case 3:
                        CreateChess(item, "b_ma", sprites[2], false);
                        break;
                    case 4:
                        CreateChess(item, "b_pao", sprites[3], false);
                        break;
                    case 5:
                        CreateChess(item, "b_shi", sprites[4], false);
                        break;
                    case 6:
                        CreateChess(item, "b_zhu", sprites[5], false);
                        break;
                    case 7:
                        CreateChess(item, "b_bing", sprites[6], false);
                        break;
                    case 8:
                        CreateChess(item, "r_shuai", sprites[7], true);
                        break;
                    case 9:
                        CreateChess(item, "r_ju", sprites[8], true);
                        break;
                    case 10:
                        CreateChess(item, "r_ma", sprites[9], true);
                        break;
                    case 11:
                        CreateChess(item, "r_pao", sprites[10], true);
                        break;
                    case 12:
                        CreateChess(item, "r_shi", sprites[11], true);
                        break;
                    case 13:
                        CreateChess(item, "r_xiang", sprites[12], true);
                        break;
                    case 14:
                        CreateChess(item, "r_bing", sprites[13], true);
                        break;

                    default:
                        break;
                }
            }
        }
    }

    /// <summary>
    /// 生成棋子对象
    /// </summary>
    /// <param name="itemGeObj">格子对象</param>
    /// <param name="name">棋子名称</param>
    /// <param name="chessIcon">棋子标识</param>
    /// <param name="isRed">是否是红方</param>
    /// <returns></retruns>
    void CreateChess(GameObject gridItem, string name, Sprite chessIcon, bool isRed) {
        // 实例棋子
        GameObject item = Instantiate(chessGeObj);
        // 设置父级
        item.transform.SetParent(gridItem.transform);
        // 设置组件名
        item.name = name;
        item.GetComponent<Image>().sprite = chessIcon;
        item.transform.localPosition = Vector3.zero;
        item.transform.localScale = Vector3.one;
        // 棋手,颜色区分
        item.GetComponent<ChessOrGrid>().isRed = isRed;
    }

    /// <summary>
    /// 开局初始化棋子
    /// </summary> 
    /// <returns></retruns>
    void InitChessPostion() {
        chessBoard = new int[10, 9]{
            {2,3,6,5,1,5,6,3,2},
            {0,0,0,0,0,0,0,0,0},
            {0,4,0,0,0,0,0,4,0},
            {7,0,7,0,7,0,7,0,7},
            {0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0},
            {14,0,14,0,14,0,14,0,14},
            {0,11,0,0,0,0,0,11,0},
            {0,0,0,0,0,0,0,0,0},
            {9,10,13,12,8,12,13,10,9},
        };
    }

    /// <summary>
    /// 被吃掉的棋子处理
    /// </summary>
    /// <param name="itemGeObj">被吃的棋子对象</param>
    /// <returns></retruns>
    public void BeEatChess(GameObject itemGeObj) {
        itemGeObj.transform.SetParent(eatChessPool.transform);
        itemGeObj.transform.localPosition = Vector3.zero;
    }

    /// <summary>
    /// 重置游戏
    /// </summary>
    /// <returns></retruns>
    public void ResetGame() {
        // 初始化棋盘
        InitChessPostion();
        boardGrid = new GameObject[10, 9];
        // 判断用那个棋盘
        if (chessPeople == 1) {
            boardGo = boardGos[0];
        } else {
            boardGo = boardGos[1];
        }
        // 红方先走
        playerParty = true;
        // 生成棋盘
        InitGrid();
        // 生成棋子
        InitChess();
        // 初始化规则对象
        rulesCls = new Rules();
        // 初始化棋子移动类
        chessMoveCls = new ChessMove(this);
        // 初始化将军类
        checkMateCls = new CheckMate();
        // 移动UI显示栈
        canMoveUIStack = new Stack<GameObject>();
        for (int i = 0; i < 40; i++) {
            GameObject itemUIObj = Instantiate(canMovePosUIGeObj);
            // itemUIObj.transform.SetParent(transform);
            canMoveUIStack.Push(itemUIObj);
        }
    }

    #region 游戏中UI显示隐藏

    /// <summary>
    /// 点击显示UI
    /// </summary>
    /// <param name="targetTransfrom">目标的Transfrom对象</param>
    /// <returns></retruns>
    public void ShowClickUI(Transform targetTransform) {
        clickChessUIGeObj.transform.SetParent(targetTransform);
        clickChessUIGeObj.transform.localPosition = Vector3.zero;
    }

    /// <summary>
    /// 隐藏点击的棋子UI
    /// </summary>
    /// <returns></retruns>
    public void HideClickUI() {
        clickChessUIGeObj.transform.SetParent(eatChessPool.transform);
        clickChessUIGeObj.transform.localPosition = Vector3.zero;
    }

    /// <summary>
    /// 上次走棋状态显示
    /// </summary>
    /// <param name="showPosition">上次走棋位置</param>
    /// <returns></retruns>
    public void ShowLastPositionUI(Vector3 showPosition) {
        lastPosUIGeObj.transform.position = showPosition;
    }

    /// <summary>
    /// 隐藏走棋状态显示
    /// </summary>
    /// <param name="paramName">param description</param>
    /// <returns></retruns>
    public void HideLastPositionUI() {
        lastPosUIGeObj.transform.position = new Vector3(100, 100, 100);
    }

    /// <summary>
    /// 隐藏可以吃掉棋子的UI
    /// </summary>
    /// <param name="paramName">param description</param>
    /// <returns></retruns>
    public void HideCanEatUI() {
        canEatPosUIGeObj.transform.SetParent(eatChessPool.transform);
        canEatPosUIGeObj.transform.localPosition = Vector3.one;
    }

    /// <summary>
    /// 当前选中棋子可以移动到的位置UI显示
    /// </summary>
    /// <param name="paramName">param description</param>
    /// <returns></retruns>
    public GameObject PopCanMoveUI() {
        GameObject itemGeObj = canMoveUIStack.Pop();
        itemGeObj.SetActive(true);
        return itemGeObj;
    }

    /// <summary>
    /// 回收可以移动的UI对象
    /// </summary>
    /// <param name="itemGeObj">回收可以移动的UI对象</param>
    /// <returns></retruns>
    public void PushCanMoveUI(GameObject itemGeObj) {
        canMoveUIStack.Push(itemGeObj);
        itemGeObj.transform.SetParent(eatChessPool.transform);
        itemGeObj.SetActive(false);
    }

    public void ClearCurrentCanMoveUIStack() {
        while (currentCanMoveUIStack.Count > 0) {
            PushCanMoveUI(currentCanMoveUIStack.Pop());
        }
    }

    #endregion

    private void Start() {
        chessPeople = 1;
        ResetGame();
    }

    private void Awake() {
        Instance = this;
    }

}
