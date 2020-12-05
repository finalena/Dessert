using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using System.Text;

namespace Dessert
{
    public class Pagination
    {
        int iPageSize = 10;
        int iNowPage = 0;
        int iTotalPage = 0;
        int iPageLen = 5;    // 最多顯示幾個頁碼
        int init = 0;       // 起始頁碼數
        int imax = 0;  // 結束頁碼數
        int iPageOffset = 0;  // 頁碼個數左右偏移量
        
        /// <summary>
        /// 資料的總頁數
        /// </summary>
        public int TotalPage
        {
            get { return iTotalPage; }
        }

        /// <summary>
        /// 目前在第幾頁
        /// </summary>
        public int NowPage
        {
            get { return iNowPage; }
        }

        /// <summary>
        ///  每頁顯示的筆數
        /// </summary>
        public int PageSize{ get; set; }

        /// <summary>
        /// 防呆: 當前頁數需為正整數、比「資料的總頁數」還少
        /// </summary>
        /// <param name="sQueryPage"></param>
        public Pagination(int iRecordCount, string sQueryPage)
        {
            // 資料的總頁數
            iTotalPage = ((iRecordCount + iPageSize) - 1) / iPageSize;
            
            bool bIsNumber = int.TryParse(sQueryPage, out iNowPage);

            if (sQueryPage == null)
            {
                iNowPage = 1;
            }
            else
            {
                if (bIsNumber && IsNumeric(iNowPage.ToString()) && (iNowPage <= iTotalPage))
                {
                    iNowPage = Convert.ToInt32(sQueryPage);
                }
                else
                {
                    iNowPage = 1;
                }
            }
        }
        /// <summary>
        /// 第1頁與其他頁的計算方式不同
        /// </summary>
        /// <param name="sSql"></param>
        /// <returns></returns>
        public string FormatSql(string sSql)
        {
            // 為第1頁時，輸出10-1筆資料
            // 為第2頁時，輸出20-11筆資料
            // 為第3頁時，輸出30-21筆資料
            if (iNowPage == 1)
            {
                sSql = string.Format(sSql, iPageSize, iNowPage);
            }
            else
            {
                sSql = string.Format(sSql, iNowPage * iPageSize, ((iNowPage - 1) * iPageSize) + 1);
            }
            return sSql;
        }
        
        private void PaginationSetting()
        {
            init = 1;    
            imax = iTotalPage;  
            iPageOffset = (iPageLen - 1) / 2; 

            // 總頁數大於頁碼個數時可以偏移
            if (iTotalPage > iPageLen)
            {
                // 如果當前頁較小 等於左偏移
                if (iNowPage <= iPageOffset)
                {
                    init = 1;
                    imax = iPageLen;
                } // 如果當前頁較大 等於右偏移
                else
                {   // 如果當前頁碼右偏移超出最大總頁數
                    if (iNowPage + iPageOffset >= iTotalPage + 1)
                    {
                        init = iTotalPage - iPageLen + 1;
                    }
                    else
                    {   // 左右偏移都存在時的計算
                        init = iNowPage - iPageOffset;
                        imax = iNowPage + iPageOffset;
                    }
                }
            }
        }
        /// <summary>
        /// 輸出頁碼
        /// </summary>
        /// <param name="sHref"></param>
        /// <returns></returns>
        public StringBuilder PaginationOut(string sHref)
        {
            PaginationSetting();

            StringBuilder sb = new StringBuilder();
            sb.Append("<nav aria-label=\"...\">");
            sb.Append("<ul class=\"pagination\">");

            // 如果是第一頁，則不顯示第一頁和上一頁的連接
            if (iNowPage != 1)
            {
                sb.AppendFormat("<li class=\"page-item\"><a class=\"page-link\" href=\"{0}?page=1\">第一頁</a></li>", sHref);
                sb.AppendFormat("<li class=\"page-item\"><a class=\"page-link\" href=\"{0}?page={1}\"> < </a></li>", sHref, iNowPage - 1);
            }
            for (int intA = init; intA <= imax; intA++)
            {
                if (intA == iNowPage)
                {
                    sb.AppendFormat("<li class=\"page-item active\" aria-current=\"page\"><span class=\"page-link\">{0}</span></li>",intA);
                }
                else
                {
                    sb.AppendFormat("<li class=\"page-item\"><a class=\"page-link\" href=\"{0}?page={1}\">{1}</a></li>", sHref, intA);
                }
            }
            // 如果是最後一頁，則不顯示尾頁和下一頁的連接
            if (iNowPage != iTotalPage)
            {
                sb.AppendFormat("<li class=\"page-item\"><a class=\"page-link\" href=\"{0}?page={1}\"> > </a></li>", sHref, iNowPage + 1);
                sb.AppendFormat("<li class=\"page-item\"><a class=\"page-link\" href=\"{0}?page={1}\">最後一頁</a></li>", sHref, iTotalPage);
            }
            
            sb.Append(" </ul>");
            sb.Append(" </nav>");

            return sb;
        }
        /// <summary>
        /// 輸出頁碼及查詢店家
        /// </summary>
        /// <param name="sHref"></param>
        /// <param name="sSearchStore"></param>
        /// <returns></returns>
        public StringBuilder PaginationOut(string sHref, string sSearchStore)
        {
            PaginationSetting();

            StringBuilder sb = new StringBuilder();
            sb.Append("<nav aria-label=\"...\">");
            sb.Append("<ul class=\"pagination\">");

            // 如果是第一頁，則不顯示第一頁和上一頁的連接
            if (iNowPage != 1)
            {
                sb.AppendFormat("<li class=\"page-item\"><a class=\"page-link\" href=\"{0}?page=1&q={1}\">第一頁</a></li>", sHref, sSearchStore);
                sb.AppendFormat("<li class=\"page-item\"><a class=\"page-link\" href=\"{0}?page={1}&q={2}\"> < </a></li>", sHref, iNowPage - 1, sSearchStore);
            }
            for (int intA = init; intA <= imax; intA++)
            {
                if (intA == iNowPage)
                {
                    sb.AppendFormat("<li class=\"page-item active\" aria-current=\"page\"><span class=\"page-link\">{0}</span></li>", intA);
                }
                else
                {
                    sb.AppendFormat("<li class=\"page-item\"><a class=\"page-link\" href=\"{0}?page={1}&q={2}\">{3}</a></li>", sHref, intA, sSearchStore, intA);
                }
            }
            // 如果是最後一頁，則不顯示尾頁和下一頁的連接
            if (iNowPage != iTotalPage)
            {
                sb.AppendFormat("<li class=\"page-item\"><a class=\"page-link\" href=\"{0}?page={1}&q={2}\"> > </a></li>", sHref, iNowPage + 1, sSearchStore);
                sb.AppendFormat("<li class=\"page-item\"><a class=\"page-link\" href=\"{0}?page={1}&q={2}\">最後一頁</a></li>", sHref, iTotalPage, sSearchStore);
            }
            sb.Append(" </ul>");
            sb.Append(" </nav>");

            return sb;
        }
        /// <summary>
        /// 判斷是否為正整數
        /// </summary>
        /// <param name="strNumber"></param>
        /// <returns></returns>
        private bool IsNumeric(String strNumber)
        {
            Regex NumberPattern = new Regex("^[0-9]*[1-9][0-9]*$");
            return NumberPattern.IsMatch(strNumber);
        }
    }

}