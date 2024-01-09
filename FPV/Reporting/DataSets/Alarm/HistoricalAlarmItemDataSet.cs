using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VisiWin.Alarm;
using HMI.Reporting;

namespace HMI.Reporting.DataSets.Alarm
{
    public partial class HistoricalAlarmItemDataSet
    {
        /// <summary>
        /// Füllt alle DataTables des DataSets mit den Daten aus der Liste.
        /// </summary>
        /// <param name="alarmItems"></param>
        public async Task FillWithAsync(IEnumerable<IHistoricalAlarmItem> alarmItems, CancellationToken cancellationToken = default(CancellationToken)) => await Task.Run(() => FillWithAsyncInternal(alarmItems, cancellationToken));
        private async Task FillWithAsyncInternal(IEnumerable<IHistoricalAlarmItem> alarmItems, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (alarmItems != null)
            {
                foreach (var item in await TaskHelper.RunOnUIThread(() => alarmItems.ToArray(), cancellationToken))
                {
                    await this.HistoricalAlarmItems.FillWithAsync(item, cancellationToken);
                    await this.HistoricalAlarmNotes.FillWithAsync(item, cancellationToken);
                }
                await this.Joined_ItemsAndNotes.FillWithAsync(this.HistoricalAlarmItems, this.HistoricalAlarmNotes, cancellationToken);
            }
        }

        public partial class HistoricalAlarmItemsDataTable
        {
            private readonly SemaphoreSlim _addLock = new SemaphoreSlim(1, 1);

            /// <summary>
            /// Fügt der DataTable die Daten aller AlarmItems in der Liste hinzu.
            /// </summary>
            /// <param name="alarmItems"></param>
            public async Task FillWith(IEnumerable<IHistoricalAlarmItem> alarmItems, CancellationToken cancellationToken = default(CancellationToken)) => await Task.Run(() => FillWithAsyncInternal(alarmItems, cancellationToken));
            private async Task FillWithAsyncInternal(IEnumerable<IHistoricalAlarmItem> alarmItems, CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                if (alarmItems != null)
                {
                    foreach (var item in await TaskHelper.RunOnUIThread(() => alarmItems.ToArray(), cancellationToken))
                    {
                        await this.FillWithAsyncInternal(item, cancellationToken);
                    }
                }
            }

            /// <summary>
            /// Fügt der DataTable einen Eintrag mit den Daten des AlarmItems hinzu.
            /// </summary>
            /// <param name="alarmItem"></param>
            public async Task FillWithAsync(IHistoricalAlarmItem alarmItem, CancellationToken cancellationToken = default(CancellationToken)) => await Task.Run(() => FillWithAsyncInternal(alarmItem, cancellationToken));
            private async Task FillWithAsyncInternal(IHistoricalAlarmItem alarmItem, CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                if (alarmItem != null)
                {
                    var bitmap = (await TaskHelper.RunOnUIThread(() => alarmItem.StateInfo.Image)).BitmapToByteArray();

                    await this._addLock.WaitAsync(cancellationToken);
                    try
                    {
                        await TaskHelper.RunOnUIThread(() =>
                        {
                            this.AddHistoricalAlarmItemsRow(
                                    TheAlarmItem: alarmItem,
                                    AcknowledgeTime: alarmItem.AcknowledgeTime,
                                    ActivationCounter: alarmItem.ActivationCounter,
                                    ActivationTime: alarmItem.ActivationTime,
                                    AlarmState: alarmItem.AlarmState.ToString(),
                                    AlarmClass: alarmItem.Class.ToString(),
                                    DeactivationTime: alarmItem.DeactivationTime,
                                    FileName: alarmItem.FileName,
                                    AlarmGroup: alarmItem.Group.ToString(),
                                    HasNotes: alarmItem.HasNotes,
                                    HistoricalID: alarmItem.HistoricalID + "",
                                    LocalizableParam1: alarmItem.LocalizableParam1,
                                    LocalizableText: alarmItem.LocalizableText,
                                    Machine: alarmItem.Machine,
                                    Name: alarmItem.Name,
                                    Param1: alarmItem.Param1,
                                    Param2: alarmItem.Param2,
                                    Priority: alarmItem.Priority,
                                    StateInfo: alarmItem.StateInfo.ToString(),
                                    Tag: alarmItem.Tag,
                                    Text: alarmItem.Text,
                                    User: alarmItem.User,
                                    AC_LocalizableText: alarmItem.Class.LocalizableText,
                                    AC_Name: alarmItem.Class.Name,
                                    AC_Text: alarmItem.Class.Text,
                                    AG_GroupTree: alarmItem.Group.GroupTree,
                                    AG_LocalizableText: alarmItem.Group.LocalizableText,
                                    AG_Name: alarmItem.Group.Name,
                                    AG_Text: alarmItem.Group.Text,
                                    ASI_BackColor: alarmItem.StateInfo.BackColor.ToRgbHexCode(),
                                    ASI_ForeColor: alarmItem.StateInfo.ForeColor.ToRgbHexCode(),
                                    ASI_Image: bitmap,
                                    ASI_LocalizableText: alarmItem.StateInfo.LocalizableText,
                                    ASI_Tag: alarmItem.StateInfo.Tag,
                                    ASI_Text: alarmItem.StateInfo.Text,
                                    LT_AC_Text: alarmItem.Class.LocalizableText.TryToLocalize(alarmItem.Class.Text),
                                    LT_AG_Text: alarmItem.Group.LocalizableText.TryToLocalize(alarmItem.Group.Text),
                                    LT_ASI_Text: alarmItem.StateInfo.LocalizableText.TryToLocalize(alarmItem.StateInfo.Text),
                                    LT_Param1: alarmItem.LocalizableParam1.TryToLocalize(alarmItem.Param1),
                                    LT_Text: alarmItem.LocalizableText.TryToLocalize(alarmItem.Text));
                        }, cancellationToken);
                    }
                    finally
                    {
                        _addLock.Release();
                    }
                }
            }
        }

        public partial class HistoricalAlarmNotesDataTable
        {
            private readonly SemaphoreSlim _addLock = new SemaphoreSlim(1, 1);

            /// <summary>
            /// Fügt der DataTable alle Alarmnotiz-Daten der übergebenen AlarmItem-Liste hinzu.
            /// </summary>
            /// <param name="alarmItems"></param>
            public async Task FillWithAsync(IEnumerable<IHistoricalAlarmItem> alarmItems, CancellationToken cancellationToken = default(CancellationToken)) => await Task.Run(() => FillWithAsyncInternal(alarmItems, cancellationToken));
            private async Task FillWithAsyncInternal(IEnumerable<IHistoricalAlarmItem> alarmItems, CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                if (alarmItems != null)
                {
                    foreach (var item in await TaskHelper.RunOnUIThread(() => alarmItems.ToArray(), cancellationToken))
                    {
                        await this.FillWithAsyncInternal(item, cancellationToken);
                    }
                }
            }

            /// <summary>
            /// Fügt alle Notizen des Alarms der DataTable hinzu.
            /// </summary>
            /// <param name="alarmItem"></param>
            public async Task FillWithAsync(IHistoricalAlarmItem alarmItem, CancellationToken cancellationToken = default(CancellationToken)) => await Task.Run(() => FillWithAsyncInternal(alarmItem, cancellationToken));
            private async Task FillWithAsyncInternal(IHistoricalAlarmItem alarmItem, CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                if (alarmItem != null)
                {
                    await this.FillWithAsyncInternal(await TaskHelper.RunOnUIThread(() => alarmItem.GetNotes()?.ToArray()), cancellationToken);
                }
            }

            /// <summary>
            /// Fügt alle Notizen der DataTable hinzu.
            /// </summary>
            /// <param name="alarmNotes"></param>
            public async Task FillWithAsync(IEnumerable<IHistoricalAlarmNote> alarmNotes, CancellationToken cancellationToken = default(CancellationToken)) => await Task.Run(() => FillWithAsyncInternal(alarmNotes, cancellationToken));
            private async Task FillWithAsyncInternal(IEnumerable<IHistoricalAlarmNote> alarmNotes, CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                if (alarmNotes != null)
                    foreach (var note in await TaskHelper.RunOnUIThread(() => alarmNotes.ToArray(), cancellationToken))
                    {
                        await this.FillWithAsyncInternal(note, cancellationToken);
                    }
            }

            /// <summary>
            /// Fügt einen Eintrag für diese Notiz hinzu.
            /// </summary>
            /// <param name="alarmNote"></param>
            public async Task FillWithAsync(IHistoricalAlarmNote alarmNote, CancellationToken cancellationToken = default(CancellationToken)) => await Task.Run(() => FillWithAsyncInternal(alarmNote, cancellationToken));
            private async Task FillWithAsyncInternal(IHistoricalAlarmNote alarmNote, CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                if (alarmNote != null)
                {
                    await this._addLock.WaitAsync(cancellationToken);
                    try
                    {
                        await TaskHelper.RunOnUIThread(() =>
                        {
                            this.AddHistoricalAlarmNotesRow(alarmNote.FileName, alarmNote.HistoricalAlarmID, alarmNote.ID, alarmNote.Tag, alarmNote.Text, alarmNote.TimeStamp, alarmNote.User);
                        }, cancellationToken);
                    }
                    finally
                    {
                        _addLock.Release();
                    }
                }
            }
        }

        public partial class Joined_ItemsAndNotesDataTable
        {
            private readonly SemaphoreSlim _addLock = new SemaphoreSlim(1, 1);

            /*
             * Die Daten in dieser DataTable sind angelehnt an eine SQL-Select-Anweisung mit einem Join über die 
             * HistoricalAlarmItemsDataTable und die HistoricalAlarmNotesDataTable.
             */

            /// <summary>
            /// Füllt die DataTable mit den Daten aus den beiden anderen DataTables.
            /// </summary>
            /// <param name="items"></param>
            /// <param name="notes"></param>
            public async Task FillWithAsync(HistoricalAlarmItemsDataTable items, HistoricalAlarmNotesDataTable notes, CancellationToken cancellationToken = default(CancellationToken)) => await Task.Run(() => FillWithAsyncInternal(items, notes, cancellationToken));
            private async Task FillWithAsyncInternal(HistoricalAlarmItemsDataTable items, HistoricalAlarmNotesDataTable notes, CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                if (items != null && notes != null)
                {
                    var groupedNotes = new Dictionary<string, LinkedList<int>>();
                    for (int i = 0, n = notes.Count; i < n; i++)
                    {
                        cancellationToken.ThrowIfCancellationRequested();
                        var notesRow = notes[i];
                        var key = notesRow.HistoricalAlarmID + "-" + notesRow.FileName;
                        if (groupedNotes.ContainsKey(key))
                        {
                            groupedNotes[key].AddLast(i);
                        }
                        else
                        {
                            groupedNotes.Add(key, new LinkedList<int>(new[] { i }));
                        }
                    }
                    cancellationToken.ThrowIfCancellationRequested();
                    foreach (var row in items)
                    {
                        cancellationToken.ThrowIfCancellationRequested();
                        var key = row.HistoricalID + "-" + row.FileName;
                        var indizies = groupedNotes[key];
                        if (indizies.Count > 0)
                        {
                            foreach (var i in indizies)
                            {
                                await this.FillWith(row, notes[i], cancellationToken);
                            }
                        }
                        else
                        {
                            await this.FillWith(row, null, cancellationToken);
                        }
                    }
                }
            }

            /// <summary>
            /// Füllt die DataTable mit den Daten aus den HistoricalAlarmItems.
            /// </summary>
            /// <param name="historicalAlarmItems"></param>
            public async Task FillWithAsync(IEnumerable<IHistoricalAlarmItem> historicalAlarmItems, CancellationToken cancellationToken = default(CancellationToken)) => await Task.Run(() => FillWithAsyncInternal(historicalAlarmItems, cancellationToken));
            private async Task FillWithAsyncInternal(IEnumerable<IHistoricalAlarmItem> historicalAlarmItems, CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                if (historicalAlarmItems != null)
                    foreach (var item in await TaskHelper.RunOnUIThread(() => historicalAlarmItems.ToArray(), cancellationToken))
                    {
                        await this.FillWithAsyncInternal(item, cancellationToken);
                    }
            }

            /// <summary>
            /// Fügt der DataTable Einträge hinzu die alle Daten dieses Alarms enthalten.
            /// Die Anzahl der Einträge ist abhängig von der Anzahl der Notizen zu dem Alarm.
            /// Für N > 0 werden N Einträge erzeugt, für N = 0 wird genau ein Eintrag erzeugt,
            /// allerdings sind dann alle Spalten die Informationen über Notizen enthalten null
            /// oder enthalten einen Ersatzwert.
            /// </summary>
            /// <param name="item"></param>
            public async Task FillWithAsync(IHistoricalAlarmItem item, CancellationToken cancellationToken = default(CancellationToken)) => await Task.Run(() => FillWithAsyncInternal(item, cancellationToken));
            private async Task FillWithAsyncInternal(IHistoricalAlarmItem item, CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                if (item != null)
                {
                    var notes = await TaskHelper.RunOnUIThread(() => item.GetNotes()?.ToList(), cancellationToken);
                    if (notes != null && 0 < notes.Count)
                    {
                        foreach (var note in notes)
                        {
                            await this.FillWith(item, note, cancellationToken);
                        }
                    }
                    else
                    {
                        await this.FillWith(item, null, cancellationToken);
                    }
                }
            }

            private async Task FillWith(HistoricalAlarmItemsRow row, HistoricalAlarmNotesRow notesRow, CancellationToken cancellationToken = default(CancellationToken))
            {
                cancellationToken.ThrowIfCancellationRequested();
                if (row != null)
                {
                    await _addLock.WaitAsync(cancellationToken);
                    try
                    {
                        this.AddJoined_ItemsAndNotesRow(
                                        I_TheAlarmItem: row.TheAlarmItem,
                                        I_AcknowledgeTime: row.AcknowledgeTime,
                                        I_ActivationCounter: row.ActivationCounter,
                                        I_ActivationTime: row.ActivationTime,
                                        I_AC_LocalizableText: row.AC_LocalizableText,
                                        I_AC_Name: row.AC_Name,
                                        I_AC_Text: row.AC_Text,
                                        I_AG_GroupTree: row.AG_GroupTree,
                                        I_AG_LocalizableText: row.AG_LocalizableText,
                                        I_AG_Name: row.AG_Name,
                                        I_AG_Text: row.AG_Text,
                                        I_AlarmClass: row.AlarmClass,
                                        I_AlarmGroup: row.AlarmGroup,
                                        I_AlarmState: row.AlarmState,
                                        I_ASI_BackColor: row.ASI_BackColor,
                                        I_ASI_ForeColor: row.ASI_ForeColor,
                                        I_ASI_Image: row.ASI_Image,
                                        I_ASI_LocalizableText: row.ASI_LocalizableText,
                                        I_ASI_Tag: row.ASI_Tag,
                                        I_ASI_Text: row.ASI_Text,
                                        I_DeactivationTime: row.DeactivationTime,
                                        I_FileName: row.FileName,
                                        I_HasNotes: notesRow != null,
                                        I_HistoricalID: row.HistoricalID.ToString(),
                                        I_LocalizableParam1: row.LocalizableParam1,
                                        I_LocalizableText: row.LocalizableText,
                                        I_LT_AC_Text: row.LT_AC_Text,
                                        I_LT_AG_Text: row.LT_AG_Text,
                                        I_LT_ASI_Text: row.LT_ASI_Text,
                                        I_LT_Param1: row.LT_ASI_Text,
                                        I_LT_Text: row.LT_Text,
                                        I_Machine: row.Machine,
                                        I_Name: row.Name,
                                        I_Param1: row.Param1,
                                        I_Param2: row.Param2,
                                        I_Priority: row.Priority,
                                        I_StateInfo: row.StateInfo.ToString(),
                                        I_Tag: row.Tag,
                                        I_Text: row.Text,
                                        I_User: row.User,
                                        N_ID: notesRow?.ID,
                                        N_Tag: notesRow?.Tag,
                                        N_Text: notesRow?.Text,
                                        N_Timestamp: notesRow != null ? notesRow.Timestamp : DateTime.MinValue,
                                        N_User: notesRow?.User);
                    }
                    finally
                    {
                        _addLock.Release();
                    }
                }
            }

            private async Task FillWith(IHistoricalAlarmItem item, IHistoricalAlarmNote note, CancellationToken cancellationToken = default(CancellationToken))
            {
                cancellationToken.ThrowIfCancellationRequested();
                if (item != null)
                {
                    var bitmap = (await TaskHelper.RunOnUIThread(() => item.StateInfo.Image)).BitmapToByteArray();

                    await this._addLock.WaitAsync(cancellationToken);
                    try
                    {
                        await TaskHelper.RunOnUIThread(() =>
                        {
                            this.AddJoined_ItemsAndNotesRow(
                                    I_TheAlarmItem: item,
                                    I_AcknowledgeTime: item.AcknowledgeTime,
                                    I_ActivationCounter: item.ActivationCounter,
                                    I_ActivationTime: item.ActivationTime,
                                    I_AC_LocalizableText: item.Class.LocalizableText,
                                    I_AC_Name: item.Class.Name,
                                    I_AC_Text: item.Class.Text,
                                    I_AG_GroupTree: item.Group.GroupTree,
                                    I_AG_LocalizableText: item.Group.LocalizableText,
                                    I_AG_Name: item.Group.Name,
                                    I_AG_Text: item.Group.Text,
                                    I_AlarmClass: item.Class.ToString(),
                                    I_AlarmGroup: item.Group.ToString(),
                                    I_AlarmState: item.AlarmState.ToString(),
                                    I_ASI_BackColor: item.StateInfo.BackColor.ToRgbHexCode(),
                                    I_ASI_ForeColor: item.StateInfo.ForeColor.ToRgbHexCode(),
                                    I_ASI_Image: bitmap,
                                    I_ASI_LocalizableText: item.StateInfo.LocalizableText,
                                    I_ASI_Tag: item.StateInfo.Tag,
                                    I_ASI_Text: item.StateInfo.Text,
                                    I_DeactivationTime: item.DeactivationTime,
                                    I_FileName: item.FileName,
                                    I_HasNotes: note != null,
                                    I_HistoricalID: item.HistoricalID.ToString(),
                                    I_LocalizableParam1: item.LocalizableParam1,
                                    I_LocalizableText: item.LocalizableText,
                                    I_LT_AC_Text: item.Class.LocalizableText.TryToLocalize(item.Class.Text),
                                    I_LT_AG_Text: item.Group.LocalizableText.TryToLocalize(item.Group.Text),
                                    I_LT_ASI_Text: item.StateInfo.LocalizableText.TryToLocalize(item.StateInfo.Text),
                                    I_LT_Param1: item.LocalizableParam1.TryToLocalize(item.Param1),
                                    I_LT_Text: item.LocalizableText.TryToLocalize(item.Text),
                                    I_Machine: item.Machine,
                                    I_Name: item.Name,
                                    I_Param1: item.Param1,
                                    I_Param2: item.Param2,
                                    I_Priority: item.Priority,
                                    I_StateInfo: item.StateInfo.ToString(),
                                    I_Tag: item.Tag,
                                    I_Text: item.Text,
                                    I_User: item.User,
                                    N_ID: note != null ? note.ID : "",
                                    N_Tag: note?.Tag,
                                    N_Text: note?.Text,
                                    N_Timestamp: note != null ? note.TimeStamp : DateTime.MinValue,
                                    N_User: note?.User);
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