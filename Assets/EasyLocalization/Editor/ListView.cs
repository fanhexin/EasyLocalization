using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.IMGUI.Controls;

namespace EasyLocalization.Editor
{
    public class ListView : TreeView
    {
        public event Action<string> OnSelectionChanged;
        private readonly IEnumerable<string> _items;
        private int _id = -1;

        public ListView(TreeViewState state, IEnumerable<string> items) : base(state)
        {
            showAlternatingRowBackgrounds = true;
            _items = items;
            Reload();
        }

        protected override TreeViewItem BuildRoot()
        {
            var root = new TreeViewItem {id = _id++, depth = -1, displayName = ""};
            foreach (string item in _items)
            {
                root.AddChild(new TreeViewItem {id = _id++, depth = 0, displayName = item});    
            }
            return root;
        }

        protected override bool CanMultiSelect(TreeViewItem item)
        {
            return false;
        }

        protected override void SelectionChanged(IList<int> selectedIds)
        {
            var item = FindItem(selectedIds.First(), rootItem);
            OnSelectionChanged?.Invoke(item.displayName);
        }
    }
}