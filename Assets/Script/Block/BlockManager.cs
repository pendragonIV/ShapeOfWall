using UnityEngine;

public class BlockManager : MonoBehaviour
{
    public static BlockManager instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    [SerializeField]
    private Material[] _blockColor;
    [SerializeField]
    private Material _blockColorDefault;

    public Material GetRandomBlockMaterialColor()
    {
        return _blockColor[Random.Range(0, _blockColor.Length)];
    }

    public Material GetBlockMaterialDefault()
    {
        return _blockColorDefault;
    }
}
