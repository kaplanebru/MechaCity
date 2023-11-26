using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Chain
{
     struct ItemAngleModel<T>
        {
            public T Item;
            public float Angle;
            public Vector3 ItemPosition;

            public ItemAngleModel(T item, float angle, Vector3 itemPosition)
            {
                Item = item;
                Angle = angle;
                ItemPosition = itemPosition;
            }
        }
     
    public class ClockwiseSorter<T>
    {
        T[] _items;
        private Vector3[] _itemPositions;
        List<ItemAngleModel<T>> itemAngleModels = new ();

        private Vector3 center;

        public ClockwiseSorter(T[] items, Vector3[] itemPositions)
        {
            _items = items;
            _itemPositions = itemPositions;
        }

        public T[] SortItems()
        {
            GetCenter();
            CalculateAngles();
            SortPointsByAngles();
            return _items;
        }

        void GetCenter()
        {
            center = TrigonometryHelper.Center(_itemPositions);
        }

        void CalculateAngles()
        {
            for (var i = 0; i < _itemPositions.Length; i++)
            {
                var pos = _itemPositions[i];
                float angle = Mathf.Atan2(pos.z - center.z, pos.x - center.x) * Mathf.Rad2Deg; //TODO: if z
                angle = (angle + 360) % 360;
                itemAngleModels.Add(new ItemAngleModel<T>(_items[i], angle, pos));
            }
        }
        

        void SortPointsByAngles()
        {
            itemAngleModels = itemAngleModels.OrderByDescending(i => i.Angle).ToList();
            
            for (int i = 0; i < _items.Length; i++)
            {
                _items[i] = itemAngleModels[i].Item;
                //Debug.Log(itemAngleModels[i].ItemPosition + " " + itemAngleModels[i].Angle);
            }
        }
    }
}