using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VisiWin.ApplicationFramework;
using VisiWin.Logging;
using VisiWin.Parameter;
using HMI.Reporting;

namespace HMI.Reporting.DataSets.Logging
{
    partial class HistoricalLoggingDataSet
    {
        private static readonly ILoggingService loggingService = ApplicationService.GetService<ILoggingService>();

        public async Task FillWithAsync(IEnumerable<IHistoricalLoggingEntry> loggingEntries, CancellationToken cancellationToken = default(CancellationToken)) => await Task.Run(() => FillWithAsyncInternal(loggingEntries, cancellationToken));
        private async Task FillWithAsyncInternal(IEnumerable<IHistoricalLoggingEntry> loggingEntries, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (loggingEntries != null)
                foreach (var entry in await TaskHelper.RunOnUIThread(() => loggingEntries.ToArray(), cancellationToken))
                {
                    await this.HistoricalLoggingEntries.FillWithAsync(entry, cancellationToken);
                    await this.LoggingEntryNotes.FillWithAsync(entry, cancellationToken);
                    await this.LoggingEntryParameters.FillWithAsync(entry, cancellationToken);
                }
        }

        partial class HistoricalLoggingEntriesDataTable
        {
            private readonly SemaphoreSlim _addLock = new SemaphoreSlim(1, 1);

            public async Task FillWithAsync(IEnumerable<IHistoricalLoggingEntry> loggingEntries, CancellationToken cancellationToken = default(CancellationToken)) => await Task.Run(() => FillWithAsyncInternal(loggingEntries, cancellationToken));
            private async Task FillWithAsyncInternal(IEnumerable<IHistoricalLoggingEntry> loggingEntries, CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                if (loggingEntries != null)
                    foreach (var entry in await TaskHelper.RunOnUIThread(() => loggingEntries.ToArray(), cancellationToken))
                    {
                        await this.FillWithAsync(entry, cancellationToken);
                    }
            }

            public async Task FillWithAsync(IHistoricalLoggingEntry loggingEntry, CancellationToken cancellationToken = default(CancellationToken))
            {
                cancellationToken.ThrowIfCancellationRequested();
                if (loggingEntry != null)
                {
                    await _addLock.WaitAsync(cancellationToken);
                    try
                    {
                        await TaskHelper.RunOnUIThread(() =>
                        {
                            this.AddHistoricalLoggingEntriesRow(
                                CategoryName: loggingEntry.CategoryName,
                                EventName: loggingEntry.EventName,
                                FileName: loggingEntry.FileName,
                                FullName: loggingEntry.FullName,
                                HasNotes: loggingEntry.HasNotes,
                                ID: loggingEntry.ID,
                                LocalizedCategoryName: loggingEntry.LocalizedCategoryName,
                                LocalizedEventName: loggingEntry.LocalizedEventName,
                                LocalizedText: loggingEntry.LocalizedText,
                                Machine: loggingEntry.Machine,
                                TimeStamp: loggingEntry.TimeStamp,
                                User: loggingEntry.User);
                        }, cancellationToken);
                    }
                    finally
                    {
                        _addLock.Release();
                    }
                }
            }
        }

        partial class Joined_EntriesAndNotesDataTable
        {
            private readonly SemaphoreSlim _addLock = new SemaphoreSlim(1, 1);

            public async Task FillWithAsync(IEnumerable<IHistoricalLoggingEntry> loggingEntries, CancellationToken cancellationToken = default(CancellationToken)) => await Task.Run(() => FillWithAsyncInternal(loggingEntries, cancellationToken));
            private async Task FillWithAsyncInternal(IEnumerable<IHistoricalLoggingEntry> loggingEntries, CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                if (loggingEntries != null)
                    foreach (var entry in await TaskHelper.RunOnUIThread(() => loggingEntries.ToArray(), cancellationToken))
                    {
                        await this.FillWithAsyncInternal(entry, cancellationToken);
                    }
            }

            public async Task FillWithAsync(IHistoricalLoggingEntry entry, CancellationToken cancellationToken = default(CancellationToken)) => await Task.Run(() => FillWithAsyncInternal(entry, cancellationToken));
            private async Task FillWithAsyncInternal(IHistoricalLoggingEntry entry, CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                if (entry != null)
                    await this.FillWithAsyncInternal(entry, await TaskHelper.RunOnUIThread(() => loggingService.GetLoggingNotes(entry), cancellationToken), cancellationToken);
            }

            public async Task FillWithAsync(IHistoricalLoggingEntry entry, IEnumerable<ILoggingNote> notes, CancellationToken cancellationToken = default(CancellationToken)) => await Task.Run(() => FillWithAsyncInternal(entry, notes, cancellationToken));
            private async Task FillWithAsyncInternal(IHistoricalLoggingEntry entry, IEnumerable<ILoggingNote> notes, CancellationToken cancellationToken = default(CancellationToken))
            {
                cancellationToken.ThrowIfCancellationRequested();
                if (entry != null)
                {
                    var notesList = await TaskHelper.RunOnUIThread(() => notes?.ToList(), cancellationToken);
                    if (notesList != null && 0 < notesList.Count)
                    {
                        foreach (var note in notesList)
                        {
                            await this.FillWithAsync(entry, note, cancellationToken);
                        }
                    }
                    else
                    {
                        await this.FillWithAsync(entry, null as ILoggingNote, cancellationToken);
                    }
                }
            }

            public async Task FillWithAsync(IHistoricalLoggingEntry entry, ILoggingNote note, CancellationToken cancellationToken = default(CancellationToken))
            {
                cancellationToken.ThrowIfCancellationRequested();
                if (entry != null)
                {
                    await _addLock.WaitAsync(cancellationToken);
                    try
                    {
                        await TaskHelper.RunOnUIThread(() =>
                        {
                            this.AddJoined_EntriesAndNotesRow(
                                            E_CategoryName: entry.CategoryName,
                                            E_EventName: entry.EventName,
                                            E_FileName: entry.FileName,
                                            E_FullName: entry.FullName,
                                            E_HasNotes: entry.HasNotes,
                                            E_ID: entry.ID,
                                            E_LocalizedCategoryName: entry.LocalizedCategoryName,
                                            E_LocalizedEventName: entry.LocalizedEventName,
                                            E_LocalizedText: entry.LocalizedText,
                                            E_Machine: entry.Machine,
                                            E_TimeStamp: entry.TimeStamp,
                                            E_User: entry.User,
                                            N_FileName: note != null ? note.FileName : String.Empty,
                                            N_HistoricalEntryFullName: note != null ? note.HistoricalEntryFullName : String.Empty,
                                            N_HistoricalEntryID: note != null ? note.HistoricalEntryID : 0,
                                            N_ID: note != null ? note.ID : 0,
                                            N_Text: note != null ? note.Text : String.Empty,
                                            N_Timestamp: note != null ? note.Timestamp : DateTime.MinValue,
                                            N_User: note != null ? note.User : String.Empty);
                        }, cancellationToken);
                    }
                    finally
                    {
                        _addLock.Release();
                    }
                }

            }
        }

        partial class LoggingEntryNotesDataTable
        {
            private readonly SemaphoreSlim _addLock = new SemaphoreSlim(1, 1);

            public async Task FillWithAsync(IEnumerable<IHistoricalLoggingEntry> loggingEntries, CancellationToken cancellationToken = default(CancellationToken)) => await Task.Run(() => FillWithAsyncInternal(loggingEntries, cancellationToken));
            private async Task FillWithAsyncInternal(IEnumerable<IHistoricalLoggingEntry> loggingEntries, CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                if (loggingEntries != null)
                {
                    foreach (var entry in await TaskHelper.RunOnUIThread(() => loggingEntries.ToArray(), cancellationToken))
                    {
                        await this.FillWithAsyncInternal(entry, cancellationToken);
                    }
                }
            }

            public async Task FillWithAsync(IHistoricalLoggingEntry entry, CancellationToken cancellationToken = default(CancellationToken)) => await Task.Run(() => FillWithAsyncInternal(entry, cancellationToken));
            private async Task FillWithAsyncInternal(IHistoricalLoggingEntry entry, CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                if (entry != null)
                {
                    await this.FillWithAsyncInternal(await TaskHelper.RunOnUIThread(() => loggingService.GetLoggingNotes(entry), cancellationToken), cancellationToken);
                }
            }

            public async Task FillWithAsync(IEnumerable<ILoggingNote> notes, CancellationToken cancellationToken = default(CancellationToken)) => await Task.Run(() => FillWithAsyncInternal(notes, cancellationToken));
            private async Task FillWithAsyncInternal(IEnumerable<ILoggingNote> notes, CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                if (notes != null)
                {
                    foreach (var note in await TaskHelper.RunOnUIThread(() => notes.ToArray(), cancellationToken))
                    {
                        await this.FillWithAsync(note, cancellationToken);
                    }
                }
            }

            public async Task FillWithAsync(ILoggingNote note, CancellationToken cancellationToken = default(CancellationToken))
            {
                cancellationToken.ThrowIfCancellationRequested();
                if (note != null)
                {
                    await _addLock.WaitAsync(cancellationToken);
                    try
                    {
                        await TaskHelper.RunOnUIThread(() =>
                        {
                            this.AddLoggingEntryNotesRow(note.FileName, note.HistoricalEntryFullName, note.HistoricalEntryID, note.ID, note.Text, note.Timestamp, note.User);
                        }, cancellationToken);
                    }
                    finally
                    {
                        _addLock.Release();
                    }
                }
            }
        }

        partial class LoggingEntryParametersDataTable
        {
            private readonly SemaphoreSlim _addLock = new SemaphoreSlim(1, 1);

            public async Task FillWithAsync(IEnumerable<IHistoricalLoggingEntry> loggingEntries, CancellationToken cancellationToken = default(CancellationToken)) => await Task.Run(() => FillWithAsyncInternal(loggingEntries, cancellationToken));
            private async Task FillWithAsyncInternal(IEnumerable<IHistoricalLoggingEntry> loggingEntries, CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                if (loggingEntries != null)
                {
                    foreach (var entry in await TaskHelper.RunOnUIThread(() => loggingEntries.ToArray(), cancellationToken))
                    {
                        await this.FillWithAsyncInternal(entry, cancellationToken);
                    }
                }
            }

            public async Task FillWithAsync(IHistoricalLoggingEntry entry, CancellationToken cancellationToken = default(CancellationToken)) => await Task.Run(() => FillWithAsyncInternal(entry, cancellationToken));
            private async Task FillWithAsyncInternal(IHistoricalLoggingEntry entry, CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                if (entry != null)
                {
                    foreach (var paramenter in await TaskHelper.RunOnUIThread(() => entry.StoredParameters == null ? new List<IParameterValue>() as IEnumerable<IParameterValue> : entry.StoredParameters.ToArray(), cancellationToken))
                    {
                        var r = await TaskHelper.RunOnUIThread(() => new { entry.FileName, entry.ID }, cancellationToken);
                        await this.FillWithAsync(paramenter, r.FileName, r.ID, cancellationToken);
                    }
                }
            }

            public async Task FillWithAsync(IParameterValue paramenter, string fileName, int id, CancellationToken cancellationToken = default(CancellationToken))
            {
                cancellationToken.ThrowIfCancellationRequested();
                if (paramenter != null)
                {
                    await _addLock.WaitAsync(cancellationToken);
                    try
                    {
                        await TaskHelper.RunOnUIThread(() =>
                        {
                            this.AddLoggingEntryParametersRow(
                                        Index: paramenter.Index,
                                        LocalizedName: paramenter.LocalizedName,
                                        Name: paramenter.Name,
                                        Quality: paramenter.Quality.ToString(),
                                        Timestamp: paramenter.Timestamp,
                                        Type: paramenter.Type.ToString(),
                                        QUAL_Data: paramenter.Quality.Data.ToString(),
                                        QUAL_IsBad: paramenter.Quality.IsBad,
                                        QUAL_IsGoodOrUncretain: paramenter.Quality.IsGoodOrUncertain,
                                        QUAL_Limit: paramenter.Quality.Limit.ToString(),
                                        QUAL_Value: paramenter.Quality.Value,
                                        Entry_FileName: fileName,
                                        Entry_ID: id);
                        }, cancellationToken);
                    }
                    finally
                    {
                        _addLock.Release();
                    }
                }
            }
        }
    }
}