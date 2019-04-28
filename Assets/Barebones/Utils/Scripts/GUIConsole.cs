/*-------------------------------------------------
 *    Big thanks to Emil Rainero for contributing this script!
 *--------------------------------------------------*/

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Barebones {
    /// <summary>
    ///     Holds one unity console message
    /// </summary>
    public class LogEntry {
        public LogEntry(DateTime dateTime, string message, string stackTrace, LogType type) {
            DateTime = dateTime;
            Message = message;
            StackTrace = stackTrace;
            Type = type;
        }

        public DateTime DateTime { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public LogType Type { get; set; }
    }

    /// <summary>
    ///     Implements a GUIWindow with a visual console listing
    /// </summary>
    public class GUIConsole : MonoBehaviour {
        private static readonly Dictionary<LogType, Color> _logEntryColors = new Dictionary<LogType, Color> {
            {LogType.Log, Color.white},
            {LogType.Warning, Color.yellow},
            {LogType.Assert, Color.cyan},
            {LogType.Exception, Color.red},
            {LogType.Error, Color.red}
        };

        private readonly GUIContent _clearLogEntriesLabel = new GUIContent("Clear", "Clear all log entries");

        private readonly GUIContent _collapseDuplicatesLabel =
            new GUIContent("Collapse Duplicates", "Collapse duplicate log entries");


        private readonly List<LogEntry> _logEntries = new List<LogEntry>();
        private Vector2 _scrollPosition;
        private readonly GUIContent _showDateTimeLabel = new GUIContent("Show Date/Time", "Show date/time");
        private readonly GUIContent _showLatestLabel = new GUIContent("Show Latest", "Show the latest log entries");
        private readonly Rect _titleBarRect = new Rect(0, 0, 2000, 20);

        private Rect _windowRect;
        public int BottomMargin = 20;
        public bool CollapseDuplicates;

        [Header("Margins")] public int LeftMargin = 20;

        public int MaxLogEntries = 1000;
        public int RightMargin = 20;
        public bool Show = true;
        public bool ShowDateTime = true;
        public bool ShowLatest = true;
        public string Title = "Console";
        public KeyCode toggleKey = KeyCode.Escape;
        public int TopMargin = 20;

        private void OnEnable() {
            Application.logMessageReceived += OnLogMessageReceived;
            SetWindowRect();
        }

        private void OnDisable() {
            Application.logMessageReceived -= OnLogMessageReceived;
        }

        private void Update() {
            if (Input.GetKeyDown(toggleKey)) {
                Show = !Show;
                if (Show) SetWindowRect();
            }
        }

        private void OnGUI() {
            if (Show)
                _windowRect = GUILayout.Window(666, _windowRect, ConsoleWindow,
                    string.Format("{0}  (Hit '{1}' to hide/show)", Title, toggleKey.ToString()));
        }

        private void SetWindowRect() {
            _windowRect = new Rect(LeftMargin, TopMargin, Screen.width - (LeftMargin + RightMargin),
                Screen.height - (TopMargin + BottomMargin));
        }

        private void ConsoleWindow(int windowID) {
            _scrollPosition = GUILayout.BeginScrollView(_scrollPosition);

            // Iterate through the recorded logs.
            lock (_logEntries) {
                for (var i = 0; i < _logEntries.Count; i++) {
                    var logEntry = _logEntries[i];
                    var duplicates = CollapseDuplicates ? CountDuplicates(i, logEntry) : 0;

                    GUI.contentColor = _logEntryColors[logEntry.Type]; // set the color

                    var message = FormatLogEntry(logEntry, duplicates);
                    GUILayout.Label(message); // display the message

                    if (CollapseDuplicates && duplicates > 0) i += duplicates; // skip the duplicates
                }
            }

            if (ShowLatest)
                _scrollPosition.y += 9999; // force to end

            GUILayout.EndScrollView();

            GUI.contentColor = Color.white;

            // display a row of buttons/toggles at bottom of window
            GUILayout.BeginHorizontal();
            {
                if (GUILayout.Button(_clearLogEntriesLabel)) _logEntries.Clear();

                ShowLatest = GUILayout.Toggle(ShowLatest, _showLatestLabel, GUILayout.ExpandWidth(false));
                ShowDateTime = GUILayout.Toggle(ShowDateTime, _showDateTimeLabel, GUILayout.ExpandWidth(false));
                CollapseDuplicates = GUILayout.Toggle(CollapseDuplicates, _collapseDuplicatesLabel,
                    GUILayout.ExpandWidth(false));
            }
            GUILayout.EndHorizontal();

            // allow the window to be dragged
            GUI.DragWindow(_titleBarRect);
        }

        /// <summary>
        ///     Format a log entry with all the options
        /// </summary>
        private string FormatLogEntry(LogEntry logEntry, int duplicates) {
            var message = string.Empty;
            if (ShowDateTime) message += logEntry.DateTime.ToString("HH:mm:ss.fff");
            if (CollapseDuplicates && duplicates > 0) {
                if (message.Length > 0)
                    message += " ";
                message += string.Format("({0})", duplicates + 1);
            }

            if (message.Length > 0) message += ": ";
            message += logEntry.Message;

            if (logEntry.Type == LogType.Error)
                message += " " + logEntry.StackTrace;

            return message;
        }

        /// <summary>
        ///     count consecutive log entries that are exact duplicates
        /// </summary>
        private int CountDuplicates(int startingLogIndex, LogEntry logEntry) {
            var duplicates = 0;

            for (var j = startingLogIndex + 1; j < _logEntries.Count; j++)
                if (logEntry.Message == _logEntries[j].Message)
                    duplicates++;
                else
                    break;

            return duplicates;
        }

        /// <summary>
        ///     handle a log message received event - save the log message into a list
        /// </summary>
        private void OnLogMessageReceived(string message, string stackTrace, LogType type) {
            lock (_logEntries) {
                _logEntries.Add(new LogEntry(DateTime.Now, message, stackTrace, type));

                // trim the log
                while (_logEntries.Count > MaxLogEntries) _logEntries.RemoveAt(0); // remove oldest
            }
        }
    }
}