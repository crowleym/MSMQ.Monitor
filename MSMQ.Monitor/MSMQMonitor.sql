-- queue count timeline
SELECT LogDate, COUNT(*) AS [Count]
FROM MSMQDataLogs
GROUP BY LogDate
ORDER BY LogDate

-- message time in queue
SELECT MessageID, MIN(LogDate) AS mindate, MAX(LogDate) AS maxdate, DATEDIFF(mi, MIN(LogDate), MAX(LogDate)) AS timeinqueue
FROM MSMQDataLogs
GROUP BY MessageID
ORDER BY mindate, timeinqueue
