using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using Util;

namespace LabBox.Visualization.Input
{
    public enum SelectionType { Selected, Deselected }
    public class ItemSelectionEventArgs<T> : EventArgs
    {
        public T Item { get; }
        public SelectionType SelectType { get; }

        public ItemSelectionEventArgs(T item, SelectionType selectType)
        {
            Item = item;
            SelectType = selectType;
        }
    }

    /// <summary>
    /// Maintains a set of selected items by responding to the PrimarySelect and MultiSelect input events
    /// from an input observable. Individual selection results are handled by the SelectionFunction.
    /// </summary>
    public class ItemSelector<T> : IInputHandler
    {
        private bool multiModifierDown = false;
        private readonly List<IDisposable> subs = new List<IDisposable>();

        public event EventHandler<ItemSelectionEventArgs<T>> Selection;

        public IInputObservable Input { get; }

        public ISet<T> SelectedItems { get; } = new HashSet<T>();
        public Func<Optional<T>> SelectionFunction { get; }

        public bool Enabled { get; set; } = true;

        public ItemSelector(IInputObservable input, Func<Optional<T>> selectionFunction)
        {
            Input = input;
            SelectionFunction = selectionFunction;

            SubToInput(input);
        }

        private void SubToInput(IInputObservable input)
        {
            var enabledEvts = from evt in input.InputEvents where Enabled select evt;
            var primSelects = from evt in enabledEvts where evt.Input == InputType.PrimarySelect select evt;
            var multiSelects = from evt in enabledEvts where evt.Input == InputType.MultiSelect select evt;

            subs.Add(multiSelects.Subscribe(inpt =>
            {
                multiModifierDown = inpt.State != InputState.Finish;
            }));

            subs.Add(primSelects.Where(ps => ps.State == InputState.Finish).Subscribe(inpt =>
            {
                Optional<T> selectedItem = SelectionFunction();
                selectedItem.Do(item => NewSelection(item), () => DeselectAll());
            }));
        }

        private void DeselectAll()
        {
            if (SelectedItems.Count == 0) return;
            var toRemove = SelectedItems.ToArray();
            foreach (var b in toRemove) Deselect(b);
        }

        private void Deselect(T b)
        {
            SelectedItems.Remove(b);
            Selection?.Invoke(this, new ItemSelectionEventArgs<T>(b, SelectionType.Deselected));
        }

        private void NewSelection(T selectedItem)
        {
            if (multiModifierDown)
            {
                if (SelectedItems.Add(selectedItem)) Selection?.Invoke(this, new ItemSelectionEventArgs<T>(selectedItem, SelectionType.Selected));
            }
            else
            {
                bool alreadySelected = SelectedItems.Contains(selectedItem);
                var toRemove = SelectedItems.Except(selectedItem).ToArray();
                foreach (var b in toRemove) Deselect(b);
                if (!alreadySelected)
                {
                    SelectedItems.Add(selectedItem);
                    Selection?.Invoke(this, new ItemSelectionEventArgs<T>(selectedItem, SelectionType.Selected));
                }
            }
        }

        public void Dispose()
        {
            foreach (var sub in subs) sub.Dispose();
        }        
    }
}
