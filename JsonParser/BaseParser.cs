namespace GlennDemo.JsonParser
{
    public abstract class BaseParser<ModelType> : IJsonParser<ModelType>
    {
        private Action<double>? _timeReportingAction;

        internal BaseParser(Action<double>? timeInMillisecondsReportingAction)
        {
            _timeReportingAction = timeInMillisecondsReportingAction;
        }

        public abstract ModelType DeserializeJsonStringToModel(string json);

        internal void ReportCompletionTimeNonBlocking(double milliseconds)
        {
            if (_timeReportingAction == null)
                return;

            Task.Run(() =>
            {
                // no error handling intended; the task will constrain any exception
                // intent: the system should continue running even if time reporting fails
                _timeReportingAction(milliseconds);
            });
        }
    }
}
