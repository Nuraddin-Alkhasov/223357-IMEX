using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HMI.Reporting;
using VisiWin.UserManagement;

namespace HMI.Reporting.DataSets.UserManagement
{

    partial class UserGroupDataSet
    {
        partial class UserGroupWithRightsDataTable
        {
            private readonly SemaphoreSlim _addLock = new SemaphoreSlim(1, 1);

            public async Task FillWithAsync(IEnumerable<IUserGroup> userGroups, CancellationToken cancellationToken = default(CancellationToken)) => await Task.Run(() => FillWithAsyncInternal(userGroups, cancellationToken));
            private async Task FillWithAsyncInternal(IEnumerable<IUserGroup> userGroups, CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                if (userGroups != null)
                    foreach (var ug in await TaskHelper.RunOnUIThread(() => userGroups.ToArray(), cancellationToken))
                    {
                        await this.FillWithAsyncInternal(ug, cancellationToken);
                    }
            }

            private async Task FillWithAsyncInternal(IUserGroup ug, CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var rightNames = await TaskHelper.RunOnUIThread(() => ug?.RightNames?.ToList(), cancellationToken);
                if (ug != null && rightNames != null && rightNames.Count > 0)
                {
                    foreach (var r in rightNames)
                        await this.FillWithAsyncInternal(ug, await TaskHelper.RunOnUIThread(() => ug.GetRight(r), cancellationToken), cancellationToken);
                }
                else
                {
                    await this.FillWithAsyncInternal(ug, null, cancellationToken);
                }
            }

            private async Task FillWithAsyncInternal(IUserGroup ug, IRight right, CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                if (ug != null)
                {
                    await _addLock.WaitAsync(cancellationToken);
                    try
                    {
                        await TaskHelper.RunOnUIThread(() =>
                        {
                            this.AddUserGroupWithRightsRow(
                                    UG_AutoLogOffTime: ug.AutoLogOffTime,
                                    UG_Comment: ug.Comment,
                                    UG_Level: ug.Level,
                                    UG_MaxFailedLogOns: ug.MaxFailedLogOns,
                                    UG_Name: ug.Name,
                                    UG_RenewPasswordInterval: ug.RenewPasswordInterval,
                                    UG_Text: ug.Text,
                                    UG_UsersRemovable: ug.UsersRemovable,
                                    R_Active: right == null ? false : right.Active,
                                    R_Comment: right?.Comment,
                                    R_Name: right?.Name,
                                    R_Text: right?.Text,
                                    HasRights: right != null);
                        }, cancellationToken);
                    }
                    finally
                    {
                        _addLock.Release();
                    }
                }
            }
        }

        partial class UserGroupDataTable
        {
            private readonly SemaphoreSlim _addLock = new SemaphoreSlim(1, 1);

            public async Task FillWithAsync(IEnumerable<IUserGroup> userGroups, CancellationToken cancellationToken = default(CancellationToken)) => await Task.Run(() => FillWithAsyncInternal(userGroups, cancellationToken));
            private async Task FillWithAsyncInternal(IEnumerable<IUserGroup> userGroups, CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                if (userGroups != null)
                    foreach (var ug in await TaskHelper.RunOnUIThread(() => userGroups.ToArray(), cancellationToken))
                        await this.FillWithAsyncInternal(ug, cancellationToken);
            }

            private async Task FillWithAsyncInternal(IUserGroup ug, CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                if (ug != null)
                {
                    await _addLock.WaitAsync(cancellationToken);
                    try
                    {
                        await TaskHelper.RunOnUIThread(() =>
                        {
                            this.AddUserGroupRow(
                                    AutoLogOffTime: ug.AutoLogOffTime,
                                    Comment: ug.Comment,
                                    Level: ug.Level,
                                    MaxFailedLogOns: ug.MaxFailedLogOns,
                                    Name: ug.Name,
                                    RenewPasswordInterval: ug.RenewPasswordInterval,
                                    Text: ug.Text,
                                    UsersRemovable: ug.UsersRemovable);
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
