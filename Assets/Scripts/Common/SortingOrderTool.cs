using System.Collections.Generic;
using UnityEngine;

public class SortingOrderTool
{
    /// <summary>
    /// sortingOrderDict中Renderer的sortingOrder的上下限之差
    /// </summary>
    public readonly int orderIncrease;
    private readonly Dictionary<Renderer, int> sortingOrderDict;

    public SortingOrderTool(GameObject gameObject, int orderIncrease)
    {
        this.orderIncrease = orderIncrease;
        sortingOrderDict = new Dictionary<Renderer, int>();
        Renderer[] renderers = gameObject.GetComponentsInChildren<Renderer>();
        if (renderers.Length > 0)
        {
            foreach (var renderer in renderers)
            {
                sortingOrderDict.Add(renderer, renderer.sortingOrder);
            }
        }
    }

    public SortingOrderTool(params GameObject[] gameObjects)
    {
        orderIncrease = 0;
        sortingOrderDict = new Dictionary<Renderer, int>();
        foreach (var gameObject in gameObjects)
        {
            Renderer[] renderers = gameObject.GetComponentsInChildren<Renderer>();
            if (renderers.Length > 0)
            {
                int minSortingOrder, maxSortingOrder;
                minSortingOrder = maxSortingOrder = renderers[0].sortingOrder;
                foreach (var renderer in renderers)
                {
                    sortingOrderDict.Add(renderer, renderer.sortingOrder);
                    if (renderer.sortingOrder < minSortingOrder) minSortingOrder = renderer.sortingOrder;
                    else if (renderer.sortingOrder > maxSortingOrder) maxSortingOrder = renderer.sortingOrder;
                }
                orderIncrease = Mathf.Max(maxSortingOrder - minSortingOrder + 1, orderIncrease);
            }
        }
    }

    public int SetSortingOrders(int order)
    {
        int basicSortingOrder = order * orderIncrease;
        foreach (var renderer in sortingOrderDict.Keys)
        {
            renderer.sortingOrder = sortingOrderDict[renderer] + basicSortingOrder;
        }
        return basicSortingOrder;
    }
}
