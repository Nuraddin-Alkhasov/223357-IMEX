using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VisiWin.ApplicationFramework;
using VisiWin.Controls;
using HMI.Reporting;
using VisiWin.Trend;

namespace HMI.Reporting.DataSets.Trend
{
}

namespace HMI.Reporting.DataSets.Trend
{
}

namespace VisiWin.Reporting.Components.DataSets.Trend
{
    partial class TrendDataSampleYTDataSet
    {
        partial class DataSampleDataTable
        {
            private readonly SemaphoreSlim _addLock = new SemaphoreSlim(1, 1);

            private static readonly ITrendService trendService = ApplicationService.GetService<ITrendService>();

            public async Task FillWithAsync(ITrend trend, CancellationToken cancellationToken = default(CancellationToken))
            {
                await this.FillWithAsync(trend, DateTime.MinValue, DateTime.MinValue, cancellationToken);
            }

            public async Task FillWithAsync(ITrend trend, DateTime from, DateTime to, CancellationToken cancellationToken = default(CancellationToken)) => await Task.Run(() => FillWithAsyncInternal(trend, from, to, cancellationToken));

            private async Task FillWithAsyncInternal(ITrend trend, DateTime from, DateTime to, CancellationToken cancellationToken)
            {
                if (trend != null)
                {
                    var r = await TaskHelper.RunOnUIThread(() => new { trend.ArchiveName, TrendName = trend.Name }, cancellationToken);
                    await this.FillWithAsyncInternal(r.ArchiveName, r.TrendName, from, to, cancellationToken);
                }
            }

            public async Task FillWithAsync(string archiveName, string trendName, CancellationToken cancellationToken = default(CancellationToken))
            {
                await this.FillWithAsync(archiveName, trendName, DateTime.MinValue, DateTime.MaxValue, cancellationToken);
            }

            public async Task FillWithAsync(string archiveName, string trendName, DateTime from, DateTime to, CancellationToken cancellationToken = default(CancellationToken)) =>
                await Task.Run(() => FillWithAsyncInternal(archiveName, trendName, from, to, cancellationToken));

            private async Task FillWithAsyncInternal(string archiveName, string trendName, DateTime from, DateTime to, CancellationToken cancellationToken)
            {
                var archiv = await TaskHelper.RunOnUIThread(() => trendService.GetArchive(archiveName), cancellationToken);
                await this.FillWithAsyncInternal(archiv, trendName, from, to, cancellationToken);
            }

            public async Task FillWithAsync(IArchive archiv, string trendName, CancellationToken cancellationToken = default(CancellationToken))
            {
                await this.FillWithAsync(archiv, trendName, DateTime.MinValue, DateTime.MaxValue, cancellationToken);
            }

            public async Task FillWithAsync(IArchive archiv, string trendName, DateTime from, DateTime to, CancellationToken cancellationToken = default(CancellationToken)) =>
                await Task.Run(() => FillWithAsyncInternal(archiv, trendName, from, to, cancellationToken));

            private async Task FillWithAsyncInternal(IArchive archiv, string trendName, DateTime from, DateTime to, CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                
                TrendChartSampleCollection data = new TrendChartSampleCollection();
                if (archiv != null && !string.IsNullOrEmpty(trendName))
                {
                    bool finished = false;
                    Action<object, GetTrendDataCompletedEventArgs> action = async (sender, e) =>
                        {
                            await TaskHelper.RunOnUIThread(() =>
                                {
                                    foreach (var item in e.Data)
                                    {
                                        data.Add(new TrendChartSample(item));
                                    }
                                }, cancellationToken);
                            finished = true;
                        };

                    EventHandler<GetTrendDataCompletedEventArgs> eventHandler = new EventHandler<GetTrendDataCompletedEventArgs>(action);

                    await TaskHelper.RunOnUIThread(() =>
                        {
                            archiv.GetTrendDataCompleted += eventHandler;

                            archiv.GetTrendDataAsync(trendName, from, to, "");
                        }, cancellationToken);

                    while (!finished)
                    {
                        await Task.Delay(100, cancellationToken);
                    }

                    await TaskHelper.RunOnUIThread(() => archiv.GetTrendDataCompleted -= eventHandler, cancellationToken);

                    cancellationToken.ThrowIfCancellationRequested();

                    await this.FillWithAsyncInternal(data, trendName, cancellationToken);
                }
            }

            public async Task FillWithAsync(TrendChartSampleCollection data, string trendName, CancellationToken cancellationToken = default(CancellationToken)) => await Task.Run(() => FillWithAsyncInternal(data, trendName, cancellationToken));

            private async Task FillWithAsyncInternal(TrendChartSampleCollection data, string trendName, CancellationToken cancellationToken)
            {
                var dataList = await TaskHelper.RunOnUIThread(() => data?.ToList(), cancellationToken);

                if (dataList != null && dataList.Count > 0)
                {
                    await _addLock.WaitAsync(cancellationToken);
                    try
                    {
                        var t = await TaskHelper.RunOnUIThread(() => dataList[0].XValue, cancellationToken);
                     //   this.AddDataSampleRow(null, t.AddSeconds(-1), null, trendName);
                    }
                    finally
                    {
                        _addLock.Release();
                    }

                    foreach (var dataItem in dataList)
                    {
                        await this.FillWithAsync(dataItem, trendName, cancellationToken);
                    }

                    await _addLock.WaitAsync(cancellationToken);
                    try
                    {
                        var t = await TaskHelper.RunOnUIThread(() => dataList[dataList.Count - 1].XValue, cancellationToken);
                      //  this.AddDataSampleRow(null, t.AddSeconds(-1), null, trendName);
                    }
                    finally
                    {
                        _addLock.Release();
                    }
                }
            }

            public async Task FillWithAsync(TrendChartSample dataItem, string trendName, CancellationToken cancellationToken = default(CancellationToken))
            {
                if (dataItem != null)
                {
                    await _addLock.WaitAsync(cancellationToken);
                    try
                    {
                        var r = await TaskHelper.RunOnUIThread(() => new { dataItem.State, dataItem.XValue, dataItem.YValue }, cancellationToken);

                        if (r.State == SampleState.StartedRecording)
                        {
                         //   this.AddDataSampleRow(null, r.XValue.AddSeconds(-1), null, trendName);
                        }

                      //  this.AddDataSampleRow(r.State.ToString(), r.XValue, r.YValue, trendName);
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