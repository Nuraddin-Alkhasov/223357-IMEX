using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HMI.Reporting;
using VisiWin.UserManagement;

namespace HMI.Reporting.DataSets.UserManagement
{


    partial class UserDataSet
    {
        partial class UserDataTable
        {
            private readonly SemaphoreSlim _addLock = new SemaphoreSlim(1, 1);

            public async Task FillWithAsync(IEnumerable<IUser> users, CancellationToken cancellationToken = default(CancellationToken)) => await Task.Run(() => FillWithAsyncInternal(users, cancellationToken));
            private async Task FillWithAsyncInternal(IEnumerable<IUser> users, CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                if (users != null)
                    foreach (var user in await TaskHelper.RunOnUIThread(() => users.ToArray(), cancellationToken))
                        await this.FillWithAsyncInternal(user, cancellationToken);
            }

            private async Task FillWithAsyncInternal(IUser user, CancellationToken cancellationToken = default(CancellationToken))
            {
                cancellationToken.ThrowIfCancellationRequested();
                if (user != null)
                {
                    await _addLock.WaitAsync(cancellationToken);
                    try
                    {
                        await TaskHelper.RunOnUIThread(() =>
                        {
                            var def = user.GetDefinition();
                            this.AddUserRow(
                                    Code: user.Code,
                                    Comment: user.Comment,
                                    DeactivationTime: user.DeactivationTime,
                                    FullName: user.FullName,
                                    GroupName: user.GroupName,
                                    Name: user.Name,
                                    RemainingLogOns: user.RemainingLogOns,
                                    RenewPassword: user.RenewPassword,
                                    UserState: (int)user.UserState,
                                    UD_FailedLogOns: def.FailedLogOns,
                                    UD_FirstLogOn: DateTime.MinValue,
                                    UD_InitialPassword: def.InitialPassword);
                        }, cancellationToken);
                    }
                    finally
                    {
                        _addLock.Release();
                    }
                }
            }
        }

        partial class UserWithRightDataTable
        {
            private readonly SemaphoreSlim _addLock = new SemaphoreSlim(1, 1);

            public async Task FillWithAsync(IEnumerable<IUser> users, CancellationToken cancellationToken = default(CancellationToken)) => await Task.Run(() => FillWithAsyncInternal(users, cancellationToken));
            private async Task FillWithAsyncInternal(IEnumerable<IUser> users, CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                if (users != null)
                    foreach (var user in await TaskHelper.RunOnUIThread(() => users.ToArray(), cancellationToken))
                        await this.FillWithAsyncInternal(user, cancellationToken);
            }

            private async Task FillWithAsyncInternal(IUser user, CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var rightNames = await TaskHelper.RunOnUIThread(() => user?.RightNames?.ToList(), cancellationToken);
                if (user != null && rightNames != null && rightNames.Count > 0)
                    foreach (var r in rightNames)
                        await this.FillWithAsyncInternal(user, await TaskHelper.RunOnUIThread(() => user.GetRight(r), cancellationToken), cancellationToken);
                else
                    await this.FillWithAsyncInternal(user, null, cancellationToken);
            }

            private async Task FillWithAsyncInternal(IUser user, IRight right, CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                if (user != null)
                {
                    await _addLock.WaitAsync(cancellationToken);
                    try
                    {
                        await TaskHelper.RunOnUIThread(() =>
                        {
                            var def = user.GetDefinition();
                            this.AddUserWithRightRow(
                                    U_Code: user.Code,
                                    U_Comment: user.Comment,
                                    U_DeactivationTime: user.DeactivationTime,
                                    U_FullName: user.FullName,
                                    U_GroupName: user.GroupName,
                                    U_Name: user.Name,
                                    U_RemainingLogOns: user.RemainingLogOns,
                                    U_RenewPassword: user.RenewPassword,
                                    U_UserState: (int)user.UserState,
                                    UD_FailedLogOns: def.FailedLogOns,
                                    UD_FirstLogOn: DateTime.MinValue,
                                    UD_InitialPassword: def.InitialPassword,
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
    }
}
