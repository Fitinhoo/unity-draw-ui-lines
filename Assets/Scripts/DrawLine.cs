using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;


public class DrawLine : MonoBehaviour
{
    #region <--- VARIABLES --->
    [Header( "References: " )]
    [SerializeField] private Canvas mainCanvas = default;                       /* Scene Canvas Game Object */
    [SerializeField] private List<UILineRenderer> lineRendererList = default;   /* Plugin UILineRenderer to draw a line on screen */
    [Header( "Settings: " )]
    [SerializeField] private float minDistanceBetweenPoints = .05f;             /* Minimum distance between two input points */
    [SerializeField] private int minPointsToDraw = 4;                           /* minimum amount of input points to draw a line */
    private int linesCount = default;

    #endregion
    #region <~~*~~*~~*~~*~~*~~* ENGINE METHODS   ~~*~~*~~*~~*~~*~~*~~*~~*~~*~~*~~*~~*~~*~~*~~*~~*~~*~~*~~*~~*~~*~~*~~*~~*>
    //<~~*~~*~~*~~*~~*~~*~~*~~*~~*~~*~~*~~*~~*~~*~~*~~*~~*~~*~~*~~*~~*~~*~~*~~*~~*~~*~~*~~*~~*~~*~~*~~*~~*~~*~~*~~*~~*~~*>
    private void Awake()
    {
        linesCount = 0;
    }

    private void Start()
    {
        if(!Validate()) SetEnableInput( false );
    }


    private void Update()
    {
        if (Input.GetMouseButtonDown( 0 ))
                StartNewLineDrawing();
        
        if (Input.GetMouseButton( 0 ))
                UpdateLineDrawing();

        if (Input.GetMouseButtonUp( 0 ))
            FinishLineDrawing();
    }


    #endregion
    #region <~~*~~*~~*~~*~~*~~* PUBLIC METHODS   ~~*~~*~~*~~*~~*~~*~~*~~*~~*~~*~~*~~*~~*~~*~~*~~*~~*~~*~~*~~*~~*~~*~~*~~*>
    //<~~*~~*~~*~~*~~*~~*~~*~~*~~*~~*~~*~~*~~*~~*~~*~~*~~*~~*~~*~~*~~*~~*~~*~~*~~*~~*~~*~~*~~*~~*~~*~~*~~*~~*~~*~~*~~*~~*>
    public bool Validate()
    {
        if (mainCanvas == null)
        {
            Debug.LogError( "Drawline Script: Canvas Rect Transform component not found. I will disable this script." );
            return false;
        }
        else if (lineRendererList.Count == 0)
        {
            Debug.LogError( "Drawline Script: Line Renderer component not found. I will disable this script." );
            return false;
        }
        return true;
    }


    public void SetEnableInput( bool value ) => enabled = value;


    public void RemoveAllDrawings()
    {
        linesCount = 0;
        foreach (UILineRenderer lineRenderer in lineRendererList)
        {
            lineRenderer.enabled = false;
            lineRenderer.ResetPoints();
        }
    }

    public void ResetCurrentLineRenderer()
    {
        lineRendererList[linesCount].enabled = false;
        lineRendererList[linesCount].ResetPoints();
    }


    #endregion
    #region <~~*~~*~~*~~*~~*~~* PRIVATE METHODS  ~~*~~*~~*~~*~~*~~*~~*~~*~~*~~*~~*~~*~~*~~*~~*~~*~~*~~*~~*~~*~~*~~*~~*~~*>
    //<~~*~~*~~*~~*~~*~~*~~*~~*~~*~~*~~*~~*~~*~~*~~*~~*~~*~~*~~*~~*~~*~~*~~*~~*~~*~~*~~*~~*~~*~~*~~*~~*~~*~~*~~*~~*~~*~~*>
    private void StartNewLineDrawing()
    {
        if ( linesCount == lineRendererList.Count )
            RemoveAllDrawings();

        lineRendererList[linesCount].enabled = true;
        lineRendererList[linesCount].ResetPoints();
        lineRendererList[linesCount].AddNewPoint( GetMousePosInCanvasScreen() );
    }


    private void UpdateLineDrawing()
    {
        Vector2 currentInputPos = GetMousePosInCanvasScreen();
        Vector2 lastInputPos = GetLastLinerendererPointPosition();
        if (Vector2.Distance( currentInputPos, lastInputPos ) > minDistanceBetweenPoints)
            lineRendererList[linesCount].AddNewPoint( GetMousePosInCanvasScreen() );
    }


    private void FinishLineDrawing()
    {
        if (GetCurrentLineRendererPointsAmount() > minPointsToDraw)
            linesCount++;
        else
            ResetCurrentLineRenderer();
    }


    private Vector2 GetMousePosInCanvasScreen()
    {
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle( mainCanvas.transform as RectTransform, Input.mousePosition, mainCanvas.worldCamera, out pos );
        return mainCanvas.transform.TransformPoint( pos );
    }


    private int GetCurrentLineRendererPointsAmount() => lineRendererList[linesCount].Points.Length;


    private Vector2 GetLastLinerendererPointPosition() => lineRendererList[linesCount].Points[GetCurrentLineRendererPointsAmount() - 1];

    #endregion
}