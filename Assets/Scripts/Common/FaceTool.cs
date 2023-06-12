using UnityEngine;

public class FaceTool
{
    private readonly Transform body;
    private readonly Vector3 defaultBodyScale;

    private bool faceRight;
    public bool FaceRight
    {
        get => faceRight;
        set
        {
            if (faceRight != value)
            {
                faceRight = value;
                body.localScale = new Vector3(-body.localScale.x, body.localScale.y, body.localScale.z);
            }
        }
    }

    public FaceTool(Transform body)
    {
        this.body = body;
        defaultBodyScale = body.localScale;
        faceRight = defaultBodyScale.x > 0;
    }

    public void SetFace(float faceDir)
    {
        if (!Mathf.Approximately(faceDir, 0))
        {
            FaceRight = faceDir > 0;
        }
    }

    public void Reset()
    {
        body.localScale = defaultBodyScale;
        faceRight = defaultBodyScale.x > 0;
    }
}
