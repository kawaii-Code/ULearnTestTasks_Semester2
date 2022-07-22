using System;
using System.Collections.Generic;

namespace TodoApplication
{
    public enum ActionType
    {
        Add,
        Remove,
    }

    public class Action<T>
    {
        public T Item { get; set; }
        public ActionType Type { get; set; }
        public int AtIndex { get; set; }
    }

    public class ListModel<T>
    {
        private LimitedSizeStack<Action<T>> actions;
        public List<T> Items { get; }
        public int Limit;

        public ListModel(int limit)
        {
            actions = new LimitedSizeStack<Action<T>>(limit);
            Items = new List<T>();
            Limit = limit;
        }

        public void AddItem(T item)
        {
            actions.Push(new Action<T>()
            {
                Item = item,
                Type = ActionType.Add,
                AtIndex = Items.Count
            });

            Items.Add(item);
        }

        public void RemoveItem(int index)
        {
            actions.Push(new Action<T>()
            {
                Item = Items[ index ],
                Type = ActionType.Remove,
                AtIndex = index
            });

            Items.RemoveAt(index);
        }

        public bool CanUndo()
        {
            return actions.Count > 0;
        }

        public void Undo()
        {
            Action<T> action = actions.Pop();
            switch (action.Type)
            {
                case (ActionType.Add):
                    Items.RemoveAt(Items.Count - 1);
                    break;
                case (ActionType.Remove):
                    Items.Insert(action.AtIndex, action.Item);
                    break;
            }
        }
    }
}
