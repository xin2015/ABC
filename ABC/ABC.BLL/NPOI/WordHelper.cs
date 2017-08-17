using ABC.BLL.Tabulation;
using NPOI.OpenXmlFormats.Wordprocessing;
using NPOI.XWPF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABC.BLL.NPOI
{
    public class WordHelper
    {
        private static XWPFDocument GetDocument(Table table)
        {
            int headRowCount = table.Thead.cellStatus.Count, bodyRowCount = table.Tbody.cellStatus.Count, footRowCount = table.Tfoot.cellStatus.Count;
            int colCount = 0, rowCount = headRowCount + bodyRowCount + footRowCount;
            foreach (var status in table.Thead.cellStatus)
            {
                if (status.Count > colCount) colCount = status.Count;
            }
            foreach (var status in table.Tbody.cellStatus)
            {
                if (status.Count > colCount) colCount = status.Count;
            }
            foreach (var status in table.Tfoot.cellStatus)
            {
                if (status.Count > colCount) colCount = status.Count;
            }
            XWPFDocument doc = new XWPFDocument();
            XWPFTable xt = doc.CreateTable(rowCount, colCount);
            int rowAddend = 0;
            Fill(xt, table.Thead, rowAddend);
            rowAddend += headRowCount;
            Fill(xt, table.Tbody, rowAddend);
            rowAddend += bodyRowCount;
            Fill(xt, table.Tfoot, rowAddend);
            return doc;
        }

        private static void Fill(XWPFTable xt, TPart tp, int rowAddend)
        {
            foreach (TRow tr in tp)
            {
                XWPFTableRow xtr = xt.GetRow(tr.Index + rowAddend);
                foreach (TCell tc in tr)
                {
                    XWPFTableCell cell = xtr.GetCell(tc.Index);
                    cell.SetText(tc.Value);
                    if (tc.Rowspan > 1 || tc.Colspan > 1)
                    {
                        Merge(xt, tr.Index + rowAddend, tr.Index + rowAddend + tc.Rowspan - 1, tc.Index, tc.Index + tc.Colspan - 1);
                    }
                }
            }
        }

        private static void Merge(XWPFTable xt, int firstRow, int lastRow, int firstCol, int lastCol)
        {
            for (int row = firstRow; row <= lastRow; row++)
            {
                XWPFTableRow xtr = xt.GetRow(row);
                for (int col = firstCol; col <= lastCol; col++)
                {
                    CT_TcPr tcpr = xtr.GetCell(col).GetCTTc().AddNewTcPr();
                    tcpr.AddNewVMerge().val = row == firstRow ? ST_Merge.restart : ST_Merge.@continue;
                    tcpr.AddNewHMerge().val = col == firstCol ? ST_Merge.restart : ST_Merge.@continue;
                }
            }
        }

        public static MemoryStream Export(Table table)
        {
            XWPFDocument doc = GetDocument(table);
            MemoryStream ms = new MemoryStream();
            doc.Write(ms);
            return ms;
        }

        public static void Export(Table table, string fileName)
        {
            XWPFDocument doc = GetDocument(table);
            FileStream fs = File.Create(fileName);
            doc.Write(fs);
            fs.Close();
        }
    }
}
