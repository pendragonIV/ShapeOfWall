using DG.Tweening;
using UnityEngine;

public class BlockCaster : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.tag == "PuzzleBlock" && PuzzleSpawner.instance.GetBreakableBlockCount() > 0)
                {
                    if(hit.transform.GetComponent<PuzzleBlock>().IsBreakable())
                    {
                        hit.transform.GetComponent<Collider>().enabled = false;
                        hit.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InOutBack).OnComplete(() =>
                        hit.transform.gameObject.SetActive(false));
                        PuzzleSpawner.instance.DecreaseBreakableBlockCount();
                    }
                }
            }
        }
    }
}
