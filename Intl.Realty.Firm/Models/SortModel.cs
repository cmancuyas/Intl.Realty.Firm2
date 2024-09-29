namespace Intl.Realty.Firm.Models
{
    public class SortModel
    {
        private string UpIcon = "fa fa-arrow-up";
        private string DownIcon = "fa fa-arrow-down";

        public string SortedProperty { get; set; } = string.Empty;
        public SortOrder SortOrder { get; set; }
        public string SortedExpression { get; set; } = string.Empty;
        private List<SortableColumn> _sortableColumns { get; set; } = new List<SortableColumn>();
        public void AddColumn(string columnName, bool isDefaultColumn = false)
        {
            SortableColumn column = _sortableColumns.Where(x => x.ColumnName.ToLower() == columnName.ToLower()).FirstOrDefault()!;
            if (column == null)
            {
                _sortableColumns.Add(new SortableColumn { ColumnName = columnName });
            }
        }

        public SortableColumn GetColumn(string columnName)
        {
            SortableColumn column = _sortableColumns.Where(x => x.ColumnName.ToLower() == columnName.ToLower()).FirstOrDefault()!;
            if (column == null)
            {
                _sortableColumns.Add(new SortableColumn { ColumnName = columnName });
            }
            return column!;
        }
        public void ApplySort(string sortExpression)
        {
            if (sortExpression == null)
            {
                sortExpression = "";
            }
            if (sortExpression == "")
            {
                sortExpression = this.SortedProperty;
            }

            sortExpression = sortExpression.ToLower();

            SortedExpression = sortExpression;

            foreach (SortableColumn sortableColumn in _sortableColumns)
            {
                sortableColumn.SortIcon = "";
                sortableColumn.SortExpression = sortableColumn.ColumnName;
                if (sortExpression == sortableColumn.ColumnName)
                {
                    this.SortOrder = SortOrder.Ascending;
                    this.SortedProperty = sortableColumn.ColumnName;

                    sortableColumn.SortIcon = DownIcon;
                    sortableColumn.SortExpression = sortableColumn.ColumnName + "_desc";
                }

                if (sortExpression == sortableColumn.ColumnName.ToLower() + "_desc")
                {
                    this.SortOrder = SortOrder.Descending;
                    this.SortedProperty = sortableColumn.ColumnName;

                    sortableColumn.SortIcon = UpIcon;
                    sortableColumn.SortExpression = sortableColumn.ColumnName;
                }
            }
        }
    }
}
