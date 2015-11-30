using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aspose.Words;

namespace FanFunction
{
    public class clsWord
    {
        /// <summary>
        /// 获取word内容
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns>字符串集合</returns>
        public static List<string> GetContent(string filePath)
        {
            string content = string.Empty;
            List<string> strWords = new List<string>();
            Document doc = new Document(filePath);
            Section section = (Section)doc.ChildNodes[0];
            Body body = section.Body;
            foreach (Aspose.Words.Node node in body.ChildNodes)
            {
                //分隔字符串
                var contents = node.Range.Text.Split(new string[] { "\v", "\a", "\r", ":", "|", "：" }, StringSplitOptions.RemoveEmptyEntries);
                strWords.AddRange(contents);
                //content = node.Range.Text;
                //if (node is Aspose.Words.Paragraph)
                //{
                //    Aspose.Words.Paragraph pg = (Aspose.Words.Paragraph)node;
                //    if (pg.Runs != null && pg.Runs.Count > 0)
                //    {
                //        content = pg.Runs[0].Text;
                //        strWords.Add(content);
                //    }
                //}
                //else if (node is Aspose.Words.Tables.Table)
                //{
                //    Aspose.Words.Tables.Table table = (Aspose.Words.Tables.Table)node;
                //    foreach (Aspose.Words.Tables.Row row in table.Rows)
                //    {
                //        foreach (Aspose.Words.Tables.Cell cell in row.Cells)
                //        {
                //            if (cell.Paragraphs[0].Runs[0] != null)
                //            {
                //                content = cell.Paragraphs[0].Runs[0].Text;
                //                strWords.Add(content);
                //            }
                //        }
                //    }
                //}
            }
            //去除空格
            strWords = strWords.Select(w => w.Trim()).ToList();
            return strWords;
        }
    }
}
