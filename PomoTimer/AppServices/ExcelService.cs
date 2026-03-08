using ClosedXML.Excel;
using PomoTimer.Domain;

namespace PomoTimer.Services
{
    public class ExcelService
    {
        private readonly XLWorkbook _workbook;
        public ExcelService()
        {
            _workbook = new XLWorkbook("data.xlsx");
        }

        public void WriteRow(DateTime now, DateTime startTime, DateTime endTime, string timeFrameStr, string isBreakFlagStr, string windowName)
        {
            int lastRowNum;
            if(_workbook.Worksheet("データ").LastRowUsed() != null)
            {
                lastRowNum = _workbook.Worksheet("データ").LastRowUsed()!.RowNumber();

                _workbook.Worksheet("データ").Cell("A" + Convert.ToString(lastRowNum + 1)).Value = now.ToOADate();
                _workbook.Worksheet("データ").Cell("B" + Convert.ToString(lastRowNum + 1)).Value = now.ToString("ddd");
                _workbook.Worksheet("データ").Cell("C" + Convert.ToString(lastRowNum + 1)).Value = startTime.ToOADate();
                _workbook.Worksheet("データ").Cell("D" + Convert.ToString(lastRowNum + 1)).Value = endTime.ToOADate();
                _workbook.Worksheet("データ").Cell("E" + Convert.ToString(lastRowNum + 1)).Value = timeFrameStr;
                _workbook.Worksheet("データ").Cell("F" + Convert.ToString(lastRowNum + 1)).Value = isBreakFlagStr;
                _workbook.Worksheet("データ").Cell("G" + Convert.ToString(lastRowNum + 1)).Value = windowName;
                _workbook.Save();
            }
        }
        public void WriteSkipBreakTimeData(DateTime now, TimeTableRow row, LoggerService loggerService)
        {
            WriteRow(now, row.StartTime, row.EndTime, row.TimeFrameStr, "いいえ", "Form1");
            loggerService.Info("Skip break time. <TimeFrame>");
        }
    }
}
