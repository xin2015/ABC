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
            #region 填充thead
            foreach (TRow tr in table.Thead)
            {
                XWPFTableRow xtr = xt.GetRow(tr.Index);
                foreach (TCell th in tr)
                {
                    XWPFTableCell cell = xtr.GetCell(th.Index);
                    cell.SetText(th.Value);
                    if (th.Colspan > 1)
                    {
                        CT_Tc cttc = cell.GetCTTc();
                        CT_TcPr tcpr = cttc.AddNewTcPr();
                        tcpr.gridSpan = new CT_DecimalNumber();
                        tcpr.gridSpan.val = th.Colspan.ToString();
                        if (th.Rowspan > 1)
                        {
                            tcpr.AddNewVMerge().val = ST_Merge.restart;
                            for (int i = 1; i < th.Rowspan; i++)
                            {
                                XWPFTableRow xtri = xt.GetRow(tr.Index + i);
                                XWPFTableCell celli = xtri.GetCell(th.Index);
                                CT_Tc cttci = celli.GetCTTc();
                                CT_TcPr tcpri = cttci.AddNewTcPr();
                                tcpri.gridSpan = new CT_DecimalNumber();
                                tcpri.gridSpan.val = th.Colspan.ToString();
                                tcpri.AddNewVMerge().val = ST_Merge.@continue;
                            }
                        }
                    }
                    else if (th.Rowspan > 1)
                    {
                        CT_Tc cttc = cell.GetCTTc();
                        CT_TcPr tcpr = cttc.AddNewTcPr();
                        tcpr.AddNewVMerge().val = ST_Merge.restart;
                        for (int i = 1; i < th.Rowspan; i++)
                        {
                            XWPFTableRow xtri = xt.GetRow(tr.Index + i);
                            XWPFTableCell celli = xtri.GetCell(th.Index);
                            CT_Tc cttci = celli.GetCTTc();
                            CT_TcPr tcpri = cttci.AddNewTcPr();
                            tcpri.AddNewVMerge().val = ST_Merge.@continue;
                        }
                    }
                }
            }
            //foreach (TRow tr in table.Thead)
            //{
            //    XWPFTableRow xtr = xt.GetRow(tr.Index);
            //    foreach (TCell th in tr)
            //    {
            //        XWPFTableCell cell = xtr.GetCell(th.Index);
            //        cell.SetText(th.Value);
            //        if (th.Colspan > 1)
            //        {
            //            xtr.MergeCells(th.Index, th.Index + th.Colspan - 1);
            //            if (th.Rowspan > 1)
            //            {
            //                CT_Tc cttc = cell.GetCTTc();
            //                CT_TcPr tcpr = cttc.AddNewTcPr();
            //                tcpr.AddNewVMerge().val = ST_Merge.restart;
            //                for (int i = 1; i < th.Rowspan; i++)
            //                {
            //                    XWPFTableRow xtri = xt.GetRow(tr.Index + i);
            //                    xtri.MergeCells(th.Index, th.Index + th.Colspan - 1);
            //                    XWPFTableCell celli = xtri.GetCell(th.Index);
            //                    CT_Tc cttci = celli.GetCTTc();
            //                    CT_TcPr tcpri = cttci.AddNewTcPr();
            //                    tcpri.AddNewVMerge().val = ST_Merge.@continue;
            //                }
            //            }
            //        }
            //    }
            //}
            #endregion
            //#region 填充tbody
            //foreach (TRow tr in table.Tbody)
            //{
            //    XWPFTableRow xtr = xt.GetRow(tr.Index + headRowCount);
            //    foreach (TCell td in tr)
            //    {
            //        XWPFTableCell cell = xtr.GetCell(td.Index);
            //        cell.SetText(td.Value);
            //        if (td.Colspan > 1)
            //        {
            //            xtr.MergeCells(td.Index, td.Index + td.Colspan - 1);
            //            if (td.Rowspan > 1)
            //            {
            //                CT_Tc cttc = cell.GetCTTc();
            //                CT_TcPr tcpr = cttc.AddNewTcPr();
            //                tcpr.AddNewVMerge().val = ST_Merge.restart;
            //                for (int i = 1; i < td.Rowspan; i++)
            //                {
            //                    XWPFTableRow xtri = xt.GetRow(tr.Index + headRowCount + i);
            //                    xtri.MergeCells(td.Index, td.Index + td.Colspan - 1);
            //                    XWPFTableCell celli = xtri.GetCell(td.Index);
            //                    CT_Tc cttci = celli.GetCTTc();
            //                    CT_TcPr tcpri = cttci.AddNewTcPr();
            //                    tcpri.AddNewVMerge().val = ST_Merge.@continue;
            //                }
            //            }
            //        }
            //    }
            //}
            //#endregion
            //#region 填充tfoot
            //foreach (TRow tr in table.Tbody)
            //{
            //    XWPFTableRow xtr = xt.GetRow(tr.Index + headRowCount + bodyRowCount);
            //    foreach (TCell td in tr)
            //    {
            //        XWPFTableCell cell = xtr.GetCell(td.Index);
            //        cell.SetText(td.Value);
            //        if (td.Colspan > 1)
            //        {
            //            xtr.MergeCells(td.Index, td.Index + td.Colspan - 1);
            //            if (td.Rowspan > 1)
            //            {
            //                CT_Tc cttc = cell.GetCTTc();
            //                CT_TcPr tcpr = cttc.AddNewTcPr();
            //                tcpr.AddNewVMerge().val = ST_Merge.restart;
            //                for (int i = 1; i < td.Rowspan; i++)
            //                {
            //                    XWPFTableRow xtri = xt.GetRow(tr.Index + headRowCount + bodyRowCount + i);
            //                    xtri.MergeCells(td.Index, td.Index + td.Colspan - 1);
            //                    XWPFTableCell celli = xtri.GetCell(td.Index);
            //                    CT_Tc cttci = celli.GetCTTc();
            //                    CT_TcPr tcpri = cttci.AddNewTcPr();
            //                    tcpri.AddNewVMerge().val = ST_Merge.@continue;
            //                }
            //            }
            //        }
            //    }
            //}
            //#endregion
            return doc;
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
