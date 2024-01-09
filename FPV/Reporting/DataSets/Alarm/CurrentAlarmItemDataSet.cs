using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VisiWin.Alarm;
using HMI.Reporting;

namespace HMI.Reporting.DataSets.Alarm
{
    partial class CurrentAlarmItemDataSet
    {
        partial class CurrentAlarmItemsDataTable
        {
            private readonly SemaphoreSlim _addLock = new SemaphoreSlim(1, 1);

            private enum AcknowledgeType
            {
                //MessageClass.Column.Acknowledge.EnumValue.0.1031	zurücksetzen	
                //MessageClass.Column.Acknowledge.EnumValue.0.1033	reset	
                Reset,
                //MessageClass.Column.Acknowledge.EnumValue.1.1031	quittierpflichtig	
                //MessageClass.Column.Acknowledge.EnumValue.1.1033	acknowledgement necessary	
                Acknowledge,
                //MessageClass.Column.Acknowledge.EnumValue.2.1031	doppelt quittierpflichtig	
                //MessageClass.Column.Acknowledge.EnumValue.2.1033	double acknowledgement necessary	
                DoubleAcknowledge,
                //MessageClass.Column.Acknowledge.EnumValue.3.1031	nicht quittierpflichtig	
                //MessageClass.Column.Acknowledge.EnumValue.3.1033	acknowledgement not necessary	
                NoAcknowledge
            }

            public async Task FillWithAsync(IEnumerable<IAlarmItem> currentAlarms, CancellationToken cancellationToken = default(CancellationToken)) => await Task.Run(() => this.FillWithAsyncInternal(currentAlarms, cancellationToken));
            private async Task FillWithAsyncInternal(IEnumerable<IAlarmItem> currentAlarms, CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                if (currentAlarms != null)
                {
                    foreach (var alarm in await TaskHelper.RunOnUIThread(() => currentAlarms.ToArray(), cancellationToken))
                    {
                        await this.FillWithAsyncInternal(alarm, cancellationToken);
                    }
                }
            }

            public async Task FillWithAsync(IAlarmItem a, CancellationToken cancellationToken = default(CancellationToken)) => await Task.Run(() => this.FillWithAsyncInternal(a, cancellationToken));
            private async Task FillWithAsyncInternal(IAlarmItem a, CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                if (a != null)
                {
                    var bitmap = (await TaskHelper.RunOnUIThread(() => a.StateInfo.Image)).BitmapToByteArray();

                    await _addLock.WaitAsync(cancellationToken);
                    try
                    {
                        await TaskHelper.RunOnUIThread(() =>
                        {
                            var def = a.GetAlarmDefinition();
                            this.AddCurrentAlarmItemsRow(
                                    AcknowledgeTime: a.AcknowledgeTime,
                                    ActivationCounter: a.ActivationCounter,
                                    ActivationTime: a.ActivationTime,
                                    AC_LocalizableText: a.Class.LocalizableText,
                                    AC_Name: a.Class.Name,
                                    AC_Text: a.Class.Text,
                                    ADef_AcknowledgeMode: ((AcknowledgeType)def.AcknowledgeMode).ToString(),
                                    ADef_ClassID: def.ClassID,
                                    ADef_LowActive: def.LowActive,
                                    ADef_Name: def.Name,
                                    ADef_Tag: def.Tag,
                                    AG_GroupTree: a.Group.GroupTree,
                                    AG_LocalizableText: a.Group.LocalizableText,
                                    AG_Name: a.Group.Name,
                                    AG_Text: a.Group.Text,
                                    AlarmDefinition: def.ToString(),
                                    AlarmState: a.AlarmState.ToString(),
                                    ASI_BackColor: a.StateInfo.BackColor.ToRgbHexCode(),
                                    ASI_ForeColor: a.StateInfo.ForeColor.ToRgbHexCode(),
                                    ASI_Image: bitmap,
                                    ASI_LocalizableText: a.StateInfo.LocalizableText,
                                    ASI_Tag: a.StateInfo.Tag,
                                    ASI_Text: a.StateInfo.Text,
                                    LT_AC_Text: a.Class.LocalizableText.TryToLocalize(a.Class.Text),
                                    LT_AG_Text: a.Group.LocalizableText.TryToLocalize(a.Group.Text),
                                    LT_ASI_Text: a.StateInfo.LocalizableText.TryToLocalize(a.StateInfo.Text),
                                    Class: a.Class.ToString(),
                                    DeactivationTime: a.DeactivationTime,
                                    Group: a.Group.ToString(),
                                    ID: a.ID,
                                    LocalizableParam1: a.LocalizableParam1,
                                    LocalizableText: a.LocalizableText,
                                    LT_Param1: a.LocalizableParam1.TryToLocalize(a.Param1),
                                    LT_Text: a.LocalizableText.TryToLocalize(a.Text),
                                    Machine: a.Machine,
                                    Name: a.Name,
                                    Param1: a.Param1,
                                    Param2: a.Param2,
                                    Priority: a.Priority,
                                    StateInfo: a.StateInfo.ToString(),
                                    Tag: a.Tag,
                                    Text: a.Text,
                                    User: a.User);
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