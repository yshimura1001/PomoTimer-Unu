using ClosedXML.Excel;
using PomoTimer.Domain;

namespace PomoTimer.AppServices
{
    public class ExcelService
    {
        private readonly XLWorkbook _workbook;
        private readonly TimeProviderService _timeService = DIContainer.GetTimeProviderService(false);
        public ExcelService()
        {
            _workbook = new XLWorkbook("data.xlsx");  
        }

        public void WriteRow(Time nowTime, Time startTime, Time endTime, string timeFrameStr, string isBreakFlagStr, string windowName)
        {
            int lastRowNum;
            DateTime nowDt = _timeService.ToDateTime(nowTime);
            DateTime startDt = _timeService.ToDateTime(startTime);
            DateTime endDt = _timeService.ToDateTime(endTime);
            if (_workbook.Worksheet("データ").LastRowUsed() != null)
            {
                lastRowNum = _workbook.Worksheet("データ").LastRowUsed()!.RowNumber();

                _workbook.Worksheet("データ").Cell("A" + Convert.ToString(lastRowNum + 1)).Value = nowDt.ToOADate();
                _workbook.Worksheet("データ").Cell("B" + Convert.ToString(lastRowNum + 1)).Value = nowDt.ToString("ddd");
                _workbook.Worksheet("データ").Cell("C" + Convert.ToString(lastRowNum + 1)).Value = startDt.ToOADate();
                _workbook.Worksheet("データ").Cell("D" + Convert.ToString(lastRowNum + 1)).Value = endDt.ToOADate();
                _workbook.Worksheet("データ").Cell("E" + Convert.ToString(lastRowNum + 1)).Value = timeFrameStr;
                _workbook.Worksheet("データ").Cell("F" + Convert.ToString(lastRowNum + 1)).Value = isBreakFlagStr;
                _workbook.Worksheet("データ").Cell("G" + Convert.ToString(lastRowNum + 1)).Value = windowName;
                _workbook.Save();
            }
        }
        public void WriteSkipBreakTimeData(Time nowTime, TimeTableRow row, LoggerService loggerService)
        {
            WriteRow(nowTime, row.StartTime, row.EndTime, row.TimeFrameStr, "いいえ", "Form1");
            loggerService.Info("Skip break time. <TimeFrame>");
        }
    }
}
