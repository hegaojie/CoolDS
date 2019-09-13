using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace CoolDS
{
    public class DualObserableCollection<T> where T : IComparable, new ()
    {

        public DualObserableCollection() : this(null, 0)
        { }

        public DualObserableCollection(int visualSize) : this(null, visualSize)
        { }

        public DualObserableCollection(IEnumerable<T> source) : this(source, source?.Count() ?? 0)
        { }

        public DualObserableCollection(IEnumerable<T> source, int visualSize)
        {
            VisualSize = visualSize;
            VisualElements = new ObservableCollection<T>();
            _elements = new List<T>();
            if (source == null) return;
            _elements.AddRange(source);
            InitializeVisualElements(_elements.Take(visualSize));
        }

        private void InitializeVisualElements(IEnumerable<T> source)
        {
            VisualElements.Clear();
            foreach (var element in source)
                VisualElements.Add(element);
        }

        public ObservableCollection<T> VisualElements { get; }

        public int VisualSize { get; set; }

        public int Count => _elements.Count;

        public bool CanMove1StepToLeft => CalculateActualStepsToLeft(1) > 0;

        public bool CanMove1StepToRight => CalculateActualStepsToRight(1) > 0;

        /// <summary>
        /// Return the minimum value of expected steps and maximum of possible steps to left
        /// </summary>
        /// <param name="steps">Expected steps to be moved to left</param>
        /// <returns>Actual steps can be moved to left</returns>
        private int CalculateActualStepsToLeft(int steps)
        {
            var originalVisualEnd = _visualStart + VisualSize;
            return originalVisualEnd + steps > Count ? Count - originalVisualEnd : steps;
        }
        
        /// <summary>
        /// Return the minimum value of expected steps and maximum of possible steps to right
        /// </summary>
        /// <param name="steps">Expected steps to be moved to right</param>
        /// <returns>Actual steps can be moved to right</returns>
        private int CalculateActualStepsToRight(int steps)
        {
            var newVisualStart = _visualStart - steps;
            return newVisualStart < 0 ? steps + newVisualStart : steps;
        }

        public void MoveStepsToLeft(int steps)
        {
            MoveSteps(steps, 1);
        }

        public void MoveStepsToRight(int steps)
        {
            MoveSteps(steps, -1);
        }

        private void MoveSteps(int steps, int direction)
        {
            var actualSteps = direction < 0 ? CalculateActualStepsToRight(steps) : CalculateActualStepsToLeft(steps);
            if (actualSteps < 1) return;
            _visualStart += actualSteps * direction;
            InitializeVisualElements(TakeElements(_visualStart, _visualStart + actualSteps - 1));
        }

        private IEnumerable<T> TakeElements(int start, int end)
        {
            for (var i = start; i < end; i++)
                yield return _elements[i];
        }

        public void Clear()
        {
            _visualStart = 0;
            _elements.Clear();
            VisualElements.Clear();
        }

        private int _visualStart;
        private List<T> _elements;
    }
}