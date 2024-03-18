using UnityEngine;

public class PuzzleBlock : MonoBehaviour
{
    [SerializeField]
    private bool _isBreakable = false;

    public void ChangeToRandomColor(Material[] colorToChange)
    {
        GetComponent<Renderer>().materials = colorToChange;
    }

    public void SetBreakable()
    {
        _isBreakable = true;
    }

    public bool IsBreakable()
    {
        return _isBreakable;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(GameManager.instance.IsGameOver())
        {
            return;
        }
        if (other.tag == "PuzzleBlock")
        {
            GameManager.instance.GameOfWallOver();
        }
    }
}
