using Services;
using UnityEngine;

public class EnemySortingOrder : MonoBehaviour
{
    private SortingOrderTool sortingOrderTool;

    private void Awake()
    {
        sortingOrderTool = new SortingOrderTool(gameObject,
            ServiceLocator.Get<EnemyManager>().sortingOrderIncrease);
    }

    private void Update()
    {
        sortingOrderTool.SetSortingOrders(-(int)(transform.position.y * 100));
    }
}
