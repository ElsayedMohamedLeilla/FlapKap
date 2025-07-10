namespace FlapKap.Helpers
{
    public class PagingHelper
    {
        private static int _defaultPageSize = 15;
        public static int Skip(int? PageNumber, int? PageSize)
        {
            return (PageNumber == null ? 0 : (int)PageNumber) * (PageSize == null ? 0 : (int)PageSize);
        }
        public static int Take(int? PageSize)
        {

            return PageSize == null || PageSize == 0 ? _defaultPageSize : (int)PageSize;
        }
    }
}
