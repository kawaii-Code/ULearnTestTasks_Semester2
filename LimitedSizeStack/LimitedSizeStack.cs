using System;

namespace TodoApplication
{
    public class StackItem<T>
    {
        public T Value { get; set; }
        public StackItem<T> Next { get; set; }
        public StackItem<T> Previous { get; set; }
    }

    /*public class LimitedSizeStack<T>
    {
        private T[] _items;
        private int _count;

        private int Limit => _items.Length;
        private int HeadIndex 
        { 
            get => Limit % _count;
            set => _count = value; 
        }
        public int Count => _count > Limit ?
            Limit : _count;

        public LimitedSizeStack(int limit)
        {
            _count = 0;
            _items = new T[ limit ];
        }

        public void Push(T item)
        {
            _items[HeadIndex++] = item;
        }

        public T Pop()
        {
            return _items[ HeadIndex-- ];
        }
    }*/

    public class LimitedSizeStack<T>
    {
        private StackItem<T> head;
        private StackItem<T> tail;

        private int _count;
        private int _limit;

        public LimitedSizeStack(int limit)
        {
            if (_limit < 0)
                throw new ArgumentException("Limit can't be less than zero!");

            _count = 0;
            _limit = limit;
        }

        public int Count => _count;

        public void Push(T item)
        {
            if (_limit == 0)
                return;

            var stackItem = new StackItem<T>()
                { Value = item };

            if(head == null || tail == null)
            {
                head = tail = stackItem;
                _count = 1;
            }
            else
            {
                stackItem.Previous = head;
                head.Next = stackItem;
                head = stackItem;

                if (++_count > _limit)
                    PopTail();
            }
        }

        private void DecreaseCount()
        {
            if (tail == null)
                throw new InvalidOperationException("Stack is empty!");

            _count--;
            if (_count == 0)
                head = tail = null;
        }

        private void PopTail()
        {
            tail = tail.Next;
            DecreaseCount();
            tail.Previous = null;
        }

        public T Pop()
        {
            if (head == null)
                throw new InvalidOperationException("Stack is empty!");

            T result = head.Value;
            head = head.Previous;
            DecreaseCount();
            if (head != null)
                head.Next = null;
            return result;
        }
    }
}
