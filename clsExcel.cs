using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Collections;
using System.IO;
using System.Linq;
using System.Reflection;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using System.Web;

namespace FanFunction
{

    /// <summary>
    /// 导出Excel的类，使用NPOI组件
    /// </summary>
    public class clsExcel
    {
        /// <summary>
        /// 导出Excel,指定路径
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="filePath">生成文件的路径,包含文件名,例如:C:\\123.xls</param>
        /// <param name="title">转换标题</param>
        public static void Export(DataTable dt, string filePath, Dictionary<string, string> title = null)
        {
            using (MemoryStream ms = Export(dt, title))
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                {
                    byte[] data = ms.ToArray();
                    fs.Write(data, 0, data.Length);
                    fs.Flush();
                }
            }
        }
        /// <summary>
        /// 用于Web导出
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="fileName">例如: 123.xls</param>
        /// <param name="title">转换标题</param>
        public static void ExportByWeb(DataTable dt, string fileName, Dictionary<string, string> title = null)
        {
            HttpContext curContext = HttpContext.Current;
            // 设置编码和附件格式
            curContext.Response.ContentType = "application/vnd.ms-excel";
            curContext.Response.ContentEncoding = Encoding.UTF8;
            curContext.Response.Charset = "";
            curContext.Response.AppendHeader("Content-Disposition",
                "attachment;filename=" + HttpUtility.UrlEncode(fileName, Encoding.UTF8));
            curContext.Response.BinaryWrite(Export(dt, title).GetBuffer());
            curContext.Response.End();
        }
        private static MemoryStream Export(DataTable dt, Dictionary<string, string> title = null)
        {
            IWorkbook workBook = new HSSFWorkbook();
            #region 重组DataTable
            {
                if (title != null)
                {
                    //移除列,移除title中不存在的列
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        if (!title.ContainsKey(dt.Columns[i].ColumnName))
                        {
                            dt.Columns.Remove(dt.Columns[i]);
                            i--;
                        }
                    }
                    //移动列,按照title顺序导出
                    for (int i = 0; i < title.Count; i++)
                    {
                        if (dt.Columns.Contains(title.Keys.ElementAt(i)))
                        {
                            dt.Columns[title.Keys.ElementAt(i)].SetOrdinal(i);//更改列序号
                            dt.Columns[title.Keys.ElementAt(i)].ColumnName = title.Values.ElementAt(i);//更改列名称
                        }
                    }

                }
            }
            #endregion
            #region 设置样式
            //表头样式,有边框,内容居中,字体粗体
            ICellStyle headCellStyle = workBook.CreateCellStyle();//样式
            headCellStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.CENTER;//居中
            headCellStyle.BorderBottom = BorderStyle.THIN;//边框
            headCellStyle.BorderLeft = BorderStyle.THIN;
            headCellStyle.BorderRight = BorderStyle.THIN;
            headCellStyle.BorderTop = BorderStyle.THIN;
            IFont font = workBook.CreateFont();//粗体
            font.Boldweight = 600;
            headCellStyle.SetFont(font);
            //内容样式
            ICellStyle contentCellStyle = workBook.CreateCellStyle();//样式
            contentCellStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.CENTER;//居中
            contentCellStyle.BorderBottom = BorderStyle.THIN;//边框
            contentCellStyle.BorderLeft = BorderStyle.THIN;
            contentCellStyle.BorderRight = BorderStyle.THIN;
            contentCellStyle.BorderTop = BorderStyle.THIN;
            #endregion
            #region 填充数据
            //如果行数超过65535条则新建sheet
            int rowIndex = 0;
            int currIndex = 0;
            int sheetCount = (int)Math.Ceiling(dt.Rows.Count / 65535.0);
            for (int i = 0; i < sheetCount; i++)
            {
                ISheet sheet = workBook.CreateSheet();
                #region 填充表头
                //第一行
                IRow row = sheet.CreateRow(0);
                //表头名称
                foreach (DataColumn c in dt.Columns)
                {
                    row.CreateCell(c.Ordinal, CellType.NUMERIC).SetCellValue(c.ColumnName);
                    row.GetCell(c.Ordinal).CellStyle = headCellStyle;
                    //设置列宽
                    sheet.SetColumnWidth(c.Ordinal, (Encoding.Default.GetBytes(c.ColumnName).Length + 2) * 256);
                }
                #endregion
                #region 填充内容
                for (int r = currIndex; r < dt.Rows.Count; r++)
                {
                    if (r == (i + 1) * 65535)
                    {
                        rowIndex = 0;
                        break;
                    }
                    row = sheet.CreateRow(rowIndex + 1);
                    for (int c = 0; c < dt.Columns.Count; c++)
                    {
                        row.CreateCell(c).SetCellValue(dt.Rows[r][c].ToString());
                        row.GetCell(c).CellStyle = contentCellStyle;
                    }
                    rowIndex++;
                    currIndex++;
                }
                #endregion
            }
            #endregion
            #region 创建内存流
            using (MemoryStream ms = new MemoryStream())
            {
                workBook.Write(ms);
                return ms;
            }
            #endregion
        }
    }
}