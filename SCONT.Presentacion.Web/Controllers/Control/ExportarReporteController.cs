using OfficeOpenXml;
using OfficeOpenXml.Style;
using SCONT.Aplicacion.Servicios;
using SCONT.Dominio.Entidades.Custom;
using SCONT.Infraestructura.Transversal;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.VisualBasic;
using SCONT.Dominio.Entidades;

namespace SCONT.Presentacion.Web.Controllers.Control
{
    public class ExportarReporteController : MEDBaseController
    {
        public string app_download = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        [Route("reporte/reporte_horas_trabajadas")]
        public ActionResult reporte_horas_trabajadas()
        {
            Stream stream = reporte_horas_trabajadas_XLS();
            return File(stream, app_download);
        }
        
        private void Configura_Response(string nombre_archivo)
        {
            Response.Clear();
            Response.Buffer = true;
            Response.ClearContent();
            Response.ContentType = app_download;
            Response.AddHeader("content-disposition", "attachment;filename=" + nombre_archivo + "_" + sisFuncion.GetFechaServidor().Value.ToString("ddMMyyyy_Hmmss") + ".xlsx");
            Response.Charset = "";
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
        }

        private void Reporte_Standar(ExcelWorksheet ws, string titulo, DataTable dt, bool mostrar_cabecera, OfficeOpenXml.Table.TableStyles tableStyles = OfficeOpenXml.Table.TableStyles.Light8)
        {
            //configuracion de la hoja de impresion
            ws.PrinterSettings.Orientation = eOrientation.Landscape;
            ws.PrinterSettings.PaperSize = ePaperSize.A4;
            ws.PrinterSettings.FooterMargin = .05M;
            ws.PrinterSettings.TopMargin = .05M;
            ws.PrinterSettings.LeftMargin = .05M;
            ws.PrinterSettings.RightMargin = .05M;
            ws.PrinterSettings.Scale = 75;

            string ultima_columna = sisFuncion.ToExcelIndexColum(dt.Columns.Count + 1);
            int total_fila = dt.Rows.Count;
            var f = 1;
            //Cabecera-------------------------------------------------------------------------------------
            ws.Cells["A" + f].LoadFromText("Sistema de Control de Asistencia");
            f = f + 1;
            ws.Cells["A" + f].LoadFromText("Fecha y Hora: " + sisFuncion.GetFechaServidor().ToString());
            //-----titulo
            ws.Cells["D" + f + ":" + ultima_columna + f].Merge = true;
            ws.Cells["D" + f + ":" + ultima_columna + f].Style.Font.Bold = true;
            ws.Cells["D" + f].LoadFromText(titulo);
            ws.Cells["D" + f].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            f = f + 1;
            //ws.Cells["A" + f].LoadFromText("N° de registros: " + dt.Rows.Count);
            // f = f + 1;
            ws.Cells["A1:A" + f].Style.Font.Italic = true;
            ws.Cells["A1:A" + f].Style.Font.Color.SetColor(System.Drawing.Color.Gray);
            f = f + 1;
            ws.Cells["A1:" + ultima_columna + (total_fila + f + 500).ToString()].Style.Font.Size = 8;
            //Fin Cabecera----------------------------------------------------------------------------------

            //Carga tabla
            f = f + 2;
            ws.Cells["A" + f].LoadFromDataTable(dt, mostrar_cabecera, tableStyles);
            ws.Cells["B1:" + ultima_columna + (f + 10).ToString()].AutoFitColumns();

        }

        private Stream reporte_horas_trabajadas_XLS()
        {
            DataTable dt = new DataTable();
            Parametro datParametro = (Parametro)sisSesion.Reporte_Parametro;
            dt = (DataTable)sisSesion.Lista_Reporte;

            if (dt != null)
            {
                using (ExcelPackage pck = new ExcelPackage())
                {
                    Configura_Response("Reporte_Horas_Trabajadas");
                    ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Reporte");
                    Reporte_Standar(ws, "REPORTE DE HORAS TRABAJADAS", dt, false);
                    var f = 4;

                    ws.Cells["A" + f + ":B" + f].Merge = true;
                    ws.Cells["A" + f].LoadFromText("DATOS DEL PERSONAL");
                    ws.Cells["C" + f + ":E" + f].Merge = true;
                    ws.Cells["C" + f].LoadFromText("HORARIO LABORAL");
                    ws.Cells["F" + f + ":G" + f].Merge = true;
                    ws.Cells["F" + f].LoadFromText("MARCACIÓN");
                    ws.Cells["H" + f + ":I" + f].Merge = true;
                    ws.Cells["H" + f].LoadFromText("HORARIO DE DESCANSO");
                    ws.Cells["J" + f + ":O" + f].Merge = true;
                    ws.Cells["J" + f].LoadFromText("HORAS CALCULADAS");

                    ws.Cells["A" + (f+1)].LoadFromText("N° DNI");
                    ws.Cells["B" + (f + 1)].LoadFromText("APELLIDOS Y NOMBRES");
                    ws.Cells["C" + (f + 1)].LoadFromText("FECHA");
                    ws.Cells["D" + (f + 1)].LoadFromText("H. INGRESO");
                    ws.Cells["E" + (f + 1)].LoadFromText("H. SALIDA");
                    ws.Cells["F" + (f + 1)].LoadFromText("INGRESO (A)");
                    ws.Cells["G" + (f + 1)].LoadFromText("SALIDA (B)");
                    ws.Cells["H" + (f + 1)].LoadFromText("INICIO (C )");
                    ws.Cells["I" + (f + 1)].LoadFromText("FIN (D)");
                    ws.Cells["J" + (f + 1)].LoadFromText("HORAS TOTALES\n(E = B - A)");
                    ws.Cells["K" + (f + 1)].LoadFromText("HORAS DESCANSO\n(F = D - C)");
                    ws.Cells["L" + (f + 1)].LoadFromText("HORAS TRABAJADAS\n(G = E - F)");
                    ws.Cells["M" + (f + 1)].LoadFromText("JORNADA LABORAL");
                    ws.Cells["N" + (f + 1)].LoadFromText("RETRASO INGRESO");
                    ws.Cells["O" + (f + 1)].LoadFromText("SALIDA TEMPRANA");



                    //ws.Cells["B" + f].LoadFromText("FECHA PAGADO");
                    //ws.Cells["C" + f + ":G" + f].Merge = true;
                    //ws.Cells["C" + f].LoadFromText("DOCUMENTO SUSTENTATORIO");
                    //ws.Cells["C" + (f + 1)].LoadFromText("TIPO DOC.");
                    //ws.Cells["D" + (f + 1)].LoadFromText("N° DOCUM.");
                    //ws.Cells["E" + (f + 1)].LoadFromText("DIRECCIÓN");
                    //ws.Cells["F" + (f + 1)].LoadFromText("META");
                    //ws.Cells["G" + (f + 1)].LoadFromText("NOMBRE PROVEEDOR / USUARIO / COMISIONADO");
                    //ws.Cells["H" + f + ":H" + (f + 1)].Merge = true;
                    //ws.Cells["H" + f].LoadFromText("DETALLE DEL GASTO");
                    //ws.Cells["I" + f + ":I" + (f + 1)].Merge = true;
                    //ws.Cells["I" + f].LoadFromText("IMPORTE");



                    var fill = ws.Cells["A" + f + ":O" + (f + 1)].Style.Fill;
                    fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(System.Drawing.Color.Black);
                    ws.Cells["A" + f + ":O" + (f + 1)].Style.Font.Bold = true;
                    ws.Cells["A" + f + ":O" + (f + 1)].Style.WrapText = true;
                    ws.Cells["A" + f + ":O" + (f + 1)].Style.Font.Color.SetColor(System.Drawing.Color.White);
                    ws.Cells["A" + f + ":O" + (f + 1)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells["A" + f + ":O" + (f + 1)].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    //lineas de tabla
                    ws.Cells["A" + f + ":O" + (f + 1)].Style.Border.Top.Style =
                       ws.Cells["A" + f + ":O" + (f + 1)].Style.Border.Bottom.Style =
                           ws.Cells["A" + f + ":O" + (f + 1)].Style.Border.Right.Style =
                               ws.Cells["A" + f + ":O" + (f + 1)].Style.Border.Left.Style =
                                   OfficeOpenXml.Style.ExcelBorderStyle.Thin;


                    f = f + 2;
                    //estilos personalizados
                    ws.Cells["C" + f + ":C" + (dt.Rows.Count + f).ToString()].Style.Numberformat.Format = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;
                    
                    for(int col = 3; col<= dt.Columns.Count; col++)
                    {
                        ws.Column(col).Width = (col==3 || col==12 ? 10 : 9);
                        ws.Cells[sisFuncion.ToExcelIndexColum(col) + f + ":" + sisFuncion.ToExcelIndexColum(col) + (dt.Rows.Count + f)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        if(col == 6 || col == 7)
                        {
                            fill = ws.Cells[sisFuncion.ToExcelIndexColum(col) + f + ":" + sisFuncion.ToExcelIndexColum(col) + (dt.Rows.Count + f-1)].Style.Fill;
                            fill.PatternType = ExcelFillStyle.Solid;
                            fill.BackgroundColor.SetColor(Color.FromArgb(255, 242, 204));
                        }
                        else if (col == 13)
                        {
                            fill = ws.Cells[sisFuncion.ToExcelIndexColum(col) + f + ":" + sisFuncion.ToExcelIndexColum(col) + (dt.Rows.Count + f-1)].Style.Fill;
                            fill.PatternType = ExcelFillStyle.Solid;
                            fill.BackgroundColor.SetColor(Color.FromArgb(198,224,180));
                        }
                        else if (col == 14 || col == 15)
                        {
                            fill = ws.Cells[sisFuncion.ToExcelIndexColum(col) + f + ":" + sisFuncion.ToExcelIndexColum(col) + (dt.Rows.Count + f-1)].Style.Fill;
                            fill.PatternType = ExcelFillStyle.Solid;
                            fill.BackgroundColor.SetColor(Color.FromArgb(252,228,214));
                        }
                    }

                    var ms = new System.IO.MemoryStream();
                    pck.SaveAs(ms);
                    ms.WriteTo(Response.OutputStream);
                    return ms;
                }
            }
            else
            {
                return null;
            }
        }



    }
}

