using DG.Tweening;
using System.Collections;
using UnityEngine;

public class PuzzleSpawner : MonoBehaviour
{
    public static PuzzleSpawner instance;
    private const int PUZZLE_Z = 0;
    private const int RESULT_Z = 15;
    private const float MOVE_SPEED = 5f;
    [SerializeField]
    private GameObject[] _puzzlePieces;
    [SerializeField]
    private Transform _container;

    private Coroutine _moveResultCoroutine;

    private Material[] _currentRandomMaterial;

    private int _breakAbleBlockCount = 0;

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

    private void Start()
    {
        SpawnNew();
    }

    public int GetBreakableBlockCount()
    {
        return _breakAbleBlockCount;
    }

    public void DecreaseBreakableBlockCount()
    {
        _breakAbleBlockCount--;
    }

    private void SpawnNew()
    {
        if(GameManager.instance.IsGameOver())
        {
            return;
        }

        if (_moveResultCoroutine != null) StopCoroutine(_moveResultCoroutine);

        SpawnPuzzle();
    }

    private void SpawnPuzzle()
    {
        int randomPuzzleIndex = Random.Range(0, _puzzlePieces.Length);
        GameObject puzzleToSpawn = _puzzlePieces[randomPuzzleIndex];
        Vector3 puzzleSpawnPos = new Vector3(0, 0, PUZZLE_Z);
        GameObject puzzle = Instantiate(puzzleToSpawn, puzzleSpawnPos, Quaternion.identity);
        puzzle.transform.SetParent(_container);
        foreach (Transform child in puzzle.transform.GetChild(0))
        {
            child.GetComponent<Collider>().enabled = false;
            child.localScale = Vector3.zero;
            child.DOScale(Vector3.one * .5f, 0.3f).SetEase(Ease.InOutBack).OnComplete(()=>
            {
                child.GetComponent<Collider>().enabled = true;
            });
        }

        _currentRandomMaterial = GetRandomMatrialColorForBlock();
        ChangeColorOfAllBlocks(puzzle.transform.GetChild(0));
        ChangeBlocksToBreakable(puzzle.transform.GetChild(0));

        GameObject border = puzzle.transform.GetChild(1).gameObject;
        border.SetActive(false);

        SpawnResult(puzzleToSpawn);
    }

    private void SpawnResult(GameObject puzzleToSpawn)
    {
        Vector3 resultSpawnPos = new Vector3(0, 0, RESULT_Z);
        GameObject result = Instantiate(puzzleToSpawn, resultSpawnPos, Quaternion.identity);
        result.transform.SetParent(_container);

        foreach (Transform child in result.transform.GetChild(0))
        {
            child.GetComponent<Collider>().enabled = false;
            child.localScale = Vector3.zero;
            child.DOScale(Vector3.one * .5f, 0.3f).SetEase(Ease.InOutBack).OnComplete(() =>
            {
                child.GetComponent<Collider>().enabled = true;
            });
        }

        ChangeColorOfAllBlocks(result.transform.GetChild(0));

        DisableRandomBlock(result);

        _moveResultCoroutine = StartCoroutine(MoveResultToward(result));
    }

    private void ChangeColorOfAllBlocks(Transform parent)
    {
        foreach (Transform child in parent)
        {
            PuzzleBlock puzzleBlock = child.GetComponent<PuzzleBlock>();
            if (puzzleBlock != null)
            {
                puzzleBlock.ChangeToRandomColor(_currentRandomMaterial);
            }
        }
    }

    private void ChangeBlocksToBreakable(Transform parent)
    {
        foreach (Transform child in parent)
        {
            PuzzleBlock puzzleBlock = child.GetComponent<PuzzleBlock>();
            if (puzzleBlock != null)
            {
                puzzleBlock.SetBreakable();
            }
        }
    }

    private void DisableRandomBlock(GameObject result)
    {
        _breakAbleBlockCount = 0;
        int count = 0;
        Transform puzzleContainer = result.transform.GetChild(0);

        int totalPuzzlePieces = puzzleContainer.childCount;
        int min = 2;
        int max = totalPuzzlePieces - 2;

        int randomNumberOfPieces = Random.Range(min, max);

        for (int i = randomNumberOfPieces; i < totalPuzzlePieces; i++)
        {
            int randomIndex = Random.Range(0, puzzleContainer.childCount);
            GameObject randomPiece = puzzleContainer.GetChild(randomIndex).gameObject;
            while (randomPiece.activeSelf == false)
            {
                randomIndex = Random.Range(0, puzzleContainer.childCount);
                randomPiece = puzzleContainer.GetChild(randomIndex).gameObject;
            }

            randomPiece.SetActive(false);
            count++;
        }
        _breakAbleBlockCount = totalPuzzlePieces - count;
    }

    private Material[] GetRandomMatrialColorForBlock()
    {
        Material defaultMaterial = BlockManager.instance.GetBlockMaterialDefault();
        Material blockMaterial = BlockManager.instance.GetRandomBlockMaterialColor();
        return new Material[] { defaultMaterial, blockMaterial };
    }

    private IEnumerator MoveResultToward(GameObject resultToMove)
    {
        while (resultToMove.transform.position.z > PUZZLE_Z)
        {
            resultToMove.transform.position = Vector3.MoveTowards(resultToMove.transform.position,
                new Vector3(0, 0, PUZZLE_Z),
                MOVE_SPEED * Time.deltaTime);

            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(.2f);
        GameManager.instance.UpdateScore(10);
        foreach (Transform child in _container)
        { 
            child.DOScale(Vector3.zero, 0.3f).SetEase(Ease.InBack).OnComplete(() => Destroy(child.gameObject));
        }
        yield return new WaitForSeconds(.2f);
        SpawnNew();
    }
}
