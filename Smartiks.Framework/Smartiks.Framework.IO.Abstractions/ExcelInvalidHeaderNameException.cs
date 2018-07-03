namespace Smartiks.Framework.IO.Abstractions
{
    public class ExcelInvalidHeaderNameException : ExcelException
    {
        public int RowNo { get; }

        public int ColumnNo { get; }

        public string Address { get; }

        public ExcelInvalidHeaderNameException(int rowNo, int columnNo, string address)
        {
            RowNo = rowNo;

            ColumnNo = columnNo;

            Address = address;
        }
    }
}
