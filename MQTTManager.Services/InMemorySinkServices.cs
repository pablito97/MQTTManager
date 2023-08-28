using Serilog.Core;
using Serilog.Events;
using System.Collections.Concurrent;

namespace MQTTManager.Services
{
    public class InMemorySink : ILogEventSink
    {
        private readonly ConcurrentQueue<LogEvent> _logEvents;

        public InMemorySink(ConcurrentQueue<LogEvent> logEvents)
        {
            _logEvents = logEvents;
        }

        public void Emit(LogEvent logEvent)
        {
            // Możemy wprowadzić jakiekolwiek ograniczenia dotyczące przechowywania logów, na przykład
            // przechowywać tylko logi z ostatniej godziny.
            if (logEvent.Timestamp > DateTimeOffset.Now.AddHours(-1))
            {
                _logEvents.Enqueue(logEvent);
            }

            // Jeżeli kolejka zawiera za dużo logów, usuwamy najstarsze.
            while (_logEvents.Count > 100)
            {
                _logEvents.TryDequeue(out _);
            }
        }
    }
}
